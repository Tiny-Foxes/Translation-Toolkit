using System;
using System.Collections.Generic;
using System.Text;

namespace TranslationToolKit.Business.DataModel
{
    /// <summary>
    /// A report detailing what are the differences between a reference file and a target file (if any)
    /// </summary>
    public class CountReport
    {
        public int TotalLinesCount { get; set; }

        public int TranslatedLinesCount { get; set; }

        public double Percentage => Math.Round(TranslatedLinesCount* 100.0 / TotalLinesCount, 2);

        public CountReport()
        {
        }

        /// <summary>
        /// Get a nice display string to print this report.
        /// </summary>
        /// <returns></returns>
        public string GetDisplayString()
        {
            var builder = new StringBuilder();
            builder.AppendLine("= Count Report =");
            builder.AppendLine();
            // Sections
            builder.AppendLine($"We found {TotalLinesCount} lines in the reference file.");
            builder.AppendLine($"We found {TranslatedLinesCount} lines that you have translated in the target file.");

            return builder.ToString();
        }
    }
}
