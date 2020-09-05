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
            List<string> currentSection = new List<string>();
            Section section;
            foreach (var line in file)
            {
                if (line.StartsWith("["))
                {
                    if (currentSection.Count != 0)
                    {
                        section = SectionParser.ParseSection(currentSection);
                        sections.Add(section.Title, section);
                    }                    
                    currentSection = new List<string>() { line };
                }
                else
                {
                    currentSection.Add(line);
                }
            }
            section = SectionParser.ParseSection(currentSection);
            sections.Add(section.Title, section);
            return sections;
        }
    }
}
