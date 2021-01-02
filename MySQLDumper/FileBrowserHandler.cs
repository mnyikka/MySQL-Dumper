
using DataScraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VintageCarPrinter
{
    //Handles browsing, from a textbox and button
    public class FileBrowserHandler
    {
        public enum BrowseMode { FileBrowseMode = 2, FolderBrowserMode = 4 }
        private BrowseMode _BrowseMode = BrowseMode.FolderBrowserMode;
        private TextBox TheSource;
        private Button BrowseButton;
        private string FilterFile = "*";
        private string Title = string.Empty;
        private string LastFilePath = "";
        
        /// <summary>
        /// BrowserHandler
        /// </summary>
        /// <param name="TheSource">Textbox</param>
        /// <param name="BrowseButton">Button</param>
        /// <param name="TheBrowseMode">Browse button</param>
        /// <param name="Title">Title to show on the filemode or foldermode</param>
        public FileBrowserHandler(TextBox TheSource, 
            Button BrowseButton, 
            BrowseMode TheBrowseMode, 
            string Title)
        {
            this._BrowseMode = TheBrowseMode;
            this.Title = Title;
            this.TheSource = TheSource;
            this.BrowseButton = BrowseButton;
            this.AddListeners();
            this.ClearFilterString();
            this.AddToFilerString("All files", new string[] { "*" });
        }

        /// <summary>
        /// Returns a reference to the browse button?
        /// </summary>
        public Button BROWSEBUTTON
        {
            get
            {
                return this.BrowseButton;
            }
        }

        /// <summary>
        /// Returns the textbox with the path
        /// </summary>
        public TextBox TEXTBOX
        {
            get
            {
                return this.TheSource;
            }
        }



        /// <summary>
        /// Adds listeners
        /// </summary>
        private void AddListeners()
        {
            this.BrowseButton.Click += new EventHandler(BrowseButtonClick);
            this.TheSource.TextChanged += new EventHandler(TheSource_TextChanged);
        }

        /// <summary>
        /// Sets the current text on the textbox
        /// </summary>
        /// <param name="NEWTEXT"></param>
        public void SetCurrentText(string NEWTEXT)
        {
            this.TheSource.Text = NEWTEXT;
            this.LastFilePath = NEWTEXT;
        }

        /// <summary>
        /// Sets the the last text path
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TheSource_TextChanged(object sender, EventArgs e)
        {
            this.LastFilePath = this.TheSource.Text;
        }


        /// <summary>
        /// Browse button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BrowseButtonClick(object sender, EventArgs e)
        {
            string TESTPATH = this.LastFilePath;
            if (this._BrowseMode == BrowseMode.FileBrowseMode)
            {
                if (StaticMethods.ShowPopUpOpenFileDialog(this.BrowseButton.FindForm(), ref TESTPATH, true, true, -1, this.FilterFile, this.Title))
                {
                    this.LastFilePath = TESTPATH;
                    this.TheSource.Text = TESTPATH;
                }
            }
            else
                if (this._BrowseMode == BrowseMode.FolderBrowserMode)
                {
                    if (StaticMethods.ShowFolderChooseDialog(Environment.SpecialFolder.MyComputer, true, this.BrowseButton.FindForm(), out TESTPATH) == DialogResult.OK)
                    {
                        this.LastFilePath = TESTPATH;
                        this.TheSource.Text = TESTPATH;
                    }
                }
        }

        /// <summary>
        /// Clears the filter string
        /// </summary>
        public void ClearFilterString()
        {
            this.FilterFile = string.Empty;
        }
        /// <summary>
        /// Adds to the filter string
        /// </summary>
        /// <param name="Description"></param>
        /// <param name="FileExtensions"></param>
        public void AddToFilerString(string Description, string[] FileExtensions)
        {
            StaticMethods.CreateAddFilterString(ref this.FilterFile, Description, FileExtensions);
        }

        /// <summary>
        /// Validate file or folder if exists
        /// </summary>
        public void ValidateFileFolderExists(string ErrorMessage)
        {
            if (this._BrowseMode == BrowseMode.FileBrowseMode)
            {
                Validation.ValidateFilePathExists(this.TEXTBOX.Text, ErrorMessage);
            }
            else
                if (this._BrowseMode == BrowseMode.FolderBrowserMode)
                {
                    Validation.ValidateFolderPathExists(this.TEXTBOX.Text, ErrorMessage);
                }
        }
    }
}
