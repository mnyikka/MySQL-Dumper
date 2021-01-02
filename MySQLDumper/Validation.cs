using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace DataScraper
{
    public static class Validation
    {
        /// <summary>
        /// Validates required?
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="Error"></param>
        /// <returns></returns>
        public static bool ValidateRequired(string subject, out string Error,
            string Description)
        {
            Error = "";
            subject = ("" + subject).Trim();
            if (subject.Length > 0)
            {
                return true;
            }
            Error = "Please put a value for the\n" + Description + "";
            return false;
        }


        /// <summary>
        /// Validates an integer
        /// </summary>
        /// <param name="subject">The subject to validate for</param>
        /// <param name="Error">The error to throw incase validation fails</param>
        public static void ValidateLong(string subject, string Error)
        {
            try
            {
                long TheLong = long.Parse(subject.Trim());
            }
            catch (Exception em)
            {
                throw new Exception(Error);
            }
        }
        /// <summary>
        /// Validate exists
        /// </summary>
        /// <param name="subject">The subject</param>
        /// <param name="Error">The error if any</param>
        /// <param name="Subject">The subject</param>
        /// <returns></returns>
        public static bool ValidateFolderPathExists(string subject, out string Error, string Description)
        {
            Error = "";
            try
            {
                DirectoryInfo Dir = new DirectoryInfo(subject);
                if (Dir.Exists == false) throw new Exception("Does not exist!");
                return true;
            }
            catch (Exception em)
            {
                Error = "The " + Description + " does not exist!\nor cannot be accessed.";
            }
            return false;
        }



        /// <summary>
        /// Throws an exception if the string is empty or null
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        public static void ValidateRequired(string StringToCheck, string ErrorMessage)
        {
            StringToCheck = StringToCheck.Trim();
            if (StringToCheck.Length == 0) throw new Exception(ErrorMessage);
        }

        /// <summary>
        /// Validates existence of folder paths
        /// </summary>
        /// <param name="p"></param>
        /// <param name="p_2"></param>
        public static void ValidateFolderPathExists(string FolderPath, string ErrorMessage)
        {
            try
            {
                string ErrorMessage2 = "";
                if (Directory.Exists(FolderPath) == false)
                {
                    //Create the folder in steps
                    string[] Splitter = FolderPath.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries);

                    string NewPath = Splitter[0];
                    if (NewPath.EndsWith(":"))
                    {
                        NewPath = NewPath + "\\" + Splitter[1];
                    }
                    if (Directory.Exists(NewPath) == false)
                    {
                        Directory.CreateDirectory(NewPath);
                    }
                    for (int index = 2; index < Splitter.Length; index++)
                    {
                        NewPath = NewPath + "\\" + Splitter[index];
                        if (Directory.Exists(NewPath) == false)
                        {
                            Directory.CreateDirectory(NewPath);
                        }
                    }

                }

                if (Validation.ValidateFolderPathExists(FolderPath, out ErrorMessage2, "folder") == false)
                {
                    throw new Exception(ErrorMessage);
                }
            }
            catch
            {
                throw new Exception(ErrorMessage);
            }
        }






        /// <summary>
        /// Validates whether a filepath exists, if not an error message is produced
        /// </summary>
        /// <param name="PathToTest">The path to test</param>
        /// <param name="ErrorMessage">The Error Message</param>
        public static void ValidateFilePathExists(string PathToTest, string ErrorMessage)
        {
            try
            {
                if (File.Exists(PathToTest) == false)
                {
                    throw new Exception(ErrorMessage);
                }
            }
            catch (Exception em)
            {
                throw new Exception(ErrorMessage);
            }
        }


        /// <summary>
        /// Validates the length of a string
        /// </summary>
        /// <param name="Subject">The string</param>
        /// <param name="RequiredLength">The maximum length</param>
        /// <param name="ErrorMessage">The error message should the validation fail</param>
        public static void ValidateLength(string Subject, int RequiredLength, string ErrorMessage)
        {
            if (Subject.Trim().Length > RequiredLength)
            {
                throw new Exception(ErrorMessage);
            }
        }



        /// <summary>
        /// Validates the email address
        /// </summary>
        /// <param name="EmailAddress">The email address inputted</param>
        /// <param name="ErrorMessage">The validation error message</param>
        public static void ValidateEmailAddress(string EmailAddress, string ErrorMessage)
        {
            try
            {
                Regex ValidEmailRegex = Validation.CreateValidEmailRegex();
                bool isValid = ValidEmailRegex.IsMatch(EmailAddress);
                if (isValid == false) throw new Exception(ErrorMessage);

                System.Net.Mail.MailAddress addr = new System.Net.Mail.MailAddress(EmailAddress);
                addr = null;
            }
            catch (Exception em)
            {
                throw new Exception(ErrorMessage);
            }
        }


        /// <summary>
        /// Create valid email regex
        /// </summary>
        /// <returns></returns>
        private static Regex CreateValidEmailRegex()
        {
            string validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
        }
    }
}

