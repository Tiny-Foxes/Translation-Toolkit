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
        public static IDictionary<string, Section> ProcessFileIntoSections(IList<string> file)
        {
            var sections = new Dictionary<string, Section>();
            var sectionStarted = false;
            List<string> currentSection = null;
            for(int i = 0; i < file.Count; i++)
            {
                var line = file[i];

                if(line.StartsWith("["))
                {
                    sectionStarted = !sectionStarted;

                    if (sectionStarted)
                    {
                        currentSection = new List<string>() { line };
                    }
                    else
                    {
                        var section = SectionParser.ParseSection(currentSection);
                        sections.Add(section.Title, section);
                    }
                }
                else
                {
                    if(sectionStarted)
                    {
                        currentSection.Add(line);
                    }
                }
            }
            return sections;
        }
    }
}
