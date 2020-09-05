using System.Collections.Generic;
using TranslationToolKit;
using Xunit;

namespace TranslationToolKit.Tests
{
    public class FileParserTest
    {
        [Fact]
        public void WhenGivingAValidFileThenItReturnsListOfSections()
        {
            var parser = new FileParser();

            string file = "";

            var result = parser.ProcessFileIntoSections(file);

            Assert.NotNull(result);

            foreach(var key in result.Keys)
            {
                Assert.Equal(key, result[key].Title);
            }
        }
    }
}
