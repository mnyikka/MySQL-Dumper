using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VintageCarPrinter;
using DataScraper;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace RenameFileScript
{
    public class ImportController
    {
        /// <summary>
        /// Main form
        /// </summary>
        private Form1 MainForm;

        /// <summary>
        /// Controls
        /// </summary>
        private TextBox TextBoxMySQLAssemblyLocationDump;
        private TextBox TextBoxUsernameImport;
        private TextBox TextBoxPasswordImport;
        private TextBox TextBoxImportLocation;
        private Button ButtonBrowseImportLocation;
        private TextBox TextBoxImportDatabasePrefix;

        private Controller MainController;

        private string StrMySQLAssemblyImport;
        private string StrUsernameImport;
        private string StrPasswordImport;
        private string StrImportLocation;
        private string StrDatabasePrefix;

        /// <summary>
        /// Dump controller
        /// </summary>
        /// <param name="MainForm"></param>
        public ImportController(Form1 MainForm,
            Controller MainController)
        {
            this.MainForm = MainForm;
            this.MainController = MainController;
            this.DiscoverControls();
            this.AddListeners();
        }


        /// <summary>
        /// Adds listeners
        /// </summary>
        private void AddListeners()
        {
            FileBrowserHandler Component = new FileBrowserHandler(this.TextBoxImportLocation, 
                this.ButtonBrowseImportLocation, 
                FileBrowserHandler.BrowseMode.FolderBrowserMode,
                "Please select the import folder");
        }


        /// <summary>
        /// Discovers controls
        /// </summary>
        private void DiscoverControls()
        {
            this.TextBoxMySQLAssemblyLocationDump = this.MainForm.TextBoxImportAssemblyName;
            this.TextBoxUsernameImport = this.MainForm.TextBoxImportUsername;
            this.TextBoxPasswordImport = this.MainForm.TextBoxImportPassword;
            this.TextBoxImportLocation = this.MainForm.TextBoxImportSourceFile;
            this.ButtonBrowseImportLocation = this.MainForm.ButtonBrowseSourceFile;
            this.TextBoxImportDatabasePrefix = this.MainForm.TextBoxNewDatabasePrefix;
        }


        /// <summary>
        /// Validate everything
        /// </summary>
        public void Validate()
        {
            this.MainController.ValidateMySQLExeLocation(this.TextBoxMySQLAssemblyLocationDump);
            Validation.ValidateRequired(this.TextBoxUsernameImport.Text, 
                "Username required");

            if (this.TextBoxImportLocation.Text.Trim().Length == 0)
            {
                this.TextBoxImportLocation.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\MySQLDumps\\";
            }

            this.StrImportLocation = this.TextBoxImportLocation.Text.Trim();

            Validation.ValidateRequired(this.StrImportLocation,
                "Either the import directory or filename import from\nis required");

            if (new DirectoryInfo(this.StrImportLocation).Exists == false && new FileInfo(this.StrImportLocation).Exists == false)
            {
                throw new Exception("The import source must either be a directory or a file\n" + this.StrImportLocation);
            }

            this.StrMySQLAssemblyImport = this.TextBoxMySQLAssemblyLocationDump.Text.Trim();
            this.StrUsernameImport = this.TextBoxUsernameImport.Text.Trim();
            this.StrPasswordImport = this.TextBoxPasswordImport.Text;
            this.StrDatabasePrefix = this.TextBoxImportDatabasePrefix.Text.Trim();
        }



        /// <summary>
        /// Runs the stuff
        /// </summary>
        public void Run()
        {
            this.MainController.WriteOutput("echo IMPORT DATABASES FROM FILES / LOCALHOST");
            Process Proc = null;
            string FileIn = null;
            try
            {
                string[] DatabaseList = this.DiscoverDatabaseListing();
                if (DatabaseList.Length == 0)
                {
                    this.MainController.WriteOutput("echo 0 Databases found, nothing to select from..");
                }
                else
                {
                    DatabaseList = (string[])StaticMethods.ExecuteWithInvoke(this.MainForm,
                        new Func<string[],string[]>(this.SelectItems),
                        new object[] { DatabaseList });

                    if (DatabaseList.Length == 0)
                    {
                        this.MainController.WriteOutput("echo Nothing selected?");
                        return;
                    }

                    this.MainController.WriteOutput("echo Importing selected databases...");
                    for (int index = 0; index < DatabaseList.Length; index++)
                    {
                        this.DumpDatabaseToFile(DatabaseList[index],ref Proc);
                    }
                }
            }
            catch (Exception em1)
            {
                throw em1;
            }
            finally
            {
                try
                {
                    if (Proc != null)
                    {
                        Proc.Kill();
                    }
                }
                catch
                {
 
                }
            }
        }


        /// <summary>
        /// Selects items
        /// </summary>
        /// <param name="Databases"></param>
        /// <returns></returns>
        private string[] SelectItems(string[] Databases)
        {
            DialogSelectDatabase Dia = new DialogSelectDatabase();
            Dia.Text = "Please select the database to import";
            Dia.SetLabelText("File paths listing");
            return Dia.SelectDatabases(Databases, this.MainForm);
        }

        /// <summary>
        /// Tries to dump a database to a file
        /// </summary>
        /// <param name="DatabaseName">The database name</param>
        private void DumpDatabaseToFile(string DatabaseFileName, ref Process Proc)
        {
            Proc = new Process();
            try
            {
                string ImportLocation = DatabaseFileName;
                if (File.Exists(ImportLocation) == false)
                {
                    throw new Exception("Import file does not exist " + ImportLocation);
                }

                //Append the prefix
                string ImportDatabaseName = this.StrDatabasePrefix + Path.GetFileNameWithoutExtension(new FileInfo(DatabaseFileName).FullName);


                this.CreateDatabaseInNotExists(ImportDatabaseName, ref Proc);


                Proc.StartInfo = new ProcessStartInfo();
                Proc.StartInfo.FileName = this.StrMySQLAssemblyImport;
                Proc.StartInfo.Arguments = " -u " + Controller.QuoteIfNecessary(this.StrUsernameImport) + " -p" + Controller.QuoteIfNecessary(this.StrUsernameImport) + " " + Controller.QuoteIfNecessary(ImportDatabaseName) + " -e \"source " + "" + Controller.QuoteIfNecessary(DatabaseFileName).Replace("\"", "'") + "" + "\"";

                this.MainController.WriteOutput(Controller.QuoteIfNecessary(Proc.StartInfo.FileName) + " " + Proc.StartInfo.Arguments);

                Proc.StartInfo.UseShellExecute = false;
                Proc.StartInfo.RedirectStandardOutput = true;
                Proc.StartInfo.RedirectStandardInput = true;
                Proc.StartInfo.RedirectStandardError = true;
                Proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Proc.StartInfo.CreateNoWindow = true;
                Proc.Start();

                this.MainController.WriteOutput("echo Waiting for exit...");
                Proc.WaitForExit();

                if (Proc.HasExited == false)
                {
                    try
                    {
                        Proc.Kill();
                    }
                    catch
                    {

                    }
                    throw new Exception("Importing took too long for database "+DatabaseFileName);
                }

                string Output = Proc.StandardOutput.ReadToEnd();
                Output = Output + Environment.NewLine + Proc.StandardError.ReadToEnd();
                this.MainController.WriteOutput(Output);

                if (Output.Contains("Error"))
                {
                    this.MainController.WriteOutput("echo Importation could have had errors");
                }
                else
                {
                    this.MainController.WriteOutput("echo Database IMPORT DONE " + ImportDatabaseName + " from file: " + DatabaseFileName);
                }
            }
            catch (Exception em1)
            {
                throw em1;
            }
            finally
            {
                try
                {
                    Proc.Kill();
                }
                catch
                {

                }
            }
        }

        private void CreateDatabaseInNotExists(string ImportDatabaseName, ref Process Proc)
        {
            this.MainController.WriteOutput("echo CREATE DATABASE IF NOT EXISTS " + ImportDatabaseName + "");

            Proc = new Process();
            try
            {
                Proc.StartInfo = new ProcessStartInfo();
                Proc.StartInfo.FileName = this.StrMySQLAssemblyImport;
                Proc.StartInfo.Arguments = " -u " + Controller.QuoteIfNecessary(this.StrUsernameImport) + " -p" + Controller.QuoteIfNecessary(this.StrPasswordImport) + " -e " + Controller.QuoteIfNecessary("CREATE DATABASE IF NOT EXISTS `"+ImportDatabaseName+"`");

                this.MainController.WriteOutput(Controller.QuoteIfNecessary(this.StrMySQLAssemblyImport) + " " + Proc.StartInfo.Arguments);

                Proc.StartInfo.UseShellExecute = false;
                Proc.StartInfo.RedirectStandardOutput = true;
                Proc.StartInfo.RedirectStandardInput = true;
                Proc.StartInfo.RedirectStandardError = true;
                Proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Proc.StartInfo.CreateNoWindow = true;
                Proc.Start();

                this.MainController.WriteOutput("echo Waiting for exit...");
                Proc.WaitForExit(5000 * 2);

                if (Proc.HasExited == false)
                {
                    try
                    {
                        Proc.Kill();
                    }
                    catch
                    {

                    }
                }

                string Output = Proc.StandardOutput.ReadToEnd();
                Output = Output + "\n" + Proc.StandardError.ReadToEnd();
                this.MainController.WriteOutput(Output);
                
            }
            catch (Exception em1)
            {
                throw em1;
            }
            finally
            {
                try
                {
                    Proc.Kill();
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Returns an array of databases listing, if the source is a Directory
        /// Or a single item if the source is a single sql file
        /// </summary>
        /// <returns></returns>
        private string[] DiscoverDatabaseListing()
        {
            try
            {
                DirectoryInfo NewInfo = new DirectoryInfo(this.StrImportLocation);
                List<string> ListFileOutput = null;
                if (NewInfo.Exists)
                {
                    FileInfo[] Files = NewInfo.GetFiles("*.sql", 
                        SearchOption.AllDirectories);
                    ListFileOutput = new List<string>(Files.Length);
                    for (int index = 0; index < Files.Length; index++)
                    {
                        ListFileOutput.Add(Files[index].FullName);
                    }
                    return ListFileOutput.ToArray();
                }

                FileInfo NewInfoFil = new FileInfo(this.StrImportLocation);
                if (NewInfoFil.Exists)
                {
                    return new string[] { NewInfoFil.FullName };
                }

                throw new Exception("We could not find a single file or directory from the input\nsource supplied");

            }
            catch (Exception em1)
            {
                throw em1;
            }
            finally
            {
               
            }
        }

        /// <summary>
        /// Writes the dump commands
        /// </summary>
        /// <param name="FileIn"></param>
        private void WriteCommands(string FileIn)
        {
            //First command is to list all databases

        }

        /// <summary>
        /// File with arguments must be prepared to be run
        /// </summary>
        /// <returns>Returns the temp file</returns>
        private string PrepareFile()
        {
            string FolderLocation = Path.GetTempPath() + "\\" + Form1._APPNAME;
            Validation.ValidateFolderPathExists(FolderLocation,"We are unable to write folder location\n"+FolderLocation);
            string FileLocation = FolderLocation + "\\DumpCommands.bat";
            if (File.Exists(FileLocation) == true)
                File.Delete(FileLocation);
            return FileLocation;
        }
    }
}
