using System;
using System.Collections.Generic;
using System.Text;

namespace TranslationToolKit.DataModel
{
    /// <summary>
    /// A class representing a file parsed into sections.
    /// </summary>
    public class ParsedFile
    {
        /// <summary>
        /// List of all sections in the file
        /// </summary>
        public SortedDictionary<Header, Section> Sections { get; }

        public ParsedFile()
        {
            Sections = new SortedDictionary<Header, Section>();
        }

        /// <summary>
        /// Add a section to the file.
        /// </summary>
        /// <param name="line">the line to be added</param>
        /// <param name="index">index of this section within the file</param>
        public void AddSection(Section section, int index)
        {
            Sections.Add(new Header(section.Title, 0, index), section);
        }
    }
}
