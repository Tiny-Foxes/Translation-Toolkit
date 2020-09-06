using System;
using System.IO;
using TranslationToolKit.DataModel;
using TranslationToolKit.Tests.Helper;
using Xunit;

namespace TranslationToolKit.Tests
{
    public class FileWriterTest
    {
        [Theory]
        [InlineData("en-small.ini")]
        [InlineData("en-default.ini")]
        [InlineData("en-fallback.ini")]
        [InlineData("ja-default.ini")]
        [InlineData("ja-fallback.ini")]
        public void WhenReadingAndWritingTheSameFileThenWeEndUpWithAnIdenticalFile(string fileName)
        {
            var source = $".\\Input\\FileWriter\\{fileName}";
            var destination = $".\\Output\\Result\\WriteIdenticalFile{fileName}";
            if (File.Exists(destination))
            {
                File.Delete(destination);
            }
            try
            {
                var file = FileParser.ProcessFileIntoSections(source);

                FileWriter.Write(file, destination);

                Assert.True(FileComparer.AreFilesIdentical(source, destination));
            }
            finally
            {
                // Comment this if you need to debug the test and check the output
                //if (File.Exists(destination))
                //{
                //    File.Delete(destination);
                //}
            }
        }

        [Fact]
        public void WhenProvidedWithADirectoryThatDoesntExistThenThrowsAnException()
        {
            Assert.Throws<ArgumentException>(() => FileWriter.Write(new ParsedFile(), ".\\ThisDoesntExistAtAllDontEvenTryIt\\File.ini"));
        }
    }
}