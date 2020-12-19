using System;
using Xunit;
using System.IO;
using TranslationToolKit.Business;
using TranslationToolKit.FileProcessing.Tests.Helper;

namespace TranslationToolKit.Business.Tests
{
    public class FileSynchronizerTest
    {
        [Fact]
        public void WhenFileDoesntExistThenIsFileValidThrowsAnError()
        {
            string error;
            var path = Path.GetTempFileName();
            try
            {
                Assert.True(FileSynchronizer.IsFileValid(path, out error));
                Assert.Equal("", error);
            }
            finally
            {
                File.Delete(path);
            }

            Assert.False(FileSynchronizer.IsFileValid(path, out error));
            Assert.Equal($"The file {path} doesn't exist", error);
        }

        [Fact]
        public void WhenFileDoesntExistThenRunAnalyzerThrowsAnException()
        {
            var checker = new FileSynchronizer();
            var path = Path.GetRandomFileName();

            Assert.Throws<ArgumentException>(() => checker.RunAnalyzer(path, path));
        }


        [Fact]
        public void WhenThereIsAMissingSectionThenRunAnalyzerFindsIt()
        {
            string targetPath = ".\\Input\\FileSynchronizer\\ReferenceFile.txt";
            string referencePath = ".\\Input\\FileSynchronizer\\ReferenceFileWithNewSection.txt";
            var checker = new FileSynchronizer();
            var report = checker.RunAnalyzer(referencePath, targetPath);

            Assert.NotNull(report);
            Assert.Empty(report.DeletedSections);
            Assert.Empty(report.DeletedLines);
            Assert.Empty(report.NewLines);
            Assert.Single(report.NewSections);
            Assert.Equal("[NewSection]", report.NewSections[0]);
        }

        [Fact]
        public void WhenThereIsADeletedSectionThenRunAnalyzerFindsIt()
        {
            string targetPath = ".\\Input\\FileSynchronizer\\ReferenceFile.txt";
            string referencePath = ".\\Input\\FileSynchronizer\\ReferenceFileWithDeletedSection.txt";
            var checker = new FileSynchronizer();
            var report = checker.RunAnalyzer(referencePath, targetPath);

            Assert.NotNull(report);
            Assert.Empty(report.DeletedLines);
            Assert.Empty(report.NewLines);
            Assert.Empty(report.NewSections);
            Assert.Single(report.DeletedSections);
            Assert.Equal("[OptionNames]", report.DeletedSections[0]);
        }

        [Fact]
        public void WhenThereIsANewLineThenRunAnalyzerFindsIt()
        {
            string targetPath = ".\\Input\\FileSynchronizer\\ReferenceFile.txt";
            string referencePath = ".\\Input\\FileSynchronizer\\ReferenceFileWithNewLines.txt";
            var checker = new FileSynchronizer();
            var report = checker.RunAnalyzer(referencePath, targetPath);

            Assert.NotNull(report);
            Assert.Empty(report.DeletedSections);
            Assert.Empty(report.DeletedLines);
            Assert.Empty(report.NewSections);
            Assert.Single(report.NewLines);
            Assert.Equal("[OptionNames]", report.NewLines[0].Key);
            Assert.Equal("OptionExtraEffects", report.NewLines[0].Value);
        }

        [Fact]
        public void WhenThereIsADeletedLineThenRunAnalyzerFindsIt()
        {
            string targetPath = ".\\Input\\FileSynchronizer\\ReferenceFile.txt";
            string referencePath = ".\\Input\\FileSynchronizer\\ReferenceFileDeletedLines.txt";
            var checker = new FileSynchronizer();
            var report = checker.RunAnalyzer(referencePath, targetPath);

            Assert.NotNull(report);
            Assert.Empty(report.DeletedSections);
            Assert.Empty(report.NewSections);
            Assert.Empty(report.NewLines);
            Assert.Equal(5, report.DeletedLines.Count);
            Assert.Equal("[OptionNames]", report.DeletedLines[2].Key);
            Assert.Equal("LightScreenFilter", report.DeletedLines[2].Value);
        }

        [Fact]
        public void CheckThatANewFileIsCreatedWithTheModifications()
        {
            string referencePath = ".\\Input\\FileSynchronizer\\ReferenceMiniDe.ini";
            string targetPath = ".\\Input\\FileSynchronizer\\TargetMiniDe.ini";            
            string generatedPath = ValidationHelper.SetFilePath(".\\Input\\FileSynchronizer\\TargetMiniDe.ini.generated");

            if (File.Exists(generatedPath))
            {
                File.Delete(generatedPath);
            }

            try
            {
                var checker = new FileSynchronizer();
                checker.RunAnalyzer(referencePath, targetPath);
                checker.SynchronizeFile();
                Assert.True(File.Exists(generatedPath));
                FileComparer.AreFilesIdentical(generatedPath,targetPath);
            }
            finally
            {
                if(File.Exists(generatedPath))
                {
                    File.Delete(generatedPath);
                }
            }
        }

        [Fact]
        public void MakeSureThatTheChangesApplierKeepPrefixAndSuffix()
        {
            string referencePath = ".\\Input\\FileSynchronizer\\ReferenceMiniBr.ini";
            string targetPath = ".\\Input\\FileSynchronizer\\TargetMiniBr.ini";
            string generatedPath = ValidationHelper.SetFilePath(".\\Input\\FileSynchronizer\\TargetMiniBr.ini.generated");

            if (File.Exists(generatedPath))
            {
                File.Delete(generatedPath);
            }

            try
            {
                var checker = new FileSynchronizer();
                checker.RunAnalyzer(referencePath, targetPath);
                checker.SynchronizeFile();
                Assert.True(File.Exists(generatedPath));
                FileComparer.AreFilesIdentical(generatedPath, targetPath);
            }
            finally
            {
                if (File.Exists(generatedPath))
                {
                    File.Delete(generatedPath);
                }
            }
        }
    }
}
