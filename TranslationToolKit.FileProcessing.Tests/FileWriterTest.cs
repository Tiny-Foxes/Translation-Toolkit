using System;
using System.IO;
using TranslationToolKit.FileProcessing.DataModel;
using TranslationToolKit.FileProcessing.Tests.Helper;
using Xunit;

namespace TranslationToolKit.FileProcessing.Tests
{
    public class FileWriterTest
    {
        [Theory]
        [InlineData("en-small.ini")]
        [InlineData("en-default.ini")]
        [InlineData("en-fallback.ini")]
        [InlineData("ja-default.ini")]
        [InlineData("ja-fallback.ini")]
        [InlineData("es-default.ini")]
        [InlineData("es-fallback.ini")]
        [InlineData("pl-default.ini")]
        [InlineData("pl-fallback.ini")]
        [InlineData("pt-BR-default.ini")]
        [InlineData("pt-BR-fallback.ini")]
        public void WhenReadingAndWritingTheSameProperlyFormatteFileThenWeEndUpWithAnIdenticalFile(string fileName)
        {
            var source = $".\\Input\\FileWriter\\ProperlyFormatted\\{fileName}";
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
                //Comment this if you need to debug the test and check the output
                if (File.Exists(destination))
                {
                    File.Delete(destination);
                }
            }
        }

        [Theory]
        [InlineData("en-fallback.ini")]
        [InlineData("ja-fallback.ini")]
        [InlineData("es-fallback.ini")]
        [InlineData("pl-fallback.ini")]
        [InlineData("pt-BR-fallback.ini")]
        public void WhenReadingFileWithUnproperFormatThenWriteWithProperFormat(string fileName)
        {
            var source = $".\\Input\\FileWriter\\RawFiles\\{fileName}";
            var destination = $".\\Output\\Result\\RawFilesFormatted{fileName}";
            var expected = $".\\Output\\Expected\\RawFilesFormatted{fileName}";

            if (File.Exists(destination))
            {
                File.Delete(destination);
            }
            try
            {
                var file = FileParser.ProcessFileIntoSections(source);

                FileWriter.Write(file, destination);

                Assert.True(FileComparer.AreFilesIdentical(expected, destination));
            }
            finally
            {
                //Comment this if you need to debug the test and check the output
                if (File.Exists(destination))
                {
                    File.Delete(destination);
                }
            }
        }

        [Fact]
        public void WhenProvidedWithADirectoryThatDoesntExistThenThrowsAnException()
        {
            Assert.Throws<ArgumentException>(() => FileWriter.Write(new ParsedFile(), ".\\ThisDoesntExistAtAllDontEvenTryIt\\File.ini"));
        }
    }
}