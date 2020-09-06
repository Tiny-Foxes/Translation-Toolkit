using System;
using System.Collections.Generic;
using System.Linq;
using TranslationToolKit.DataModel;

namespace TranslationToolKit
{
    /// <summary>
    /// Parser for a section from a translation file.
    /// </summary>
    public static class SectionParser
    {
        /// <summary>
        /// Parse the provided lines, which should cover a full section, into a section object
        /// with a title and a bunch of Lines
        /// </summary>
        /// <param name="lines">the lines of the section we want to parse</param>
        /// <returns></returns>
        public static Section ParseSection(List<string> lines)
        {
            if(lines.Count == 0)
            {
                throw new ArgumentException("Tried to parse section but provided lines list is empty", nameof(lines));
            }
            var section = new Section();

            var titleIndex = lines.FindIndex(x => x.StartsWith("["));
            if (titleIndex == -1)
            {
                throw new ArgumentException("Tried to parse section but no section title found", nameof(lines));
            }
            section.Title = lines[titleIndex];

            string comment = string.Empty;
            int currentIndex = 0;
            foreach (var line in lines.Skip(titleIndex+1))
            {
                ProcessLine(line.TrimStart(), section, ref currentIndex, ref comment);
            }
            return section;
        }

        /// <summary>
        /// Process one line of data
        /// </summary>
        /// <param name="line"></param>
        /// <param name="section"></param>
        /// <param name="comment"></param>
        private static void ProcessLine(string line, Section section, ref int currentIndex, ref string comment)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                section.AddEmptyLine(currentIndex++);
                return;
            }
            if (line.StartsWith("#"))
            {
                // Note: we don't increment index on comments because comments are added as part of the line they comment.
                comment += line;
                return;
            }

            int delimiterPosition = line.IndexOf('=');
            if (delimiterPosition >= 1)
            {
                var title = line.Substring(0, delimiterPosition);
                var value = line.Substring(delimiterPosition + 1);
                section.AddLine(new Line(title, value, comment), currentIndex++);
                comment = "";
            }
        }
    }
}
