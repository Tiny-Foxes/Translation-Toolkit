using System;
using System.Collections.Generic;
using System.Text;
using TranslationToolKit.DataModel;

namespace TranslationToolKit
{
    /// <summary>
    /// Parser for a translation file
    /// </summary>
    public class FileParser
    {
        /// <summary>
        /// Parse a file into a list of sections.
        /// </summary>
        /// <param name="file">the file to parse</param>
        /// <returns></returns>
        public static ParsedFile ProcessFileIntoSections(IList<string> file)
        {
            var result = new ParsedFile();

            if(file.Count == 0)
            {
                return result;
            }

            List<string> currentSection = new List<string>();
            Section section;
            var index = 0;

            foreach (var line in file)
            {
                // Notice the beginning of a section.
                if (line.StartsWith("["))
                {
                    if (currentSection.Count != 0)
                    {
                        section = SectionParser.ParseSection(currentSection);
                        result.AddSection(section, index++);
                    }                    
                    currentSection = new List<string>() { line };
                }
                else
                {
                    // Count != 0 means we have inserted a title, so we can
                    // start adding lines to that.
                    if(currentSection.Count != 0)
                    {
                        currentSection.Add(line);
                    }                    
                }
            }

            // Finish up with the last section
            if (currentSection.Count != 0)
            {
                section = SectionParser.ParseSection(currentSection);
                result.AddSection(section, index++);
            }
            return result;
        }
    }
}
