using System.IO;

namespace TranslationToolKit.Business
{
    public static class ValidationHelper
    {
        /// <summary>
        /// Check that the file provided (in the form of a path),
        /// is a valid file for analysis.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool IsFileValid(string path, out string error)
        {
            error = "";
            ValidationHelper.DoesFileExist(path, ref error);
            return (error == "");
        }

        /// <summary>
        /// Verify that a file exists, otherwise write an error.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="error"></param>
        public static void DoesFileExist(string path, ref string error)
        {
            if (!File.Exists(path))
            {
                error += $"The file {path} doesn't exist";
            };
        }


        /// <summary>
        /// Set a proper file path from the path we were provided
        /// (in particular, we root it when there is just a file name).
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string SetFilePath(string path)
        {
            return Path.GetFullPath(path);
        }
    }
}
