using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

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
        public IDictionary<Header, Line> Lines { get; }

        private int emptyLineOccurences = 0;
        /// <summary>
        /// The generated suffix for empty lines
        /// </summary>
        private int EmptyLineOccurences => emptyLineOccurences++;

        public Section()
        {
            Title = "";
            Lines = new Dictionary<Header, Line>();
        }

        /// <summary>
        /// When file has an empty line, keep track of it with a generated header name,
        /// so we can retranscribe it later on (translation sections may be divided in blocks)
        /// </summary>
        public void AddEmptyLine()
        {
            var header = new Header("", EmptyLineOccurences);
            Lines.Add(header, new Line(string.Empty, string.Empty));
        }

        /// <summary>
        /// Add a line to the section.
        /// </summary>
        /// <param name="line"></param>
        public void AddLine(Line line)
        {
            Lines.Add(new Header(line.Key, 0), line);
        }
    }
}
