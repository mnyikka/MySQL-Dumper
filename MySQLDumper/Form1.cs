using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataScraper;

namespace RenameFileScript
{
    public partial class Form1 : Form
    {
        public UIState State;
        public const string _APPNAME = "MYSQL_DUMP_IMPORT";
        public Form1()
        {
            InitializeComponent();

            this.State = new UIState(Form1._APPNAME);

            this.State.AddControlUIState(new UIState.ControlUIState(this.TextBoxDumpLocation,
                 UIState.UIStateProperty.Text,
                 this.TextBoxDumpLocation.Text));

            this.State.AddControlUIState(new UIState.ControlUIState(this.TextBoxImportPassword,
                 UIState.UIStateProperty.Text,
                 this.TextBoxImportPassword.Text));

            this.State.AddControlUIState(new UIState.ControlUIState(this.TextBoxImportSourceFile,
                 UIState.UIStateProperty.Text,
                 this.TextBoxImportSourceFile.Text));

            this.State.AddControlUIState(new UIState.ControlUIState(this.TextBoxImportUsername,
                 UIState.UIStateProperty.Text,
                 this.TextBoxImportUsername.Text));

            this.State.AddControlUIState(new UIState.ControlUIState(this.TextBoxMySQLAssemblyLocationDump,
                 UIState.UIStateProperty.Text,
                 this.TextBoxMySQLAssemblyLocationDump.Text));


            this.State.AddControlUIState(new UIState.ControlUIState(this.TextBoxPasswordDump,
              UIState.UIStateProperty.Text,
              this.TextBoxPasswordDump.Text));

            this.State.AddControlUIState(new UIState.ControlUIState(this.TextBoxUsernameDump,
              UIState.UIStateProperty.Text,
              this.TextBoxUsernameDump.Text));

            this.State.AddControlUIState(new UIState.ControlUIState(this.TextBoxNewDatabasePrefix,
              UIState.UIStateProperty.Text,
              this.TextBoxNewDatabasePrefix.Text));
            
            this.State.LoadSavedData();
            this.State.SaveUIData();
            this.Test();
            new Controller(this);
        }

        /// <summary>
        /// Tests pdf extraction etc
        /// </summary>
        private void Test()
        {
        }

        internal void SaveUIState()
        {
            this.State.SaveUIData();
        }
    }
}
