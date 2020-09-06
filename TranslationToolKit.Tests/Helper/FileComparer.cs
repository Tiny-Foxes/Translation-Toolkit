using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace TranslationToolKit.Tests.Helper
{
    /// <summary>
    /// A file comparer, courtesy of
    /// https://stackoverflow.com/questions/211008/c-sharp-file-management
    /// </summary>
    public static class FileComparer
    {
        public static bool AreFilesIdentical(string f1, string f2)
        {
            // get file length and make sure lengths are identical
            long length = new FileInfo(f1).Length;
            if (length != new FileInfo(f2).Length)
                return false;

            byte[] buf1 = new byte[4096];
            byte[] buf2 = new byte[4096];

            // open both for reading
            using (FileStream stream1 = File.OpenRead(f1))
            using (FileStream stream2 = File.OpenRead(f2))
            {
                // compare content for equality
                int b1, b2;
                while (length > 0)
                {
                    // figure out how much to read
                    int toRead = buf1.Length;
                    if (toRead > length)
                        toRead = (int)length;
                    length -= toRead;

                    // read a chunk from each and compare
                    b1 = stream1.Read(buf1, 0, toRead);
                    b2 = stream2.Read(buf2, 0, toRead);
                    for (int i = 0; i < toRead; ++i)
                        if (buf1[i] != buf2[i])
                            return false;
                }
            }

            return true;
        }
    }

    public class FileComparerTest
    {
        [Fact]
        public void BasicTests()
        {
            Assert.True(FileComparer.AreFilesIdentical(".\\Input\\FileWriter\\FileComparer\\en-fallback.ini", ".\\Input\\FileWriter\\FileComparer\\en-fallback.ini"));
            Assert.False(FileComparer.AreFilesIdentical(".\\Input\\FileWriter\\FileComparer\\en-default.ini", ".\\Input\\FileWriter\\FileComparer\\en-fallback.ini"));
            Assert.False(FileComparer.AreFilesIdentical(".\\Input\\FileWriter\\FileComparer\\en-fallback.ini", ".\\Input\\FileWriter\\FileComparer\\en-fallback-withwrongendoflines.ini"));
        } 
    }
}
