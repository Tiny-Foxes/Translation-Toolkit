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
            var lines = File.ReadAllLines(".\\Input\\FileParser\\en-default.ini");

            var result = FileParser.ProcessFileIntoSections(lines);

            Assert.NotNull(result);
            Assert.NotEmpty(result);

            foreach(var key in result.Keys)
            {
                Assert.Equal(key, result[key].Title);
            }
        }
    }
}
