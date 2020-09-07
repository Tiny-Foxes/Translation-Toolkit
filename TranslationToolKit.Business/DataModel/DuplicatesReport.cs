using System.Collections.Generic;
using System.Text;

namespace TranslationToolKit.Business.DataModel
{
    /// <summary>
    /// A report detailing what kind of duplicates (if any)
    /// was found in a file.
    /// </summary>
    public class DuplicatesReport
    {
        /// <summary>
        /// Path of the file
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// The list of duplicated sections (by their name)
        /// </summary>
        public List<string> DuplicatedSections { get; set; }

        /// <summary>
        /// The list of the duplicated names (Section Name + Line Title)
        /// </summary>
        public List<KeyValuePair<string, string>> DuplicatedLines { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DuplicatesReport()
        {
            FilePath = "";
            DuplicatedSections = new List<string>();
            DuplicatedLines = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Get a nice display string to print this report.
        /// </summary>
        /// <returns></returns>
        public string GetDisplayString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("= Duplicates Report =");
            builder.AppendLine();

            // Sections
            builder.AppendLine("== Duplicated Section(s) Found ==");
            if (DuplicatedSections.Count == 0)
            {
                builder.AppendLine("No duplicate found !");
            }
            else
            {
                foreach (var section in DuplicatedSections)
                {
                    builder.AppendLine(section);
                }
            }
            builder.AppendLine();

            // Lines
            builder.AppendLine("== Duplicated Line(s) Found ==");
            if (DuplicatedLines.Count == 0)
            {
                builder.AppendLine("No duplicate found !");
            }
            else
            {
                foreach (var line in DuplicatedLines)
                {
                    builder.AppendLine($"{line.Key}::{line.Value}");
                }
            }
            builder.AppendLine();

            return builder.ToString();
        }
    }
}
