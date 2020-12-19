using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// The file, once parsed. We store it during analysis,
        /// then we keep it in case we're asked for removing duplicates.
        /// </summary>
        public ParsedFile ParsedFile { get; private set; }

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
            ValidationHelper.DoesFileExist(path, ref error);
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
            Report.FilePath = ValidationHelper.SetFilePath(path);

            ParsedFile = FileParser.ProcessFileIntoSections(path);

            Report.DuplicatedSections = ParsedFile.Where(x => x.Key.OccurenceIndex != 0)
                                                    .Select(x => x.Value.Title)
                                                    .Distinct()
                                                    .ToList();

            foreach(var pair in ParsedFile)
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

        /// <summary>
        /// Generate a new file (same name, with .generated added at the end)
        /// that doesn't have any more duplicates.
        /// Must run analyzer before running this.
        /// </summary>
        /// <returns></returns>
        public string RemoveDuplicates()
        {
            if(Report.FilePath == "")
            {
                throw new ArgumentException("Report is empty, please run the analyzer first");
            }
            if(Report.DuplicatedLines.Count == 0 && Report.DuplicatedSections.Count == 0)
            {
                return "";
            }
            string newPath = $"{Report.FilePath}.generated";
            if(File.Exists(newPath))
            {
                throw new ArgumentException($"File {newPath} already exists");
            }

            var processedFile = ProcessDuplicates();

            FileWriter.Write(processedFile, newPath);

            return newPath;
        }

        /// <summary>
        /// Process the parsedFile, create a new ParsedFile that doesn't have duplicate.
        /// </summary>
        /// <returns></returns>
        private ParsedFile ProcessDuplicates()
        {
            var newFile = new ParsedFile
            {
                // Add the header
                FileHeader = ParsedFile.FileHeader
            };

            // Process the sections.
            var sectionsWithDuplicatedLines = Report.DuplicatedLines.Select(x => x.Key);
            int indexLag = 0;
            foreach (var sectionPair in ParsedFile)
            {
                var section = sectionPair.Value;
                if (Report.DuplicatedSections.Contains(section.Title))
                {
                    if (sectionPair.Key.OccurenceIndex > 0)
                    {
                        indexLag++;
                    }
                    else
                    {
                        var lines = ParsedFile.Where(x => x.Key.HeaderKey == section.Title)
                                                 .OrderBy(x => x.Key.OccurenceIndex)
                                                 .SelectMany(x => x.Value);


                        var newSection = CreateNewSectionWithNoDuplicate(section, lines);
                        newFile.AddSection(newSection, sectionPair.Key.Index - indexLag);
                    }
                }
                else
                {
                    if (sectionsWithDuplicatedLines.Contains(section.Title))
                    {
                        section = CreateNewSectionWithNoDuplicate(section, section);
                    }
                    newFile.AddSection(section, sectionPair.Key.Index - indexLag);
                }
            }

            return newFile;
        }

        /// <summary>
        /// Create a new section, based on an old one + a list of lines to add in the section.
        /// </summary>
        /// <param name="oldSection"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        private Section CreateNewSectionWithNoDuplicate(Section oldSection, IEnumerable<KeyValuePair<Header, Line>> lines)
        {
            var newSection = new Section()
            {
                Title = oldSection.Title,
                SectionComment = oldSection.SectionComment,
                SectionSuffix = oldSection.SectionSuffix
            };            
            int index = 0;
            foreach (var linePair in lines)
            {
                var header = linePair.Key;
                if(header.HeaderKey == "")
                {
                    newSection.AddEmptyLine(index++, linePair.Value.Comment);
                }
                if (!newSection.Any(x => x.Key.HeaderKey.Equals(header.HeaderKey)))
                {                    
                    newSection.AddLine(linePair.Value, index++);
                }
            }
            return newSection;
        }
    }
}
