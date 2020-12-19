using System;
using TranslationToolKit.Business;
using System.Text;
using Xunit;
using System.IO;

namespace TranslationToolKit.Business.Tests
{
    public class ChangesApplierTest
    {
        [Fact]
        public void WhenFileDoesntExistThenIsFileValidThrowsAnError()
        {
            string error;
            var path = Path.GetTempFileName();
            try
            {
                Assert.True(ChangesApplier.IsFileValid(path, out error));
                Assert.Equal("", error);
            }
            finally
            {
                File.Delete(path);
            }

            Assert.False(ChangesApplier.IsFileValid(path, out error));
            Assert.Equal($"The file {path} doesn't exist", error);
        }

        [Fact]
        public void WhenFileDoesntExistThenRunAnalyzerThrowsAnException()
        {
            var checker = new ChangesApplier();
            var path = Path.GetRandomFileName();

            Assert.Throws<ArgumentException>(() => checker.RunAnalyzer(path, path));
        }


        [Fact]
        public void WhenThereIsAMissingSectionThenRunAnalyzerFindsIt()
        {
            string targetPath = ".\\Input\\ChangesApplier\\ReferenceFile.txt";
            string referencePath = ".\\Input\\ChangesApplier\\ReferenceFileWithNewSection.txt";
            var checker = new ChangesApplier();
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
            string targetPath = ".\\Input\\ChangesApplier\\ReferenceFile.txt";
            string referencePath = ".\\Input\\ChangesApplier\\ReferenceFileWithDeletedSection.txt";
            var checker = new ChangesApplier();
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
            string targetPath = ".\\Input\\ChangesApplier\\ReferenceFile.txt";
            string referencePath = ".\\Input\\ChangesApplier\\ReferenceFileWithNewLines.txt";
            var checker = new ChangesApplier();
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
            string targetPath = ".\\Input\\ChangesApplier\\ReferenceFile.txt";
            string referencePath = ".\\Input\\ChangesApplier\\ReferenceFileDeletedLines.txt";
            var checker = new ChangesApplier();
            var report = checker.RunAnalyzer(referencePath, targetPath);

            Assert.NotNull(report);
            Assert.Empty(report.DeletedSections);
            Assert.Empty(report.NewSections);
            Assert.Empty(report.NewLines);
            Assert.Equal(5, report.DeletedLines.Count);
            Assert.Equal("[OptionNames]", report.DeletedLines[2].Key);
            Assert.Equal("LightScreenFilter", report.DeletedLines[2].Value);
        }
    }
}
