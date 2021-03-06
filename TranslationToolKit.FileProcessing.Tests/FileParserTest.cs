﻿using System.IO;
using Xunit;
using System.Collections.Generic;
using TranslationToolKit.FileProcessing.DataModel;
using System.Linq;

namespace TranslationToolKit.FileProcessing.Tests
{
    public class FileParserTest
    {
        [Fact]
        public void WhenGivingAValidFileThenItReturnsListOfSections()
        {
            var lines = File.ReadAllLines(".\\Input\\FileParser\\en-smallfile.ini");

            var result = FileParser.ProcessFileIntoSections(lines);
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);

            foreach (var key in result.Keys)
            {
                Assert.Equal(key.HeaderKey, result[key].Title);
            }

            Assert.Equal("[Common]", result[0].Title);
            Assert.Equal(7, result[0].Count);
            Assert.Equal("\n", result[0].SectionSuffix);
            Assert.Equal("[Screen]", result[1].Title);
            Assert.Equal(1, result[1].Count);
            Assert.Equal("\n", result[1].SectionSuffix);
            Assert.Equal("[ScreenWithMenuElements]", result[2].Title);
            Assert.Equal(3, result[2].Count);
            Assert.Equal("\n", result[2].SectionSuffix);
            Assert.Equal("[ScreenTitleMenu]", result[3].Title);
            Assert.Equal(9, result[3].Count);
            Assert.Equal("", result[3].SectionSuffix);
        }

        [Fact]
        public void WhenThereAreAdditionalWhiteLinesThenLinesBeforeFirstSectionAreIgnoredAndLinesAfterAreInLastSection()
        {
            var lines = File.ReadAllLines(".\\Input\\FileParser\\en-smallfile-withaddedwhitelines.ini");

            var result = FileParser.ProcessFileIntoSections(lines);
            Assert.NotNull(result);
            Assert.Equal(4, result.Count);

            foreach (var key in result.Keys)
            {
                Assert.Equal(key.HeaderKey, result[key].Title);
            }

            Assert.Equal("[Common]", result[0].Title);
            Assert.Equal(7, result[0].Count);
            Assert.Equal("\n", result[0].SectionSuffix);
            Assert.Equal("[Screen]", result[1].Title);
            Assert.Equal(1, result[1].Count);
            Assert.Equal("\n", result[1].SectionSuffix);
            Assert.Equal("[ScreenWithMenuElements]", result[2].Title);
            Assert.Equal(3, result[2].Count);
            Assert.Equal("\n", result[2].SectionSuffix);
            Assert.Equal("[ScreenTitleMenu]", result[3].Title);
            Assert.Equal(9, result[3].Count);
            Assert.Equal("\n\n", result[3].SectionSuffix);
        }

        [Fact]
        public void WhenFileIsEmptyThenParserReturnsEmptyList()
        {
            var result = FileParser.ProcessFileIntoSections(new List<string>());
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void WhenFileHasNoSectionTitleThenParserReturnsEmptyList()
        {
            var lines = File.ReadAllLines(".\\Input\\FileParser\\en-smallfile-notitle.ini");
            var result = FileParser.ProcessFileIntoSections(lines);
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
        }

        [Theory]
        [InlineData("en-default",69)]
        [InlineData("en-fallback", 202)]
        public void TestParseFullFile(string fileName, int expectedSectionCount)
        {
            var lines = File.ReadAllLines($".\\Input\\FileParser\\{fileName}.ini");

            var result = FileParser.ProcessFileIntoSections(lines);
            Assert.NotNull(result);
            Assert.Equal(expectedSectionCount, result.Count);
        }


        [Fact]
        public void WhenThereIsTextAtTheStartOfAFileThenItIsAddedAsAHeader()
        {
            var result = FileParser.ProcessFileIntoSections($".\\Input\\FileParser\\ja-fallback-WithHeader.ini");
            Assert.NotNull(result);
            Assert.NotNull(result.FileHeader);
            Assert.Equal($"// Stepmania用日本語言語パック(fallback用) Ver.2-20150411{EnvironmentConstants.EndOfLine}{EnvironmentConstants.EndOfLine}// 翻訳終わった部分には済マーク入れます{EnvironmentConstants.EndOfLine}// 5.0.7の追加オプションの説明を追加{EnvironmentConstants.EndOfLine}// BGはもう少しお待ちください{EnvironmentConstants.EndOfLine}{EnvironmentConstants.EndOfLine}// find_missing_strings_in_theme_translationsで無かった分を補完 -hanubeki 2015/04/14{EnvironmentConstants.EndOfLine}// New-Options-menu向けに翻訳を追加 -hanubeki 2015/04/24{EnvironmentConstants.EndOfLine}// 5.3 OutFox向けに翻訳を追加 -hanubeki 2020/02/11{EnvironmentConstants.EndOfLine}{EnvironmentConstants.EndOfLine}", result.FileHeader);
        }

        [Fact]
        public void WhenThereIsACommentOnTopOfASectionThenAddItAsACommentToTheSection()
        {
            var result = FileParser.ProcessFileIntoSections($".\\Input\\FileParser\\es-withsectioncomment.txt");
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);

            Assert.Equal(14, result[0].Count);
            Assert.Equal("\n", result[0].SectionSuffix);

            Assert.Equal(1, result[1].Count);
            Assert.Equal("# Time to manage each screen's options", result[1].SectionComment);
            Assert.Equal(new Line("HelpText", "&BACK; Regresar &START; Seleccionar &MENULEFT;&MENURIGHT; Mover", ""), result[1][0]);
            Assert.Equal("# HeaderSubText does not need translation, we don't even use it.\n\n", result[1].SectionSuffix);
        }

        [Fact]
        public void WhenThereAreDuplicateSectionsThenTheOccurenceIndexIsIncreased()
        {
            var result = FileParser.ProcessFileIntoSections($".\\Input\\FileParser\\SimpleFileWithDuplicateSection.txt");

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            var list = result.Keys.ToList();
            Assert.Equal(0, list[0].OccurenceIndex);
            Assert.Equal(0, list[1].OccurenceIndex);
            Assert.Equal(1, list[2].OccurenceIndex);

            Assert.Equal("[ScreenSetBGFit]", result[0].Title);
            Assert.Equal("[OptionNames]", result[1].Title);
            Assert.Equal("[ScreenSetBGFit]", result[2].Title);
        }
    }
}
