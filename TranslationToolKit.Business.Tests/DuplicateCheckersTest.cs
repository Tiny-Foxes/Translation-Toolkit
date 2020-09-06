using System;
using System.IO;
using Xunit;

namespace TranslationToolKit.Business.Tests
{
    public class DuplicateCheckersTest
    {
        [Fact]
        public void WhenFileDoesntExistThenIsFileValidThrowsAnError()
        {
            string error;
            var path = Path.GetTempFileName();
            try
            {
                Assert.True(DuplicatesChecker.IsFileValid(path, out error));
                Assert.Equal("", error);
            }
            finally
            {
                File.Delete(path);
            }

            Assert.False(DuplicatesChecker.IsFileValid(path, out error));
            Assert.Equal($"The file {path} doesn't exist", error);
        }

        [Fact]
        public void WhenFileDoesntExistThenRunAnalyzerThrowsAnException()
        {
            var checker = new DuplicatesChecker();
            var path = Path.GetRandomFileName();

            Assert.Throws<ArgumentException>(() => checker.RunAnalyzer(path));
        }

        [Fact]
        public void WhenRunAnalyzerThenReturnsReportWithPathOfTheFile()
        {
            string path = ".\\Input\\DuplicatesChecker\\SimpleFileWithDuplicateSection.txt";
            var checker = new DuplicatesChecker();
            var report = checker.RunAnalyzer(path);

            Assert.NotNull(report);
            Assert.Equal(path, report.FilePath);
        }

        [Fact]
        public void WhenThereIsADuplicateSectionThenRunAnalyzerFindsIt()
        {
            string path = ".\\Input\\DuplicatesChecker\\SimpleFileWithDuplicateSection.txt";
            var checker = new DuplicatesChecker();
            var report = checker.RunAnalyzer(path);

            Assert.NotNull(report);
            Assert.Single(report.DuplicatedSections);
            Assert.Equal("[ScreenSetBGFit]", report.DuplicatedSections[0]);
        }

        [Fact]
        public void WhenThereIsTheSameDuplicateSectionSeveralTimesThenItOnlyReturnsOnce()
        {
            string path = ".\\Input\\DuplicatesChecker\\SimpleFileWithDuplicateSectionTwice.txt";
            var checker = new DuplicatesChecker();
            var report = checker.RunAnalyzer(path);

            Assert.NotNull(report);
            Assert.Single(report.DuplicatedSections);
            Assert.Equal("[ScreenSetBGFit]", report.DuplicatedSections[0]);
        }
    }
}
