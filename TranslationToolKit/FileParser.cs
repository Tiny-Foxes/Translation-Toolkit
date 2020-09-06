using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// <param name="fileLines">the file to parse, split into lines.</param>
        /// <returns></returns>
        public static ParsedFile ProcessFileIntoSections(string filePath)
        {
            var lines = File.ReadAllLines(filePath).ToList();
            return ProcessFileIntoSections(lines);
        }

        /// <summary>
        /// Parse a file into a list of sections.
        /// </summary>
        /// <param name="fileLines">the file to parse, split into lines.</param>
        /// <returns></returns>
        public static ParsedFile ProcessFileIntoSections(IList<string> fileLines)
        {
            var result = new ParsedFile();

            var parser = new SectionParser();

            if(fileLines.Count == 0)
            {
                return result;
            }

            int firstSectionIndex = 0;
            var header = string.Empty;
            while (firstSectionIndex < fileLines.Count && !fileLines[firstSectionIndex].StartsWith("["))
            {
                header += $"{fileLines[firstSectionIndex]}{EnvironmentConstants.EndOfLine}";
                firstSectionIndex++;
            }
            result.FileHeader = header;
            

            List<string> currentSection = new List<string>();
            Section section;
            var index = 0;            

            foreach (var line in fileLines)
            {
                // [ indicates the start of a section, we're storing sections one by one
                if (line.StartsWith("["))
                {
                    if (currentSection.Count != 0)
                    {
                        section = parser.ParseSection(currentSection);
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
                section = parser.ParseSection(currentSection);
                result.AddSection(section, index++);
            }
            return result;
        }
    }
}
