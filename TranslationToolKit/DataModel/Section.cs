using System;
using System.Collections.Generic;
using System.Text;

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


        private IDictionary<string, Line> lines;
        /// <summary>
        /// List of lines in the section
        /// </summary>
        public IDictionary<string, Line> Lines => lines ??= new Dictionary<string, Line>();

        /// <summary>
        /// A constant that is used to generate a title for empty lines, so we can track and keep them.
        /// </summary>
        public const string EmptyLineTitlePrefix = "ToolKitEmptyLineSection";

        private int suffix = 0;
        /// <summary>
        /// The generated suffix for empty lines
        /// </summary>
        private int EmptyLineSuffix => suffix++;

        public void AddEmptyLine()
        {
            var header = $"{EmptyLineTitlePrefix}{EmptyLineSuffix}";
            Lines.Add(header, new Line(header, ""));
        }
    }
}
