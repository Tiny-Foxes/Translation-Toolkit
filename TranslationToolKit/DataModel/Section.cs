using System.Collections.Generic;

namespace TranslationToolKit.DataModel
{
    /// <summary>
    /// Represents a section in the translation line.
    /// The section has a title, and a list of lines.
    /// </summary>
    public class Section
    {
        /// <summary>
        /// Title of the section. Ex: [ScreenPracticeMenu]
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// List of lines in the section
        /// </summary>
        public SortedDictionary<Header, Line> Lines { get; }

        private int emptyLineOccurences = 0;
        /// <summary>
        /// The generated suffix for empty lines
        /// </summary>
        private int EmptyLineOccurences => emptyLineOccurences++;

        public Section()
        {           
            Title = "";
            Lines = new SortedDictionary<Header, Line>();
        }

        /// <summary>
        /// When file has an empty line, keep track of it with a generated header name,
        /// so we can retranscribe it later on (translation sections may be divided in blocks)
        /// </summary>
        /// <param name="index">index of this line within the section</param>
        public void AddEmptyLine(int index)
        {
            var header = new Header("", EmptyLineOccurences, index);
            Lines.Add(header, new Line(string.Empty, string.Empty));
        }

        /// <summary>
        /// Add a line to the section.
        /// </summary>
        /// <param name="line">the line to be added</param>
        /// <param name="index">index of this line within the section</param>
        public void AddLine(Line line, int index)
        {
            Lines.Add(new Header(line.Key, 0, index), line);
        }
    }
}
