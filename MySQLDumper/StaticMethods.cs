// Type: DataScraper.StaticMethods
// Assembly: DataScraper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\Program Files (x86)\I.T. River Inc\Data Scraper\DataScraper.exe
//using DevExpress.Data;
//using DevExpress.LookAndFeel;
//using DevExpress.XtraBars;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraEditors.Repository;
//using DevExpress.XtraGrid;
//using DevExpress.XtraGrid.Columns;
//using DevExpress.XtraGrid.Views.Base;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraTab;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
//using TabIndexer;
using System.Net.Mail;
using System.Net;
using RenameFileScript;

namespace DataScraper
{
    public static class StaticMethods
    {
        public static string MoneyFormat;
        public static string MoneyFormat3;
        public static string DateTimFormatDDD_MMM_d_yyyy;
        public static string DateTimFormatdddd_MMMM_dd_yyyy_h_mm_ss_tt;
        public static string DataTimFormatMMddyyyyhhmmsstt;
        public static string DateTimeFormatMMddyyyyhhmmssffftt;
        public static DateTime DateTimMinSQLValue;


        static StaticMethods()
        {
            StaticMethods.MoneyFormat = "{0:#,#.##}";
            StaticMethods.MoneyFormat3 = "{0:#,#.###}";
            StaticMethods.DateTimFormatDDD_MMM_d_yyyy = "ddd, MMM d, yyyy";
            StaticMethods.DateTimFormatdddd_MMMM_dd_yyyy_h_mm_ss_tt = "ddd, MMM d, yyyy h:mm:ss tt";
            StaticMethods.DataTimFormatMMddyyyyhhmmsstt = "MM-dd-yyyy hh-mm-ss tt";
            StaticMethods.DateTimeFormatMMddyyyyhhmmssffftt = "MM/dd/yyyy hh:mm:ss.fff tt";
            StaticMethods.DateTimMinSQLValue = new DateTime(1970, 1, 1);
        }


      


        /// <summary>
        /// Calculates the correlation
        /// </summary>
        /// <param name="Xs"></param>
        /// <param name="Ys"></param>
        /// <returns></returns>
        public static Double Correlation(Double[] Xs, Double[] Ys)
        {
            Double sumX = 0;
            Double sumX2 = 0;
            Double sumY = 0;
            Double sumY2 = 0;
            Double sumXY = 0;

            int n = Xs.Length < Ys.Length ? Xs.Length : Ys.Length;

            for (int i = 0; i < n; ++i)
            {
                Double x = Xs[i];
                Double y = Ys[i];

                sumX += x;
                sumX2 += x * x;
                sumY += y;
                sumY2 += y * y;
                sumXY += x * y;
            }

            Double stdX = Math.Sqrt(sumX2 / n - sumX * sumX / n / n);
            Double stdY = Math.Sqrt(sumY2 / n - sumY * sumY / n / n);
            Double covariance = (sumXY / n - sumX * sumY / n / n);

            return covariance / stdX / stdY;
        }



        /// <summary>
        /// Sends an email using C#
        /// </summary>
        /// <param name="Message"></param>
        public static void SendMail(MailMessage Message, string EmailAddress, string Password)
        {
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.googlemail.com";
            if (Message.From == null)
            {
                Message.From = new MailAddress(EmailAddress);
            }
            if (Message.To.Count == 0)
            {
                Message.To.Add(EmailAddress);
                //Message.To.Add(EmailAddress);
            }
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(EmailAddress, Password);
            client.Send(Message);
        }




        /// <summary>
        /// Does a case insensitive replacement for a string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="oldValue"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        public static string ReplaceCaseInsensitive(string str, string oldValue, string newValue)
        {
            int prevPos = 0;
            string retval = str;
            // find the first occurence of oldValue
            int pos = retval.IndexOf(oldValue, StringComparison.InvariantCultureIgnoreCase);
            while (pos > -1)
            {
                // remove oldValue from the string
                retval = str.Remove(pos, oldValue.Length);
                // insert newValue in it's place
                retval = retval.Insert(pos, newValue);
                // check if oldValue is found further down
                prevPos = pos + newValue.Length;
                pos = retval.IndexOf(oldValue, prevPos, StringComparison.InvariantCultureIgnoreCase);
            }
            return retval;
        }


        /// <summary>
        /// Gets the grids selection into the cliboard
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        /*public static string GetGridSelectionForClipboard(GridView view)
        {
            if(view.SelectedRowsCount == 0) return string.Empty;
            const string CellDelimiter = "\t";
            const string LineDelimiter = "\r\n";
            StringBuilder result = new StringBuilder(10000);

            // iterate cells and compose a tab delimited string of cell values
            for(int i = view.SelectedRowsCount - 1; i >= 0; i--) {
                int row = view.GetSelectedRows()[i];
                for(int j = 0; j < view.VisibleColumns.Count; j++) {
                    result.Append( view.GetRowCellDisplayText(row, view.VisibleColumns[j]) );
                    if(j != view.VisibleColumns.Count - 1)
                        result.Append( CellDelimiter );
                }                    
                if(i != 0)
                    result.Append( LineDelimiter );
            }

            return result.ToString();
        }*/

        /// <summary>
        /// Returns the assembly directory
        /// </summary>
        /// <returns></returns>
        public static string GetAssemblyDirectory()
        {
            string Str = Assembly.GetExecutingAssembly().Location;
            return new FileInfo(Str).Directory.FullName;
        }


        /// <summary>
        /// Highlights all the text in the richtexbox
        /// </summary>
        /// <param name="myRtb"></param>
        /// <param name="word"></param>
        /// <param name="color"></param>
        public static void HighlightTextInRichTextBox(RichTextBox myRtb, string word, Color color)
        {
            int s_start = myRtb.SelectionStart, startIndex = 0, index;
            if (word.Trim().Length == 0) return;
            //myRtb.SelectionColor = Color.Black;
            while ((index = myRtb.Text.IndexOf(word, startIndex, StringComparison.OrdinalIgnoreCase)) != -1)
            {
                myRtb.SelectionColor = color;
                myRtb.Select(index, word.Length);
                myRtb.SelectionColor = color;
                startIndex = index + word.Length;
                //myRtb.SelectionColor = Color.Black;
            }
            myRtb.Invalidate();
            myRtb.SelectionStart = s_start;
            myRtb.SelectionLength = 0;
            myRtb.SelectionColor = Color.Black;
        }


        /// <summary>
        /// Splits a quoted string
        /// </summary>
        /// <param name="line"></param>
        /// <param name="delimeter"></param>
        /// <returns></returns>
        public static string[] SplitQuoted(string line, char delimeter)
        {
            string[] array;
            List<string> list = new List<string>();
            do
            {
                if (line.StartsWith("\""))
                {
                    line = line.Substring(1);
                    int idx = line.IndexOf("\"");
                    while (line.IndexOf("\"", idx) == line.IndexOf("\"\"", idx))
                    {
                        idx = line.IndexOf("\"\"", idx) + 2;
                    }
                    idx = line.IndexOf("\"", idx);
                    list.Add(line.Substring(0, idx));
                    line = line.Substring(idx + 2);
                }
                else
                {
                    list.Add(line.Substring(0, Math.Max(line.IndexOf(delimeter), 0)));
                    line = line.Substring(line.IndexOf(delimeter) + 1);
                }
            }
            while (line.IndexOf(delimeter) != -1);
            list.Add(line);
            array = new string[list.Count];
            list.CopyTo(array);
            return array;
        }


        /*public static void InitLookUp(RepositoryItemGridLookUpEdit LookUpEdi, string SqlSel, string DisplayMem, string ValueMem, SqlConnection SqlCon)
        {
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(SqlSel, SqlCon);
            if (LookUpEdi.DataSource == null)
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add(DisplayMem);
                dataTable.Columns.Add(ValueMem);
                LookUpEdi.DisplayMember = DisplayMem;
                LookUpEdi.ValueMember = ValueMem;
                sqlDataAdapter.Fill(dataTable);
                LookUpEdi.DataSource = (object)dataTable;
            }
            else
            {
                DataTable dataTable = (DataTable)LookUpEdi.DataSource;
                if (!dataTable.Columns.Contains(DisplayMem))
                    dataTable.Columns.Add(DisplayMem);
                if (!dataTable.Columns.Contains(ValueMem))
                    dataTable.Columns.Add(ValueMem);
                LookUpEdi.DisplayMember = DisplayMem;
                LookUpEdi.ValueMember = ValueMem;
                dataTable.Clear();
                sqlDataAdapter.Fill(dataTable);
                LookUpEdi.DataSource = (object)dataTable;
            }
        }*/



        public static void KillAllThreads()
        {
            Process.GetCurrentProcess().Kill();
        }



        public static DialogResult DisplayErrorMessage(Form FOR, Exception EXCE, bool SHOW_EXCEPTION, string MESSAGE)
        {
            //MessageBox.AllowCustomLookAndFeel = true;
            ThreadAbortException threadAbortException = EXCE as ThreadAbortException;
            string str = string.Concat((object)EXCE);
            if (threadAbortException != null)
                str = str + (object)"\nThreadAbortException ExceptionState\n" + (string)threadAbortException.ExceptionState;
            if (SHOW_EXCEPTION)
                MESSAGE = MESSAGE + "\n" + str;
            return MessageBox.Show((IWin32Window)FOR, MESSAGE, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        public static DialogResult DisplayErrorMessage(Form FOR, Exception EXCE, bool SHOW_EXCEPTION, string MESSAGE, bool BOO_RETRY_CANCEL)
        {
            //MessageBox.AllowCustomLookAndFeel = true;
            if (SHOW_EXCEPTION)
                MESSAGE = MESSAGE + (object)"\n" + (string)(object)EXCE;
            if (!BOO_RETRY_CANCEL)
                return MessageBox.Show((IWin32Window)FOR, MESSAGE, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            else
                return MessageBox.Show((IWin32Window)FOR, MESSAGE, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
        }

        public static DialogResult DisplayInformationMessage(Form FOR, Exception EXCE, bool SHOW_EXCEPTION, string MESSAGE, bool BOO_RETRY_CANCEL)
        {
            //MessageBox.AllowCustomLookAndFeel = true;
            if (SHOW_EXCEPTION)
                MESSAGE = MESSAGE + (object)"\n" + (string)(object)EXCE;
            if (!BOO_RETRY_CANCEL)
                return MessageBox.Show((IWin32Window)FOR, MESSAGE, "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            else
                return MessageBox.Show((IWin32Window)FOR, MESSAGE, "Success", MessageBoxButtons.RetryCancel, MessageBoxIcon.Asterisk);
        }



        public static void SetControlVisibility(Control CTRL, bool VISIBLE)
        {
            CTRL.Visible = VISIBLE;
        }

        public static DataRow GetCurrentItemAsDataRow(BindingSource BIN_SRC)
        {
            DataRowView dataRowView = BIN_SRC.Current as DataRowView;
            if (dataRowView == null)
                return (DataRow)null;
            else
                return dataRowView.Row;
        }

        public static void CloseAllMdiForms(Form Form1)
        {
            foreach (Form form in Form1.MdiChildren)
                form.Close();
        }

        public static void SetText(Control AControl, string Text)
        {
            AControl.Text = Text;
        }

        public static void SetEnabled(Control AControl, bool Enabled)
        {
            AControl.Enabled = Enabled;
        }







        public static void SetParameterValuesDBNull(SqlCommand sqlCommand)
        {
            for (int index = 0; index < sqlCommand.Parameters.Count; ++index)
            {
                if (sqlCommand.Parameters[index].Value == null)
                    sqlCommand.Parameters[index].Value = (object)DBNull.Value;
            }
        }

        public static string EscapeSpecialCharsLike(string Search)
        {
            char[] chArray = Search.ToCharArray();
            if (chArray.Length == 0)
                return "";
            StringBuilder stringBuilder = new StringBuilder(chArray.Length * 2);
            for (int index = 0; index < chArray.Length; ++index)
            {
                char ch = chArray[index];
                if (ch.Equals('*'))
                {
                    stringBuilder.Append("[");
                    stringBuilder.Append("*");
                    stringBuilder.Append("]");
                }
                else if (ch.Equals('%'))
                {
                    stringBuilder.Append("[");
                    stringBuilder.Append("%");
                    stringBuilder.Append("]");
                }
                else if (ch.Equals('['))
                {
                    stringBuilder.Append("[");
                    stringBuilder.Append("[");
                    stringBuilder.Append("]");
                }
                else if (ch.Equals(']'))
                {
                    stringBuilder.Append("[");
                    stringBuilder.Append("]");
                    stringBuilder.Append("]");
                }
                else if (ch.Equals('\\'))
                    stringBuilder.Append("''");
                else
                    stringBuilder.Append(ch);
            }
            return ((object)stringBuilder).ToString();
        }

        public static void ShowAllColumns(DataTable DAT_TAB)
        {
            for (int index = 0; index < DAT_TAB.Columns.Count; ++index)
            {
                int num = (int)MessageBox.Show(DAT_TAB.Columns[index].ColumnName ?? "");
            }
        }

        public static void ShowAllColumns(DataRow DAT_ROW)
        {
            DataTable table = DAT_ROW.Table;
            StringBuilder stringBuilder = new StringBuilder(1000);
            for (int index = 0; index < table.Columns.Count; ++index)
            {
                stringBuilder.Append("Column name " + table.Columns[index].ColumnName);
                stringBuilder.Append("\n");
                object obj = DAT_ROW[table.Columns[index].ColumnName];
                string str = string.Concat(obj);
                if (obj == DBNull.Value)
                    str = "<DBNull.Value>";
                stringBuilder.Append("Value:\n" + str);
                DialogResult dialogResult = MessageBox.Show(((object)stringBuilder).ToString(), "Values", MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                stringBuilder.Remove(0, stringBuilder.Length);
                if (dialogResult == DialogResult.Cancel)
                    break;
            }
        }

        /*public static void SetColumnVisibility(LookUpColumnInfoCollection LooColInfCol, string[] StrArr)
        {
            int num1 = (int)MessageBox.Show("look up info column count " + (object)LooColInfCol.Count);
            for (int index1 = 0; index1 < LooColInfCol.Count; ++index1)
            {
                string fieldName = LooColInfCol[index1].FieldName;
                int num2 = (int)MessageBox.Show("look up info column name " + fieldName);
                bool flag = false;
                for (int index2 = 0; index2 < StrArr.Length; ++index2)
                {
                    string str = StrArr[index2];
                    int num3 = (int)MessageBox.Show("String array " + str);
                    if (fieldName.Equals(str))
                    {
                        flag = true;
                        int num4 = (int)MessageBox.Show("Equals " + str);
                        break;
                    }
                }
                LooColInfCol[index1].Visible = flag;
            }
        }*/

        public static bool TryRollBack(SqlConnection SqlCon)
        {
            try
            {
                StaticMethods.QuerySingle(SqlCon, "ROLLBACK TRANSACTION");
                return true;
            }
            catch
            {
            }
            return false;
        }

        public static void InitDataSet(ref DataSet DatSet, SqlConnection Sql_Con, string SqlSel)
        {
            if (DatSet == null)
                DatSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(SqlSel, Sql_Con);
            DatSet.Clear();
            ((DataAdapter)sqlDataAdapter).Fill(DatSet);
            Sql_Con.Close();
        }

        public static void InitDataSet(ref DataSet DatSet, SqlConnection Sql_Con, SqlCommand SqlCom)
        {
            if (DatSet == null)
                DatSet = new DataSet();
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(SqlCom);
            DatSet.Clear();
            ((DataAdapter)sqlDataAdapter).Fill(DatSet);
            Sql_Con.Close();
        }

        public static string PunctuateString(string Text)
        {
            StringBuilder stringBuilder = (StringBuilder)null;
            if (Text != null && Text.Length > 0)
            {
                stringBuilder = new StringBuilder(Text.Length * Text.Length);
                string[] strArray = Text.Split(new char[1]
        {
          ' '
        }, StringSplitOptions.RemoveEmptyEntries);
                for (int index = 0; index < strArray.Length; ++index)
                {
                    if (strArray[index].Length == 1)
                        strArray[index] = strArray[index].Substring(0, 1).ToUpper();
                    if (strArray[index].Length > 1)
                        strArray[index] = strArray[index].Substring(0, 1).ToUpper() + strArray[index].Substring(1, strArray[index].Length - 1).ToLower();
                    if (index == 0)
                        stringBuilder.Append(strArray[index]);
                    else
                        stringBuilder.Append(" " + strArray[index]);
                }
            }
            if (stringBuilder != null)
                return ((object)stringBuilder).ToString();
            else
                return "";
        }

        public static DialogResult ShowMessage(IWin32Window Owner, string Message, string Title, MessageBoxButtons EnumButtons, MessageBoxIcon EnumIcons)
        {
            //MessageBox.AllowCustomLookAndFeel = true;
            return MessageBox.Show(Owner, Message, Title, EnumButtons, EnumIcons);
        }

        public static bool IsNameValid(string SampleName, out string ErrorMessage)
        {
            bool flag = true;
            ErrorMessage = (string)null;
            if (SampleName == null)
            {
                ErrorMessage = "Misssing name.";
                return false;
            }
            else
            {
                SampleName = SampleName.Trim();
                if (SampleName.Length == 0)
                {
                    ErrorMessage = "Misssing name.";
                    return false;
                }
                else
                {
                    StringBuilder stringBuilder = new StringBuilder(SampleName.Length);
                    foreach (char c in SampleName.ToCharArray())
                    {
                        if (!char.IsLetter(c) && !object.Equals((object)c, (object)' '))
                        {
                            stringBuilder.Append("\"");
                            stringBuilder.Append(c);
                            stringBuilder.Append("\", ");
                        }
                    }
                    if (stringBuilder.Length > 0)
                    {
                        stringBuilder.Remove(stringBuilder.Length - 2, 2);
                        stringBuilder.Insert(0, "The name " + SampleName + "\nhas some invalid characters,\n");
                        ErrorMessage = ((object)stringBuilder).ToString();
                        return false;
                    }
                    else
                    {
                        string[] strArray = SampleName.Split(new char[1]
            {
              ' '
            }, StringSplitOptions.RemoveEmptyEntries);
                        if (strArray.Length == 3)
                        {
                            if (strArray[0].Length <= 1 || strArray[2].Length <= 1)
                            {
                                ErrorMessage = "Misplaced initial.\nAn initial must be between two words.";
                                return false;
                            }
                        }
                        else if (strArray.Length == 2)
                        {
                            if (strArray[0].Length <= 1 || strArray[1].Length <= 1)
                            {
                                ErrorMessage = "A name with two words\nis not expected to have initials.";
                                return false;
                            }
                        }
                        else if (strArray.Length == 1)
                        {
                            ErrorMessage = "Missing surname.";
                            return false;
                        }
                        else if (strArray.Length > 3)
                        {
                            ErrorMessage = "Only a maximum of 3 words per name\nor two words and an initial between.";
                            return false;
                        }
                        return flag;
                    }
                }
            }
        }

        public static void InitDataTable(ref DataTable DatTab, SqlConnection Sql_Con, SqlCommand SqlSelCom, bool if_show_exception, Form ForToShowError)
        {
            string MESSAGE = "Sorry, errors were encountered while connecting to the database.\nPlease click to retry.";
            while (true)
            {
                bool flag = true;
                Exception EXCE = (Exception)null;
                try
                {
                    if (DatTab == null)
                        DatTab = new DataTable();
                    SqlDataAdapter sqlDataAdapter = new SqlDataAdapter("", Sql_Con);
                    sqlDataAdapter.SelectCommand = SqlSelCom;
                    DatTab.Clear();
                    sqlDataAdapter.Fill(DatTab);
                }
                catch (Exception ex)
                {
                    flag = false;
                    EXCE = ex;
                }
                finally
                {
                    Sql_Con.Close();
                }
                if (!flag)
                {
                    int num = (int)StaticMethods.DisplayErrorMessage(ForToShowError, EXCE, if_show_exception, MESSAGE);
                }
                else
                    break;
            }
        }

        public static void InitDataTable(DataTable DatTab, SqlConnection SQL_CON, object SqlDatAd, bool if_show_exception, Form ForToShowError, object[] args)
        {
            string MESSAGE = "Sorry, errors were encountered while connecting to the database.\nPlease click to retry.";
            while (true)
            {
                bool flag = true;
                Exception EXCE = (Exception)null;
                try
                {
                    System.Type type = SqlDatAd.GetType();
                    PropertyInfo property = type.GetProperty("Connection", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (property == null)
                        throw new Exception("Cannot find a property called Connection in your adapter object");
                    property.SetValue(SqlDatAd, (object)SQL_CON, (object[])null);
                    if (args == null)
                        args = new object[0];
                    object[] args1 = new object[args.Length + 1];
                    args1[0] = (object)DatTab;
                    for (int index = 1; index < args1.Length && index - 1 < args.Length; ++index)
                        args1[index] = args[index - 1];
                    type.InvokeMember("Fill", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, (Binder)null, SqlDatAd, args1);
                }
                catch (Exception ex)
                {
                    flag = false;
                    EXCE = ex;
                }
                finally
                {
                    SQL_CON.Close();
                }
                if (!flag)
                {
                    int num = (int)StaticMethods.DisplayErrorMessage(ForToShowError, EXCE, if_show_exception, MESSAGE);
                }
                else
                    break;
            }
        }

        public static void InitDataSet(DataSet DatSet, SqlConnection SQL_CON, object SqlDatAd, bool if_show_exception, Form ForToShowError, object[] args)
        {
            string MESSAGE = "Sorry, errors were encountered while connecting to the database.\nPlease click to retry.";
            while (true)
            {
                bool flag = true;
                Exception EXCE = (Exception)null;
                try
                {
                    System.Type type = SqlDatAd.GetType();
                    PropertyInfo property = type.GetProperty("Connection", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (property == null)
                        throw new Exception("Cannot find a property called Connection in your adapter object");
                    property.SetValue(SqlDatAd, (object)SQL_CON, (object[])null);
                    if (args == null)
                        args = new object[0];
                    object[] args1 = new object[args.Length + 1];
                    args1[0] = (object)DatSet;
                    for (int index = 1; index < args1.Length && index - 1 < args.Length; ++index)
                        args1[index] = args[index - 1];
                    type.InvokeMember("Fill", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, (Binder)null, SqlDatAd, args1);
                }
                catch (Exception ex)
                {
                    flag = false;
                    EXCE = ex;
                }
                finally
                {
                    SQL_CON.Close();
                }
                if (!flag)
                {
                    int num = (int)StaticMethods.DisplayErrorMessage(ForToShowError, EXCE, if_show_exception, MESSAGE);
                }
                else
                    break;
            }
        }

        public static void InitDataTable(DataTable DatTab, SqlConnection SQL_CON, object SqlDatAd, bool if_show_exception, Form ForToShowError)
        {
            string MESSAGE = "Sorry, errors were encountered while connecting to the database.\nPlease click to retry.";
            while (true)
            {
                bool flag = true;
                Exception EXCE = (Exception)null;
                try
                {
                    System.Type type = SqlDatAd.GetType();
                    PropertyInfo property = type.GetProperty("Connection", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (property == null)
                        throw new Exception("Cannot find a property called Connection in your adapter object");
                    property.SetValue(SqlDatAd, (object)SQL_CON, (object[])null);
                    type.InvokeMember("Fill", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, (Binder)null, SqlDatAd, new object[1]
          {
            (object) DatTab
          });
                }
                catch (Exception ex)
                {
                    flag = false;
                    EXCE = ex;
                }
                finally
                {
                    SQL_CON.Close();
                }
                if (!flag)
                {
                    int num = (int)StaticMethods.DisplayErrorMessage(ForToShowError, EXCE, if_show_exception, MESSAGE);
                }
                else
                    break;
            }
        }

        public static void InitDataTable(DataSet DatSet, SqlConnection SQL_CON, object SqlDatAd, bool if_show_exception, Form ForToShowError)
        {
            string MESSAGE = "Sorry, errors were encountered while connecting to the database.\nPlease click to retry.";
            while (true)
            {
                bool flag = true;
                Exception EXCE = (Exception)null;
                try
                {
                    System.Type type = SqlDatAd.GetType();
                    PropertyInfo property = type.GetProperty("Connection", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                    if (property == null)
                        throw new Exception("Cannot find a property called Connection in your adapter object");
                    property.SetValue(SqlDatAd, (object)SQL_CON, (object[])null);
                    type.InvokeMember("Fill", BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod, (Binder)null, SqlDatAd, new object[1]
          {
            (object) DatSet
          });
                }
                catch (Exception ex)
                {
                    flag = false;
                    EXCE = ex;
                }
                finally
                {
                    SQL_CON.Close();
                }
                if (!flag)
                {
                    int num = (int)StaticMethods.DisplayErrorMessage(ForToShowError, EXCE, if_show_exception, MESSAGE);
                }
                else
                    break;
            }
        }

        public static object QuerySingle(SqlConnection Sql_Con, string Sql)
        {
            object obj = (object)null;
            SqlCommand sqlCommand = new SqlCommand(Sql, Sql_Con);
            if (Sql_Con.State != ConnectionState.Open)
                Sql_Con.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            if (sqlDataReader.Read())
                obj = sqlDataReader.GetValue(0);
            Sql_Con.Close();
            if (obj == null)
                obj = (object)DBNull.Value;
            return obj;
        }

        public static object QuerySingle(SqlConnection Sql_Con, string Sql, bool IfCloseConnection, bool IF_SHOW_EXCEPTION, Form FOR, bool if_error_restart)
        {
            Exception EXCE;
            object obj;
            do
            {
                EXCE = (Exception)null;
                bool flag = false;
                obj = (object)null;
                try
                {
                    SqlCommand sqlCommand = new SqlCommand(Sql, Sql_Con);
                    if (Sql_Con.State != ConnectionState.Open)
                        Sql_Con.Open();
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.Read())
                        obj = sqlDataReader.GetValue(0);
                    sqlDataReader.Close();
                    if (obj == null)
                        obj = (object)DBNull.Value;
                }
                catch (Exception ex)
                {
                    flag = true;
                    EXCE = ex;
                }
                finally
                {
                    if (flag)
                    {
                        if (IfCloseConnection)
                            Sql_Con.Close();
                        if (if_error_restart)
                        {
                            int num = (int)StaticMethods.DisplayErrorMessage(FOR, EXCE, IF_SHOW_EXCEPTION, "Error accessing database.\nPlease retry");
                        }
                    }
                    else if (IfCloseConnection)
                        Sql_Con.Close();
                }
                if (!flag)
                    goto label_21;
            }
            while (if_error_restart);
            throw EXCE;
        label_21:
            return obj;
        }
        /*
        public static void ForceGridCommitCurrentRow(GridControl DevExpGri)
        {
            GridView gridView = DevExpGri.FocusedView as GridView;
            gridView.CloseEditor();
            gridView.UpdateCurrentRow();
            gridView.GridControl.Parent.Focus();
        }
        */
        /*public static void InitLookUpEdit(LookUpEdit LookUp, SqlConnection Sql_Con, string SqlSel, string DispMem, string ValMem, string NullText, bool if_show_exception, Form FormToShowException)
        {
            DataTable dataTable = LookUp.Properties.DataSource as DataTable;
            if (dataTable == null)
            {
                StaticMethods.InitDataTable(ref dataTable, Sql_Con, SqlSel, if_show_exception, FormToShowException);
                LookUp.Properties.DataSource = (object)dataTable;
            }
            else
                StaticMethods.InitDataTable(ref dataTable, Sql_Con, SqlSel, if_show_exception, FormToShowException);
            LookUp.Properties.DisplayMember = DispMem;
            LookUp.Properties.ValueMember = ValMem;
            LookUp.Properties.NullText = NullText;
        }*/
        /*
        public static void SetCheckedListFilterOptionsAndMultiSelect(GridControl GRI_CONT, bool multiselect_property)
        {
            GridControlViewCollection views = GRI_CONT.Views;
            for (int index1 = 0; index1 < views.Count; ++index1)
            {
                GridView gridView = views[index1] as GridView;
                if (gridView != null)
                {
                    gridView.OptionsSelection.MultiSelect = multiselect_property;
                    for (int index2 = 0; index2 < gridView.Columns.Count; ++index2)
                    {
                        GridColumn gridColumn = gridView.Columns[index2];
                        if (gridColumn.UnboundType == UnboundColumnType.Bound)
                            gridColumn.OptionsFilter.FilterPopupMode = FilterPopupMode.CheckedList;
                    }
                }
            }
        }
        */
        /*
        public static void InitLookUpEdit(LookUpEdit LookUp, DataTable DatTab)
        {
            LookUp.Properties.DataSource = (object)DatTab;
        }*/

        /*public static void InitLookUpEdit(LookUpEdit LookUp, DataTable DatTab, string DispMem, string ValMem, string NullText)
        {
            LookUp.Properties.DataSource = (object)DatTab;
            LookUp.Properties.DisplayMember = DispMem;
            LookUp.Properties.ValueMember = ValMem;
            LookUp.Properties.NullText = NullText;
            if (LookUp.Properties.Columns.Count != 0)
                return;
            LookUp.Properties.Columns.Add(new LookUpColumnInfo(DispMem));
            LookUp.Properties.Columns[0].Caption = "";
        }*/

        /*public static void InitLookUpEdit(LookUpEdit LookUp, DataView DatVie, string DispMem, string ValMem, string NullText)
        {
            LookUp.Properties.DataSource = (object)DatVie;
            LookUp.Properties.DisplayMember = DispMem;
            LookUp.Properties.ValueMember = ValMem;
            LookUp.Properties.NullText = NullText;
            if (LookUp.Properties.Columns.Count != 0)
                return;
            LookUp.Properties.Columns.Add(new LookUpColumnInfo(DispMem));
            LookUp.Properties.Columns[0].Caption = "";
        }*/

        public static bool ValidatePayrollCode(string payroll_code, out string ERR)
        {
            ERR = "";
            payroll_code = payroll_code.Trim();
            string[] strArray = payroll_code.Split(new char[1]
      {
        '/'
      });
            if (strArray.Length != 2)
            {
                ERR = "'" + payroll_code + "' is not valid.\nPlease put it in the form xxxx/xxxx";
                return false;
            }
            else
            {
                if (strArray.Length == 2)
                {
                    long result = -1L;
                    if (!long.TryParse(strArray[0], out result))
                    {
                        ERR = "'" + payroll_code + "' is not valid.\nThe first part should be numeric!";
                        return false;
                    }
                    else if (!long.TryParse(strArray[1], out result))
                    {
                        ERR = "'" + payroll_code + "' is not valid.\nThe second part should be numeric!";
                        return false;
                    }
                }
                return true;
            }
        }

        public static SqlCommand GetUpdatingCommand(SqlDataAdapter SqlDatAd, DataRow DAT_ROW)
        {
            if (DAT_ROW.RowState == DataRowState.Unchanged)
                return (SqlCommand)null;
            if (DAT_ROW.RowState == DataRowState.Added)
                return SqlDatAd.InsertCommand;
            if (DAT_ROW.RowState == DataRowState.Deleted)
                return SqlDatAd.DeleteCommand;
            if (DAT_ROW.RowState == DataRowState.Modified)
                return SqlDatAd.UpdateCommand;
            else
                return (SqlCommand)null;
        }

        public static void RestoreDataRowState(DataRowState DAT_ROW_STAT, DataRow DAT_ROW)
        {
            if (DAT_ROW_STAT == DataRowState.Added && DAT_ROW.RowState != DAT_ROW_STAT)
            {
                DAT_ROW.AcceptChanges();
                DAT_ROW.SetAdded();
            }
            else
            {
                if (DAT_ROW_STAT != DataRowState.Modified || DAT_ROW.RowState == DAT_ROW_STAT)
                    return;
                DAT_ROW.AcceptChanges();
                DAT_ROW.SetModified();
            }
        }

        public static string GetErrorValue(SqlCommand SqlCom)
        {
            return string.Concat(SqlCom.Parameters["@ERROR"].Value);
        }

        public static void CloseForm(Form For)
        {
            For.Close();
        }

        public static int AddOnlyOneNewRowBindingSrc(BindingSource BIN_SRC, bool check_if_new_exists)
        {
            DataTable datTabFromBinSrc = StaticMethods.GetDatTabFromBinSrc(BIN_SRC, (string)null);
            DataRow DAT_ROW = (DataRow)null;
            if (!check_if_new_exists)
                DAT_ROW = (BIN_SRC.AddNew() as DataRowView).Row;
            else if (check_if_new_exists)
            {
                DataView dataView = DataTableExtensions.AsDataView(datTabFromBinSrc);
                dataView.RowStateFilter = DataViewRowState.Added;
                if (dataView.Count > 0)
                {
                    int num = datTabFromBinSrc.Rows.IndexOf(dataView[0].Row);
                    BIN_SRC.CurrencyManager.Position = num;
                }
                else
                    DAT_ROW = (BIN_SRC.AddNew() as DataRowView).Row;
            }
            BIN_SRC.EndEdit();
            StaticMethods.SetBindingSourcePosition(DAT_ROW, BIN_SRC);
            return BIN_SRC.CurrencyManager.Position;
        }

        public static int AddOnlyOneNewRowDataTable(DataTable DAT, bool check_if_new_exists, ref DataRow DAT_ROW)
        {
            if (!check_if_new_exists)
            {
                DAT_ROW = DAT.NewRow();
                DAT.Rows.Add(DAT_ROW);
                return DAT.Rows.Count;
            }
            else
            {
                if (check_if_new_exists)
                {
                    DataView dataView = DataTableExtensions.AsDataView(DAT);
                    dataView.RowStateFilter = DataViewRowState.Added;
                    if (dataView.Count == 0)
                    {
                        DAT_ROW = DAT.NewRow();
                        DAT.Rows.Add(DAT_ROW);
                    }
                    else
                        DAT_ROW = dataView[0].Row;
                }
                return DAT.Rows.Count;
            }
        }

        public static int AddOnlyOneNewRowDataTable(DataTable DAT, bool check_if_new_exists)
        {
            if (!check_if_new_exists)
            {
                DAT.Rows.Add(DAT.NewRow());
                return DAT.Rows.Count;
            }
            else
            {
                if (check_if_new_exists)
                {
                    DataView dataView = DataTableExtensions.AsDataView(DAT);
                    dataView.RowStateFilter = DataViewRowState.Added;
                    if (dataView.Count == 0)
                        DAT.Rows.Add(DAT.NewRow());
                }
                return DAT.Rows.Count;
            }
        }

        public static void LoadData(ref DataTable DT, SqlConnection SqlCon, string Sel, bool ClrDatTab)
        {
            if (DT == null)
                DT = new DataTable();
            if (ClrDatTab)
                DT.Clear();
            new SqlDataAdapter(Sel, SqlCon).Fill(DT);
        }

        public static DataTable GetDatTabFromBinSrc(BindingSource BinSrc, string TabNamOpt)
        {
            DataSet dataSet = BinSrc.DataSource as DataSet;
            DataTable dataTable = BinSrc.DataSource as DataTable;
            BindingSource BinSrc1 = BinSrc.DataSource as BindingSource;
            DataView dataView = BinSrc.DataSource as DataView;
            if (BinSrc1 != null)
                return StaticMethods.GetDatTabFromBinSrc(BinSrc1, (string)null);
            if (dataView != null)
                dataTable = dataView.Table;
            if (dataSet != null)
            {
                if (TabNamOpt == null)
                    TabNamOpt = BinSrc.DataMember;
                dataTable = dataSet.Tables[TabNamOpt];
            }
            return dataTable;
        }

        /*public static DataTable DatTabFromGridCtrol(GridControl GRI_CON, string TabNamOpt)
        {
            DataSet dataSet = GRI_CON.DataSource as DataSet;
            DataTable dataTable = GRI_CON.DataSource as DataTable;
            DataView dataView = GRI_CON.DataSource as DataView;
            BindingSource BinSrc = GRI_CON.DataSource as BindingSource;
            if (BinSrc != null)
                return StaticMethods.GetDatTabFromBinSrc(BinSrc, TabNamOpt);
            if (dataTable != null)
                return dataTable;
            if (dataView != null)
                return dataView.Table;
            if (dataSet != null)
                dataTable = dataSet.Tables[TabNamOpt];
            return dataTable;
        }*/

        public static void ShowAllSqlCommandParameterValues(SqlCommand sqlCommand)
        {
            for (int index = 0; index < sqlCommand.Parameters.Count; ++index)
            {
                int num = (int)MessageBox.Show("PARAMETER_NAME " + (object)sqlCommand.Parameters[index].ParameterName + "  VALUE " + (string)sqlCommand.Parameters[index].Value + " SOURCE COLUMN " + sqlCommand.Parameters[index].SourceColumn + " SOURCE VERSION " + (string)(object)sqlCommand.Parameters[index].SourceVersion);
            }
        }

        public static void SetParametersAllowNulls(SqlCommand SqlCom, bool AllowNulls)
        {
            for (int index = 0; index < SqlCom.Parameters.Count; ++index)
                SqlCom.Parameters[index].IsNullable = AllowNulls;
        }

        public static string[] GetDistinctValuesFromColumn(DataTable Source, string DistinctColumn)
        {
            SortedList<string, object> sortedList = new SortedList<string, object>(Source.Rows.Count);
            for (int index = 0; index < Source.DefaultView.Count; ++index)
            {
                string key = string.Concat(Source.DefaultView[index].Row[DistinctColumn]);
                if (!sortedList.ContainsKey(key))
                    sortedList.Add(key, (object)null);
            }
            string[] array = new string[sortedList.Keys.Count];
            sortedList.Keys.CopyTo(array, 0);
            return array;
        }

        public static int CopyDistinctValuesFromColumn(DataTable Source, DataTable CopyTo, string DistinctColumn, string[] arr_othercolumns)
        {
            Source.Rows.Clear();
            SortedList<string, object> sortedList = new SortedList<string, object>(Source.Rows.Count);
            for (int index1 = 0; index1 < Source.Rows.Count; ++index1)
            {
                DataRow dataRow1 = Source.Rows[index1];
                string key = string.Concat(dataRow1[DistinctColumn]);
                if (!sortedList.ContainsKey(key))
                {
                    sortedList.Add(key, (object)null);
                    DataRow dataRow2 = CopyTo.NewRow();
                    dataRow2[DistinctColumn] = dataRow1[DistinctColumn];
                    if (arr_othercolumns != null)
                    {
                        for (int index2 = 0; index2 < arr_othercolumns.Length; ++index2)
                            dataRow2[arr_othercolumns[index2]] = dataRow1[arr_othercolumns[index2]];
                    }
                }
            }
            return CopyTo.Rows.Count;
        }

        public static void SetPositionCenterControl(Control CTRL_BOTTOM, Control CONTROL_UP)
        {
            Form form1 = CTRL_BOTTOM as Form;
            Form form2 = CONTROL_UP as Form;
            Point point1 = Point.Empty;
            Point point2 = Point.Empty;
            Point point3;
            if (form1 == null)
            {
                CTRL_BOTTOM.FindForm();
                point3 = CTRL_BOTTOM.Parent.PointToScreen(CTRL_BOTTOM.Location);
            }
            else
                point3 = form1.DesktopLocation;
            point3 = new Point(CONTROL_UP.Width / 2 + point3.X, CONTROL_UP.Height / 2 + point3.Y);
            if (form2 != null)
                form2.StartPosition = FormStartPosition.Manual;
            CONTROL_UP.Location = point3;
        }

        public static void AdjustControlLocation(Control CTRL, Rectangle RECT_FIT_TO)
        {
            Point pt = CTRL.Location;
            Size size = CTRL.Size;
            bool flag = false;
            if (!RECT_FIT_TO.Contains(pt))
            {
                flag = true;
                pt = new Point(RECT_FIT_TO.X + RECT_FIT_TO.Width / 2 - CTRL.Width / 2, RECT_FIT_TO.Y + RECT_FIT_TO.Height / 2 - CTRL.Height / 2);
            }
            else if (pt.X + size.Width > RECT_FIT_TO.Width)
            {
                flag = true;
                int num = RECT_FIT_TO.Width - (pt.X + size.Width);
                pt.X = pt.X + num;
            }
            else if (pt.Y + size.Height > RECT_FIT_TO.Height)
            {
                flag = true;
                int num = RECT_FIT_TO.Height - (pt.Y + size.Height);
                pt.Y = pt.Y + num;
            }
            if (!flag)
                return;
            CTRL.Location = pt;
        }

        public static DataView GetDataViewFromBindingSrc(BindingSource BinSrc)
        {
            DataView dataView;
            if (BinSrc.Current == null)
            {
                if (BinSrc.Count > 0)
                {
                    BinSrc.CurrencyManager.Position = 0;
                    dataView = ((DataRowView)BinSrc.Current).DataView;
                }
                else
                    dataView = BinSrc.DataSource as DataView ?? StaticMethods.GetDatTabFromBinSrc(BinSrc, (string)null).DefaultView;
            }
            else
                dataView = ((DataRowView)BinSrc.Current).DataView;
            return dataView;
        }

        public static object GetValueOfPublicPropertyInObject(object Object, string PropertyName)
        {
            System.Type type = Object.GetType();
            PropertyInfo property = type.GetProperty(PropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (property != null)
                return property.GetValue(Object, (object[])null);
            throw new Exception(string.Concat(new object[4]
      {
        (object) "Cannot find a property called ",
        (object) PropertyName,
        (object) " in Object of type ",
        (object) type
      }));
        }

        public static void SetValueOfPublicPropertyInObject(object Object, string PropertyName, object Value)
        {
            System.Type type = Object.GetType();
            PropertyInfo property = type.GetProperty(PropertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (property == null)
                throw new Exception(string.Concat(new object[4]
        {
          (object) "Cannot find a property called ",
          (object) PropertyName,
          (object) " in Object of type ",
          (object) type
        }));
            else
                property.SetValue(Object, Value, (object[])null);
        }

        public static void SetParameters(SqlCommand SqlCom, DataRow DAT_ROW)
        {
            DataColumnCollection columns = DAT_ROW.Table.Columns;
            for (int index = 0; index < SqlCom.Parameters.Count; ++index)
            {
                string sourceColumn = SqlCom.Parameters[index].SourceColumn;
                if (columns.Contains(sourceColumn))
                    SqlCom.Parameters[index].Value = DAT_ROW[sourceColumn];
            }
        }

        public static void SetParametersSourceColumnNull(SqlCommand SqlCom, string ColParName)
        {
            SqlCom.Parameters[ColParName].SourceColumn = (string)null;
        }

        public static void SetParametersSourceColumnName(SqlCommand SqlCom, string ColParName, string SourceColumnName)
        {
            SqlCom.Parameters[ColParName].SourceColumn = SourceColumnName;
        }

        public static bool DisplayNotification(Form For, DataTable DAT_TAB, bool show_notification_if_no_data_ONLY)
        {
            int count = DAT_TAB.Rows.Count;
            //MessageBox.AllowCustomLookAndFeel = true;
            if (count == 0)
            {
                int num1 = (int)MessageBox.Show((IWin32Window)For, "No records were found.", "No records.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (count > 0 && !show_notification_if_no_data_ONLY)
            {
                int num2 = (int)MessageBox.Show((IWin32Window)For, (string)(object)count + (object)" records loaded.", (string)(object)count + (object)" records loaded.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return count > 0;
        }

        public static bool DisplayNotification(Form For, DataTable DAT_TAB, bool show_notification_if_no_data_ONLY, string NotificationMessage)
        {
            int count = DAT_TAB.Rows.Count;
            //MessageBox.AllowCustomLookAndFeel = true;
            if (count == 0)
            {
                int num1 = (int)MessageBox.Show((IWin32Window)For, NotificationMessage, "No records.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (count > 0 && !show_notification_if_no_data_ONLY)
            {
                int num2 = (int)MessageBox.Show((IWin32Window)For, (string)(object)count + (object)" records loaded.", (string)(object)count + (object)" records loaded.", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            return count > 0;
        }

        /*public static void DisableDatabaseOperations(DataNavigator Dat)
        {
            Dat.Buttons.Append.Visible = false;
            Dat.Buttons.Remove.Visible = false;
            Dat.Buttons.EndEdit.Visible = false;
            Dat.Buttons.CancelEdit.Visible = false;
        }*/

        public static void ShowSuccessMessage(Form FormObj)
        {
            //MessageBox.AllowCustomLookAndFeel = true;
            int num = (int)MessageBox.Show((IWin32Window)FormObj, "Update successfull", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public static void ShowSuccessMessage(Form FormObj, string Activity)
        {
            //MessageBox.AllowCustomLookAndFeel = true;
            int num = (int)MessageBox.Show((IWin32Window)FormObj, Activity + "", "Success", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        public static void InitTransaction(SqlCommand SqlCom, SqlConnection SqlCon, ref SqlTransaction SqlTran)
        {
            SqlCom.Connection = SqlCon;
            if (SqlCon.State == ConnectionState.Closed || SqlCon.State == ConnectionState.Broken)
                SqlCon.Open();
            if (SqlTran == null)
                SqlTran = SqlCon.BeginTransaction();
            SqlCom.Transaction = SqlTran;
        }
        /*
        public static void ExpandAllDetails(GridView view)
        {
            view.ExpandAllGroups();
        }
        */

        public static void InitDataTable(DataTable dataTable, SqlConnection sqlConnection, string p, bool if_show_exception, Form ForToShowError)
        {
            SqlCommand sql_Com = new SqlCommand(p, sqlConnection);
            StaticMethods.InitDataTable(dataTable, sqlConnection, sql_Com, if_show_exception, ForToShowError);
        }

        public static void InitDataTable(DataTable dataTable, SqlConnection sqlConnection, SqlCommand sql_Com, bool if_show_exception, Form ForToShowError)
        {
            if (dataTable == null)
                throw new Exception("Datatable cannot be null");
            StaticMethods.InitDataTable(ref dataTable, sqlConnection, sql_Com, if_show_exception, ForToShowError);
        }

        /// <summary>
        /// Splits a csv line
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string[] SplitCsvLineToArray(string TheString)
        {
            return StaticMethods.SplitQuoted(TheString, ',');
        }

        /// <summary>
        /// Splits a csv line
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static List<string> SplitCsvLine(string TheString)
        {
            List<string> NewString = new List<string>(100);
            string[] TheStringArray = StaticMethods.SplitQuoted(TheString, ',');
            for (int index = 0; index < TheStringArray.Length; index++)
            {
                NewString.Add(TheStringArray[index]);
            }
            return NewString;
        }


        /// <summary>
        /// Plays a .wav sound
        /// </summary>
        /// <param name="FilePath"></param>
        public static void PlaySound(string FilePath)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer(FilePath);
            player.Play();
        }

        public static object ExecuteWithInvoke(Control AControl, Delegate DelegateMethod, object[] Parameters)
        {
            return !AControl.InvokeRequired ? DelegateMethod.DynamicInvoke(Parameters) : (Parameters != null ? AControl.Invoke(DelegateMethod, Parameters) : AControl.Invoke(DelegateMethod));
        }

        /*public static void SetGroupBoxLookAndFeelDifferent(Control Ctrl_src, UserLookAndFeel LookAndFeel)
        {
            for (int index = 0; index < Ctrl_src.Controls.Count; ++index)
            {
                Control Ctrl_src1 = Ctrl_src.Controls[index];
                if (Ctrl_src1.Controls.Count > 0)
                    StaticMethods.SetGroupBoxLookAndFeelDifferent(Ctrl_src1, LookAndFeel);
                GroupControl groupControl = Ctrl_src1 as GroupControl;
                if (groupControl != null)
                {
                    groupControl.LookAndFeel.Assign(LookAndFeel);
                    groupControl.LookAndFeel.UseDefaultLookAndFeel = false;
                    groupControl.LookAndFeel.UseWindowsXPTheme = false;
                }
            }
        }*/

        /*public static void InitLookUpEdit(LookUpEdit lookUpEdit, object datasource)
        {
            lookUpEdit.Properties.DataSource = datasource;
        }*/

        /*public static void ResizeFormWithHeightLayout(Control ImmCont, Control TheForm, int IntHeightApart, int OffsetBetweenImmediateControlAndForm)
        {
            SortedList<int, Control> sortedList = new SortedList<int, Control>(ImmCont.Controls.Count);
            for (int index = 0; index < ImmCont.Controls.Count; ++index)
            {
                if (sortedList.ContainsKey(ImmCont.Controls[index].TabIndex))
                    throw new Exception("Unable to arrange controls, tab indexes must be unique!!!!");
                sortedList.Add(ImmCont.Controls[index].TabIndex, ImmCont.Controls[index]);
            }
            int num1 = 0;
            for (int index = 0; index < sortedList.Count; ++index)
            {
                Control control1 = sortedList[sortedList.Keys[index]];
                GroupBox groupBox = sortedList[sortedList.Keys[index]] as GroupBox;
                GroupControl groupControl = sortedList[sortedList.Keys[index]] as GroupControl;
                if (groupBox == null ? (groupControl == null ? control1.Visible : groupControl.Visible) : groupBox.Visible)
                {
                    int y = IntHeightApart + num1;
                    Point location = control1.Location;
                    int x1 = location.X;
                    location = control1.Location;
                    int x2 = location.X;
                    int num2;
                    if (x1 == x2)
                    {
                        location = control1.Location;
                        num2 = location.Y == y ? 1 : 0;
                    }
                    else
                        num2 = 0;
                    if (num2 == 0)
                    {
                        Control control2 = control1;
                        location = control1.Location;
                        Point point = new Point(location.X, y);
                        control2.Location = point;
                    }
                    num1 = y + control1.Height;
                }
            }
            if (ImmCont.Height != num1)
                ImmCont.Height = num1;
            if (TheForm.Height == num1 + OffsetBetweenImmediateControlAndForm)
                return;
            TheForm.Height = num1 + OffsetBetweenImmediateControlAndForm;
        }*/



        public static string GetAsSqlDateTime(DateTime DAT_TIM)
        {
            return DAT_TIM.ToString("yyyy-MM-dd hh:mm:ss tt");
        }

        public static void SetControlVisibleIfNot(Control CTRL, bool if_visible)
        {
            if (CTRL.Visible == if_visible)
                return;
            CTRL.Visible = if_visible;
        }

        public static bool IsRowCompletelyBlank(DataRow DAT_ROW, bool boo_if_trim_string)
        {
            for (int index = 0; index < DAT_ROW.Table.Columns.Count; ++index)
            {
                object obj = DAT_ROW[index];
                if (obj != null && obj != DBNull.Value)
                {
                    if (boo_if_trim_string)
                    {
                        if (string.Concat(obj).Trim().Length == 0)
                            continue;
                    }
                    else if (string.Concat(obj).Length == 0)
                        continue;
                    return false;
                }
            }
            return true;
        }

        public static void CloseAllMdiFormsExceptForm(Form1 ParentForm, Form AForm)
        {
            Form[] mdiChildren = ParentForm.MdiChildren;
            for (int index = 0; index < mdiChildren.Length; ++index)
            {
                if (AForm != mdiChildren[index])
                    mdiChildren[index].Close();
            }
        }

        public static void ShowHideControls(Control[] CTRL_ARR, bool ifVisible)
        {
            for (int index = 0; index < CTRL_ARR.Length; ++index)
                CTRL_ARR[index].Visible = ifVisible;
        }

        public static void InitializeStoredProcedureFully(SqlCommand SqlCom, string str_pro_txt, bool add_src_columns)
        {
            str_pro_txt = str_pro_txt.Trim();
            str_pro_txt = str_pro_txt.Replace('\t', ' ');
            str_pro_txt = str_pro_txt.Replace('\n', ' ');
            str_pro_txt = str_pro_txt.Replace('\r', ' ');
            if (!str_pro_txt.ToUpper().StartsWith("EXEC") && !str_pro_txt.ToUpper().StartsWith("EXECUTE"))
                throw new Exception("Error initializing procedure, your string must start with either EXEC or EXECUTE ");
            string[] strArray1 = str_pro_txt.Split(new char[1]
      {
        '@'
      }, StringSplitOptions.RemoveEmptyEntries);
            string str1 = strArray1[0].Trim() + " ";
            string[] strArray2 = (string[])null;
            string[] strArray3 = (string[])null;
            string[] strArray4 = (string[])null;
            string[] strArray5 = (string[])null;
            bool flag = false;
            if (strArray1.Length > 1)
            {
                flag = true;
                strArray2 = new string[strArray1.Length - 1];
                strArray3 = new string[strArray2.Length];
                strArray4 = new string[strArray2.Length];
                strArray5 = new string[strArray2.Length];
                for (int index = 1; index < strArray1.Length; ++index)
                {
                    string[] strArray6 = strArray1[index].Split(new char[1]
          {
            ' '
          }, StringSplitOptions.RemoveEmptyEntries);
                    if (strArray6.Length < 2)
                        throw new Exception("Sorry a datatype was expected for the parameter name @" + strArray6[0]);
                    strArray5[index - 1] = strArray6[0];
                    strArray2[index - 1] = "@" + strArray6[0];
                    strArray3[index - 1] = strArray6[1].Replace(',', ' ').Trim();
                    if (strArray6.Length == 2)
                        strArray4[index - 1] = "IN";
                    else if (strArray6.Length == 3)
                        strArray4[index - 1] = strArray6[2].Replace(',', ' ').Trim();
                    else if (strArray6.Length > 3)
                        throw new Exception("Unable to understand the Sql text near the parameter named @" + strArray6[0]);
                }
            }
            SqlCom.CommandText = "";
            SqlCom.Parameters.Clear();
            if (!flag)
            {
                SqlCom.CommandText = str1;
            }
            else
            {
                SqlCom.CommandText = str1;
                for (int index = 0; index < strArray2.Length; ++index)
                {
                    SqlCom.CommandText = SqlCom.CommandText + " " + strArray2[index];
                    if (!strArray4[index].Equals("IN"))
                        SqlCom.CommandText = SqlCom.CommandText + " " + strArray4[index];
                    if (index + 1 < strArray2.Length)
                        SqlCom.CommandText = SqlCom.CommandText + ", ";
                    else
                        SqlCom.CommandText = SqlCom.CommandText ?? "";
                }
                for (int index = 0; index < strArray2.Length; ++index)
                {
                    string sourceColumn = (string)null;
                    int result = 0;
                    string parameterName = strArray2[index];
                    if (add_src_columns)
                        sourceColumn = strArray5[index];
                    string str2 = strArray4[index].ToUpper();
                    ParameterDirection parameterDirection;
                    switch (str2)
                    {
                        case "OUT":
                            parameterDirection = ParameterDirection.InputOutput;
                            break;
                        case "OUTPUT":
                            parameterDirection = ParameterDirection.Output;
                            break;
                        case "IN":
                            parameterDirection = ParameterDirection.Input;
                            break;
                        default:
                            throw new Exception("Unable to understand parameter direction " + str2 + " for parameter name " + strArray2[index]);
                    }
                    string[] strArray6 = strArray3[index].ToUpper().Split(new char[1]
          {
            '('
          });
                    string str3 = strArray6[0];
                    if (strArray6.Length > 1)
                    {
                        strArray6[1] = strArray6[1].Replace(')', ' ').Trim();
                        strArray6[1] = strArray6[1].Split(new char[2]
            {
              ',',
              ' '
            }, StringSplitOptions.RemoveEmptyEntries)[0];
                        if (!int.TryParse(strArray6[1].Trim(), out result))
                            throw new Exception("Unable to determine the length of parameter name " + strArray2[index] + " of a datatype " + str3);
                    }
                    SqlDbType dbType;
                    switch (str3)
                    {
                        case "VARCHAR":
                            dbType = SqlDbType.VarChar;
                            break;
                        case "NVARCHAR":
                            dbType = SqlDbType.NVarChar;
                            break;
                        case "BIGINT":
                            dbType = SqlDbType.BigInt;
                            break;
                        case "VARBINARY":
                            dbType = SqlDbType.Binary;
                            break;
                        case "BIT":
                            dbType = SqlDbType.Bit;
                            break;
                        case "DATE":
                            dbType = SqlDbType.Date;
                            break;
                        case "CHAR":
                            dbType = SqlDbType.Char;
                            break;
                        case "DATETIME":
                            dbType = SqlDbType.DateTime;
                            break;
                        case "DATETIME2":
                            dbType = SqlDbType.DateTime2;
                            break;
                        case "NUMERIC":
                            dbType = SqlDbType.Real;
                            break;
                        case "INT":
                            dbType = SqlDbType.Int;
                            break;
                        case "SMALLINT":
                            dbType = SqlDbType.SmallInt;
                            break;
                        case "SMALLMONEY":
                            dbType = SqlDbType.SmallMoney;
                            break;
                        case "TEXT":
                            dbType = SqlDbType.Text;
                            break;
                        case "TIME":
                            dbType = SqlDbType.Time;
                            break;
                        case "TIMESTAMP":
                            dbType = SqlDbType.Timestamp;
                            break;
                        case "TINYINT":
                            dbType = SqlDbType.TinyInt;
                            break;
                        case "XML":
                            dbType = SqlDbType.Xml;
                            break;
                        default:
                            throw new Exception("Unable to understand parameter type " + str3 + " for parameter name " + strArray2[index]);
                    }
                    SqlParameter sqlParameter = !add_src_columns ? new SqlParameter(parameterName, dbType, result) : new SqlParameter(parameterName, dbType, result, sourceColumn);
                    sqlParameter.Direction = parameterDirection;
                    SqlCom.Parameters.Add(sqlParameter);
                }
            }
        }

        public static void InitializeStoredProcedureFully(SqlCommand SqlCom, string str_pro_txt, string[] ArrExcludeSrcColumns)
        {
            StaticMethods.InitializeStoredProcedureFully(SqlCom, str_pro_txt, true);
            if (ArrExcludeSrcColumns == null)
                return;
            for (int index = 0; index < ArrExcludeSrcColumns.Length; ++index)
                SqlCom.Parameters[ArrExcludeSrcColumns[index]].SourceColumn = (string)null;
        }

        public static void WriteXmlTag(string XmlTagName, object Value, StreamWriter StreamWriter, bool start_tag, bool close_tag, bool skip_line, bool if_close_stream)
        {
            if (start_tag)
                StreamWriter.Write("<" + XmlTagName + ">");
            if (Value != null)
            {
                Value = (object)string.Concat(Value).Replace("<", "&lt;").Replace(">", "&gt;");
                StreamWriter.Write(string.Concat(Value));
            }
            if (close_tag)
                StreamWriter.Write("</" + XmlTagName + ">");
            if (skip_line)
                StreamWriter.WriteLine();
            if (!if_close_stream)
                return;
            StreamWriter.Close();
        }

        public static void WriteXmlTag(string XmlTagName, object Value, StringBuilder StringBuild, bool start_tag, bool close_tag, bool skip_line)
        {
            if (start_tag)
                StringBuild.Append("<" + XmlTagName + ">");
            if (Value != null)
                StringBuild.Append(string.Concat(Value));
            if (close_tag)
                StringBuild.Append("</" + XmlTagName + ">");
            if (!skip_line)
                return;
            StringBuild.Append(Environment.NewLine);
        }

        public static void SetSqlCommandParameterValue(SqlCommand SqlCom, string ParName, object value)
        {
            if (value == null)
                SqlCom.Parameters[ParName].Value = (object)DBNull.Value;
            else
                SqlCom.Parameters[ParName].Value = value;
        }

        public static object GetSqlCommandParameterValue(SqlCommand SqlCom, string ParName, object NullValue)
        {
            if (SqlCom.Parameters[ParName].Value == DBNull.Value || SqlCom.Parameters[ParName].Value == null)
                return NullValue;
            else
                return SqlCom.Parameters[ParName].Value;
        }

        public static void ResetSqlParameterValuesToDBNull(SqlCommand SqlCom, string[] ArrayExclude)
        {
            if (ArrayExclude == null)
            {
                for (int index = 0; index < SqlCom.Parameters.Count; ++index)
                    SqlCom.Parameters[index].Value = (object)DBNull.Value;
            }
            else
            {
                for (int index1 = 0; index1 < SqlCom.Parameters.Count; ++index1)
                {
                    string parameterName = SqlCom.Parameters[index1].ParameterName;
                    bool flag = false;
                    for (int index2 = 0; index2 < ArrayExclude.Length; ++index2)
                    {
                        if (parameterName.Equals(ArrayExclude[index2]))
                            flag = true;
                    }
                    if (!flag)
                        SqlCom.Parameters[index1].Value = (object)DBNull.Value;
                }
            }
        }

        public static void SetControlEnabledIfNot(Control CTRL, bool p)
        {
            if (CTRL.Enabled == p)
                return;
            CTRL.Enabled = p;
        }

        public static ToolStripItem GetNextToolStripItem(ToolStripItem ToolStripItem, bool forward_seek)
        {
            ToolStrip owner = ToolStripItem.Owner;
            int num = owner.Items.IndexOf(ToolStripItem);
            int count = owner.Items.Count;
            if (forward_seek)
            {
                if (num + 1 < count)
                    return owner.Items[num + 1];
            }
            else if (!forward_seek && num > 0)
                return owner.Items[num - 1];
            return (ToolStripItem)null;
        }

        public static void InitDataTable(ref DataTable dataTable, SqlConnection sqlConnection, string SqlString, bool if_show_exception, Form FormToShowError)
        {
            if (dataTable == null)
                dataTable = new DataTable();
            StaticMethods.InitDataTable(dataTable, sqlConnection, SqlString, if_show_exception, FormToShowError);
        }



        /*public static DataRow GetDataRowFromHandle(GridControl GRI_CONT, int ROW_HANDLE)
        {
            DataRow dataRow = (DataRow)null;
            DataRowView dataRowView = GRI_CONT.FocusedView.GetRow(ROW_HANDLE) as DataRowView;
            if (dataRowView != null)
                dataRow = dataRowView.Row;
            return dataRow;
        }*/
        /*
        public static DataRow GetFocusedDataRowFromHandle(GridControl GRI_CONT)
        {
            GridView gridView = GRI_CONT.MainView as GridView;
            return StaticMethods.GetDataRowFromHandle(GRI_CONT, gridView.FocusedRowHandle);
        }
        */
        public static bool ExecuteCommandGetErrorValue(SqlCommand SqlCom, string ErrrParName, string SuccessParName, out string ErrorReturned, Form FOR, bool IF_SHOW_EXCEPTION, bool if_close_connection)
        {
            bool flag1;
            while (true)
            {
                Exception EXCE = (Exception)null;
                bool flag2 = false;
                flag1 = false;
                ErrorReturned = (string)null;
                try
                {
                    if (SqlCom.Connection.State != ConnectionState.Open)
                        SqlCom.Connection.Open();
                    SqlCom.ExecuteNonQuery();
                    ErrorReturned = (string)null;
                    flag1 = (bool)SqlCom.Parameters[SuccessParName].Value;
                    ErrorReturned = (string)StaticMethods.GetSqlCommandParameterValue(SqlCom, ErrrParName, (object)null);
                }
                catch (Exception ex)
                {
                    flag2 = true;
                    EXCE = ex;
                }
                finally
                {
                    if (if_close_connection)
                        SqlCom.Connection.Close();
                }
                if (flag2)
                {
                    int num = (int)StaticMethods.DisplayErrorMessage(FOR, EXCE, IF_SHOW_EXCEPTION, "Sorry, errors occured while accessing database.\nPlease click to retry.");
                }
                else
                    break;
            }
            return flag1;
        }

        public static object[] ExecuteCommandReturnParameterValue(SqlCommand SqlCom, bool if_close_connection, bool IF_SHOW_EXCEPTION, Form FOR, string[] ParNames, bool RethrowException)
        {
            Exception EXCE;
            bool flag;
            while (true)
            {
                EXCE = (Exception)null;
                flag = false;
                try
                {
                    if (SqlCom.Connection.State != ConnectionState.Open)
                        SqlCom.Connection.Open();
                    SqlCom.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    flag = true;
                    EXCE = ex;
                }
                finally
                {
                    if (if_close_connection)
                        SqlCom.Connection.Close();
                }
                if (flag && !RethrowException)
                {
                    int num = (int)StaticMethods.DisplayErrorMessage(FOR, EXCE, IF_SHOW_EXCEPTION, "Sorry, errors occured while accessing database.\nPlease click to retry.");
                }
                else
                    break;
            }
            if (flag && RethrowException)
                throw EXCE;
            if (ParNames == null)
                return (object[])null;
            object[] objArray = new object[ParNames.Length];
            for (int index = 0; index < ParNames.Length; ++index)
                objArray[index] = SqlCom.Parameters[ParNames[index]].Value;
            return objArray;
        }
        /*
        public static void SetFocusedRowHandle(DataRow DAT_ROW, GridControl gridControl, bool RefreshImmediately, string ColNameToComp)
        {
            GridView gridView = (GridView)gridControl.FocusedView;
            DataRow dataRow1 = gridView.GetDataRow(gridView.FocusedRowHandle);
            if (dataRow1 == DAT_ROW)
                return;
            int rowHandle1 = gridView.RowCount - 1;
            string str1 = "";
            string str2 = "";
            if (dataRow1 != null)
            {
                str1 = string.Concat(DAT_ROW[ColNameToComp]);
                string str3 = string.Concat(dataRow1[ColNameToComp]);
                str2 = "";
                if (str1.Equals(str3))
                    return;
            }
            for (int rowHandle2 = 0; rowHandle2 < gridView.DataRowCount; ++rowHandle2)
            {
                DataRow dataRow2 = gridView.GetDataRow(rowHandle2);
                if (dataRow2 != null && string.Concat(dataRow2[ColNameToComp]).Equals(str1))
                {
                    gridView.FocusedRowHandle = rowHandle2;
                    break;
                }
                else
                {
                    DataRow dataRow3 = gridView.GetDataRow(rowHandle1);
                    if (dataRow3 != null && string.Concat(dataRow3[ColNameToComp]).Equals(str1))
                    {
                        gridView.FocusedRowHandle = rowHandle1;
                        break;
                    }
                    else
                        --rowHandle1;
                }
            }
            if (!RefreshImmediately)
                return;
            gridView.GridControl.Refresh();
        }
        */

        public static object InitializeIfNull(object oBJECT, System.Type TYP_OBJ)
        {
            if (oBJECT == null)
            {
                System.Type type = TYP_OBJ;
                ConstructorInfo constructor = type.GetConstructor(new System.Type[0]);
                if (constructor == null)
                    throw new Exception("Objects of type " + type.ToString() + " do not have a parameterless constructor defined");
                oBJECT = constructor.Invoke((object[])null);
            }
            return oBJECT;
        }
        /*
        public static DataRow[] GetSelectedRowsAsDataRows(GridControl GRI_CON)
        {
            GridView gridView = GRI_CON.FocusedView as GridView;
            ArrayList arrayList = new ArrayList(gridView.SelectedRowsCount);
            int[] selectedRows = gridView.GetSelectedRows();
            for (int index = 0; index < selectedRows.Length; ++index)
            {
                if (selectedRows[index] >= 0)
                {
                    DataRow dataRow = gridView.GetDataRow(selectedRows[index]);
                    if (dataRow != null)
                        arrayList.Add((object)dataRow);
                }
            }
            DataRow[] dataRowArray = new DataRow[arrayList.Count];
            arrayList.CopyTo((Array)dataRowArray, 0);
            return dataRowArray;
        }*/

        public static DataRow[] GetSelectedRowsAsDataRows(DataGridView GRI_VIE, bool if_return_current_row_if_no_selection)
        {
            ArrayList arrayList = new ArrayList(GRI_VIE.SelectedRows.Count);
            for (int index = 0; index < GRI_VIE.SelectedRows.Count; ++index)
                arrayList.Add((object)StaticMethods.GetDataGridViewAsDataRow(GRI_VIE.SelectedRows[index]));
            if (GRI_VIE.SelectedRows.Count == 0 && GRI_VIE.CurrentRow != null && if_return_current_row_if_no_selection)
            {
                return new DataRow[1]
        {
          StaticMethods.GetDataGridViewAsDataRow(GRI_VIE.CurrentRow)
        };
            }
            else
            {
                DataRow[] dataRowArray = new DataRow[arrayList.Count];
                arrayList.CopyTo((Array)dataRowArray, 0);
                return dataRowArray;
            }
        }

        public static DataRow[] GetAllRowsIntoArray(DataTable DAT_TAB)
        {
            ArrayList arrayList = new ArrayList(DAT_TAB.Rows.Count);
            for (int index = 0; index < DAT_TAB.Rows.Count; ++index)
                arrayList.Add((object)DAT_TAB.Rows[index]);
            DataRow[] dataRowArray = new DataRow[arrayList.Count];
            arrayList.CopyTo((Array)dataRowArray, 0);
            return dataRowArray;
        }

        /*public static object GetColumnValueFromRowHandle(string FieldName, int RowHand, GridControl GRI_CONT)
        {
            DataRow dataRow = ((ColumnView)GRI_CONT.FocusedView).GetDataRow(RowHand);
            if (dataRow == null)
                return (object)null;
            else
                return dataRow[FieldName];
        }*/

        

        public static Control[] GetAllControlsBoundToBindingSrc(Control CTRL, BindingSource BIN_SRC)
        {
            ArrayList ARR_LIS = new ArrayList(400);
            StaticMethods.AddControlsBoundToList(ARR_LIS, CTRL, BIN_SRC);
            Control[] controlArray = new Control[ARR_LIS.Count];
            ARR_LIS.CopyTo((Array)controlArray, 0);
            return controlArray;
        }

        public static Control[] GetAllControls(Control FOR_PARENT)
        {
            ArrayList ARR_LIS = new ArrayList(250);
            for (int index = 0; index < FOR_PARENT.Controls.Count; ++index)
                StaticMethods.AddControlsFromControl(ARR_LIS, FOR_PARENT.Controls[index]);
            Control[] controlArray = new Control[ARR_LIS.Count];
            ARR_LIS.CopyTo((Array)controlArray, 0);
            return controlArray;
        }

        public static void AddControlsFromControl(ArrayList ARR_LIS, Control CTRL)
        {
            ARR_LIS.Add((object)CTRL);
            for (int index = 0; index < CTRL.Controls.Count; ++index)
            {
                Control CTRL1 = CTRL.Controls[index];
                if (CTRL1.Controls.Count > 0)
                    StaticMethods.AddControlsFromControl(ARR_LIS, CTRL1);
                else
                    ARR_LIS.Add((object)CTRL1);
            }
        }

        public static void AddControlsBoundToList(ArrayList ARR_LIS, Control CTRL, BindingSource BIN_SRC)
        {
            if (StaticMethods.IsControlBoundToBinding(CTRL, BIN_SRC) && !ARR_LIS.Contains((object)CTRL))
                ARR_LIS.Add((object)CTRL);
            for (int index = 0; index < CTRL.Controls.Count; ++index)
            {
                Control CTRL1 = CTRL.Controls[index];
                if (StaticMethods.IsControlBoundToBinding(CTRL1, BIN_SRC) && !ARR_LIS.Contains((object)CTRL1))
                    ARR_LIS.Add((object)CTRL1);
                if (CTRL1.Controls.Count > 0)
                    StaticMethods.AddControlsBoundToList(ARR_LIS, CTRL1, BIN_SRC);
            }
        }

        public static bool IsControlBoundToBinding(Control CTRL, BindingSource BIN_SRC)
        {
            if (CTRL.DataBindings.BindableComponent == null || CTRL.DataBindings.BindableComponent.DataBindings == null)
                return false;
            for (int index = 0; index < CTRL.DataBindings.BindableComponent.DataBindings.Count; ++index)
            {
                if (((BindingsCollection)CTRL.DataBindings.BindableComponent.DataBindings)[index].DataSource == BIN_SRC)
                    return true;
            }
            return false;
        }

        public static void AddEventHandlerToObject(object EventHan, string EventName, object ObjectToAddHandler)
        {
            System.Reflection.EventInfo @event = ObjectToAddHandler.GetType().GetEvent(EventName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (@event == null)
                throw new Exception(string.Concat(new object[4]
        {
          (object) "Unable to find event called ",
          (object) EventName,
          (object) " in object of type ",
          (object) ObjectToAddHandler.GetType()
        }));
            else
                @event.AddEventHandler(ObjectToAddHandler, (Delegate)EventHan);
        }

        public static string GetBoundPropertyName(Control CTRL)
        {
            if (CTRL.DataBindings.BindableComponent == null || CTRL.DataBindings.BindableComponent.DataBindings == null || CTRL.DataBindings.BindableComponent.DataBindings.Count == 0)
                return (string)null;
            else
                return ((BindingsCollection)CTRL.DataBindings.BindableComponent.DataBindings)[0].PropertyName;
        }

        public static Control[] GetAllControlsArrangedByTabIndex(Control ParentControl)
        {
            ArrayList arrayList = new ArrayList();
            SortedList<int, Control> sortedList = new SortedList<int, Control>(ParentControl.Controls.Count);
            for (int index = 0; index < ParentControl.Controls.Count; ++index)
                sortedList.Add(ParentControl.Controls[index].TabIndex, ParentControl.Controls[index]);
            Control[] array = new Control[sortedList.Values.Count];
            sortedList.Values.CopyTo(array, 0);
            return array;
        }



        public static string GetStringValueOfDataRow(DataRow DAT_ROW, string ColumnName, object NullValue)
        {
            return string.Concat(StaticMethods.GetValueOfDataRowColumn(DAT_ROW, ColumnName, NullValue));
        }

        public static int GetIntegerValueOfDataRow(DataRow DAT_ROW, string ColumnName, int NullValue)
        {
            int num;
            try
            {
                num = int.Parse(string.Concat(StaticMethods.GetValueOfDataRowColumn(DAT_ROW, ColumnName, (object)NullValue)));
            }
            catch (ArgumentException ex)
            {
                return NullValue;
            }
            catch (FormatException ex)
            {
                return NullValue;
            }
            catch (OverflowException ex)
            {
                return NullValue;
            }
            return num;
        }

        public static long GetLongValueOfDataRow(DataRow DAT_ROW, string ColumnName, long NullValue)
        {
            long num;
            try
            {
                num = long.Parse(string.Concat(StaticMethods.GetValueOfDataRowColumn(DAT_ROW, ColumnName, (object)NullValue)));
            }
            catch (ArgumentException ex)
            {
                return NullValue;
            }
            catch (FormatException ex)
            {
                return NullValue;
            }
            catch (OverflowException ex)
            {
                return NullValue;
            }
            return num;
        }

        public static int GetIntValueOfDataRow(DataRow DAT_ROW, string ColumnName, int NullValue)
        {
            int num;
            try
            {
                num = int.Parse(string.Concat(StaticMethods.GetValueOfDataRowColumn(DAT_ROW, ColumnName, (object)NullValue)));
            }
            catch (ArgumentException ex)
            {
                return NullValue;
            }
            catch (FormatException ex)
            {
                return NullValue;
            }
            catch (OverflowException ex)
            {
                return NullValue;
            }
            return num;
        }

        public static bool GetBooleanValueOfDataRow(DataRow DAT_ROW, string ColumnName, object NullValue)
        {
            bool result = false;
            string str = string.Concat(StaticMethods.GetValueOfDataRowColumn(DAT_ROW, ColumnName, NullValue));
            if (bool.TryParse(str, out result))
                return result;
            else
                return !str.Equals("0") && !str.Equals("No") && (str.Equals("1") || str.Equals("Yes") || bool.Parse(str));
        }

        public static object GetValueOfDataRowColumn(DataRow DAT_ROW, string ColumnName, object NullValue)
        {
            object obj = DAT_ROW[ColumnName];
            if (obj == null || obj == DBNull.Value)
                return NullValue;
            else
                return obj;
        }

        public static object GetValueOfDataRowColumn(DataRow DAT_ROW, int col_index, object NullValue)
        {
            object obj = DAT_ROW[col_index];
            if (obj == null || obj == DBNull.Value)
                return NullValue;
            else
                return obj;
        }

        public static T Cast<T>(object o)
        {
            return (T)o;
        }

        public static void SetSqlParameterValueFromCommand(SqlCommand sqlCommand_src, string ParaNameSrc, SqlCommand sqlCommand_destination, string ParaNameDestination)
        {
            sqlCommand_destination.Parameters[ParaNameDestination].Value = sqlCommand_src.Parameters[ParaNameSrc].Value;
        }

        public static string[] GetXmlTags(string TagName, string XmlText)
        {
            TagName = "</" + TagName + ">";
            XmlText = XmlText.Replace('\n', ' ');
            XmlText = XmlText.Replace('\r', ' ');
            XmlText = XmlText.Trim();
            if (XmlText.IndexOf(TagName) == -1)
            {
                return new string[1]
        {
          XmlText
        };
            }
            else
            {
                bool flag = XmlText.EndsWith(TagName);
                string[] strArray = XmlText.Split(new string[1]
        {
          TagName
        }, StringSplitOptions.RemoveEmptyEntries);
                for (int index = 0; index < strArray.Length; ++index)
                {
                    if (index + 1 < strArray.Length)
                        strArray[index] = strArray[index] + TagName;
                    else if (flag)
                        strArray[index] = strArray[index] + TagName;
                }
                return strArray;
            }
        }

        public static int SetValueOfTag(string TagName, ref string XmlText, string StrVal)
        {
            string str = TagName + ">";
            TagName = "<" + TagName + ">";
            char[] chArray1 = TagName.ToCharArray();
            char[] chArray2 = XmlText.ToCharArray();
            char[] chArray3 = new char[chArray1.Length];
            StringBuilder stringBuilder = new StringBuilder(XmlText.Length);
            bool flag1 = true;
            int num1 = 0;
            int num2 = 0;
            for (int index1 = 0; index1 < chArray2.Length; ++index1)
            {
                bool flag2 = true;
                bool flag3 = true;
                char ch = chArray2[index1];
                if (!flag1 && ch.Equals('<'))
                    flag1 = true;
                for (int index2 = 0; index2 < chArray3.Length; ++index2)
                {
                    int index3 = num2 + index2;
                    if (index3 < chArray2.Length)
                    {
                        chArray3[index2] = chArray2[index3];
                    }
                    else
                    {
                        flag2 = false;
                        flag3 = false;
                    }
                }
                if (flag2)
                {
                    for (int index2 = 0; index2 < chArray3.Length; ++index2)
                    {
                        if (!chArray1[index2].Equals(chArray3[index2]))
                        {
                            flag3 = false;
                            break;
                        }
                    }
                }
                if (flag1)
                    stringBuilder.Append(ch);
                if (flag2 && flag3 && flag1)
                {
                    stringBuilder.Append(str);
                    stringBuilder.Append(StrVal);
                    ++num1;
                    flag1 = false;
                }
                ++num2;
            }
            XmlText = ((object)stringBuilder).ToString();
            return num1;
        }

        public static string[] GetValueOfTags(string TagName, string XmlText)
        {
            return StaticMethods.GetXMLBetweenTags(XmlText, TagName, true);
        }

        public static void WriteStringToFile(string Path, string STRXML)
        {
            StreamWriter streamWriter = new StreamWriter(Path);
            streamWriter.Write(STRXML);
            ((TextWriter)streamWriter).Flush();
            streamWriter.Close();
        }

        public static void SaveAsCSVToFile(string Path, DataView DataView, bool first_line_headers, string Delimiter, string[] ColumnNamesToWrite)
        {
            StringBuilder stringBuilder = new StringBuilder(10000);
            string[] strArray = new string[DataView.Table.Columns.Count];
            for (int index = 0; index < strArray.Length; ++index)
                strArray[index] = DataView.Table.Columns[index].ColumnName ?? "";
            if (ColumnNamesToWrite != null)
                strArray = ColumnNamesToWrite;
            for (int index1 = 0; index1 < DataView.Count; ++index1)
            {
                if (first_line_headers && index1 == 0)
                {
                    for (int index2 = 0; index2 < strArray.Length; ++index2)
                    {
                        stringBuilder.Append("\"" + strArray[index2] + "\"");
                        if (index2 + 1 < strArray.Length)
                            stringBuilder.Append(Delimiter);
                    }
                    stringBuilder.Append(Environment.NewLine);
                }
                DataRow row = DataView[index1].Row;
                for (int index2 = 0; index2 < strArray.Length; ++index2)
                {
                    stringBuilder.Append("\"" + row[strArray[index2]] + "\"");
                    if (index2 + 1 < strArray.Length)
                        stringBuilder.Append(Delimiter);
                }
                if (index1 + 1 < DataView.Count)
                    stringBuilder.Append(Environment.NewLine);
            }
            StaticMethods.WriteStringToFile(Path, ((object)stringBuilder).ToString());
        }

        public static double RoundOff(string STR, int DecimalPlaces, bool ThrowExceptionIfStringCantBeParsed)
        {
            double result = 0.0;
            bool flag = double.TryParse(STR, out result);
            if (!flag && ThrowExceptionIfStringCantBeParsed)
                throw new Exception("The string cannot be parsed as a double " + STR);
            if (!flag && !ThrowExceptionIfStringCantBeParsed)
                return 0.0;
            else
                return Math.Round(result, DecimalPlaces, MidpointRounding.AwayFromZero);
        }

        /*public static bool SetHiddenOrVisibleIfColumnExists(string ColumnFieldName, GridControl GRI_CONT, bool hidden_visible)
        {
            GridView gridView = (GridView)GRI_CONT.FocusedView;
            try
            {
                gridView.Columns[ColumnFieldName].Visible = hidden_visible;
            }
            catch
            {
                return false;
            }
            return true;
        }*/

        /*public static bool SetCaptionTextIfColumnExists(string ColumnFieldName, GridControl GRI_CONT, string caption_text)
        {
            GridView gridView = (GridView)GRI_CONT.FocusedView;
            try
            {
                gridView.Columns[ColumnFieldName].Caption = caption_text;
            }
            catch
            {
                return false;
            }
            return true;
        }*/



        public static bool SetBindingSourcePosition(DataRow DAT_ROW, BindingSource BindingSrcEditor)
        {
            int index1 = BindingSrcEditor.Count - 1;
            for (int index2 = 0; index2 < BindingSrcEditor.Count; ++index2)
            {
                if ((BindingSrcEditor[index2] as DataRowView).Row == DAT_ROW)
                {
                    BindingSrcEditor.CurrencyManager.Position = index2;
                    return true;
                }
                else if ((BindingSrcEditor[index1] as DataRowView).Row == DAT_ROW)
                {
                    BindingSrcEditor.CurrencyManager.Position = index1;
                    return true;
                }
                else
                    --index1;
            }
            return false;
        }

        /*public static int GetRowHandle(DataRow DAT_ROW, GridControl gridControl)
        {
            int num = DAT_ROW.Table.Rows.IndexOf(DAT_ROW);
            GridView gridView = gridControl.MainView as GridView;
            for (int rowHandle = num; rowHandle < gridView.RowCount; ++rowHandle)
            {
                DataRow dataRow = gridView.GetDataRow(rowHandle);
                if (DAT_ROW == dataRow)
                    return rowHandle;
            }
            for (int rowHandle = num; rowHandle > -1; --rowHandle)
            {
                DataRow dataRow = gridView.GetDataRow(rowHandle);
                if (DAT_ROW == dataRow)
                    return rowHandle;
            }
            return int.MinValue;
        }*/

        public static void SetAllColumnsAllowNulls(DataTable DatTab, bool booIfAllowUnit)
        {
            for (int index = 0; index < DatTab.Columns.Count; ++index)
                DatTab.Columns[index].AllowDBNull = booIfAllowUnit;
        }

        public static bool ShowPopUpOpenFileDialog(Form For, ref string StartFilePath, bool check_if_file_exists, bool check_if_path_exists, long MaxFileSize, string FileFilters, string CustomTitle)
        {
            do
            {
                string str = string.Empty;
                if (CustomTitle == null)
                    CustomTitle = "Select a file";
                StaticMethods.GetDirectory(StartFilePath, out StartFilePath);
                if (StartFilePath == null)
                    StartFilePath = "C:\\";
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = FileFilters;
                openFileDialog.InitialDirectory = StartFilePath;
                openFileDialog.CheckFileExists = check_if_file_exists;
                openFileDialog.CheckPathExists = check_if_path_exists;
                openFileDialog.Title = CustomTitle;
                if (openFileDialog.ShowDialog((IWin32Window)For) == DialogResult.OK)
                {
                    StartFilePath = openFileDialog.FileName;
                    if ((MaxFileSize != -1L && new FileInfo(StartFilePath).Length > MaxFileSize) == false)
                        //MessageBox.AllowCustomLookAndFeel = true;
                        //else
                        goto label_9;
                }
                else
                    goto label_10;
            }
            while (MessageBox.Show((IWin32Window)For, "The file is too large", "Please select another file", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) != DialogResult.Cancel);
            return false;
        label_9:
            return true;
        label_10:
            return false;
        }

        public static bool GetDirectory(string FilePath, out string DirPath)
        {
            DirPath = (string)null;
            FileInfo fileInfo;
            FileAttributes attributes;
            try
            {
                fileInfo = new FileInfo(FilePath);
                attributes = fileInfo.Attributes;
            }
            catch (Exception ex)
            {
                return false;
            }
            DirPath = attributes != FileAttributes.Directory ? fileInfo.DirectoryName : FilePath;
            return true;
        }

        /*public static void SetCursorAtEnd(TextEdit textEdit, Control CtrlWithFocus)
        {
            int length = textEdit.Text.Length;
            if (length == 0)
                return;
            KeyEventArgs e = new KeyEventArgs(Keys.S);
            textEdit.Focus();
            for (int index = 0; index < length + 2; ++index)
                textEdit.SendKey(e);
            CtrlWithFocus.Focus();
        }*/

        public static bool GetIfExtensionContainsString(string FileName, string Extension)
        {
            return new FileInfo(FileName).Extension.Contains(Extension);
        }

        public static bool GetIfFileExists(string FilePath)
        {
            return new FileInfo(FilePath).Exists;
        }

        public static string GetErrorMessageAboutFile(string FilePath)
        {
            FileInfo file = new FileInfo(FilePath);
            if (!StaticMethods.GetIfFileExists(FilePath))
                return file.Name + " does not exist";
            if (StaticMethods.IsFileLocked(file))
                return file.Name + " is opened in another program";
            else
                return (string)null;
        }

        public static bool IsFileLocked(FileInfo file)
        {
            FileStream fileStream = (FileStream)null;
            try
            {
                fileStream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException ex)
            {
                return true;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
            return false;
        }

        public static bool LoadDataFromCSVTextFile(ref DataTable DatTabToLoad, string FileName, string ColumnDelimiter, out Exception ExcErrorMess, string[] ArrSrcColumnsInDataTable, string[] ArrMatchingColumnsInCSVFile, bool IfRowOneIsColumnNames)
        {
            ExcErrorMess = (Exception)null;
            bool flag1 = true;
            bool flag2 = false;
            int[] numArray = (int[])null;
            char[] chArray = ColumnDelimiter.ToCharArray();
            try
            {
                string messageAboutFile = StaticMethods.GetErrorMessageAboutFile(FileName);
                if (messageAboutFile != null)
                    throw new Exception(messageAboutFile);
                if (ArrMatchingColumnsInCSVFile != null || ArrSrcColumnsInDataTable != null)
                {
                    flag2 = true;
                    if (ArrMatchingColumnsInCSVFile != null && ArrSrcColumnsInDataTable == null)
                        throw new Exception("The array for the source columns cannot be null, if the array for matching columns is null");
                    if (ArrMatchingColumnsInCSVFile == null && ArrSrcColumnsInDataTable != null)
                        throw new Exception("The array for the matching columns cannot be null, if the array for source datatable columns is null");
                    if (ArrMatchingColumnsInCSVFile != null && ArrSrcColumnsInDataTable != null && !IfRowOneIsColumnNames)
                        throw new Exception("Invalid argument!!! The flag for getting the column names in the first row must be set to true if the datatable would be copied with mapping information");
                    if (ArrMatchingColumnsInCSVFile.Length != ArrSrcColumnsInDataTable.Length)
                        throw new Exception("The length of the source column array must be equal to that of the matching columns array");
                    if (DatTabToLoad == null)
                    {
                        DatTabToLoad = new DataTable();
                        for (int index = 0; index < ArrSrcColumnsInDataTable.Length; ++index)
                            DatTabToLoad.Columns.Add(ArrSrcColumnsInDataTable[index], typeof(string));
                    }
                }
                string[] strArray1 = File.ReadAllLines(FileName);
                if (flag2)
                {
                    for (int index1 = 0; index1 < strArray1.Length; ++index1)
                    {
                        if (index1 == 0 && IfRowOneIsColumnNames)
                        {
                            string[] strArray2 = strArray1[index1].Split(chArray);
                            numArray = new int[strArray2.Length];
                            string columnName = "";
                            for (int index2 = 0; index2 < strArray2.Length; ++index2)
                            {
                                string str = strArray2[index2];
                                for (int index3 = 0; index3 < ArrMatchingColumnsInCSVFile.Length; ++index3)
                                {
                                    if (ArrMatchingColumnsInCSVFile[index3].Equals(str))
                                    {
                                        columnName = ArrSrcColumnsInDataTable[index3];
                                        break;
                                    }
                                }
                                numArray[index2] = DatTabToLoad.Columns.IndexOf(columnName);
                            }
                        }
                        if (index1 != 0)
                        {
                            string[] strArray2 = strArray1[index1].Split(chArray);
                            for (int index2 = 0; index2 < strArray2.Length; ++index2)
                            {
                                DataRow row = DatTabToLoad.NewRow();
                                for (int index3 = 0; index3 < strArray2.Length; ++index3)
                                    row[numArray[index3]] = (object)strArray2[index3];
                                DatTabToLoad.Rows.Add(row);
                            }
                        }
                    }
                }
                else if (!flag2)
                {
                    for (int index1 = 0; index1 < strArray1.Length; ++index1)
                    {
                        string[] strArray2 = strArray1[index1].Split(chArray);
                        if (index1 == 0)
                        {
                            DatTabToLoad = new DataTable();
                            for (int index2 = 0; index2 < strArray2.Length; ++index2)
                            {
                                if (IfRowOneIsColumnNames)
                                    DatTabToLoad.Columns.Add(strArray2[index2], typeof(string));
                                else
                                    DatTabToLoad.Columns.Add("Column" + (object)index2, typeof(string));
                            }
                        }
                        if (index1 != 0 || !IfRowOneIsColumnNames)
                        {
                            DataRow row = DatTabToLoad.NewRow();
                            for (int index2 = 0; index2 < strArray2.Length; ++index2)
                                row[index2] = (object)strArray2[index2];
                            DatTabToLoad.Rows.Add(row);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExcErrorMess = ex;
                flag1 = false;
            }
            finally
            {
            }
            return flag1;
        }

        public static bool GetColumnNamesFromCSV(string FilePath, string StrDelim, out string[] StrArrColName)
        {
            bool flag = true;
            char[] chArray = StrDelim.ToCharArray();
            StrArrColName = new string[0];
            try
            {
                string[] strArray = File.ReadAllLines(FilePath);
                if (strArray.Length == 0)
                    flag = false;
                else
                    StrArrColName = strArray[0].Split(chArray);
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public static bool GetIfArrayContainsElement(string[] ArrStr, string StrEle, bool booIgnoCase, bool booTrimBeforeCompare)
        {
            if (booIgnoCase)
                StrEle = StrEle.ToLower();
            if (booTrimBeforeCompare)
                StrEle = StrEle.Trim();
            for (int index = 0; index < ArrStr.Length; ++index)
            {
                string str = ArrStr[index];
                if (booIgnoCase)
                    str = str.ToLower();
                if (booTrimBeforeCompare)
                    str = str.Trim();
                if (str.Equals(StrEle))
                    return true;
            }
            return false;
        }

        public static void ReInitializeThread(ref Thread ThreadToInit, bool StartThread, ThreadStart ThreaStar)
        {
            while (ThreadToInit != null)
            {
                ThreadToInit.Abort();
                ThreadToInit = (Thread)null;
            }
            ThreadToInit = new Thread(ThreaStar);
            if (!StartThread)
                return;
            ThreadToInit.Start();
        }

        /*public static void SetSelectedTabPage(XtraTabControl XtraTabCon, XtraTabPage XtraTabPagSelected)
        {
            XtraTabCon.SelectedTabPage = XtraTabPagSelected;
        }*/

        public static DataView CreateDataViewFromDataTable(DataTable Dat, string RowFilter)
        {
            DataView dataView = new DataView(Dat);
            dataView.RowFilter = RowFilter;
            return dataView;
        }

        /*public static void DoPerformClickOnToolStripWithInvoke(ToolStrip TooStrBar, ToolStripButton TooStriBut)
        {
            // ISSUE: method pointer
            StaticMethods.ExecuteWithInvoke((Control)TooStrBar, (Delegate)new DelToolStripButton((DoPerformClickOnToolStripButton)), new object[1]
      {
        (object) TooStriBut
      });
        }*/

        public static void DoPerformClickOnToolStripButton(ToolStripButton TooStriBut)
        {
            TooStriBut.PerformClick();
        }

        public static DialogResult ShowFolderChooseDialog(Environment.SpecialFolder StartFolder, bool IfShowNewFolder, Form ParentForm, out string SelectedPath)
        {
            SelectedPath = (string)null;
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.RootFolder = StartFolder;
            folderBrowserDialog.ShowNewFolderButton = IfShowNewFolder;
            DialogResult dialogResult = folderBrowserDialog.ShowDialog((IWin32Window)ParentForm);
            if (dialogResult == DialogResult.OK)
            {
                SelectedPath = folderBrowserDialog.SelectedPath;
                if (!SelectedPath.EndsWith("\\"))
                    SelectedPath = SelectedPath + "\\";
            }
            return dialogResult;
        }

        public static void SetValuesFromDataRow(SqlCommand sqlCommand, DataRow DAT_ROW, bool exclude_output_parameters, bool check_if_table_has_columns, string[] StrArrToExclude)
        {
            DataTable table = DAT_ROW.Table;
            for (int index1 = 0; index1 < sqlCommand.Parameters.Count; ++index1)
            {
                string parameterName = sqlCommand.Parameters[index1].ParameterName;
                if ((sqlCommand.Parameters[index1].Direction != ParameterDirection.Output || !exclude_output_parameters) && (!check_if_table_has_columns || table.Columns.Contains(sqlCommand.Parameters[index1].SourceColumn)))
                {
                    bool flag = false;
                    if (StrArrToExclude != null)
                    {
                        for (int index2 = 0; index2 < StrArrToExclude.Length; ++index2)
                        {
                            if (StrArrToExclude[index2].Equals(parameterName, StringComparison.Ordinal))
                            {
                                flag = true;
                                break;
                            }
                        }
                    }
                    if (!flag)
                        sqlCommand.Parameters[index1].Value = DAT_ROW[sqlCommand.Parameters[index1].SourceColumn];
                }
            }
        }

        public static string[] GetAllFiles(string DirPath, bool ignore_exception)
        {
            ArrayList ArrLis = new ArrayList(3000);
            StaticMethods.AddFiles(ArrLis, DirPath, ignore_exception);
            string[] strArray = new string[ArrLis.Count];
            ArrLis.CopyTo((Array)strArray, 0);
            return strArray;
        }

        public static void AddFiles(ArrayList ArrLis, string DirPath, bool ignore_exception)
        {
            DirectoryInfo[] directoryInfoArray = new DirectoryInfo[0];
            FileInfo[] fileInfoArray = new FileInfo[0];
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(DirPath);
                fileInfoArray = directoryInfo.GetFiles();
                directoryInfoArray = directoryInfo.GetDirectories();
            }
            catch (Exception ex)
            {
                if (fileInfoArray == null)
                    fileInfoArray = new FileInfo[0];
                if (directoryInfoArray == null)
                    directoryInfoArray = new DirectoryInfo[0];
                if (!ignore_exception)
                    throw ex;
            }
            for (int index = 0; index < fileInfoArray.Length; ++index)
                ArrLis.Add((object)fileInfoArray[index].FullName);
            for (int index = 0; index < directoryInfoArray.Length; ++index)
                StaticMethods.AddFiles(ArrLis, directoryInfoArray[index].FullName, ignore_exception);
        }

        /*public static void SetProgressBarValue(double progress_value, ProgressBarControl PROG_BAR_CONT)
        {
            PROG_BAR_CONT.EditValue = (object)progress_value;
        }*/

        public static string QuoteString(string StrToQuote, string quotes)
        {
            if (quotes == null)
                quotes = "'";
            return quotes + StrToQuote + quotes;
        }

        public static bool GetIfBlank(object p, bool show)
        {
            bool flag = false;
            if (p == null)
            {
                p = (object)"<OBJECT> IS NULL";
                flag = true;
            }
            else if (p == DBNull.Value)
            {
                p = (object)"<OBJECT> IS EQUAL TO DBNull.Value";
                flag = true;
            }
            else if (p.ToString().Trim().Length == 0)
                flag = true;
            if (show)
            {
                int num = (int)MessageBox.Show(string.Concat(p));
            }
            return flag;
        }

        public static bool GetIfColumnsUnique(DataView DatVie, string[] ArrColNames, bool if_trim_before_compare, ref DataRowView DatRowVieThatsSimilar, ref int index)
        {
            SortedList<string, string> sortedList1 = new SortedList<string, string>(DatVie.Count);
            DatRowVieThatsSimilar = (DataRowView)null;
            index = -1;
            StringBuilder stringBuilder = new StringBuilder(1000);
            string key = (string)null;
            SortedList<string, string> sortedList2;
            for (int index1 = 0; index1 < DatVie.Count; ++index1)
            {
                DataRowView dataRowView = DatVie[index1];
                stringBuilder.Remove(0, stringBuilder.Length);
                for (int index2 = 0; index2 < ArrColNames.Length; ++index2)
                {
                    key = StaticMethods.GetStringValueOfDataRow(dataRowView.Row, ArrColNames[index2], (object)"");
                    if (if_trim_before_compare)
                        key = key.Trim();
                }
                if (!sortedList1.ContainsKey(key))
                {
                    sortedList1.Add(key, (string)null);
                }
                else
                {
                    DatRowVieThatsSimilar = dataRowView;
                    index = index1;
                    sortedList1.Clear();
                    sortedList2 = (SortedList<string, string>)null;
                    return false;
                }
            }
            sortedList1.Clear();
            sortedList2 = (SortedList<string, string>)null;
            stringBuilder.Remove(0, stringBuilder.Length);
            return true;
        }

        /*public static DataRow GetSelectedItemAsDataRow(LookUpEdit lookUpEdit)
        {
            object editValue = lookUpEdit.EditValue;
            if (editValue == null)
                return (DataRow)null;
            object dataSource = lookUpEdit.Properties.DataSource;
            BindingSource BinSrc = dataSource as BindingSource;
            DataTable dataTable = dataSource as DataTable;
            DataView dataView = dataSource as DataView;
            string valueMember = lookUpEdit.Properties.ValueMember;
            if (valueMember == null)
                throw new Exception("Value member not set on lookupedit!!!");
            if (BinSrc != null)
            {
                dataView = StaticMethods.GetDataViewFromBindingSrc(BinSrc);
                if (dataView == null)
                    throw new Exception("Unable to get the dataview from the binding source");
            }
            if (dataTable != null)
                dataView = dataTable.DefaultView;
            if (dataView != null)
            {
                int index1 = dataView.Count - 1;
                for (int index2 = 0; index2 < dataView.Count; ++index2)
                {
                    DataRowView dataRowView1 = dataView[index2];
                    if (string.Concat(editValue).Equals(string.Concat(dataRowView1[valueMember])))
                        return dataRowView1.Row;
                    DataRowView dataRowView2 = dataView[index1];
                    if (string.Concat(editValue).Equals(string.Concat(dataRowView2[valueMember])))
                        return dataRowView2.Row;
                    --index1;
                }
            }
            if (dataView == null)
                throw new Exception("Unable to retrieve dataview from the datasource!");
            else
                return (DataRow)null;
        }*/

        /*public static void SetSelectedItemInLookUp(DataRow DAT_ROW, string column_to_look_for_in_datarow, LookUpEdit lookUpEdit)
        {
            object obj = DAT_ROW[column_to_look_for_in_datarow];
            lookUpEdit.EditValue = obj;
        }*/

        /*public static void RefreshGridControl(GridControl GRI_CON)
        {
            GRI_CON.RefreshDataSource();
            GRI_CON.Refresh();
        }*/

        public static int GetCountThatContains(string RowFilter, DataTable dataTable)
        {
            DataView dataView = new DataView(dataTable);
            dataView.RowFilter = RowFilter;
            return dataView.Count;
        }

        public static int GetCountThatContains(string RowFilter, DataTable dataTable, ref DataRow DAT_ROW)
        {
            DataView dataView = new DataView(dataTable);
            dataView.RowFilter = RowFilter;
            if (dataView.Count > 0)
                DAT_ROW = dataView[0].Row;
            return dataView.Count;
        }

        public static void SetCommonValueForColumn(string[] column_names, object[] object_values, DataView dataView, DataRowState DatRowState, bool change_rows_to_new_state)
        {
            for (int index1 = 0; index1 < dataView.Count; ++index1)
            {
                DataRow row = dataView[index1].Row;
                if (column_names != null && object_values != null)
                {
                    for (int index2 = 0; index2 < column_names.Length; ++index2)
                        row[column_names[index2]] = object_values[index2];
                }
                if (change_rows_to_new_state && row.RowState != DatRowState)
                {
                    row.AcceptChanges();
                    if (DatRowState == DataRowState.Added)
                        row.SetAdded();
                    else if (DatRowState == DataRowState.Deleted)
                        row.Delete();
                    else if (DatRowState == DataRowState.Modified)
                        row.SetModified();
                }
            }
        }

        public static bool GetIfAllColumnsFilled(string[] colum_names, DataRow DAT_ROW, bool only_these_or_excluding_these)
        {
            if (only_these_or_excluding_these)
            {
                for (int index = 0; index < colum_names.Length; ++index)
                {
                    if (StaticMethods.GetIfBlank(DAT_ROW[colum_names[index]], false))
                        return false;
                }
                return true;
            }
            else
            {
                DataColumnCollection columns = DAT_ROW.Table.Columns;
                for (int index1 = 0; index1 < columns.Count; ++index1)
                {
                    string columnName = columns[index1].ColumnName;
                    bool flag = false;
                    for (int index2 = 0; index2 < colum_names.Length; ++index2)
                    {
                        if (columnName.Equals(colum_names[index2], StringComparison.Ordinal))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag && StaticMethods.GetIfBlank(DAT_ROW[columnName], false))
                        return false;
                }
                return true;
            }
        }

        public static DataRow MakeNewRow(DataRow DatRow, bool if_append_row_to_table, string[] ArrColumns, object[] ObjVals)
        {
            DataTable table = DatRow.Table;
            DataRow row = table.NewRow();
            if (ArrColumns != null)
            {
                for (int index = 0; index < ArrColumns.Length; ++index)
                {
                    object obj = ObjVals[index] ?? DatRow[ArrColumns[index]];
                    row[ArrColumns[index]] = obj;
                }
            }
            if (if_append_row_to_table)
                table.Rows.Add(row);
            return row;
        }

        public static DataRow MakeNewRow(DataTable DatTab, bool if_append_row_to_table, string[] ArrColumns, object[] ObjVals)
        {
            DataRow row = DatTab.NewRow();
            if (ArrColumns != null)
            {
                for (int index = 0; index < ArrColumns.Length; ++index)
                {
                    object obj = ObjVals[index];
                    row[ArrColumns[index]] = obj;
                }
            }
            if (if_append_row_to_table)
                DatTab.Rows.Add(row);
            return row;
        }

        public static DataRow GetDataGridViewAsDataRow(DataGridViewRow dataGridViewRow)
        {
            object dataBoundItem = dataGridViewRow.DataBoundItem;
            if (dataBoundItem == null)
                return (DataRow)null;
            DataRowView dataRowView = dataBoundItem as DataRowView;
            if (dataRowView == null)
                return (DataRow)null;
            else
                return dataRowView.Row;
        }

        public static DataRow GetSelectedItemInComboBoxColumnAsDataRow(DataGridViewComboBoxColumn dataGridViewComboBoxColumn, int RowIndex)
        {
            DataGridView dataGridView = dataGridViewComboBoxColumn.DataGridView;
            int index1 = dataGridViewComboBoxColumn.Index;
            string valueMember = dataGridViewComboBoxColumn.ValueMember;
            object obj = dataGridView[index1, RowIndex].Value;
            if (obj == null)
                return (DataRow)null;
            string str = string.Concat(obj);
            DataView dataView = dataGridViewComboBoxColumn.DataSource as DataView;
            DataTable dataTable = dataGridViewComboBoxColumn.DataSource as DataTable;
            BindingSource bindingSource = dataGridViewComboBoxColumn.DataSource as BindingSource;
            if (dataTable != null)
                dataView = dataTable.DefaultView;
            if (dataView != null)
            {
                for (int index2 = 0; index2 < dataView.Count; ++index2)
                {
                    DataRow row = dataView[index2].Row;
                    if (string.Concat(row[valueMember]).Equals(str, StringComparison.Ordinal))
                        return row;
                }
            }
            if (bindingSource != null)
            {
                for (int index2 = 0; index2 < bindingSource.Count; ++index2)
                {
                    DataRowView dataRowView = bindingSource[index2] as DataRowView;
                    if (dataRowView != null)
                    {
                        DataRow row = dataRowView.Row;
                        if (string.Concat(row[valueMember]).Equals(str, StringComparison.Ordinal))
                            return row;
                    }
                }
            }
            return (DataRow)null;
        }

        public static bool CheckValueDataType(string TAR_COL_DATA_TYPE, int TAR_COL_DATA_LEN, int TAR_COL_DATA_DECIMAL_PLACES, string ASSIGN_VALUE, out string Error)
        {
            Error = (string)null;
            bool flag = true;
            if (ASSIGN_VALUE == null || ASSIGN_VALUE.Length == 0)
                return true;
            switch (TAR_COL_DATA_TYPE)
            {
                case "A":
                    if (ASSIGN_VALUE.Length > TAR_COL_DATA_LEN)
                    {
                        Error = "The length of the value is too long for that data-type\nTrim " + (object)(ASSIGN_VALUE.Length - ASSIGN_VALUE.Length) + " characters for it to fit";
                        flag = false;
                        break;
                    }
                    else
                        break;
                case "B":
                    bool result1 = false;
                    if (!bool.TryParse(ASSIGN_VALUE, out result1))
                    {
                        Error = "The value cannot be accepted as a boolean. It has to be either true or false";
                        flag = false;
                        break;
                    }
                    else
                        break;
                case "D":
                    DateTime result2 = DateTime.Now;
                    if (!DateTime.TryParse(ASSIGN_VALUE, out result2))
                    {
                        Error = "The value cannot be accepted as a date / time. \nIt has to represent a valid date!";
                        flag = false;
                        break;
                    }
                    else
                        break;
                case "N":
                    double result3 = -1.0;
                    if (!double.TryParse(ASSIGN_VALUE, out result3))
                    {
                        Error = "The value cannot be accepted as a number!";
                        flag = false;
                    }
                    if (flag && StaticMethods.GetCountDecimalsLength((object)result3) > TAR_COL_DATA_DECIMAL_PLACES)
                    {
                        Error = "The value has too many decimal places.\nThe number of decimal places must not exceed " + (object)TAR_COL_DATA_DECIMAL_PLACES;
                        flag = false;
                        break;
                    }
                    else
                        break;
                case "W":
                    long result4 = -1L;
                    if (!long.TryParse(ASSIGN_VALUE, out result4))
                    {
                        Error = "The value cannot be accepted as a whole number!\nPlease change the datatype to numeric if you want to include decimals";
                        flag = false;
                        break;
                    }
                    else
                        break;
                default:
                    throw new ArgumentException("Unknown data type category!" + TAR_COL_DATA_TYPE);
            }
            return flag;
        }

        private static int GetCountDecimalsLength(object DoubTest)
        {
            double num = 0.0;
            try
            {
                num = double.Parse(string.Concat(DoubTest));
            }
            catch
            {
                throw new ArgumentException("The object cannot be parsed as a number. The object is '" + DoubTest + "'");
            }
            string[] strArray = DoubTest.ToString().Split(new char[1]
      {
        '.'
      });
            if (strArray.Length == 1)
                return 0;
            else
                return strArray[1].Length;
        }

        public static bool CheckDataTypeCompatibility(string TAR_COL_DATA_TYPE, int TAR_COL_DATA_LEN, int TAR_COL_DEC_NO, string SRC_COL_DATA_TYPE, int SRC_COL_DATA_LEN, int SRC_COL_DEC_NO, out string Error)
        {
            Error = (string)null;
            bool flag = true;
            if (TAR_COL_DATA_TYPE.Equals("A") && SRC_COL_DATA_TYPE.Equals("A"))
            {
                if (SRC_COL_DATA_LEN > TAR_COL_DATA_LEN)
                {
                    Error = "The length of the source cannot be more than that of the target!";
                    flag = false;
                }
            }
            else if (TAR_COL_DATA_TYPE.Equals("A") && SRC_COL_DATA_TYPE.Equals("N"))
            {
                int num = 19 + TAR_COL_DATA_LEN;
                if (TAR_COL_DATA_LEN < num)
                {
                    Error = "The target column length is lesser than the source column length!";
                    flag = false;
                }
            }
            else if (TAR_COL_DATA_TYPE.Equals("A") && SRC_COL_DATA_TYPE.Equals("B"))
            {
                int num = 5;
                if (TAR_COL_DATA_LEN < num)
                {
                    Error = "The target column length is lesser than the source column length!";
                    flag = false;
                }
            }
            else if (TAR_COL_DATA_TYPE.Equals("A") && SRC_COL_DATA_TYPE.Equals("W"))
            {
                int num = 19;
                if (TAR_COL_DATA_LEN < num)
                {
                    Error = "The target column length is lesser than the source column length!";
                    flag = false;
                }
            }
            else if (TAR_COL_DATA_TYPE.Equals("A") && SRC_COL_DATA_TYPE.Equals("D"))
            {
                int num = 9;
                if (TAR_COL_DATA_LEN < num)
                {
                    Error = "The target column length is lesser than the source column length!";
                    flag = false;
                }
            }
            else if (TAR_COL_DATA_TYPE.Equals("N") && SRC_COL_DATA_TYPE.Equals("A"))
            {
                Error = "You cannot map alphanumerical data into a numerical column!";
                flag = false;
            }
            else if (TAR_COL_DATA_TYPE.Equals("N") && SRC_COL_DATA_TYPE.Equals("N"))
                flag = true;
            else if (TAR_COL_DATA_TYPE.Equals("N") && SRC_COL_DATA_TYPE.Equals("B"))
                flag = true;
            else if (TAR_COL_DATA_TYPE.Equals("N") && SRC_COL_DATA_TYPE.Equals("W"))
                flag = true;
            else if (TAR_COL_DATA_TYPE.Equals("N") && SRC_COL_DATA_TYPE.Equals("D"))
            {
                Error = "You cannot map dates data into a numerical column!";
                flag = false;
            }
            else if (TAR_COL_DATA_TYPE.Equals("B") && SRC_COL_DATA_TYPE.Equals("A"))
            {
                Error = "You cannot map text data into a boolean column!";
                flag = false;
            }
            else if (TAR_COL_DATA_TYPE.Equals("B") && SRC_COL_DATA_TYPE.Equals("N"))
            {
                Error = "You cannot map numerical data into a boolean column!";
                flag = false;
            }
            else if (TAR_COL_DATA_TYPE.Equals("B") && SRC_COL_DATA_TYPE.Equals("B"))
                flag = true;
            else if (TAR_COL_DATA_TYPE.Equals("B") && SRC_COL_DATA_TYPE.Equals("W"))
            {
                Error = "You cannot map numerical data into a boolean column!";
                flag = false;
            }
            else if (TAR_COL_DATA_TYPE.Equals("B") && SRC_COL_DATA_TYPE.Equals("D"))
            {
                Error = "You cannot map datetime data into a boolean column!";
                flag = false;
            }
            else if (TAR_COL_DATA_TYPE.Equals("W") && SRC_COL_DATA_TYPE.Equals("A"))
            {
                Error = "You cannot map numerical data from a text column!";
                flag = false;
            }
            else if (TAR_COL_DATA_TYPE.Equals("W") && SRC_COL_DATA_TYPE.Equals("N"))
            {
                Error = "You cannot map numerical data with decimals onto a column that allows no decimals";
                flag = false;
            }
            else if (TAR_COL_DATA_TYPE.Equals("W") && SRC_COL_DATA_TYPE.Equals("B"))
                flag = true;
            else if (TAR_COL_DATA_TYPE.Equals("W") && SRC_COL_DATA_TYPE.Equals("W"))
                flag = true;
            else if (TAR_COL_DATA_TYPE.Equals("W") && SRC_COL_DATA_TYPE.Equals("D"))
                flag = true;
            else if (TAR_COL_DATA_TYPE.Equals("D") && SRC_COL_DATA_TYPE.Equals("A"))
            {
                Error = "You cannot map data with decimals onto a column that allows no decimals";
                flag = false;
            }
            else if (TAR_COL_DATA_TYPE.Equals("D") && SRC_COL_DATA_TYPE.Equals("N"))
            {
                Error = "You cannot map numeric data type onto a target column that picks dates";
                flag = false;
            }
            else if (TAR_COL_DATA_TYPE.Equals("D") && SRC_COL_DATA_TYPE.Equals("B"))
            {
                Error = "You cannot map boolean data onto a column that takes dates only";
                flag = false;
            }
            else if (TAR_COL_DATA_TYPE.Equals("D") && SRC_COL_DATA_TYPE.Equals("W"))
            {
                Error = "You cannot map numerical data onto a column that takes dates only";
                flag = false;
            }
            else if (TAR_COL_DATA_TYPE.Equals("D") && SRC_COL_DATA_TYPE.Equals("D"))
                flag = true;
            else
                throw new Exception("Unknown combination! Source data type '" + SRC_COL_DATA_TYPE + "' Target data type '" + TAR_COL_DATA_TYPE + "'");
            return flag;
        }

        public static void CreateAddFilterString(ref string FilterText, string DescriptionOfFiles, string[] ArrExtensions)
        {
            StringBuilder stringBuilder = new StringBuilder();
            FilterText = FilterText ?? "";
            stringBuilder.Append(FilterText);
            if (FilterText.Length > 0)
                stringBuilder.Append("|");
            stringBuilder.Append(DescriptionOfFiles);
            stringBuilder.Append("|");
            for (int index = 0; index < ArrExtensions.Length; ++index)
            {
                stringBuilder.Append(ArrExtensions[index]);
                if (index + 1 < ArrExtensions.Length)
                    stringBuilder.Append(";");
            }
            FilterText = ((object)stringBuilder).ToString();
        }

        public static object GetDataSourceBoundToColumn(DataGridView dataGridView, string p, bool throw_exception_on_error)
        {
            object obj = (object)StaticMethods.GetDataGridColumnColumnByBoundColumnName(dataGridView, p);
            if (obj == null)
            {
                if (throw_exception_on_error)
                    throw new ArgumentException("The grid does not contain a column with the dataproperty name '" + p + "'");
                else
                    return (object)null;
            }
            else
            {
                DataGridViewComboBoxColumn viewComboBoxColumn = obj as DataGridViewComboBoxColumn;
                if (viewComboBoxColumn != null)
                    return viewComboBoxColumn.DataSource;
                if (throw_exception_on_error)
                    throw new ArgumentException("The column with the dataproperty name '" + p + "' is not a DataGridViewComboBoxColumn ");
                else
                    return (object)null;
            }
        }

        public static DataGridViewColumn GetDataGridColumnColumnByBoundColumnName(DataGridView dataGridView, string p)
        {
            for (int index = 0; index < dataGridView.ColumnCount; ++index)
            {
                if (dataGridView.Columns[index].DataPropertyName.Equals(p, StringComparison.Ordinal))
                    return dataGridView.Columns[index];
            }
            return (DataGridViewColumn)null;
        }

        public static void RestoreDataRowState(DataRowState dataRowState, DataView DatVie)
        {
            for (int index = 0; index < DatVie.Count; ++index)
            {
                DataRow row = DatVie[index].Row;
                StaticMethods.RestoreDataRowState(dataRowState, row);
            }
        }

        public static int GetDecimalPlaceCount(double _double_parse, bool ignore_ending_zeros)
        {
            string[] strArray = _double_parse.ToString().Split(new char[1]
      {
        '.'
      });
            if (strArray.Length < 2)
                return 0;
            if (!ignore_ending_zeros)
                return strArray[1].Length;
            strArray[1] = StaticMethods.ReverseString(strArray[1]);
            StringBuilder stringBuilder = new StringBuilder(500);
            for (int index = 0; index < strArray[1].Length; ++index)
            {
                if (strArray[1][index].CompareTo('0') != 0)
                    stringBuilder.Append(strArray[1][index]);
            }
            return stringBuilder.Length;
        }

        public static string ReverseString(string s)
        {
            char[] chArray = s.ToCharArray();
            Array.Reverse((Array)chArray);
            return new string(chArray);
        }

        public static DataRow GetFirstRowWithValue(object objects_value, string column_name, DataView DAT_VIE)
        {
            DataRow dataRow = (DataRow)null;
            for (int index = 0; index < DAT_VIE.Count; ++index)
            {
                DataRow row = DAT_VIE[index].Row;
                if (string.Concat(row[column_name]).Equals(string.Concat(objects_value), StringComparison.Ordinal))
                {
                    dataRow = row;
                    break;
                }
            }
            return dataRow;
        }

        public static string[] SplitStringWithConcentation(string STR_SAMPLE, string STR_DELIMITER, string STR_QUOTES)
        {
            char ch = 'ÿ';
            string newValue = string.Concat((object)ch);
            while (STR_SAMPLE.Contains(newValue))
                newValue = newValue + (object)ch;
            STR_SAMPLE = STR_SAMPLE.Replace(STR_QUOTES + STR_DELIMITER + STR_QUOTES, newValue);
            STR_SAMPLE = STR_SAMPLE.Replace(STR_QUOTES + STR_DELIMITER, newValue);
            if (STR_SAMPLE.Contains(newValue))
            {
                if (STR_SAMPLE.StartsWith(STR_QUOTES, StringComparison.Ordinal))
                    STR_SAMPLE = STR_SAMPLE.Remove(0, STR_QUOTES.Length);
                if (STR_SAMPLE.EndsWith(STR_QUOTES, StringComparison.Ordinal))
                    STR_SAMPLE = STR_SAMPLE.Remove(STR_SAMPLE.Length - STR_QUOTES.Length, STR_QUOTES.Length);
            }
            else
                STR_SAMPLE = STR_SAMPLE.Replace(STR_DELIMITER, newValue);
            return STR_SAMPLE.Split(newValue.ToCharArray());
        }

        /*public static void SetFocusedRow(int handle, GridControl Contr)
        {
            ((ColumnView)Contr.MainView).FocusedRowHandle = handle;
        }*/

        public static void SetProgressBarValue(int start_value, int end_value, System.Windows.Forms.ProgressBar ProgBar)
        {
            int num = int.Parse(string.Concat((object)Math.Round(double.Parse(string.Concat((object)start_value)) / double.Parse(string.Concat((object)end_value)) * 100.0, 0, MidpointRounding.AwayFromZero)));
            if (start_value != end_value)
                num = 99;
            ProgBar.Value = num;
        }

        public static void SetMenuSeparators(object ContextMenuStripOrToolStripMenuItem)
        {
            bool flag1 = false;
            int num1 = -1;
            ToolStripItemCollection stripItemCollection = (ToolStripItemCollection)null;
            ContextMenuStrip contextMenuStrip = ContextMenuStripOrToolStripMenuItem as ContextMenuStrip;
            ToolStripMenuItem toolStripMenuItem1 = ContextMenuStripOrToolStripMenuItem as ToolStripMenuItem;
            if (contextMenuStrip != null)
                stripItemCollection = contextMenuStrip.Items;
            else if (toolStripMenuItem1 != null)
                stripItemCollection = toolStripMenuItem1.DropDownItems;
            if (contextMenuStrip == null && toolStripMenuItem1 == null)
                throw new SystemException("The object can only be a Menu Item or a Context menuStrip");
            for (int index = 0; index < stripItemCollection.Count; ++index)
            {
                ToolStripItem toolStripItem = stripItemCollection[index];
                ToolStripSeparator toolStripSeparator = toolStripItem as ToolStripSeparator;
                ToolStripMenuItem toolStripMenuItem2 = toolStripItem as ToolStripMenuItem;
                if (toolStripMenuItem2 != null && toolStripMenuItem2.Available && toolStripMenuItem2.DropDownItems.Count > 0)
                    StaticMethods.SetMenuSeparators((object)toolStripMenuItem2);
                int num2 = -1;
                if (toolStripSeparator != null)
                    num2 = 2;
                if (toolStripMenuItem2 != null && toolStripMenuItem2.Available)
                    num2 = 1;
                if (num1 == -1)
                {
                    if (num2 == 2)
                    {
                        flag1 = true;
                        break;
                    }
                    else if (num2 == 1)
                        num1 = num2;
                }
                if (num1 == 1)
                {
                    if (num2 == 1)
                    {
                        flag1 = true;
                        break;
                    }
                    else if (num2 == 2)
                        num1 = num2;
                }
                if (num1 == 2)
                {
                    if (num2 == 2)
                    {
                        flag1 = true;
                        break;
                    }
                    else if (num2 == 1)
                        num1 = num2;
                }
            }
            if (num1 == 2)
                flag1 = true;
            if (!flag1)
                return;
            ArrayList arrayList = new ArrayList(stripItemCollection.Count);
            for (int index = 0; index < stripItemCollection.Count; ++index)
            {
                ToolStripItem toolStripItem = stripItemCollection[index];
                ToolStripSeparator toolStripSeparator = toolStripItem as ToolStripSeparator;
                ToolStripMenuItem toolStripMenuItem2 = toolStripItem as ToolStripMenuItem;
                if (toolStripMenuItem2 != null)
                    arrayList.Add((object)toolStripMenuItem2);
            }
            stripItemCollection.Clear();
            for (int index1 = 0; index1 < arrayList.Count; ++index1)
            {
                bool flag2 = false;
                ToolStripMenuItem toolStripMenuItem2 = arrayList[index1] as ToolStripMenuItem;
                stripItemCollection.Add((ToolStripItem)toolStripMenuItem2);
                if (toolStripMenuItem2.Available)
                {
                    for (int index2 = index1 + 1; index2 < arrayList.Count; ++index2)
                    {
                        if ((arrayList[index2] as ToolStripMenuItem).Available)
                        {
                            flag2 = true;
                            break;
                        }
                    }
                }
                if (flag2)
                {
                    ToolStripSeparator toolStripSeparator = new ToolStripSeparator();
                    stripItemCollection.Add((ToolStripItem)toolStripSeparator);
                }
            }
        }

        public static void SetToolStripDropDownMenuSeparators(ToolStripMenuItem TooStri)
        {
        }

        public static void CreateColumnAndNumberRows(DataView dataView, string ColumnName)
        {
            DataTable table = dataView.Table;
            if (!table.Columns.Contains(ColumnName))
                table.Columns.Add(ColumnName);
            int index1 = table.Columns.IndexOf(ColumnName);
            for (int index2 = 0; index2 < dataView.Count; ++index2)
                dataView[index2][index1] = (object)(index2 + 1);
        }

        public static string[] GetMacAddresses(out string[] StrIPAddresses, out string[] StrDescriptions)
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            StrIPAddresses = new string[0];
            StrDescriptions = new string[0];
            ArrayList arrayList1 = new ArrayList(networkInterfaces.Length);
            ArrayList arrayList2 = new ArrayList(networkInterfaces.Length);
            ArrayList arrayList3 = new ArrayList(networkInterfaces.Length);
            for (int index = 0; index < networkInterfaces.Length; ++index)
            {
                if (networkInterfaces[index].OperationalStatus == OperationalStatus.Up)
                {
                    arrayList1.Add((object)networkInterfaces[index].GetPhysicalAddress().ToString());
                    if (networkInterfaces[index].GetIPProperties().GatewayAddresses.Count > 0)
                        arrayList2.Add((object)networkInterfaces[index].GetIPProperties().GatewayAddresses[0].Address.ToString());
                    else
                        arrayList2.Add((object)"");
                    arrayList3.Add((object)networkInterfaces[index].Description);
                }
            }
            string[] strArray = new string[arrayList1.Count];
            StrIPAddresses = new string[arrayList2.Count];
            StrDescriptions = new string[arrayList3.Count];
            arrayList1.CopyTo((Array)strArray, 0);
            arrayList2.CopyTo((Array)StrIPAddresses, 0);
            arrayList3.CopyTo((Array)StrDescriptions, 0);
            StringBuilder stringBuilder = new StringBuilder();
            for (int index1 = 0; index1 < strArray.Length; ++index1)
            {
                char[] chArray = strArray[index1].ToCharArray();
                stringBuilder.Remove(0, stringBuilder.Length);
                int num = 0;
                for (int index2 = 0; index2 < chArray.Length; ++index2)
                {
                    stringBuilder.Append(chArray[index2]);
                    ++num;
                    if (num >= 2)
                    {
                        num = 0;
                        if (index2 + 1 < chArray.Length)
                            stringBuilder.Append(":");
                    }
                }
                strArray[index1] = ((object)stringBuilder).ToString();
            }
            return strArray;
        }

        public static void TrimEndEmptyRows(DataTable dataTable)
        {
            ArrayList arrayList = new ArrayList(dataTable.Rows.Count);
            for (int index = 0; index < dataTable.DefaultView.Count; ++index)
            {
                DataRow row = dataTable.DefaultView[index].Row;
                bool flag = StaticMethods.IsRowCompletelyBlank(row, true);
                if (flag)
                    arrayList.Add((object)row);
                else if (!flag)
                    arrayList.Clear();
            }
            for (int index = 0; index < arrayList.Count; ++index)
            {
                DataRow row = arrayList[index] as DataRow;
                dataTable.Rows.Remove(row);
            }
        }

        public static string ConvertFromEscapedXMLTags(string XML_Text)
        {
            XML_Text = (XML_Text ?? "").Replace("&lt;", "<").Replace("&gt;", ">");
            return XML_Text;
        }

        public static string ConvertToEscapedXMLTags(string XML_Text)
        {
            XML_Text = (XML_Text ?? "").Replace("<", "&lt;").Replace(">", "&gt;");
            return XML_Text;
        }

        public static string[] GetAllStringsBetweenTags(string Str1, string StartTag, string EndTag, bool Trim)
        {
            ArrayList arrayList = new ArrayList(1000);
            string oldValue = "{\n" + 'þ' + 'þ' + 'þ' + "" + DateTime.Now.Ticks + "\n}";
            Str1 = Str1.Replace(StartTag + EndTag, StartTag + oldValue + EndTag);
            if (Str1.IndexOf(StartTag) != -1)
            {
                Str1 = Str1.Substring(Str1.IndexOf(StartTag));
                string[] strArray1 = Str1.Split(new string[1]
        {
          StartTag
        }, StringSplitOptions.RemoveEmptyEntries);
                for (int index = 0; index < strArray1.Length; ++index)
                {
                    if (strArray1[index].IndexOf(EndTag) != -1)
                    {
                        string[] strArray2 = strArray1[index].Split(new string[1]
            {
              EndTag
            }, StringSplitOptions.RemoveEmptyEntries);
                        if (strArray2.Length > 0)
                        {
                            if (Trim)
                                strArray2[0] = strArray2[0].Trim();
                            arrayList.Add((object)strArray2[0]);
                        }
                    }
                }
            }
            string[] strArray = new string[arrayList.Count];
            arrayList.CopyTo((Array)strArray, 0);
            for (int index = 0; index < strArray.Length; ++index)
                strArray[index] = strArray[index].Replace(oldValue, "");
            return strArray;
        }

        public static string[] GetXMLBetweenTags(string StrInput, string TagName, bool Trim)
        {
            TagName = TagName.Trim('<', '>');
            string StartTag = "<" + TagName + ">";
            string EndTag = "</" + TagName + ">";
            return StaticMethods.GetAllStringsBetweenTags(StrInput, StartTag, EndTag, Trim);
        }

        public static void DoThreadSleepAccurate(int SleepTime)
        {
            if (double.Parse(string.Concat((object)SleepTime)) % 100.0 == 0.0)
            {
                while (SleepTime > 0)
                {
                    SleepTime -= 100;
                    Thread.Sleep(100);
                }
            }
            else
            {
                int millisecondsTimeout = int.Parse(string.Concat((object)(double.Parse(string.Concat((object)SleepTime)) % 100.0)));
                int num = SleepTime - millisecondsTimeout;
                while (num > 0)
                {
                    num -= 100;
                    Thread.Sleep(100);
                }
                Thread.Sleep(millisecondsTimeout);
            }
        }

        public static string TrimEndOrStart(string Subject, string Search, bool TrimEnd)
        {
            if (TrimEnd)
            {
                if (!Subject.EndsWith(Search))
                    return Subject;
                Subject = StaticMethods.ReverseString(Subject);
                while (true)
                {
                    string[] strArray = Subject.Split(new string[1]
          {
            StaticMethods.ReverseString(Search)
          }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (strArray.Length == 1 && !Subject.Equals(strArray[0], StringComparison.Ordinal))
                        Subject = strArray[0];
                    else
                        break;
                }
                Subject = StaticMethods.ReverseString(Subject);
                return Subject;
            }
            else
            {
                if (!Subject.StartsWith(Search))
                    return Subject;
                while (true)
                {
                    string[] strArray = Subject.Split(new string[1]
          {
            Search
          }, 2, StringSplitOptions.RemoveEmptyEntries);
                    if (!Subject.Equals(strArray[0], StringComparison.Ordinal))
                        Subject = strArray[0];
                    else
                        break;
                }
                return Subject;
            }
        }

        public static int GetAsPercentage(object Int1, object Int2)
        {
            double num1 = double.Parse(string.Concat(Int1));
            double num2 = double.Parse(string.Concat(Int2));
            if (num1 == 0.0 && num2 == 0.0)
                return 0;
            double num3 = Math.Round(num1 / num2 * 100.0, 0);
            if (num3 == 100.0 && num2 != num1)
                num3 = 99.0;
            return int.Parse(string.Concat((object)num3));
        }

        public static void SetToolTip(Control _ControlToSetToolTip, string Text)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(_ControlToSetToolTip, Text);
            toolTip.UseAnimation = true;
            toolTip.UseFading = true;
        }

        public static bool ShowPopUpOpenFileDialog(Form For, ref string StartFilePath, bool check_if_file_exists, bool check_if_path_exists, bool allow_multiplefiles, long MaxFileSize, string FileFilters, string CustomTitle, out string[] FilesChosen)
        {
        label_1:
            string str = string.Empty;
            FilesChosen = new string[0];
            if (CustomTitle == null)
                CustomTitle = "Select a file";
            StaticMethods.GetDirectory(StartFilePath, out StartFilePath);
            if (StartFilePath == null)
                StartFilePath = "C:\\";
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = FileFilters;
            openFileDialog.InitialDirectory = StartFilePath;
            openFileDialog.CheckFileExists = check_if_file_exists;
            openFileDialog.CheckPathExists = check_if_path_exists;
            openFileDialog.Multiselect = allow_multiplefiles;
            openFileDialog.Title = CustomTitle;
            if (openFileDialog.ShowDialog((IWin32Window)For) != DialogResult.OK)
                return false;
            if (!openFileDialog.Multiselect)
            {
                StartFilePath = openFileDialog.FileName;
                FilesChosen = new string[1];
                FilesChosen[0] = StartFilePath;
            }
            else
            {
                FilesChosen = openFileDialog.FileNames;
                StartFilePath = "";
                for (int index = 0; index < FilesChosen.Length; ++index)
                {
                    StartFilePath = StartFilePath + FilesChosen[index];
                    if (index + 1 < FilesChosen.Length)
                        StartFilePath = StartFilePath + "|";
                }
            }
            for (int index = 0; index < FilesChosen.Length; ++index)
            {
                string fileName = FilesChosen[index];
                if (MaxFileSize != -1L && new FileInfo(fileName).Length > MaxFileSize)
                {
                    //MessageBox.AllowCustomLookAndFeel = true;
                    if (MessageBox.Show((IWin32Window)For, "The file is too large", "Please select another file", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Cancel)
                        return false;
                    else
                        goto label_1;
                }
            }
            return true;
        }

        public static ToolStripItem GetToolStripWithTextCaseInsensitive(object ContextStripToolStrip, string Text)
        {
            ContextMenuStrip contextMenuStrip = ContextStripToolStrip as ContextMenuStrip;
            ToolStripMenuItem toolStripMenuItem = ContextStripToolStrip as ToolStripMenuItem;
            ToolStripItem toolStripItem = ContextStripToolStrip as ToolStripItem;
            if (toolStripItem != null && toolStripItem.Text.Equals(Text, StringComparison.OrdinalIgnoreCase))
                return toolStripItem;
            if (toolStripMenuItem != null)
            {
                if (toolStripMenuItem.Text.Equals(Text, StringComparison.OrdinalIgnoreCase))
                    return (ToolStripItem)toolStripMenuItem;
                for (int index = 0; index < toolStripMenuItem.DropDownItems.Count; ++index)
                {
                    if (toolStripMenuItem.DropDownItems[index].Text.Equals(Text, StringComparison.OrdinalIgnoreCase))
                        return toolStripMenuItem.DropDownItems[index];
                    ToolStripItem textCaseInsensitive = StaticMethods.GetToolStripWithTextCaseInsensitive((object)toolStripMenuItem.DropDownItems[index], Text);
                    if (textCaseInsensitive != null)
                        return textCaseInsensitive;
                }
            }
            if (contextMenuStrip != null)
            {
                for (int index = 0; index < contextMenuStrip.Items.Count; ++index)
                {
                    if (contextMenuStrip.Items[index].Text.Equals(Text, StringComparison.OrdinalIgnoreCase))
                        return contextMenuStrip.Items[index];
                    ToolStripItem textCaseInsensitive = StaticMethods.GetToolStripWithTextCaseInsensitive((object)contextMenuStrip.Items[index], Text);
                    if (textCaseInsensitive != null)
                        return textCaseInsensitive;
                }
            }
            if (toolStripMenuItem == null && toolStripItem == null && contextMenuStrip == null)
                throw new Exception("An object of a ContextMenuStrip or ToolStripMenuItem is expected!\n" + ContextStripToolStrip);
            else
                return (ToolStripItem)null;
        }

        public static Point GetLocationOnForm(Control control)
        {
            return control.FindForm().PointToClient(control.Parent.PointToScreen(control.Location));
        }

        public static Image Resize(Image img, Size NewSize)
        {
            int width1 = img.Width;
            int height1 = img.Height;
            int width2 = NewSize.Width;
            int height2 = NewSize.Height;
            if (height2 == height1 && width2 == width1)
                return img;
            Bitmap bitmap = new Bitmap(width2, height2);
            Graphics graphics = Graphics.FromImage((Image)bitmap);
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.DrawImage(img, 0, 0, width2, height2);
            graphics.Dispose();
            return (Image)bitmap;
        }


        /// <summary>
        /// Reads all text from the current text path
        /// </summary>
        /// <param name="CurrentTextPath"></param>
        /// <returns></returns>
        public static string ReadAllText(string CurrentTextPath)
        {
            StreamReader StreamReader = new StreamReader(CurrentTextPath);
            string Text = StreamReader.ReadToEnd();
            StreamReader.Close();
            StreamReader.Dispose();
            return Text;

        }

        /// <summary>
        /// Returns this to CSV
        /// </summary>
        /// <param name="THELIST"></param>
        /// <returns></returns>
        public static string ToCSV(List<string> THELIST)
        {
            StringBuilder TheBuilder = new StringBuilder(1000);
            for (int index = 0; index < THELIST.Count; index++)
            {
                if (THELIST[index].Contains("\""))
                {
                    THELIST[index] = THELIST[index].Replace("\"", "\"\"");
                }
                if (THELIST[index].Contains(","))
                {
                    TheBuilder.Append("\"" + THELIST[index] + "\"");
                }
                else
                {
                    TheBuilder.Append(THELIST[index]);
                }
                if (index + 1 < THELIST.Count)
                {
                    TheBuilder.Append(",");
                }
            }
            return TheBuilder.ToString();
        }


        /// <summary>
        /// Creates a datetime from a Unix TimeStamp?
        /// </summary>
        /// <param name="TimeStamp">Unix TimeStamp</param>
        /// <returns></returns>
        public static DateTime CreateDateTimeFromMySQLUnixTimeStamp(object TimeStamp)
        {
            double TheTimeStamp = double.Parse("" + TimeStamp);
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            dtDateTime = dtDateTime.AddSeconds(TheTimeStamp);
            return dtDateTime;
        }

        /// <summary>
        /// Does a string comparison that ignores spaces and case rules
        /// </summary>
        /// <param name="Subject">The subject</param>
        /// <param name="CompareWith">What to compare with</param>
        /// <returns>Returns true if they are equal,</returns>
        public static bool CompareStringsNoSpacesCaseInsensitive(string Subject,
            string CompareWith)
        {
            Subject = Subject.Replace(" ", string.Empty).Trim();
            Subject = Subject.Replace(" ", string.Empty);
            Subject = Subject.Replace(" ", string.Empty);
            Subject = Subject.ToUpper();

            CompareWith = CompareWith.Replace(" ", string.Empty).Trim();
            CompareWith = CompareWith.Replace(" ", string.Empty);
            CompareWith = CompareWith.Replace(" ", string.Empty);
            CompareWith = CompareWith.ToUpper();

            return Subject.Equals(CompareWith, StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>
        /// Writes a chunk of bytes to a file
        /// </summary>
        /// <param name="p"></param>
        /// <param name="BytesPDF"></param>
        public static void WriteBytesToFile(string FilePath, byte[] FileBytes)
        {
            FileStream NewFileStream = null;
            try
            {
                NewFileStream = new FileStream(FilePath, FileMode.Create);
                NewFileStream.Write(FileBytes, 0, FileBytes.Length);
                NewFileStream.Flush();
            }
            catch (Exception em)
            {
                throw em;
            }
            finally
            {
                try
                {
                    if (NewFileStream != null)
                    {
                        NewFileStream.Close();
                        NewFileStream.Dispose();
                    }
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// Writes the text to file
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="STRXML"></param>
        public static void WriteTextToFile(string Path, string STR)
        {
            StreamWriter STR_WRI = new StreamWriter(Path);
            STR_WRI.Write(STR);
            STR_WRI.Flush();
            STR_WRI.Close();
        }


        /// <summary>
        /// Swaps the dates
        /// </summary>
        /// <param name="StartTime">The StartTime</param>
        /// <param name="EndTime">The EndTime</param>
        public static void SwapDates(ref DateTime StartTime,
            ref DateTime EndTime)
        {
            DateTime TempDateTime = StartTime;
            if (StartTime.Ticks > EndTime.Ticks)
            {
                StartTime = EndTime;
                EndTime = TempDateTime;
            }
        }

        internal static void KillProgramIfMoreThanOneInstanceDetected(string MutexID)
        {
            bool result;
            var mutex = new System.Threading.Mutex(true, MutexID, out result);
            if (!result)
            {
                StaticMethods.KillAllThreads();
                return;
            }
            GC.KeepAlive(mutex);
        }

        /// <summary>
        /// Suggests the best filename to use
        /// </summary>
        /// <param name="DestinationPath">Begin folder path</param>
        /// <param name="Filename">Filename</param>
        /// <returns>Returns full file name</returns>
        internal static string SuggestGoodFileNameForDirectory(string DestinationPath, 
            string Filename)
        {
            int counter = 1;
            string TestPath = DestinationPath + "\\" + Filename;
            while (File.Exists(TestPath) == true)
            {
                TestPath = DestinationPath + "\\" + Path.GetFileNameWithoutExtension(Filename) + " (" + counter + ")" + "."+Path.GetExtension(Filename).Trim(new char[]{'.'});
                counter = counter + 1;
            }
            return TestPath;
        }
    }
}
