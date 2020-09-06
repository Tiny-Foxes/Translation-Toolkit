using System;
using TranslationToolKit.FileProcessing.DataModel;
using Xunit;

namespace TranslationToolKit.FileProcessing.Tests.DataModel
{
    public class LineTest
    {
        [Theory]
        [InlineData("myTitle","myText",true)]
        [InlineData("myTitle", "blabla", false)]
        [InlineData("oops", "myText", false)]
        [InlineData("", "", false)]
        [InlineData("", "myText", false)]
        [InlineData("myTitle", "", false)]
        [InlineData(null, "myText", false)]
        [InlineData("myTitle", null, false)]
        public void EqualsTest(string title, string text, bool expectedResult)
        {
            var testObject = new Line(title, text);
            var expected = new Line("myTitle", "myText");
            Assert.Equal(expectedResult, expected.Equals(testObject));
            Assert.Equal(expectedResult, testObject.Equals(expected));
        }

        [Theory]
        [InlineData("myTitle", "myText", null, false)]
        [InlineData("myTitle", "blabla", null, false)]
        [InlineData("oops", "myText", null, false)]
        [InlineData("", "", null, false)]
        [InlineData("", "myText", null, false)]
        [InlineData("myTitle", "", null, false)]
        [InlineData(null, "myText", null, false)]
        [InlineData("myTitle", null, null, false)]
        [InlineData("myTitle", "myText", "test", true)]
        [InlineData("myTitle", "blabla", "test", false)]
        [InlineData("oops", "myText", "test", false)]
        [InlineData("", "", "test", false)]
        [InlineData("", "myText", "test", false)]
        [InlineData("myTitle", "", "test", false)]
        [InlineData(null, "myText", "test", false)]
        [InlineData("myTitle", null, "test", false)]
        public void EqualsWithCommentTest(string title, string text, string comment, bool expectedResult)
        {
            var testObject = new Line(title, text, comment);
            var expected = new Line("myTitle", "myText", "test");
            Assert.Equal(expectedResult, expected.Equals(testObject));
            Assert.Equal(expectedResult, testObject.Equals(expected));
        }


        [Fact]
        public void EqualsTestNull()
        {
            Line testObject = null;
            var expected = new Line("myTitle", "myText");
            Assert.False(expected.Equals(testObject));
        }

        [Fact]
        public void EqualsTestAnotherObjectType()
        {
            string testObjectNull = null;
            string testObject = "test";
            var expected = new Line("myTitle", "myText");
            Assert.False(expected.Equals(testObject));
            Assert.False(expected.Equals(testObjectNull));
        }

        [Theory]
        [InlineData("","","","")]
        [InlineData("","ddsdsd", "", "")]
        [InlineData("", "ddsdsd", "# Left voluntarily empty", "# Left voluntarily empty\n")]
        [InlineData("", "ddsdsd", "#", "#\n")]
        [InlineData("title", "value", "# comment", "# comment\ntitle=value")]
        [InlineData("title", "value", "// comment", "// comment\ntitle=value")]
        [InlineData("title", "", "// comment", "// comment\ntitle=")]
        public void DisplayStringTest(string title, string value, string comment, string expected)
        {
            var line = new Line(title, value, comment);
            Assert.Equal(expected, line.DisplayString);
        }
    }
}
