using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TranslationToolKit.DataModel;
using Xunit;

namespace TranslationToolKit.Tests
{
    public class SectionParserTest
    {
        [Fact]
        public void WhenProvidingWithAListOfLinesThenTheDictionaryKeysWillBeTheLinesKeysAndTheOccurenceIndex()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\BasicSection.txt").ToList();
            var section = SectionParser.ParseSection(lines);

            Assert.NotNull(section);

            foreach (var header in section.Lines.Keys)
            {
                Assert.Equal(header.Key, section.Lines[header].Key);
                Assert.Equal(0, header.OccurenceIndex);
            }
        }

        [Fact]
        public void WhenProvidedWithASimpleSectionThenParseTheSection()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\BasicSection.txt").ToList();

            var section = SectionParser.ParseSection(lines);

            Assert.NotNull(section);
            Assert.Equal("[ScreenTitleMenu]", section.Title);
            Assert.Equal(9, section.Lines.Count);
            foreach (var header in section.Lines.Keys)
            {
                Assert.Equal(header.Key, section.Lines[header].Key);
            }
            Assert.Equal(new Line("Fitness", "Fitness Mode"), section.Lines[new Header("Fitness",0)]);
            Assert.Equal(new Line("HelpText", "&BACK; Exit &START; Select &MENUUP;&MENUDOWN; Move"), section.Lines[new Header("HelpText", 0)]);
            Assert.Equal(new Line("Network OK", "Network OK"), section.Lines[new Header("Network OK", 0)]);
            Assert.Equal(new Line("Offline", "Offline"), section.Lines[new Header("Offline", 0)]);
            Assert.Equal(new Line("Connected to %s", "Connected to %s"), section.Lines[new Header("Connected to %s", 0)]);
            Assert.Equal(new Line("CurrentGametype", "Current Gametype"), section.Lines[new Header("CurrentGametype", 0)]);
            Assert.Equal(new Line("LifeDifficulty", "Life Difficulty"), section.Lines[new Header("LifeDifficulty", 0)]);
            Assert.Equal(new Line("TimingDifficulty", "Timing Difficulty"), section.Lines[new Header("TimingDifficulty", 0)]);
            Assert.Equal(new Line("%i Songs (%i Groups), %i Courses", "%i Songs (%i Groups), %i Courses"), section.Lines[new Header("%i Songs (%i Groups), %i Courses", 0)]);
        }

        [Fact]
        public void WhenProvidedWithAnEmptyOrNullListThenThrowsAnException()
        {
            var exception = Assert.Throws<ArgumentException>(() => SectionParser.ParseSection(new List<string>()));
            Assert.Equal("Tried to parse section but provided lines list is empty (Parameter 'lines')", exception.Message);
        }

        [Fact]
        public void WhenProvidedWithASectionWithNoTitleThenThrowsAnException()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\NoTitle.txt").ToList();

            var exception = Assert.Throws<ArgumentException>(() => SectionParser.ParseSection(lines));
            Assert.Equal("Tried to parse section but no section title found (Parameter 'lines')", exception.Message);
        }

        [Fact]
        public void WhenProvidedWithASectionWithEmptyLinesThenTheTitleIsStillFound()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\BasicSectionWithAddedWhiteLines.txt").ToList();

            var section = SectionParser.ParseSection(lines);
            Assert.Equal("[ScreenTitleMenu]", section.Title);
        }

        [Fact]
        public void WhenProvidedWithASectionWithAddedWhiteLinesThenWhiteLinesAtTheStartAreIgnored()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\BasicSectionWithAddedWhiteLines.txt").ToList();

            var section = SectionParser.ParseSection(lines);

            Assert.NotNull(section);
            Assert.Equal("[ScreenTitleMenu]", section.Title);
            Assert.Equal(9, section.Lines.Count);
            foreach (var header in section.Lines.Keys)
            {
                Assert.Equal(header.Key, section.Lines[header].Key);
            }
            Assert.Equal(new Line("Fitness", "Fitness Mode"), section.Lines[new Header("Fitness", 0)]);
            Assert.Equal(new Line("HelpText", "&BACK; Exit &START; Select &MENUUP;&MENUDOWN; Move"), section.Lines[new Header("HelpText", 0)]);
            Assert.Equal(new Line("Network OK", "Network OK"), section.Lines[new Header("Network OK", 0)]);
            Assert.Equal(new Line("Offline", "Offline"), section.Lines[new Header("Offline", 0)]);
            Assert.Equal(new Line("Connected to %s", "Connected to %s"), section.Lines[new Header("Connected to %s", 0)]);
            Assert.Equal(new Line("CurrentGametype", "Current Gametype"), section.Lines[new Header("CurrentGametype", 0)]);
            Assert.Equal(new Line("LifeDifficulty", "Life Difficulty"), section.Lines[new Header("LifeDifficulty", 0)]);
            Assert.Equal(new Line("TimingDifficulty", "Timing Difficulty"), section.Lines[new Header("TimingDifficulty", 0)]);
            Assert.Equal(new Line("%i Songs (%i Groups), %i Courses", "%i Songs (%i Groups), %i Courses"), section.Lines[new Header("%i Songs (%i Groups), %i Courses", 0)]);
        }

        [Fact]
        public void WhenProvidedWithCommentsThenTheyAreAddedToTheNextLine()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\SectionWithComments.txt").ToList();

            var section = SectionParser.ParseSection(lines);

            Assert.NotNull(section);
            Assert.Equal("[ScreenTestFonts]", section.Title);
            Assert.Equal(5, section.Lines.Count);
            foreach (var header in section.Lines.Keys)
            {
                Assert.Equal(header.Key, section.Lines[header].Key);
            }
            Assert.Equal(new Line("Text1", "WWWWWWWWWWWW", "# W is annoying:"), section.Lines[new Header("Text1", 0)]);
            Assert.Equal(new Line("Text2", "The argument goes something like this: &oq;I refuse to prove that I exist,&cq; says God, &oq;for proof denies faith, and without faith I am nothing.&cq;::::&oq;But,&cq; says Man, &oq;The Babel fish is a dead giveaway, isn't it? It could not have evolved by chance. It proves you exist, and so therefore, by your own arguments, you don't. QED.&cq;::::&oq;Oh dear,&cq; says God, &oq;I hadn't thought of that,&cq; and promptly vanished in a puff of logic.", "# Realistic excerpt:"), section.Lines[new Header("Text2", 0)]);
            Assert.Equal(new Line("Text3", "01234567890 ::100.00% 12345% 9:00 50/100", "# Numbers test:"), section.Lines[new Header("Text3", 0)]);
            Assert.Equal(new Line("Text4", "$50% (Test) \"test's\" &oq;test's&cq;, #1! 1+2-3*4/5.6::3:00 a; b; 5<6 6>5  2^3=8 Why? test@example.com::[Foo] {bar} Press &START; 3|4 &oq;1_2_3&cq;", "# General symbol test, and test alignment against another font."), section.Lines[new Header("Text4", 0)]);
            Assert.Equal(new Line("Text5", ""), section.Lines[new Header("Text5", 0)]);
        }

        [Fact]
        public void WhenProvidedWithEmptyLineThenTheyAreAddedToTheSection()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\SectionWithEmptyLine.txt").ToList();

            var section = SectionParser.ParseSection(lines);

            Assert.NotNull(section);
            Assert.Equal("[ScreenPracticeMenu]", section.Title);
            Assert.Equal(7, section.Lines.Count);
            foreach (var header in section.Lines.Keys)
            {
                Assert.Equal(header.Key, section.Lines[header].Key);
            }
            Assert.Equal(new Line("HeaderText", "Practice Songs"), section.Lines[new Header("HeaderText", 0)]);
            Assert.Equal(new Line("", ""), section.Lines[new Header("", 0)]);
            Assert.Equal(new Line("ExplanationGroup", "Select the song group."), section.Lines[new Header("ExplanationGroup", 0)]);
            Assert.Equal(new Line("ExplanationSong", "Select the song you wish to practice."), section.Lines[new Header("ExplanationSong", 0)]);
            Assert.Equal(new Line("ExplanationAction", "Press Start to practice!"), section.Lines[new Header("ExplanationAction", 0)]);
            Assert.Equal(new Line("ExplanationStepsType", "Select the StepsType you want to practice."), section.Lines[new Header("ExplanationStepsType", 0)]);
            Assert.Equal(new Line("ExplanationSteps", "Select a notechart."), section.Lines[new Header("ExplanationSteps", 0)]);
        }

        // Assert.Equal(new Line("", ""), section.Lines[""]);
    }
}
