using System;
using System.Linq;
using System.Text;
using TranslationToolKit.Business.DataModel;
using TranslationToolKit.FileProcessing;

namespace TranslationToolKit.Business
{
    public class LinesCounter
    {
        /// <summary>
        /// Analysis report, detailing the difference between our two files.
        /// </summary>
        public CountReport Report { get; private set; }

        /// <summary>
        /// Native Language Section is a bunch of lines that are in their original language.
        /// Thus they never get translated. This is the name of that section.
        /// </summary>
        public const string NativeLanguageSectionName = "[NativeLanguageNames]";

        public LinesCounter()
        {
            Report = new CountReport();
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
        /// Compare the reference file to the target file.
        /// Doesn't modify anything, just provide a report of differences.
        /// </summary>
        /// <param name="referencePath"></param>
        /// <param name="targetPath"></param>
        /// <returns></returns>
        public CountReport RunAnalyzer(string referencePath, string targetPath)
        {
            if (!IsFileValid(referencePath, out string error))
            {
                throw new ArgumentException($"The provided reference file is wrong: {error}", nameof(error));
            }
            if (!IsFileValid(targetPath, out error))
            {
                throw new ArgumentException($"The provided target file is wrong: {error}", nameof(error));
            }

            var parsedReferenceFile = FileParser.ProcessFileIntoSections(referencePath);
            var parsedTargetFile = FileParser.ProcessFileIntoSections(targetPath);

            Report.TotalLinesCount = 0;
            Report.TranslatedLinesCount = 0;

            foreach (var referencePair in parsedReferenceFile)
            {
                // Native Language lines should not be translated, so we skip it.
                if(referencePair.Key.HeaderKey.Equals(NativeLanguageSectionName))
                {
                    continue;
                }
                Report.TotalLinesCount += referencePair.Value.Keys.Count(x => x.HeaderKey != string.Empty);
                var targetSectionHeader = parsedTargetFile.Keys.FirstOrDefault(x => x.HeaderKey == referencePair.Key.HeaderKey);
                if (targetSectionHeader != null)
                {
                    var targetSection = parsedTargetFile[targetSectionHeader.Index];
                    var referenceSection = referencePair.Value;
                    foreach (var linePair in referenceSection)
                    {
                        if(linePair.Key.HeaderKey == string.Empty)
                        {
                            continue;
                        }
                        var header = targetSection.Keys.FirstOrDefault(x => x.HeaderKey == linePair.Key.HeaderKey);
                        if (header != null)
                        {
                            var translatedValue = targetSection[header].TranslatedValue;
                            var originalValue = linePair.Value.TranslatedValue;
                            if (translatedValue != originalValue)
                            {
                                Report.TranslatedLinesCount++;
                            }
                        }
                    }
                }
            }
            return Report;
        }    
    }
}
