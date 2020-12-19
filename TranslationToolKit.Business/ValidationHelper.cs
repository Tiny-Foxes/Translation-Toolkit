using System.IO;

namespace TranslationToolKit.Business
{
    public static class ValidationHelper
    {
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
