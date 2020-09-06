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
    }
}
