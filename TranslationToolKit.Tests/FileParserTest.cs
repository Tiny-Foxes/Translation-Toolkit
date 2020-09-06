using System.IO;
using Xunit;
using System.Collections.Generic;

namespace TranslationToolKit.Tests
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
            Assert.Equal(8, result[0].Count);
            Assert.Equal("[Screen]", result[1].Title);
            Assert.Equal(2, result[1].Count);
            Assert.Equal("[ScreenWithMenuElements]", result[2].Title);
            Assert.Equal(4, result[2].Count);
            Assert.Equal("[ScreenTitleMenu]", result[3].Title);
            Assert.Equal(9, result[3].Count);
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
            Assert.Equal(8, result[0].Count);
            Assert.Equal("[Screen]", result[1].Title);
            Assert.Equal(2, result[1].Count);
            Assert.Equal("[ScreenWithMenuElements]", result[2].Title);
            Assert.Equal(4, result[2].Count);
            Assert.Equal("[ScreenTitleMenu]", result[3].Title);
            Assert.Equal(11, result[3].Count);
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
    }
}
