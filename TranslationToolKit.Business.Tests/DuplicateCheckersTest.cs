using System;
using System.IO;
using TranslationToolKit.FileProcessing.Tests.Helper;
using Xunit;

namespace TranslationToolKit.Business.Tests
{
    public class DuplicateCheckersTest
    {
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
            Assert.Equal(Path.GetFullPath(path), report.FilePath);
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

        [Fact]
        public void WhenThereIsADuplicateFieldThenRunAnalyzerFindsIt()
        {
            string path = ".\\Input\\DuplicatesChecker\\SimpleFileWithDuplicatedLine.txt";
            var checker = new DuplicatesChecker();
            var report = checker.RunAnalyzer(path);

            Assert.NotNull(report);
            Assert.Equal(3, report.DuplicatedLines.Count);
            Assert.Equal("Indonesian", report.DuplicatedLines[0].Value);
            Assert.Equal("[NativeLanguageNames]", report.DuplicatedLines[0].Key);
            Assert.Equal("Hebrew", report.DuplicatedLines[1].Value);
            Assert.Equal("[NativeLanguageNames]", report.DuplicatedLines[1].Key);
            Assert.Equal("Yiddish", report.DuplicatedLines[2].Value);
            Assert.Equal("[NativeLanguageNames]", report.DuplicatedLines[2].Key);
        }

        [Fact]
        public void WhenThereIsTheSameDuplicateLineSeveralTimesThenItOnlyReturnsOnce()
        {
            string path = ".\\Input\\DuplicatesChecker\\SimpleFileWithDuplicatedLineTwice.txt";
            var checker = new DuplicatesChecker();
            var report = checker.RunAnalyzer(path);

            Assert.NotNull(report);
            Assert.Equal(3, report.DuplicatedLines.Count);
            Assert.Equal("Indonesian", report.DuplicatedLines[0].Value);
            Assert.Equal("[NativeLanguageNames]", report.DuplicatedLines[0].Key);
            Assert.Equal("Hebrew", report.DuplicatedLines[1].Value);
            Assert.Equal("[NativeLanguageNames]", report.DuplicatedLines[1].Key);
            Assert.Equal("Yiddish", report.DuplicatedLines[2].Value);
            Assert.Equal("[NativeLanguageNames]", report.DuplicatedLines[2].Key);
        }

        [Theory]
        [InlineData(".\\Input\\DuplicatesChecker\\en-fallback.ini.generated", ".\\Input\\DuplicatesChecker\\en-fallback.ini", ".\\Output\\Expected\\DuplicatesChecker\\en-fallback.ini")]
        [InlineData(".\\Input\\DuplicatesChecker\\en-default.ini.generated", ".\\Input\\DuplicatesChecker\\en-default.ini", ".\\Output\\Expected\\DuplicatesChecker\\en-default.ini")]
        public void WhenProvidedWithAFileWithDuplicateThenRemoveDuplicatesCreateANewFileWithoutDuplicates(string generatedFile, string input, string expected)
        {
            try
            {
                // Prepare
                if (File.Exists(generatedFile))
                {
                    File.Delete(generatedFile);
                }
                string path = input;
                var checker = new DuplicatesChecker();
                checker.RunAnalyzer(path);

                // Test
                var resultPath = checker.RemoveDuplicates();

                // Validate
                Assert.Equal(Path.GetFullPath(generatedFile), resultPath);

                Assert.True(FileComparer.AreFilesIdentical(expected, resultPath));
            }
            finally
            {
                // Comment this if you need to debug this unit test and check the result
                //if (File.Exists(generatedFile))
                //{
                //    File.Delete(generatedFile);
                //}
            }
        }

        [Fact]
        public void WhenProvidedWithAFileWithNoDuplicateThenRemoveDuplicatesDoesntDoAnything()
        {
            string path = ".\\Output\\Expected\\DuplicatesChecker\\en-fallback.ini";
            var checker = new DuplicatesChecker();
            checker.RunAnalyzer(path);

            var resultPath = checker.RemoveDuplicates();
            Assert.Equal("", resultPath);
        }

        [Fact]
        public void WhenRemoveDuplicatesIsCalledBeforeAnalyzerThenThrowsAnException()
        {
            var checker = new DuplicatesChecker();

            var result = Assert.Throws<ArgumentException>(() => checker.RemoveDuplicates());
            Assert.Equal("Report is empty, please run the analyzer first", result.Message);
        }

        [Theory]
        [InlineData(".\\Output\\Result\\PathRoot.ini", ".\\Output\\Result\\PathRoot.ini")]
        [InlineData(".\\rooted.ini", ".\\rooted.ini")]
        [InlineData("unrooted.ini", ".\\unrooted.ini")]
        public void WhenPathIsUnrootedThenReturnsARootedPath(string path, string expectedValue)
        {
            try
            {
                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                }
                var checker = new DuplicatesChecker();
                var report = checker.RunAnalyzer(path);
                Assert.Equal(Path.GetFullPath(expectedValue), report.FilePath);
            }
            finally
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
}
