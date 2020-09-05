using System;
using System.Collections.Generic;
using System.Text;
using TranslationToolKit.DataModel;
using Xunit;

namespace TranslationToolKit.Tests.DataModel
{
    public class HeaderTest
    {
        [Fact]
        public void TestToString()
        {
            var header = new Header("ABC", 2);
            Assert.Equal("[Key:ABC|Index:2]", header.ToString());
        }
    }
}
