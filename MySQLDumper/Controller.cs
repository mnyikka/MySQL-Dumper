using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using DataScraper;
using System.Windows.Forms;
using System.IO;

namespace RenameFileScript
{
    public partial class Controller
    {
        /// <summary>
        /// Current processing mode
        /// </summary>
        public enum ProcessingMode 
        { 
            DumpMode=2, 
            ImportMode=4
        } 

        private Form1 MainForm;
        private Thread ThreadRunOperations;
        private ProcessingMode KeepFilesMode = ProcessingMode.DumpMode;
        private Exception LastError = null;

        private DumpController DumpController_;
        private ImportController ImportController_;


        /// <summary>
        /// Controller / 
        /// </summary>
        /// <param name="MainForm"></param>
        public Controller(Form1 MainForm)
        {
            this.MainForm = MainForm;
            this.AddListeners();

            this.DumpController_ = new DumpController(MainForm, 
                this);
            this.ImportController_ = new ImportController(MainForm,
                this);
        }

        /// <summary>
        /// Listeners
        /// </summary>
        private void AddListeners()
        {
            this.MainForm.ButtonBeginDump.Click += new EventHandler(AllButtonsClick);
            this.MainForm.ButtonBeginImport.Click += new EventHandler(AllButtonsClick);
            this.MainForm.FormClosing += new FormClosingEventHandler(MainForm_FormClosing);
        }

        /// <summary>
        /// Before the form closes, throw a relevant exception
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainForm_FormClosing(object sender, 
            FormClosingEventArgs e)
        {
            try
            {
                this.CheckThrowExceptionIfThreadStillAlive();
            }
            catch (Exception em1)
            {
                StaticMethods.DisplayErrorMessage(this.MainForm,
                    null,
                    false,
                    em1.Message);
                e.Cancel = true;
            }
        }

            /// <summary>
            /// All buttons clicked
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            void AllButtonsClick(object sender, EventArgs e)
            {
                try
                {
                    if (sender == this.MainForm.ButtonBeginDump)
                    {
                        if (Controller.IsStartButton(this.MainForm.ButtonBeginDump) == true)
                        {
                            this.DumpController_.Validate();
                            this.MainForm.SaveUIState();
                            this.KeepFilesMode = ProcessingMode.DumpMode;
                            StaticMethods.ReInitializeThread(ref this.ThreadRunOperations,
                                true,
                                new ThreadStart(this.DoProcessing));
                        }
                        else
                        {
                            StaticMethods.ReInitializeThread(ref this.ThreadRunOperations,
                               false,
                               new ThreadStart(this.DoProcessing));
                        }
                    }
                    else
                    if (sender == this.MainForm.ButtonBeginImport)
                    {
                        if (Controller.IsStartButton(this.MainForm.ButtonBeginImport) == true)
                        {
                            this.ImportController_.Validate();
                            this.MainForm.SaveUIState();
                            this.KeepFilesMode = ProcessingMode.ImportMode;
                            StaticMethods.ReInitializeThread(ref this.ThreadRunOperations,
                                true,
                                new ThreadStart(this.DoProcessing));
                        }
                        else
                        {
                            StaticMethods.ReInitializeThread(ref this.ThreadRunOperations,
                               false,
                               new ThreadStart(this.DoProcessing));
                        }
                    }
                }
                catch (Exception em)
                {
                    StaticMethods.DisplayErrorMessage(this.MainForm,
                        null,
                        false,
                        em.Message);
                }
            }

            private static bool IsStartButton(Button button)
            {
                return button.Text.StartsWith("Start");
            }

        /// <summary>
        /// Throws an exception if an operation is still going on
        /// </summary>
        private void CheckThrowExceptionIfThreadStillAlive()
        {
            string OngoingOperation = string.Empty;

            if ( this.KeepFilesMode == ProcessingMode.ImportMode )
            {
                OngoingOperation = "Import database mode";
            }
            else
            if (this.KeepFilesMode == ProcessingMode.DumpMode)
            {
                OngoingOperation = "Dump mode";
            }
            
            if (this.ThreadRunOperations != null && this.ThreadRunOperations.IsAlive == true)
            {
                throw new Exception("Please stop the ongoing operation '" + OngoingOperation + "'");
            }
        }

        #region StartEnd

        /// <summary>
        /// On End Processing
        /// </summary>
        private void OnEndProcessing()
        {
            if (this.MainForm.InvokeRequired)
            {
                StaticMethods.ExecuteWithInvoke(this.MainForm,
                    new MethodInvoker(this.OnEndProcessing),
                    null);
                return;
            }


            if (this.KeepFilesMode == ProcessingMode.DumpMode)
            {
                this.MainForm.ButtonBeginImport.Enabled = true;
                this.MainForm.ButtonBeginDump.Text = "Start";
            }
            else
            if (this.KeepFilesMode == ProcessingMode.ImportMode)
            {
                this.MainForm.ButtonBeginDump.Enabled = true;
                this.MainForm.ButtonBeginImport.Text = "Start";
            }
            if (this.LastError != null && (this.LastError is ThreadAbortException) == false)
            {
                StaticMethods.DisplayErrorMessage(this.MainForm,
                    null,
                    false,
                    this.LastError.Message);
                return;
            }
            if (this.LastError == null)
            {
                StaticMethods.ShowSuccessMessage(this.MainForm,
                    "Processing finished successfully!");
            }
            
        }
        /// <summary>
        /// On Start Processing
        /// </summary>
        private void OnStartProcessing()
        {
            if (this.MainForm.InvokeRequired)
            {
                StaticMethods.ExecuteWithInvoke(this.MainForm,
                    new MethodInvoker(this.OnStartProcessing),
                    null);
                return;
            }
            if (this.KeepFilesMode == ProcessingMode.DumpMode)
            {
                this.MainForm.ButtonBeginImport.Enabled = false;
                this.MainForm.ButtonBeginDump.Text = "Cancel";
            }
            else
            if(this.KeepFilesMode == ProcessingMode.ImportMode)
            {
                this.MainForm.ButtonBeginDump.Enabled = false;
                this.MainForm.ButtonBeginImport.Text = "Cancel";
            }
            this.MainForm.TextBoxCommandPrompt.Clear();
            this.LastError = null;
        }

        #region Processing
        /// <summary>
        /// Runs the data-processing
        /// </summary>
        private void DoProcessing()
        {
            try
            {
                this.OnStartProcessing();
                if (this.KeepFilesMode == ProcessingMode.DumpMode)
                {
                    this.DumpController_.Run();
                }
                else
                if (this.KeepFilesMode == ProcessingMode.ImportMode)
                {
                    this.ImportController_.Run();
                }

            }
            catch (Exception em1)
            {
                this.LastError = em1;
            }
            finally
            {
                this.OnEndProcessing();
            }
        }
        
        public static string RemoveUnnecessaryCharacters(string SenderName)
        {
            StringBuilder NewBuild = new StringBuilder(SenderName.Length);
            for (int index = 0; index < SenderName.Length; index++)
            {
                if (char.IsLetterOrDigit(SenderName[index]) == true || SenderName[index] == '_' )
                {
                    NewBuild.Append(SenderName[index]);
                }
                else
                {
                    NewBuild.Append(" ");
                }
            }
            SenderName = NewBuild.ToString();
            SenderName = StaticMethods.PunctuateString(SenderName).ToUpper();
            return SenderName.Trim();
        }

        #endregion

        private static string MakeFileName(string InName)
        {
            string[] Delimiters = new string[] { " - ", "_" };

            for (int index = 0; index < Delimiters.Length; index++)
            {
                string[] array_ = InName.Split(new string[] { Delimiters[index] }, StringSplitOptions.RemoveEmptyEntries);

                //LAST ONE MUST BE A DATE
                string CheckDate = array_[array_.Length - 1];
                CheckDate = CheckDate.Split(new char[] {'.'},StringSplitOptions.None)[0];
                CheckDate = CheckDate.Split(new char[] {'('},StringSplitOptions.None)[0];
                CheckDate = CheckDate.Trim(new char[] { ' ' });
                CheckDate = CheckDate.Trim(new char[] { ' ' });
                try
                {
                    DateTime NewDT = DateTime.ParseExact(CheckDate, "yyyy-MM-dd", null);

                    string Subject = string.Empty;
                    for (int checkindex = 1; checkindex < array_.Length - 1; checkindex ++)
                    {
                        Subject = Subject + array_[checkindex] + " ";
                    }
                    Subject = Subject.Trim(new char[] { ' ' });
                    return NewDT.ToString("yyyy-MM-dd") + "_" + array_[0] + "_" + Subject + ".pdf";
                }
                catch
                {

                }
            }

            throw new Exception("Cannot make name from " + InName);
        }

        /// <summary>
        /// Files status
        /// </summary>
        /// <param name="Status"></param>
        private void SetStatus(string StatusText)
        {
            if (this.MainForm.StatusLabel.Owner.InvokeRequired == true)
            {
                StaticMethods.ExecuteWithInvoke(this.MainForm.StatusLabel.Owner,
                    new Action<string>(this.SetStatus),
                    new object[] { StatusText });
                return;
            }
            this.MainForm.StatusLabel.Text = StatusText;
        }

        #endregion

        /// <summary>
        /// Validates the mysql.exe
        /// </summary>
        /// <param name="textBox"></param>
        internal void ValidateMySQLExeLocation(TextBox textBox)
        {
            string Suggested = string.Empty;

            if (textBox.Text.Trim().Length == 0)
            {
                textBox.Text = @"C:\Program Files\MariaDB 10.3\bin\mysql.exe";
            }

            if (File.Exists(textBox.Text) == false)
            {
                Suggested = textBox.Text.ToUpper();
                Suggested = Suggested.Replace(("PROGRAM FILES\\"), 
                    ("Program Files (x86)\\").ToUpper());

                if (File.Exists(Suggested) == false)
                {
                    Suggested = textBox.Text.ToUpper();
                    Suggested = Suggested.Replace(("Program Files (x86)\\").ToUpper(),
                        ("PROGRAM FILES\\")
                    );
                }

                if (File.Exists(Suggested) == true)
                {
                    textBox.Text = Suggested;
                }
            }

            Validation.ValidateFilePathExists(textBox.Text,
                "We are unable to find the path to the executable\n" + textBox.Text);

            FileInfo NewInfo = new FileInfo(textBox.Text);
            if (NewInfo.Extension.ToUpper().Equals(".EXE") == false)
                throw new Exception("Unexpected extension");
        }

        /// <summary>
        /// Quotes a string if it has spaces
        /// </summary>
        /// <param name="InString">The string subject</param>
        /// <returns></returns>
        internal static string QuoteIfNecessary(string InString)
        {
            if (string.IsNullOrEmpty(InString))
                return string.Empty;
            if (InString.Contains(" "))
                return "\"" + InString + "\"";
            return InString;
        }

       

        /// <summary>
        /// Writes an output on the console window
        /// </summary>
        /// <param name="OutputLine">The text to write on the output</param>
        internal void WriteOutput(string OutputLine)
        {
            if (this.MainForm.TextBoxCommandPrompt.InvokeRequired == true)
            {
                 StaticMethods.ExecuteWithInvoke(this.MainForm.TextBoxCommandPrompt,
                    new Action<string>(this.WriteOutput),
                    new object[] { OutputLine });
                 return;
            }
            this.MainForm.TextBoxCommandPrompt.AppendText(OutputLine + Environment.NewLine);
            this.MainForm.MainTabControl.SelectedTab = ((TabPage)this.MainForm.TextBoxCommandPrompt.Parent);

        }
    }
}
