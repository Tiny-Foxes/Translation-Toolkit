using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TranslationToolKit.Business
{
    /// <summary>
    /// This class is dedicated to find duplicate screens/translation lines 
    /// in a translation file.
    /// </summary>
    public class DuplicatesChecker
    {
        public static bool IsFileValid(string path, out string error)
        {
            error = "";
            if(!File.Exists(path))
            {
                error += $"The file {path} doesn't exist";
            }
            return (error == "");
        }

        public bool RunAnalyzer(string path)
        {
            if(!IsFileValid(path, out string error))
            {
                throw new ArgumentException($"Error while checking for duplicates: {error}", nameof(error));
            }
            return true;
        }
    }
}
