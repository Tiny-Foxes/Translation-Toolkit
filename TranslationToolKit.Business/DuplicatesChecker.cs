using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using TranslationToolKit.Business.DataModel;
using TranslationToolKit.FileProcessing;
using TranslationToolKit.FileProcessing.DataModel;

namespace TranslationToolKit.Business
{
    /// <summary>
    /// This class process files to find duplicate screens/translation lines 
    /// in a translation file.
    /// </summary>
    public class DuplicatesChecker
    {
        /// <summary>
        /// Analysis report, detailing any duplicate in the file.
        /// </summary>
        public DuplicatesReport Report { get; private set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public DuplicatesChecker()
        {
            Report = new DuplicatesReport();
        }

        /// <summary>
        /// Check that the file provided (in the form of a path),
        /// is a valid file for analysis.
        /// </summary>
        /// <param name="path"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static bool IsFileValid(string path, out string error)
        {
            error = "";
            if(!File.Exists(path))
            {
                error += $"The file {path} doesn't exist";
            }
            return (error == "");
        }

        /// <summary>
        /// Analyze a file to find the duplicates in it.
        /// Doesn't modify the file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public DuplicatesReport RunAnalyzer(string path)
        {
            if(!IsFileValid(path, out string error))
            {
                throw new ArgumentException($"Error while checking for duplicates: {error}", nameof(error));
            }
            Report.FilePath = path;

            var parsedFile = FileParser.ProcessFileIntoSections(path);

            Report.DuplicatedSections = parsedFile.Where(x => x.Key.OccurenceIndex != 0)
                                                    .Select(x => x.Value.Title)
                                                    .Distinct()
                                                    .ToList();

            //foreach(var section in Report.DuplicatedSections)
            //{
            //    var sections = parsedFile.Where(x => x.Value.Title == section)
            //        .Select(x => x.Value);

            //    var duplicates = sections.SelectMany(x => x.Keys)
            //                       .GroupBy(x => x.HeaderKey)
            //                       .Where(g => g.Count() > 1)
            //                       .Select(y => new KeyValuePair<string,string>(section,y.Key))
            //                       .ToList();

            //    Report.DuplicatedLinesInDuplicatedSections.AddRange(duplicates);
            //}

            foreach(var pair in parsedFile)
            {
                var section = pair.Value;
                var duplicates = section.Where(x => x.Key.HeaderKey != string.Empty 
                                                    && x.Key.OccurenceIndex != 0)
                            .Select(x => x.Value.TranslationKey)
                            .Distinct()
                            .ToList();
                foreach(var duplicate in duplicates)
                {
                    Report.DuplicatedLines.Add(new KeyValuePair<string, string>(section.Title, duplicate));
                }
            }

            return Report;
        }
    }
}
