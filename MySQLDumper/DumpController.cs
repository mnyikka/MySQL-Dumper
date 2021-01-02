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
    public class DumpController
    {
        /// <summary>
        /// Main form
        /// </summary>
        private Form1 MainForm;

        /// <summary>
        /// Controls
        /// </summary>
        private TextBox TextBoxMySQLAssemblyLocationDump;
        private TextBox TextBoxUsernameDump;
        private TextBox TextBoxPasswordDump;
        private TextBox TextBoxDumpLocation;
        private Button ButtonBrowseDumpLocation;
        private Controller MainController;

        private string StrMySQLAssemblyLocationDump;
        private string StrUsernameDump;
        private string StrPasswordDump;
        private string StrDumpLocation;

        /// <summary>
        /// Dump controller
        /// </summary>
        /// <param name="MainForm"></param>
        public DumpController(Form1 MainForm,
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
            FileBrowserHandler Component = new FileBrowserHandler(this.TextBoxDumpLocation, 
                this.ButtonBrowseDumpLocation, 
                FileBrowserHandler.BrowseMode.FolderBrowserMode,
                "Please select dumping folder");
        }


        /// <summary>
        /// Discovers controls
        /// </summary>
        private void DiscoverControls()
        {
            this.TextBoxMySQLAssemblyLocationDump = this.MainForm.TextBoxMySQLAssemblyLocationDump;
            this.TextBoxUsernameDump = this.MainForm.TextBoxUsernameDump;
            this.TextBoxPasswordDump = this.MainForm.TextBoxPasswordDump;
            this.TextBoxDumpLocation = this.MainForm.TextBoxDumpLocation;
            this.ButtonBrowseDumpLocation = this.MainForm.ButtonBrowseDumpLocation;
        }


        /// <summary>
        /// Validate everything
        /// </summary>
        public void Validate()
        {
            this.MainController.ValidateMySQLExeLocation(this.TextBoxMySQLAssemblyLocationDump);
            Validation.ValidateRequired(this.TextBoxUsernameDump.Text, 
                "Username required");
            if (this.TextBoxDumpLocation.Text.Trim().Length == 0)
            {
                this.TextBoxDumpLocation.Text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\MySQLDumps\\";
            }

            Validation.ValidateFolderPathExists(this.TextBoxDumpLocation.Text,
                "Please make sure the folder location exists\n" + this.TextBoxDumpLocation.Text);

            this.StrMySQLAssemblyLocationDump = this.TextBoxMySQLAssemblyLocationDump.Text.Trim();
            this.StrDumpLocation = this.TextBoxDumpLocation.Text.Trim();
            this.StrUsernameDump = this.TextBoxUsernameDump.Text.Trim();
            this.StrPasswordDump = this.TextBoxPasswordDump.Text;
        }



        /// <summary>
        /// Runs the stuff
        /// </summary>
        public void Run()
        {
            this.MainController.WriteOutput("echo DUMP-DATABASES PROCESS FROM LOCALHOST");
            Process Proc = null;
            string FileIn = null;
            try
            {
                //FileIn = this.PrepareFile();
                string[] DatabaseList = this.DiscoverDatabaseListing(ref Proc);
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

                    this.MainController.WriteOutput("echo Dumping selected databases to destination folder");
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
            return Dia.SelectDatabases(Databases, this.MainForm);
        }

        /// <summary>
        /// Tries to dump a database to a file
        /// </summary>
        /// <param name="DatabaseName">The database name</param>
        private void DumpDatabaseToFile(string DatabaseName, ref Process Proc)
        {
            Proc = new Process();
            try
            {
                string DumpSQLLocation = this.StrDumpLocation + "\\" + DatabaseName + ".sql";
                if (File.Exists(DumpSQLLocation) == true)
                {
                    File.Delete(DumpSQLLocation);
                }

                Proc.StartInfo = new ProcessStartInfo();
                //Proc.StartInfo.WorkingDirectory = this.StrDumpLocation;
                Proc.StartInfo.FileName = Path.GetDirectoryName(this.StrMySQLAssemblyLocationDump) + "\\" + "mysqldump.exe";
                Proc.StartInfo.Arguments = " -u " + Controller.QuoteIfNecessary(this.StrUsernameDump) + " -p" + Controller.QuoteIfNecessary(this.StrUsernameDump) + " --databases " + Controller.QuoteIfNecessary(DatabaseName) + " --result-file=" + Controller.QuoteIfNecessary(DumpSQLLocation);

                this.MainController.WriteOutput(Controller.QuoteIfNecessary(Proc.StartInfo.FileName) + " " + Proc.StartInfo.Arguments);

                Proc.StartInfo.UseShellExecute = false;
                Proc.StartInfo.RedirectStandardOutput = true;
                Proc.StartInfo.RedirectStandardInput = true;
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
                    throw new Exception("Dumping took too long for database "+DatabaseName);
                }

                string Output = Proc.StandardOutput.ReadToEnd();
                this.MainController.WriteOutput(Output);

                if (File.Exists(DumpSQLLocation) == false)
                {
                    throw new Exception("Dumping was not successfull for file\n" + DumpSQLLocation);
                }

                this.MainController.WriteOutput("echo Database DUMPED OK " + DatabaseName + " to file " + DumpSQLLocation);

                this.MainController.WriteOutput("echo Processing DUMPED FILE..to remove USE / and CREATE DATABASE " + DumpSQLLocation);
                this.PostProcessFile(DumpSQLLocation);
                this.MainController.WriteOutput("echo DONE");
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

        private void PostProcessFile(string DumpSQLLocation)
        {
            StreamReader NewRead = new StreamReader(DumpSQLLocation);
            string FileIn = this.PrepareFile();
            StreamWriter NewWrite = new StreamWriter(FileIn, false);

            try
            {
                string NEWLINE = NewRead.ReadLine();
                int HasReplace = 0;
                while (NEWLINE != null)
                {
                    if (HasReplace < 2)
                    {
                        if (NEWLINE.StartsWith("CREATE DATABASE"))
                        {
                            NEWLINE = NEWLINE.Replace("CREATE DATABASE", "-- CREATE DATABASE");
                            this.MainController.WriteOutput(NEWLINE);
                            HasReplace = HasReplace + 1;
                        }

                        if (NEWLINE.StartsWith("USE "))
                        {
                            NEWLINE = NEWLINE.Replace("USE", "-- USE");
                            this.MainController.WriteOutput(NEWLINE);
                            HasReplace = HasReplace + 1;
                        }
                    }

                    //Lines.Add(NEWLINE);
                    NewWrite.WriteLine(NEWLINE);
                    NewWrite.Flush();
                    NEWLINE = NewRead.ReadLine();
                }


                NewRead.Close();
                NewRead.Dispose();
                NewRead = null;

                NewWrite.Flush();
                NewWrite.Close();
                NewWrite.Dispose();
                NewWrite = null;

                File.Copy(FileIn,DumpSQLLocation, true);
                File.Delete(FileIn);
            }
            catch (Exception em1)
            {
                throw em1;
            }
            finally
            {
                try
                {
                    NewRead.Close();
                    NewRead.Dispose();
                    NewRead = null;
                }
                catch
                {
 
                }

                try
                {
                    NewWrite.Close();
                    NewWrite.Dispose();
                    NewWrite = null;
                }
                catch
                {

                }

                try
                {
                    File.Delete(FileIn);
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Returns an array of databases listing
        /// </summary>
        /// <returns></returns>
        private string[] DiscoverDatabaseListing(ref Process Proc)
        {
            Proc = new Process();
            try
            {
                Proc.StartInfo = new ProcessStartInfo();
                Proc.StartInfo.FileName = this.StrMySQLAssemblyLocationDump;
                Proc.StartInfo.Arguments = " -u " + Controller.QuoteIfNecessary(this.StrUsernameDump) + " -p" + Controller.QuoteIfNecessary(this.StrPasswordDump) + " -e " + Controller.QuoteIfNecessary("SHOW DATABASES");

                this.MainController.WriteOutput(Controller.QuoteIfNecessary(this.StrMySQLAssemblyLocationDump) + " " + Proc.StartInfo.Arguments);
                
                Proc.StartInfo.UseShellExecute = false;
                Proc.StartInfo.RedirectStandardOutput = true;
                Proc.StartInfo.RedirectStandardInput = true;
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
                this.MainController.WriteOutput(Output);

                string[] Databases = Output.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                List<string> List_ = new List<string>(Databases.Length);
                List_.AddRange(Databases);
                if (List_.Count < 1)
                    return new string[] { };

                string[] OutputArray = new string[List_.Count - 1];
                for (int index = 1; index < List_.Count; index++)
                {
                    OutputArray[index - 1] = List_[index];
                }

                return OutputArray;
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
            string FileLocation = FolderLocation + "\\TempFile"+DateTime.Now.Ticks+".sql";
            if (File.Exists(FileLocation) == true)
                File.Delete(FileLocation);
            return FileLocation;
        }
    }
}
