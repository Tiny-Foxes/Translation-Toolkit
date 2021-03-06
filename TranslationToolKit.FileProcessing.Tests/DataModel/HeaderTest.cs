﻿using TranslationToolKit.FileProcessing.DataModel;
using Xunit;

namespace TranslationToolKit.FileProcessing.Tests.DataModel
{
    public class HeaderTest
    {
        [Fact]
        public void TestToString()
        {
            var header = new Header("ABC", 2, 37);
            Assert.Equal("[Key:ABC|OccurrenceIndex:2|Index:37]", header.ToString());
        }
    }
}
