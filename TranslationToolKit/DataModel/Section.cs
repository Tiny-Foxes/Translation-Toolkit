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
        public IDictionary<string, Line> Lines { get; }

        /// <summary>
        /// A constant that is used to generate a title for empty lines, so we can track and keep them.
        /// </summary>
        public const string EmptyLineTitlePrefix = "ToolKitEmptyLineSection";

        private int suffix = 0;
        /// <summary>
        /// The generated suffix for empty lines
        /// </summary>
        private int EmptyLineSuffix => suffix++;

        public Section()
        {
            Lines = new Dictionary<string, Line>();
        }

        /// <summary>
        /// When file has an empty line, keep track of it with a generated header name,
        /// so we can retranscribe it later on (translation sections may be divided in blocks)
        /// </summary>
        public void AddEmptyLine()
        {
            var header = $"{EmptyLineTitlePrefix}{EmptyLineSuffix}";
            Lines.Add(header, new Line(header, ""));
        }
    }
}
