using System;
using Xunit;
using System.IO;

namespace TranslationToolKit.Business.Tests
{
    public class LinesCounterTest
    {
        [Fact]
        public void WhenFileDoesntExistThenRunAnalyzerThrowsAnException()
        {
            var checker = new LinesCounter();
            var path = Path.GetRandomFileName();

            Assert.Throws<ArgumentException>(() => checker.RunAnalyzer(path, path));
        }


        [Fact]
        public void CountingAFullyTranslatedSection()
        {
            string referencePath = ".\\Input\\LinesCounter\\SectionReference.txt";
            string targetPath = ".\\Input\\LinesCounter\\SectionTranslated.txt";
            var checker = new LinesCounter();
            var report = checker.RunAnalyzer(referencePath, targetPath);

            Assert.NotNull(report);
            Assert.Equal(7, report.TotalLinesCount);
            Assert.Equal(7, report.TranslatedLinesCount);
            Assert.Equal(100.0, report.Percentage);
        }

        [Fact]
        public void CountingAPartiallyTranslatedSection()
        {
            string referencePath = ".\\Input\\LinesCounter\\SectionReference.txt";
            string targetPath = ".\\Input\\LinesCounter\\SectionPartiallyTranslated.txt";
            var checker = new LinesCounter();
            var report = checker.RunAnalyzer(referencePath, targetPath);

            Assert.NotNull(report);
            Assert.Equal(7, report.TotalLinesCount);
            Assert.Equal(4, report.TranslatedLinesCount);
            Assert.Equal(57.14, report.Percentage);
        }

        [Fact]
        public void CounterDoesntCountEmptyLinesInReference()
        {
            string referencePath = ".\\Input\\LinesCounter\\SectionReferenceWithWhitespace.txt";
            string targetPath = ".\\Input\\LinesCounter\\SectionTranslated.txt";
            var checker = new LinesCounter();
            var report = checker.RunAnalyzer(referencePath, targetPath);

            Assert.NotNull(report);
            Assert.Equal(7, report.TotalLinesCount);
            Assert.Equal(7, report.TranslatedLinesCount);
            Assert.Equal(100, report.Percentage);
        }

        [Fact]
        public void CounterDoesntCountEmptyLinesInTarget()
        {
            string referencePath = ".\\Input\\LinesCounter\\SectionReferenceWithWhitespace.txt";
            string targetPath = ".\\Input\\LinesCounter\\SectionTranslated.txt";
            var checker = new LinesCounter();
            var report = checker.RunAnalyzer(referencePath, targetPath);

            Assert.NotNull(report);
            Assert.Equal(7, report.TotalLinesCount);
            Assert.Equal(7, report.TranslatedLinesCount);
            Assert.Equal(100, report.Percentage);
        }

        [Fact]
        public void CountMultipleSections()
        {
            string referencePath = ".\\Input\\LinesCounter\\ReferenceWithMultipleSections.txt";
            string targetPath = ".\\Input\\LinesCounter\\TargetWithMultipleSections.txt";
            var checker = new LinesCounter();
            var report = checker.RunAnalyzer(referencePath, targetPath);

            Assert.NotNull(report);
            Assert.Equal(20, report.TotalLinesCount);
            Assert.Equal(18, report.TranslatedLinesCount);
            Assert.Equal(90.0, report.Percentage);
        }

        [Fact]
        public void NativeLanguageSectionIsNotIncludedInTheCount()
        {
            string referencePath = ".\\Input\\LinesCounter\\SectionReferenceWithNativeLanguageSection.txt";
            string targetPath = ".\\Input\\LinesCounter\\SectionTranslated.txt";
            var checker = new LinesCounter();
            var report = checker.RunAnalyzer(referencePath, targetPath);

            Assert.NotNull(report);
            Assert.Equal(11, report.TotalLinesCount);
            Assert.Equal(7, report.TranslatedLinesCount);
            Assert.Equal(63.64, report.Percentage);
        }
    }
}
