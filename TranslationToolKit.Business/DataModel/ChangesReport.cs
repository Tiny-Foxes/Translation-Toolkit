using System;
using System.Collections.Generic;
using System.Text;

namespace TranslationToolKit.Business.DataModel
{
    /// <summary>
    /// A report detailing what are the differences between a reference file and a target file (if any)
    /// </summary>
    public class ChangesReport
    {
        /// <summary>
        /// Path of the reference file
        /// </summary>
        public string ReferencePath { get; set; }

        /// <summary>
        /// Path of the target file
        /// </summary>
        public string TargetPath { get; set; }

        /// <summary>
        /// List of sections that are in the reference file but not in the target file
        /// </summary>
        public List<string> NewSections { get; set; }

        /// <summary>
        /// List of sections that are in the the target file but not in the reference file
        /// </summary>
        public List<string> DeletedSections { get; set; }

        /// <summary>
        /// List of sections that are in the reference file but not in the target file
        /// </summary>
        public List<KeyValuePair<string, string>> NewLines { get; set; }

        /// <summary>
        /// List of lines that are in the the target file but not in the reference file
        /// </summary>
        public List<KeyValuePair<string, string>> DeletedLines { get; set; }


        public bool IssuesFound
        {
            get 
            {
                return DeletedSections.Count != 0
                    || DeletedLines.Count != 0
                    || NewSections.Count != 0
                    || NewLines.Count != 0;
            }
        }

        public ChangesReport()
        {
            NewSections = new List<string>();
            NewLines = new List<KeyValuePair<string, string>>();

            DeletedSections = new List<string>();
            DeletedLines = new List<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Get a nice display string to print this report.
        /// </summary>
        /// <returns></returns>
        public string GetDisplayString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("= Changes Report =");
            builder.AppendLine();

            // Sections
            builder.AppendLine("== New section(s) available in master but absent in target ==");
            if (NewSections.Count == 0)
            {
                builder.AppendLine("Nothing missing :)");
            }
            else
            {
                foreach (var section in NewSections)
                {
                    builder.AppendLine(section);
                }
            }
            builder.AppendLine("== Deleted section(s) in master but still in target ==");
            if (DeletedSections.Count == 0)
            {
                builder.AppendLine("No deletion found :)");
            }
            else
            {
                foreach (var section in DeletedSections)
                {
                    builder.AppendLine(section);
                }
            }
            builder.AppendLine();

            // Lines
            builder.AppendLine("== New lines(s) available in master but absent in target ==");
            if (NewLines.Count == 0)
            {
                builder.AppendLine("Nothing missing :)");
            }
            else
            {
                foreach (var line in NewLines)
                {
                    builder.AppendLine($"{line.Key}::{line.Value}");
                }
            }
            builder.AppendLine("== Deleted lines(s) in master but still in target ==");
            if (DeletedLines.Count == 0)
            {
                builder.AppendLine("No deletion found :)");
            }
            else
            {
                foreach (var line in DeletedLines)
                {
                    builder.AppendLine($"{line.Key}::{line.Value}");
                }
            }
            builder.AppendLine();

            return builder.ToString();
        }
    }
}
