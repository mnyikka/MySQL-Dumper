namespace RenameFileScript
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.StatusStripStatus = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainTabControl = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.ButtonBeginDump = new System.Windows.Forms.Button();
            this.TextBoxMySQLAssemblyLocationDump = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBoxDumpLocation = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.TextBoxUsernameDump = new System.Windows.Forms.TextBox();
            this.TextBoxPasswordDump = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ButtonBrowseDumpLocation = new System.Windows.Forms.Button();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.TextBoxCommandPrompt = new System.Windows.Forms.TextBox();
            this.ButtonBrowseSourceFile = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TextBoxImportPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.TextBoxImportUsername = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TextBoxImportSourceFile = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ButtonBeginImport = new System.Windows.Forms.Button();
            this.TextBoxImportAssemblyName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.TextBoxNewDatabasePrefix = new System.Windows.Forms.TextBox();
            this.StatusStripStatus.SuspendLayout();
            this.MainTabControl.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // StatusStripStatus
            // 
            this.StatusStripStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.StatusStripStatus.Location = new System.Drawing.Point(5, 423);
            this.StatusStripStatus.Name = "StatusStripStatus";
            this.StatusStripStatus.Size = new System.Drawing.Size(710, 22);
            this.StatusStripStatus.TabIndex = 7;
            this.StatusStripStatus.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(39, 17);
            this.StatusLabel.Text = "Ready";
            // 
            // MainTabControl
            // 
            this.MainTabControl.Controls.Add(this.tabPage5);
            this.MainTabControl.Controls.Add(this.tabPage4);
            this.MainTabControl.Controls.Add(this.tabPage1);
            this.MainTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTabControl.Location = new System.Drawing.Point(5, 5);
            this.MainTabControl.Name = "MainTabControl";
            this.MainTabControl.SelectedIndex = 0;
            this.MainTabControl.Size = new System.Drawing.Size(710, 418);
            this.MainTabControl.TabIndex = 8;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.ButtonBrowseDumpLocation);
            this.tabPage5.Controls.Add(this.groupBox1);
            this.tabPage5.Controls.Add(this.TextBoxDumpLocation);
            this.tabPage5.Controls.Add(this.label1);
            this.tabPage5.Controls.Add(this.ButtonBeginDump);
            this.tabPage5.Controls.Add(this.TextBoxMySQLAssemblyLocationDump);
            this.tabPage5.Controls.Add(this.label13);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(702, 392);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Dump mode";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // ButtonBeginDump
            // 
            this.ButtonBeginDump.Location = new System.Drawing.Point(24, 188);
            this.ButtonBeginDump.Name = "ButtonBeginDump";
            this.ButtonBeginDump.Size = new System.Drawing.Size(55, 23);
            this.ButtonBeginDump.TabIndex = 24;
            this.ButtonBeginDump.Text = "Start";
            this.ButtonBeginDump.UseVisualStyleBackColor = true;
            // 
            // TextBoxMySQLAssemblyLocationDump
            // 
            this.TextBoxMySQLAssemblyLocationDump.Location = new System.Drawing.Point(24, 37);
            this.TextBoxMySQLAssemblyLocationDump.Name = "TextBoxMySQLAssemblyLocationDump";
            this.TextBoxMySQLAssemblyLocationDump.Size = new System.Drawing.Size(637, 20);
            this.TextBoxMySQLAssemblyLocationDump.TabIndex = 23;
            this.TextBoxMySQLAssemblyLocationDump.Text = "C:\\Program Files (x86)\\MariaDB 10.3\\bin\\mysql.exe";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(21, 21);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(132, 13);
            this.label13.TabIndex = 22;
            this.label13.Text = "MySQL assembly Location";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.TextBoxNewDatabasePrefix);
            this.tabPage4.Controls.Add(this.label8);
            this.tabPage4.Controls.Add(this.ButtonBrowseSourceFile);
            this.tabPage4.Controls.Add(this.groupBox2);
            this.tabPage4.Controls.Add(this.TextBoxImportSourceFile);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Controls.Add(this.ButtonBeginImport);
            this.tabPage4.Controls.Add(this.TextBoxImportAssemblyName);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(702, 392);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Import mode";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 149);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 25;
            this.label1.Text = "Dump location";
            // 
            // TextBoxDumpLocation
            // 
            this.TextBoxDumpLocation.Location = new System.Drawing.Point(24, 165);
            this.TextBoxDumpLocation.Name = "TextBoxDumpLocation";
            this.TextBoxDumpLocation.Size = new System.Drawing.Size(589, 20);
            this.TextBoxDumpLocation.TabIndex = 26;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TextBoxPasswordDump);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.TextBoxUsernameDump);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(24, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(309, 80);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Credentials";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Username:";
            // 
            // TextBoxUsernameDump
            // 
            this.TextBoxUsernameDump.Location = new System.Drawing.Point(73, 19);
            this.TextBoxUsernameDump.Name = "TextBoxUsernameDump";
            this.TextBoxUsernameDump.Size = new System.Drawing.Size(222, 20);
            this.TextBoxUsernameDump.TabIndex = 1;
            this.TextBoxUsernameDump.Text = "root";
            // 
            // TextBoxPasswordDump
            // 
            this.TextBoxPasswordDump.Location = new System.Drawing.Point(73, 49);
            this.TextBoxPasswordDump.Name = "TextBoxPasswordDump";
            this.TextBoxPasswordDump.PasswordChar = '*';
            this.TextBoxPasswordDump.Size = new System.Drawing.Size(222, 20);
            this.TextBoxPasswordDump.TabIndex = 3;
            this.TextBoxPasswordDump.Text = "root";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Password:";
            // 
            // ButtonBrowseDumpLocation
            // 
            this.ButtonBrowseDumpLocation.Location = new System.Drawing.Point(615, 163);
            this.ButtonBrowseDumpLocation.Name = "ButtonBrowseDumpLocation";
            this.ButtonBrowseDumpLocation.Size = new System.Drawing.Size(55, 23);
            this.ButtonBrowseDumpLocation.TabIndex = 28;
            this.ButtonBrowseDumpLocation.Text = "Browse";
            this.ButtonBrowseDumpLocation.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.TextBoxCommandPrompt);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(702, 392);
            this.tabPage1.TabIndex = 5;
            this.tabPage1.Text = "Command prompt output";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // TextBoxCommandPrompt
            // 
            this.TextBoxCommandPrompt.BackColor = System.Drawing.Color.Black;
            this.TextBoxCommandPrompt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TextBoxCommandPrompt.Font = new System.Drawing.Font("Arial", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBoxCommandPrompt.ForeColor = System.Drawing.Color.Lime;
            this.TextBoxCommandPrompt.Location = new System.Drawing.Point(3, 3);
            this.TextBoxCommandPrompt.Multiline = true;
            this.TextBoxCommandPrompt.Name = "TextBoxCommandPrompt";
            this.TextBoxCommandPrompt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBoxCommandPrompt.Size = new System.Drawing.Size(696, 386);
            this.TextBoxCommandPrompt.TabIndex = 0;
            this.TextBoxCommandPrompt.Text = "Ready";
            // 
            // ButtonBrowseSourceFile
            // 
            this.ButtonBrowseSourceFile.Location = new System.Drawing.Point(617, 173);
            this.ButtonBrowseSourceFile.Name = "ButtonBrowseSourceFile";
            this.ButtonBrowseSourceFile.Size = new System.Drawing.Size(55, 23);
            this.ButtonBrowseSourceFile.TabIndex = 35;
            this.ButtonBrowseSourceFile.Text = "Browse";
            this.ButtonBrowseSourceFile.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TextBoxImportPassword);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.TextBoxImportUsername);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(25, 65);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(309, 80);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Credentials";
            // 
            // TextBoxImportPassword
            // 
            this.TextBoxImportPassword.Location = new System.Drawing.Point(73, 49);
            this.TextBoxImportPassword.Name = "TextBoxImportPassword";
            this.TextBoxImportPassword.PasswordChar = '*';
            this.TextBoxImportPassword.Size = new System.Drawing.Size(222, 20);
            this.TextBoxImportPassword.TabIndex = 3;
            this.TextBoxImportPassword.Text = "root";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Password:";
            // 
            // TextBoxImportUsername
            // 
            this.TextBoxImportUsername.Location = new System.Drawing.Point(73, 19);
            this.TextBoxImportUsername.Name = "TextBoxImportUsername";
            this.TextBoxImportUsername.Size = new System.Drawing.Size(222, 20);
            this.TextBoxImportUsername.TabIndex = 1;
            this.TextBoxImportUsername.Text = "root";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Username:";
            // 
            // TextBoxImportSourceFile
            // 
            this.TextBoxImportSourceFile.Location = new System.Drawing.Point(26, 175);
            this.TextBoxImportSourceFile.Name = "TextBoxImportSourceFile";
            this.TextBoxImportSourceFile.Size = new System.Drawing.Size(589, 20);
            this.TextBoxImportSourceFile.TabIndex = 33;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 159);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(218, 13);
            this.label6.TabIndex = 32;
            this.label6.Text = "Source file location, or Source folder location";
            // 
            // ButtonBeginImport
            // 
            this.ButtonBeginImport.Location = new System.Drawing.Point(24, 255);
            this.ButtonBeginImport.Name = "ButtonBeginImport";
            this.ButtonBeginImport.Size = new System.Drawing.Size(55, 23);
            this.ButtonBeginImport.TabIndex = 31;
            this.ButtonBeginImport.Text = "Start";
            this.ButtonBeginImport.UseVisualStyleBackColor = true;
            // 
            // TextBoxImportAssemblyName
            // 
            this.TextBoxImportAssemblyName.Location = new System.Drawing.Point(25, 39);
            this.TextBoxImportAssemblyName.Name = "TextBoxImportAssemblyName";
            this.TextBoxImportAssemblyName.Size = new System.Drawing.Size(637, 20);
            this.TextBoxImportAssemblyName.TabIndex = 30;
            this.TextBoxImportAssemblyName.Text = "C:\\Program Files (x86)\\MariaDB 10.3\\bin\\mysql.exe";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 23);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(132, 13);
            this.label7.TabIndex = 29;
            this.label7.Text = "MySQL assembly Location";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(23, 214);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(130, 13);
            this.label8.TabIndex = 36;
            this.label8.Text = "Optional (Database Prefix)";
            // 
            // TextBoxNewDatabasePrefix
            // 
            this.TextBoxNewDatabasePrefix.Location = new System.Drawing.Point(25, 230);
            this.TextBoxNewDatabasePrefix.Name = "TextBoxNewDatabasePrefix";
            this.TextBoxNewDatabasePrefix.Size = new System.Drawing.Size(173, 20);
            this.TextBoxNewDatabasePrefix.TabIndex = 37;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(720, 450);
            this.Controls.Add(this.MainTabControl);
            this.Controls.Add(this.StatusStripStatus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.Text = "MySQL Dumper";
            this.StatusStripStatus.ResumeLayout(false);
            this.StatusStripStatus.PerformLayout();
            this.MainTabControl.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip StatusStripStatus;
        public System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        public System.Windows.Forms.TabControl MainTabControl;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        public System.Windows.Forms.TextBox TextBoxMySQLAssemblyLocationDump;
        private System.Windows.Forms.Label label13;
        public System.Windows.Forms.Button ButtonBeginDump;
        public System.Windows.Forms.TextBox TextBoxDumpLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.TextBox TextBoxPasswordDump;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox TextBoxUsernameDump;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.Button ButtonBrowseDumpLocation;
        private System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.TextBox TextBoxCommandPrompt;
        public System.Windows.Forms.Button ButtonBrowseSourceFile;
        private System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.TextBox TextBoxImportPassword;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox TextBoxImportUsername;
        private System.Windows.Forms.Label label5;
        public System.Windows.Forms.TextBox TextBoxImportSourceFile;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.Button ButtonBeginImport;
        public System.Windows.Forms.TextBox TextBoxImportAssemblyName;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox TextBoxNewDatabasePrefix;
        private System.Windows.Forms.Label label8;
    }
}

