using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RenameFileScript
{
    public class FileBank
    {
        /// <summary>
        /// 
        /// </summary>
        private SortedList<string, SortedList<DateTime, List<FileMeta>>> SorLisData;
        private int capacity;
        private int totalfilecount;
        /// <summary>
        /// Stores data about files 
        /// </summary>
        public FileBank(int capacity)
        {
            this.SorLisData = new SortedList<string, 
                SortedList<DateTime, List<FileMeta>>>(capacity);
            this.capacity = capacity;
            this.totalfilecount = 0;
        }


        /// <summary>
        /// Adds a file returns the count
        /// </summary>
        /// <param name="NewFile"></param>
        public void AddFile(FileMeta NewFile)
        {
            if (this.SorLisData.ContainsKey(NewFile.SENDER.ToUpper()) == false)
            {
                this.SorLisData.Add(NewFile.SENDER, 
                    new SortedList<DateTime, List<FileMeta>>(this.capacity / 2));
            }

            SortedList<DateTime,List<FileMeta>> Current = this.SorLisData[NewFile.SENDER.ToUpper()];

            if (Current.ContainsKey(NewFile.CREATEDDATETIME) == false)
            {
                Current.Add(NewFile.CREATEDDATETIME, 
                    new List<FileMeta>(this.capacity / 4));
            }

            Current[NewFile.CREATEDDATETIME].Add(NewFile);
            this.totalfilecount = this.totalfilecount + 1;
        }



        /// <summary>
        /// Finds a matching file
        /// Returns the first match found
        /// </summary>
        /// <param name="Search">The object to search from</param>
        /// <returns>Returns the closest file match</returns>
        public FileMeta FindMatch(FileMeta Search)
        {
            if (this.SorLisData.ContainsKey(Search.SENDER))
            {
                SortedList<DateTime,List<FileMeta>> Current = this.SorLisData[Search.SENDER];
                if (Current.ContainsKey(Search.CREATEDDATETIME))
                {
                    //Find by subject
                    List<FileMeta> LIST = Current[Search.CREATEDDATETIME];
                    for (int index = 0; index < LIST.Count; index++)
                    {
                        if (LIST[index].SUBJECT.Equals(Search.SUBJECT, StringComparison.Ordinal))
                        {
                            return LIST[index];
                        }
                    }

                    for (int index = 0; index < LIST.Count; index++)
                    {
                        if (LIST[index].SUBJECT.Equals(Search.SUBJECT, StringComparison.OrdinalIgnoreCase))
                        {
                            return LIST[index];
                        }
                    }

                    for (int index = 0; index < LIST.Count; index++)
                    {
                        if (Controller.RemoveUnnecessaryCharacters(LIST[index].SUBJECT).Equals(Controller.RemoveUnnecessaryCharacters(Search.SUBJECT), StringComparison.OrdinalIgnoreCase))
                        {
                           return LIST[index];
                        }
                    }

                    for (int index = 0; index < LIST.Count; index++)
                    {
                        if (Controller.RemoveUnnecessaryCharacters(Search.SUBJECT).StartsWith(Controller.RemoveUnnecessaryCharacters(LIST[index].SUBJECT), 
                            StringComparison.OrdinalIgnoreCase))
                        {
                            return LIST[index];
                        }
                    }

                    return null;
                }
                else
                {
                    return null;
                }
            }
            return null;
        }
    }
}
