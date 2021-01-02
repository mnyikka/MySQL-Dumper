using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Security.Cryptography;

namespace DataScraper
{
    public class UIState
    {
        public enum UIStateProperty { Text = 1, Checked = 2, CheckBoxListItems = 3 }
        private SortedList<int, UIState.ControlUIState> SortedListControls;
        public static int CryptOffset = 1288474;
        public static bool USE_ENCRYPTION = true;
        private string AppName;
        private string FilePath;

        /// <summary>
        /// Saves the state of the UI on closing and opening
        /// </summary>
        public UIState(string ApplicationFolderName)
        {
            this.AppName = ApplicationFolderName;
            this.AppName = Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData) + "\\" + this.AppName;
            FilePath = this.AppName + "\\config.data";
            if (Directory.Exists(this.AppName) == false)
            {
                Directory.CreateDirectory(this.AppName);
            }
            this.SortedListControls = new SortedList<int, ControlUIState>(1000);
        }

        public string AppDir()
        {
            return this.AppName;
        }

        /// <summary>
        /// Adds the UI State to the Control
        /// </summary>
        /// <param name="ControlUIState"></param>
        public void AddControlUIState(UIState.ControlUIState ControlUIState)
        {
            if (ControlUIState.LOAD_ORDER != -1)
            {
                this.SortedListControls.Add(ControlUIState.LOAD_ORDER, ControlUIState);
            }
            else
            {
                while (this.SortedListControls.ContainsKey(ControlUIState.LOAD_ORDER))
                {
                    ControlUIState.LOAD_ORDER = ControlUIState.LOAD_ORDER + 1;
                }
                this.SortedListControls.Add(ControlUIState.LOAD_ORDER, ControlUIState);
            }
        }

        /// <summary>
        /// Loads past saved data
        /// </summary>
        public void LoadSavedData()
        {

            string XMLData = "";
            StreamReader StrRead = null;
            try
            {
                StrRead = new StreamReader(this.FilePath);
                StringBuilder ReadData = new StringBuilder(100000);
                XMLData = StrRead.ReadToEnd();
                StrRead.Close();

                //Decode the if we are using encryption
                //Triple DES Encryption
                if (UIState.USE_ENCRYPTION)
                {
                    UIStateEncoder UIEncoder = new UIStateEncoder();
                    UIEncoder.EncryptedPassword = XMLData;
                    XMLData = UIEncoder.DecryptWithByteArray();
                }
            }
            catch
            {
                try { StrRead.Close(); }
                catch { }
            }


            bool DidAllControlsLoad = true;
            for (int index = 0; index < this.SortedListControls.Count; index++)
            {
                ControlUIState ContrUIState = this.SortedListControls[this.SortedListControls.Keys[index]];
                if (ContrUIState.LoadProperty(XMLData, true) == false)
                {
                    DidAllControlsLoad = false;
                    break;
                }
            }

            if (DidAllControlsLoad == false)
            {
                for (int index = 0; index < this.SortedListControls.Count; index++)
                {
                    ControlUIState ContrUIState = this.SortedListControls[this.SortedListControls.Keys[index]];
                    ContrUIState.LoadProperty("", false);
                }
                this.SaveUIData();
                this.LoadSavedData();
            }
            else
            {
                for (int index = 0; index < this.SortedListControls.Count; index++)
                {
                    ControlUIState ContrUIState = this.SortedListControls[this.SortedListControls.Keys[index]];
                    ContrUIState.LoadProperty(XMLData, false);
                }
            }


        }
        /// <summary>
        /// Saves the UI Data
        /// </summary>
        public void SaveUIData()
        {
            StringBuilder StrBuilder = new StringBuilder(100000);
            StringBuilder StrBuilder_Save = new StringBuilder(100000);

            for (int index = 0; index < this.SortedListControls.Count; index++)
            {
                ControlUIState ContrUIState = this.SortedListControls[this.SortedListControls.Keys[index]];
                StrBuilder.Append(ContrUIState.XML_SAVE);
                if (index + 1 < this.SortedListControls.Count)
                {
                    StrBuilder.Append(Environment.NewLine);
                }
            }
            //Encode each character
            if (UIState.USE_ENCRYPTION)
            {
                UIStateEncoder UIEncoder = new UIStateEncoder();
                StrBuilder_Save.Append(UIEncoder.EncryptWithByteArray(StrBuilder.ToString()));
            }
            else
            {
                StrBuilder_Save.Append(StrBuilder.ToString());
            }

            try
            {
                StreamWriter StrWriter = new StreamWriter(this.FilePath);
                StrWriter.Write(StrBuilder_Save.ToString());
                StrWriter.Flush();
                StrWriter.Close();
            }
            catch { }
        }


        public class ControlUIState
        {
            private int load_order;
            private UIStateProperty PropertyToSaveLoad;
            private Control Control;
            private object DefaultValueOnFailLoad;
            private string Description;
            public const string PropertyDescriptionTag = "PropertyDescription";
            public const string ValueTag = "Value";
            public const string ItemTag = "Item";
            public const string NameTag = "Name";
            public ControlUIState(Control Control, UIStateProperty PropertyToSaveLoad, object DefaultValueOnFailLoad, int LoadOrderOrKey, string Description)
            {
                this.Control = Control;
                this.PropertyToSaveLoad = PropertyToSaveLoad;
                this.DefaultValueOnFailLoad = DefaultValueOnFailLoad;
                this.load_order = LoadOrderOrKey;
                this.Description = "" + Description;
            }
            public ControlUIState(Control Control, UIStateProperty PropertyToSaveLoad, object DefaultValueOnFailLoad)
            {
                this.Control = Control;
                this.PropertyToSaveLoad = PropertyToSaveLoad;
                this.DefaultValueOnFailLoad = DefaultValueOnFailLoad;
                this.load_order = -1;
                this.Description = "";
            }
            public int LOAD_ORDER
            {
                get
                {
                    return this.load_order;
                }
                set
                {
                    this.load_order = value;
                }
            }
            /// <summary>
            /// Produces XML to Save this property
            /// </summary>
            public string XML_SAVE
            {
                get
                {
                    string PropertyValue = "";
                    StringBuilder StrBuilder = new StringBuilder(1000);
                    StringBuilder StrBuiXMLBui = new StringBuilder(1000);

                    if (this.PropertyToSaveLoad != UIStateProperty.CheckBoxListItems)
                    {
                        PropertyValue = "" + StaticMethods.GetValueOfPublicPropertyInObject(this.Control, "" + this.PropertyToSaveLoad);
                        StaticMethods.WriteXmlTag(UIState.ControlUIState.PropertyDescriptionTag, this.Description, StrBuiXMLBui, true, true, true);
                        StaticMethods.WriteXmlTag(UIState.ControlUIState.ValueTag, PropertyValue, StrBuiXMLBui, true, true, true);
                        StaticMethods.WriteXmlTag("" + this.LOAD_ORDER, (Environment.NewLine + StrBuiXMLBui.ToString()), StrBuilder, true, true, false);
                    }

                    ///We would get all the items, and check if they are checked
                    if (this.PropertyToSaveLoad == UIStateProperty.CheckBoxListItems)
                    {
                        StaticMethods.WriteXmlTag(UIState.ControlUIState.PropertyDescriptionTag, this.Description, StrBuiXMLBui, true, true, true);
                        StringBuilder ItemStrBuilder = new StringBuilder(1000);
                        //Write each item one by one
                        CheckedListBox CheckLisBox = this.Control as CheckedListBox;
                        for (int index = 0; index < CheckLisBox.Items.Count; index++)
                        {
                            ItemStrBuilder.Remove(0, ItemStrBuilder.Length);
                            StaticMethods.WriteXmlTag(UIState.ControlUIState.ItemTag, "", ItemStrBuilder, true, false, true);
                            StaticMethods.WriteXmlTag(UIState.ControlUIState.NameTag, "" + CheckLisBox.Items[index], ItemStrBuilder, true, true, true);
                            StaticMethods.WriteXmlTag(UIState.ControlUIState.ValueTag, "" + (CheckLisBox.CheckedIndices.Contains(index)), ItemStrBuilder, true, true, true);
                            StaticMethods.WriteXmlTag(UIState.ControlUIState.ItemTag, "", ItemStrBuilder, false, true, false);
                            //Append that to the rest of our XML
                            StrBuiXMLBui.Append(ItemStrBuilder.ToString());
                            if (index + 1 < CheckLisBox.Items.Count)
                            {
                                StrBuiXMLBui.Append(Environment.NewLine);
                            }
                        }
                        StaticMethods.WriteXmlTag("" + this.LOAD_ORDER, (Environment.NewLine + StrBuiXMLBui.ToString()), StrBuilder, true, true, true);
                    }


                    return StrBuilder.ToString().Trim(Environment.NewLine.ToCharArray());
                }
            }


            /// <summary>
            /// Loads the property from an XML Text
            /// </summary>
            /// <param name="XML_Text">The XML Text</param>
            /// <returns>True if the property was found and loaded</returns>
            public bool LoadProperty(string XML_Text, bool TestMode)
            {
                object prop_value = this.DefaultValueOnFailLoad;
                string SourceTag = "";
                bool IfPerfectLoad = true;
                try
                {

                    SourceTag = StaticMethods.GetValueOfTags("" + this.LOAD_ORDER, XML_Text)[0];

                    if (this.PropertyToSaveLoad != UIStateProperty.CheckBoxListItems)
                    {
                        prop_value = StaticMethods.GetValueOfTags(UIState.ControlUIState.ValueTag, SourceTag)[0];
                    }

                    switch (this.PropertyToSaveLoad)
                    {
                        case UIStateProperty.Checked:
                            prop_value = bool.Parse("" + prop_value);
                            if (TestMode == false)
                            {
                                StaticMethods.SetValueOfPublicPropertyInObject(this.Control, "" + this.PropertyToSaveLoad, prop_value);
                            }
                            break;

                        case UIStateProperty.Text:
                            prop_value = StaticMethods.ConvertFromEscapedXMLTags("" + prop_value);
                            if (TestMode == false)
                            {
                                StaticMethods.SetValueOfPublicPropertyInObject(this.Control, "" + this.PropertyToSaveLoad, prop_value);
                            }
                            break;
                        case UIStateProperty.CheckBoxListItems:
                            string[] Items = StaticMethods.GetValueOfTags(UIState.ControlUIState.ItemTag, SourceTag);
                            //For each of the items we must load them onto the CheckBox
                            ArrayList ArrLisNames = new ArrayList(Items.Length);
                            ArrayList ArrLisChecked = new ArrayList(Items.Length);
                            //We must parse the string / and the boolean
                            for (int index = 0; index < Items.Length; index++)
                            {
                                string Item = StaticMethods.ConvertFromEscapedXMLTags(Items[index]);
                                ArrLisNames.Add(StaticMethods.GetValueOfTags(UIState.ControlUIState.NameTag, Item)[0]);
                                ArrLisChecked.Add(bool.Parse(StaticMethods.GetValueOfTags(UIState.ControlUIState.ValueTag, Item)[0]));
                            }

                            if (TestMode == false)
                            {
                                //If we reached upto here, clear all the collection that the checkbox has
                                CheckedListBox CheckLisBox = this.Control as CheckedListBox;
                                CheckLisBox.Items.Clear();
                                //Lets add the items::
                                for (int index = 0; index < ArrLisNames.Count; index++)
                                {
                                    CheckLisBox.Items.Add(ArrLisNames[index], ((bool)ArrLisChecked[index]));
                                }
                            }
                            break;

                    }
                }
                catch
                {
                    IfPerfectLoad = false;
                }

                if ((this.PropertyToSaveLoad == UIStateProperty.Checked || this.PropertyToSaveLoad == UIStateProperty.Text) && TestMode == false)
                {
                    StaticMethods.SetValueOfPublicPropertyInObject(this.Control, "" + this.PropertyToSaveLoad, prop_value);
                }

                return IfPerfectLoad;
            }


        }
    }








    /// <summary>
    /// Provides triple DES Encryption
    /// </summary>
    class UIStateEncoder
    {
        string mEncryptedPassword;
        // Change the two values below to be something other than the example.
        // Once changed and in use, do not change the value below again or you
        // won't be able to decrypt previously stored passwords.
        string mByteArray = "1$#2#%51@ZVas#lPARa0$!y";
        byte[] mInitializationVector = { 0x01, 0x33, 0x56, 0x73, 0x90, 0xAB, 0xf3, 0xEA };

        public UIStateEncoder()
        {
        }

        public UIStateEncoder(string inPassword)
        {
            mEncryptedPassword = EncryptWithByteArray(inPassword, mByteArray);
        }

        public string EncryptWithByteArray(string inPassword)
        {
            mEncryptedPassword = EncryptWithByteArray(inPassword, mByteArray);
            return mEncryptedPassword;
        }

        private string EncryptWithByteArray(string inPassword, string inByteArray)
        {
            try
            {
                byte[] tmpKey = new byte[20];
                tmpKey = System.Text.Encoding.UTF8.GetBytes(inByteArray.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputArray = System.Text.Encoding.UTF8.GetBytes(inPassword);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(tmpKey, mInitializationVector), CryptoStreamMode.Write);
                cs.Write(inputArray, 0, inputArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DecryptWithByteArray()
        {
            return DecryptWithByteArray(mEncryptedPassword, mByteArray);
        }

        private string DecryptWithByteArray(string strText, string strEncrypt)
        {
            try
            {
                byte[] tmpKey = new byte[20];
                tmpKey = System.Text.Encoding.UTF8.GetBytes(strEncrypt.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                Byte[] inputByteArray = inputByteArray = Convert.FromBase64String(strText);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(tmpKey, mInitializationVector), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string EncryptedPassword
        {
            get { return mEncryptedPassword; }
            set { mEncryptedPassword = value; }
        }

        public string ByteArray
        {
            get { return mByteArray; }
            set { mByteArray = value; }
        }
    }




}

