using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using TranslationToolKit.Business.DataModel;
using TranslationToolKit.FileProcessing;
using TranslationToolKit.FileProcessing.DataModel;

namespace TranslationToolKit.Business
{
    public class FileSynchronizer
    {
        /// <summary>
        /// Analysis report, detailing the difference between our two files.
        /// </summary>
        public ChangesReport Report { get; private set; }

        /// <summary>
        /// The reference file, once parsed. We store it during analysis,
        /// then we keep it in case we're asked to sync the files.
        /// </summary>
        public ParsedFile ParsedReferenceFile { get; private set; }

        /// <summary>
        /// The target file, once parsed. We store it during analysis,
        /// then we keep it in case we're asked to sync the files.
        /// </summary>
        public ParsedFile ParsedTargetFile { get; private set; }

        public FileSynchronizer()
        {
            Report = new ChangesReport();
        }

        /// <summary>
        /// Compare the reference file to the target file.
        /// Doesn't modify anything, just provide a report of differences.
        /// </summary>
        /// <param name="referencePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public ChangesReport RunAnalyzer(string referencePath, string targetPath)
        {
            if (!ValidationHelper.IsFileValid(referencePath, out string error))
            {
                throw new ArgumentException($"The provided reference file is wrong: {error}", nameof(error));
            }
            if (!ValidationHelper.IsFileValid(targetPath, out error))
            {
                throw new ArgumentException($"The provided target file is wrong: {error}", nameof(error));
            }
            Report.ReferencePath = ValidationHelper.SetFilePath(referencePath);
            Report.TargetPath = ValidationHelper.SetFilePath(targetPath);

            ParsedReferenceFile = FileParser.ProcessFileIntoSections(referencePath);
            ParsedTargetFile = FileParser.ProcessFileIntoSections(targetPath);

            foreach (var referencePair in ParsedReferenceFile)
            {
                var targetSectionHeader = ParsedTargetFile.Keys.FirstOrDefault(x => x.HeaderKey == referencePair.Key.HeaderKey);
                if (targetSectionHeader == null)
                {
                    Report.NewSections.Add(referencePair.Value.Title);
                }
                else
                {
                    var targetSection = ParsedTargetFile[targetSectionHeader.Index];
                    var referenceSection = referencePair.Value;
                    foreach (var linePair in referenceSection)
                    {
                        if (!targetSection.Keys.Any(x => x.HeaderKey == linePair.Key.HeaderKey))
                        {
                            Report.NewLines.Add(new KeyValuePair<string, string>(referencePair.Value.Title, linePair.Value.TranslationKey));
                        }
                    }

                    foreach (var linePair in targetSection)
                    {
                        if (!referenceSection.Keys.Any(x => x.HeaderKey == linePair.Key.HeaderKey))
                        {
                            Report.DeletedLines.Add(new KeyValuePair<string, string>(referencePair.Value.Title, linePair.Value.TranslationKey));
                        }
                    }
                }
            }

            foreach (var sectionPair in ParsedTargetFile)
            {
                if (!ParsedReferenceFile.Keys.Any(x => x.HeaderKey == sectionPair.Key.HeaderKey))
                {
                    Report.DeletedSections.Add(sectionPair.Value.Title);
                }
            }

            return Report;
        }

        /// <summary>
        /// Generate a new file (same name, with .generated added at the end)
        /// that is up-to-date with the reference file
        /// Must run analyzer before running this.
        /// </summary>
        /// <returns></returns>
        public string SynchronizeFile()
        {
            if (Report.TargetPath == "")
            {
                throw new ArgumentException("Report is empty, please run the analyzer first");
            }
            if (!Report.IssuesFound)
            {
                return "";
            }
            string newPath = $"{Report.TargetPath}.generated";
            if (File.Exists(newPath))
            {
                throw new ArgumentException($"File {newPath} already exists");
            }

            var processedFile = SyncFile();

            FileWriter.Write(processedFile, newPath);

            return newPath;
        }

        /// <summary>
        /// Sync the target file.
        /// </summary>
        /// <returns></returns>
        private ParsedFile SyncFile()
        {
            var newFile = new ParsedFile
            {
                FileHeader = ParsedTargetFile.FileHeader
            };

            foreach(var referenceSectionPair in ParsedReferenceFile)
            {
                var sectionName = referenceSectionPair.Key.HeaderKey;
                if (Report.NewSections.Any(x => x.Equals(sectionName)))
                {
                    newFile.AddSection(referenceSectionPair.Value, referenceSectionPair.Key.Index);
                }
                else
                {
                    var targetSectionHeader = ParsedTargetFile.Keys.FirstOrDefault(x => x.HeaderKey == sectionName);
                    var targetSection = ParsedTargetFile[targetSectionHeader.Index];
                    if (Report.NewLines.Any(x => x.Key.Equals(sectionName)) || Report.DeletedLines.Any(x => x.Key.Equals(sectionName)))
                    {
                        var newSection = SyncSection(referenceSectionPair.Value, targetSection);
                        newFile.AddSection(newSection, referenceSectionPair.Key.Index);
                    }
                    else
                    {
                        newFile.AddSection(targetSection, referenceSectionPair.Key.Index);
                    }                    
                }
            }
            return newFile;
        }

        /// <summary>
        /// Synchronize the target section with the referenceSection.
        /// </summary>
        /// <param name="referenceSection"></param>
        /// <param name="targetSection"></param>
        /// <returns></returns>
        private Section SyncSection(Section referenceSection, Section targetSection)
        {
            var newSection = new Section()
            {
                Title = targetSection.Title,
                SectionComment = targetSection.SectionComment,
                SectionSuffix = targetSection.SectionSuffix
            };

            int index = 0;
            foreach (var linePair in referenceSection)
            {
                var header = linePair.Key;
                if (header.HeaderKey == "")
                {
                    newSection.AddEmptyLine(index++, linePair.Value.Comment);
                }
                else
                {
                    var targetLine = targetSection.Keys.FirstOrDefault(x => x.HeaderKey.Equals(linePair.Key.HeaderKey));
                    if (targetLine != null)
                    {
                        newSection.AddLine(targetSection[targetLine], index++);
                    }
                    else
                    {
                        newSection.AddLine(linePair.Value, index++);
                    }
                }
            }
            return newSection;
        }
    }
}
