using System;
using System.Collections;
using System.IO;
using System.Text;
using TranslationToolKit.DataModel;

namespace TranslationToolKit
{
    /// <summary>
    /// A writer for our file, using Unicode and Unix style endings.
    /// </summary>
    public class FileWriter
    {
        public static void Write(ParsedFile file, string destination)
        {
            var directoryName = Path.GetDirectoryName(destination);
            if(!Directory.Exists(directoryName))
            {
                throw new ArgumentException($"Directory {directoryName} doesn't exist", destination);
            }

            var writer = new StreamWriter(destination,false,new UTF8Encoding(false));
            try
            {
                writer.Write(file.FileHeader);

                foreach (var sectionData in file)
                {
                    var section = sectionData.Value;

                    writer.Write(section.Title);
                    writer.Write(EnvironmentConstants.EndOfLine);

                    foreach (var lineData in section)
                    {
                        writer.Write(lineData.Value.DisplayString);
                        writer.Write(EnvironmentConstants.EndOfLine);
                    }
                }
            }
            finally
            {
                writer.Close();
            }
        }
    }
}
