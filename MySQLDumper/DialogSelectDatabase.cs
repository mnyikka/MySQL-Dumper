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
    public partial class DialogSelectDatabase : Form
    {
        public DialogSelectDatabase()
        {
            InitializeComponent();
            this.AddListeners();
        }

        /// <summary>
        /// Adds listeners
        /// </summary>
        private void AddListeners()
        {
            this.ButtonAccept.Click += new EventHandler(AllButtons_Click);
            this.ButtonCancel.Click += new EventHandler(AllButtons_Click);
        }

        /// <summary>
        /// On click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AllButtons_Click(object sender, EventArgs e)
        {
            try
            {
                if (sender == this.ButtonAccept)
                {
                    string[] Array = this.GetSelectedItems();
                    if (Array.Length == 0)
                        throw new Exception("Please select at least a database item");
                    this.Visible = false;
                }
                else
                if (sender == this.ButtonCancel)
                {
                    this.checkedListBoxDatabaseListing.Items.Clear();
                    this.Visible = false;
                }
            }
            catch (Exception em)
            {
                StaticMethods.DisplayErrorMessage(this,
                    null,
                    false,
                    em.Message);
            }
            finally
            {
 
            }
        }


        /// <summary>
        /// Selects the databases to use
        /// </summary>
        /// <param name="ListArray"></param>
        /// <param name="MainForm"></param>
        /// <returns></returns>
        public string[] SelectDatabases(string[] ListArray, 
            Form MainForm)
        {
            this.LoadDatabases(ListArray);
            this.ShowDialog(MainForm);
            return this.GetSelectedItems();
        }

        /// <summary>
        /// Only the items selected would be returned
        /// </summary>
        /// <returns>Returns an array</returns>
        private string[] GetSelectedItems()
        {
            List<string> ItemsSelected = new List<string>(this.checkedListBoxDatabaseListing.Items.Count);
            for (int index = 0; index < this.checkedListBoxDatabaseListing.Items.Count; index++)
            {
                if (this.checkedListBoxDatabaseListing.GetItemChecked(index) == true)
                {
                    ItemsSelected.Add(""+this.checkedListBoxDatabaseListing.Items[index]);
                }
            }
            return ItemsSelected.ToArray();
        }

        /// <summary>
        /// Loads all the databases
        /// </summary>
        /// <param name="ListArray">The array to select databases from</param>
        private void LoadDatabases(string[] ListArray)
        {
            this.checkedListBoxDatabaseListing.Items.Clear();
            for (int index = 0; index < ListArray.Length; index++)
            {
                this.checkedListBoxDatabaseListing.Items.Add(ListArray[index],
                    (ListArray[index].Contains("information_schema") == false && ListArray[index].Contains("mysql") == false)  
                    );
            }
        }

        internal void SetLabelText(string Text)
        {
            this.label1.Text = Text;
        }
    }
}
