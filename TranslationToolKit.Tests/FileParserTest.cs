using System.IO;
using Xunit;
using TranslationToolKit.DataModel;

namespace TranslationToolKit.Tests
{
    public class FileParserTest
    {
        [Fact]
        public void WhenGivingAValidFileThenItReturnsListOfSections()
        {
            var lines = File.ReadAllLines(".\\Input\\FileParser\\en-smallfile.ini");

            var parsedFile = FileParser.ProcessFileIntoSections(lines);
            var result = parsedFile.Sections;
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.Equal(4, result.Keys.Count);

            foreach (var key in result.Keys)
            {
                Assert.Equal(key.HeaderKey, result[key].Title);
            }

            Assert.Equal("[Common]", result[new Header("[Common]", 0, 0)].Title);
            Assert.Equal("[Screen]", result[new Header("[Common]", 0, 1)].Title);
            Assert.Equal("[ScreenWithMenuElements]", result[new Header("[Common]", 0, 2)].Title);
            Assert.Equal("[ScreenTitleMenu]", result[new Header("[Common]", 0, 3)].Title);
        }
    }
}
