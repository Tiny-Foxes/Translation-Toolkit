using System.Collections.Generic;
using System.IO;
using TranslationToolKit;
using Xunit;

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
            Assert.NotEmpty(result);
            Assert.Equal(4, result.Keys.Count);

            foreach (var key in result.Keys)
            {
                Assert.Equal(key, result[key].Title);
            }
        }
    }
}
