using System;
using System.IO;
using System.Text;
using Xunit;

namespace TranslationToolKit.Business.Tests
{
    public class ValidationHelperTest
    {
        [Fact]
        public void WhenFileDoesntExistThenIsFileValidThrowsAnError()
        {
            string error;
            var path = Path.GetTempFileName();
            try
            {
                Assert.True(ValidationHelper.IsFileValid(path, out error));
                Assert.Equal("", error);
            }
            finally
            {
                File.Delete(path);
            }

            Assert.False(ValidationHelper.IsFileValid(path, out error));
            Assert.Equal($"The file {path} doesn't exist", error);
        }
    }
}
