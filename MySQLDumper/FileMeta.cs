using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RenameFileScript
{
    /// <summary>
    /// Creates a new file bank object
    /// </summary>
    public class FileMeta
    {

        private DateTime dtTimeDate;
        private string sender;
        private string subject;
        private FileInfo fileobject;

        /// <summary>
        /// New file meta object
        /// </summary>
        /// <param name="FilInfo">The fileinfo object</param>
        public FileMeta(FileInfo FilInfo)
        {
            this.fileobject = FilInfo;
            this.dtTimeDate = DateTime.MinValue;
            this.subject = string.Empty;
            this.sender = string.Empty;
            this.ParseData();
        }

        /// <summary>
        /// Parses the data required/
        /// </summary>
        private void ParseData()
        {
            string[] split = Path.GetFileNameWithoutExtension(this.fileobject.Name).Split(new char[]{'_'},3, 
                StringSplitOptions.None);
            //Get datetime
            try
            {
                string[] dt_time = split[0].Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                this.dtTimeDate = new DateTime(int.Parse(dt_time[0]),
                    int.Parse(dt_time[1]),
                    int.Parse(dt_time[2]),
                    0,
                    0,
                    0);
            }
            catch (Exception em12)
            {
 
            }

            try
            {
                this.sender = ((split[1]).Trim()).Replace("  ", " ").Replace("  ", " ");
            }
            catch (Exception em23)
            {
 
            }


            try
            {
                this.subject = (split[2]).Trim();
            }
            catch (Exception emmew)
            {
 
            }
        }


        /// <summary>
        /// The sender
        /// </summary>
        public string SENDER
        {
            get
            {
                return (""+(this.sender)).ToUpper().Trim();
            }
        }

        /// <summary>
        /// Date time created
        /// </summary>
        public DateTime CREATEDDATETIME
        {
            get
            {
                return this.dtTimeDate;
            }
        }


        /// <summary>
        /// The subject
        /// </summary>
        public string SUBJECT
        {
            get
            {
                return (""+this.subject).Trim();
            }
        }


        /// <summary>
        /// File object
        /// </summary>
        public FileInfo FILE
        {
            get
            {
                return this.fileobject;
            }
        }

    }
}
