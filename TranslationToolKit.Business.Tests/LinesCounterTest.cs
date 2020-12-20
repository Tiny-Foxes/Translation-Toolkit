﻿using System;
using Xunit;
using System.IO;
using TranslationToolKit.Business;
using TranslationToolKit.FileProcessing.Tests.Helper;

namespace TranslationToolKit.Business.Tests
{
    public class LinesCounterTest
    {
        [Fact]
        public void WhenFileDoesntExistThenIsFileValidThrowsAnError()
        {
            string error;
            var path = Path.GetTempFileName();
            try
            {
                Assert.True(LinesCounter.IsFileValid(path, out error));
                Assert.Equal("", error);
            }
            finally
            {
                File.Delete(path);
            }

            Assert.False(LinesCounter.IsFileValid(path, out error));
            Assert.Equal($"The file {path} doesn't exist", error);
        }

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
    }
}
