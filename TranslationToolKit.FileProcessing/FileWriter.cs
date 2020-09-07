using System;
using System.IO;
using System.Text;
using TranslationToolKit.FileProcessing.DataModel;

namespace TranslationToolKit.FileProcessing
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

                    // Header
                    if(section.SectionComment != string.Empty)
                    {
                        writer.Write(section.SectionComment);
                        writer.Write(EnvironmentConstants.EndOfLine);
                    }
                    // Title
                    writer.Write(section.Title);
                    writer.Write(EnvironmentConstants.EndOfLine);
                    // Lines
                    foreach (var lineData in section)
                    {
                        writer.Write(lineData.Value.DisplayString);
                        writer.Write(EnvironmentConstants.EndOfLine);
                    }
                    // Suffix
                    if (section.SectionSuffix != string.Empty)
                    {
                        writer.Write(section.SectionSuffix);
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
