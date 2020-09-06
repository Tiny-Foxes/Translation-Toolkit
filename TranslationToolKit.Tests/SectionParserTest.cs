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
            var section = new SectionParser().ParseSection(lines);

            Assert.NotNull(section);

            foreach (var header in section.Keys)
            {
                Assert.Equal(header.HeaderKey, section[header].TranslationKey);
                Assert.Equal(0, header.OccurenceIndex);
            }
        }

        [Fact]
        public void WhenProvidedWithASimpleSectionThenTheSectionIsParsed()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\BasicSection.txt").ToList();

            var section = new SectionParser().ParseSection(lines);

            Assert.NotNull(section);
            Assert.Equal("[ScreenTitleMenu]", section.Title);
            Assert.Equal(9, section.Count);
            foreach (var header in section.Keys)
            {
                Assert.Equal(header.HeaderKey, section[header].TranslationKey);
            }
            var index = 0;
            Assert.Equal(new Line("Fitness", "Fitness Mode"), section[new Header("Fitness", 0, index++)]);
            Assert.Equal(new Line("HelpText", "&BACK; Exit &START; Select &MENUUP;&MENUDOWN; Move"), section[new Header("HelpText", 0, index++)]);
            Assert.Equal(new Line("Network OK", "Network OK"), section[new Header("Network OK", 0, index++)]);
            Assert.Equal(new Line("Offline", "Offline"), section[new Header("Offline", 0, index++)]);
            Assert.Equal(new Line("Connected to %s", "Connected to %s"), section[new Header("Connected to %s", 0, index++)]);
            Assert.Equal(new Line("CurrentGametype", "Current Gametype"), section[new Header("CurrentGametype", 0, index++)]);
            Assert.Equal(new Line("LifeDifficulty", "Life Difficulty"), section[new Header("LifeDifficulty", 0, index++)]);
            Assert.Equal(new Line("TimingDifficulty", "Timing Difficulty"), section[new Header("TimingDifficulty", 0, index++)]);
            Assert.Equal(new Line("%i Songs (%i Groups), %i Courses", "%i Songs (%i Groups), %i Courses"), section[new Header("%i Songs (%i Groups), %i Courses", 0, index++)]);
        }


        [Fact]
        public void WhenTheSectionIsParsedThenTheDictionaryIsSortedByIndex()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\BasicSection.txt").ToList();

            var section = new SectionParser().ParseSection(lines);

            Assert.NotNull(section);
            Assert.Equal("[ScreenTitleMenu]", section.Title);
            Assert.Equal(9, section.Count);
            foreach (var header in section.Keys)
            {
                Assert.Equal(header.HeaderKey, section[header].TranslationKey);
            }

            Assert.Equal(new Line("Fitness", "Fitness Mode"), section[0]);
            Assert.Equal(new Line("HelpText", "&BACK; Exit &START; Select &MENUUP;&MENUDOWN; Move"), section[1]);
            Assert.Equal(new Line("Network OK", "Network OK"), section[2]);
            Assert.Equal(new Line("Offline", "Offline"), section[3]);
            Assert.Equal(new Line("Connected to %s", "Connected to %s"), section[4]);
            Assert.Equal(new Line("CurrentGametype", "Current Gametype"), section[5]);
            Assert.Equal(new Line("LifeDifficulty", "Life Difficulty"), section[6]);
            Assert.Equal(new Line("TimingDifficulty", "Timing Difficulty"), section[7]);
            Assert.Equal(new Line("%i Songs (%i Groups), %i Courses", "%i Songs (%i Groups), %i Courses"), section[8]);
        }

        [Fact]
        public void WhenProvidedWithAnEmptyOrNullListThenThrowsAnException()
        {
            var exception = Assert.Throws<ArgumentException>(() => new SectionParser().ParseSection(new List<string>()));
            Assert.Equal("Tried to parse section but provided lines list is empty (Parameter 'lines')", exception.Message);
        }

        [Fact]
        public void WhenProvidedWithASectionWithNoTitleThenThrowsAnException()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\NoTitle.txt").ToList();

            var exception = Assert.Throws<ArgumentException>(() => new SectionParser().ParseSection(lines));
            Assert.Equal("Tried to parse section but no section title found (Parameter 'lines')", exception.Message);
        }

        [Fact]
        public void WhenProvidedWithASectionWithEmptyLinesThenTheTitleIsStillFound()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\BasicSectionWithAddedWhiteLines.txt").ToList();

            var section = new SectionParser().ParseSection(lines);
            Assert.Equal("[ScreenTitleMenu]", section.Title);
        }

        [Fact]
        public void WhenProvidedWithASectionWithAddedWhiteLinesThenWhiteLinesAtTheStartAreIgnored()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\BasicSectionWithAddedWhiteLines.txt").ToList();

            var section = new SectionParser().ParseSection(lines);

            Assert.NotNull(section);
            Assert.Equal("[ScreenTitleMenu]", section.Title);
            Assert.Equal(9, section.Count);
            foreach (var header in section.Keys)
            {
                Assert.Equal(header.HeaderKey, section[header].TranslationKey);
            }
            var index = 0;
            Assert.Equal(new Line("Fitness", "Fitness Mode"), section[index++]);
            Assert.Equal(new Line("HelpText", "&BACK; Exit &START; Select &MENUUP;&MENUDOWN; Move"), section[index++]);
            Assert.Equal(new Line("Network OK", "Network OK"), section[index++]);
            Assert.Equal(new Line("Offline", "Offline"), section[index++]);
            Assert.Equal(new Line("Connected to %s", "Connected to %s"), section[index++]);
            Assert.Equal(new Line("CurrentGametype", "Current Gametype"), section[index++]);
            Assert.Equal(new Line("LifeDifficulty", "Life Difficulty"), section[index++]);
            Assert.Equal(new Line("TimingDifficulty", "Timing Difficulty"), section[index++]);
            Assert.Equal(new Line("%i Songs (%i Groups), %i Courses", "%i Songs (%i Groups), %i Courses"), section[index++]);
        }


        [Fact]
        public void WhenProvidedWithASectionWithAddedWhiteLinesAtTheEndThenWhiteLinesAreAddedToTheList()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\BasicSectionWithAddedWhiteLinesAtTheEnd.txt").ToList();

            var section = new SectionParser().ParseSection(lines);

            Assert.NotNull(section);
            Assert.Equal("[ScreenTitleMenu]", section.Title);
            Assert.Equal(13, section.Count);
            foreach (var header in section.Keys)
            {
                Assert.Equal(header.HeaderKey, section[header].TranslationKey);
            }
            var index = 0;
            Assert.Equal(new Line("Fitness", "Fitness Mode"), section[new Header("Fitness", 0, index++)]);
            Assert.Equal(new Line("HelpText", "&BACK; Exit &START; Select &MENUUP;&MENUDOWN; Move"), section[new Header("HelpText", 0, index++)]);
            Assert.Equal(new Line("Network OK", "Network OK"), section[new Header("Network OK", 0, index++)]);
            Assert.Equal(new Line("Offline", "Offline"), section[new Header("Offline", 0, index++)]);
            Assert.Equal(new Line("Connected to %s", "Connected to %s"), section[new Header("Connected to %s", 0, index++)]);
            Assert.Equal(new Line("CurrentGametype", "Current Gametype"), section[new Header("CurrentGametype", 0, index++)]);
            Assert.Equal(new Line("LifeDifficulty", "Life Difficulty"), section[new Header("LifeDifficulty", 0, index++)]);
            Assert.Equal(new Line("TimingDifficulty", "Timing Difficulty"), section[new Header("TimingDifficulty", 0, index++)]);
            Assert.Equal(new Line("%i Songs (%i Groups), %i Courses", "%i Songs (%i Groups), %i Courses"), section[new Header("%i Songs (%i Groups), %i Courses", 0, index++)]);
            Assert.Equal(new Line("", ""), section[new Header("", 0, index++)]);
            Assert.Equal(new Line("", ""), section[new Header("", 1, index++)]);
            Assert.Equal(new Line("", ""), section[new Header("", 2, index++)]);
            Assert.Equal(new Line("", ""), section[new Header("", 3, index++)]);
        }

        [Fact]
        public void WhenThereIsADuplicateKeyThenKeyIsAddedButOccurenceIndexIsIncremented()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\SectionWithDuplicate.txt").ToList();

            var section = new SectionParser().ParseSection(lines);

            Assert.NotNull(section);
            Assert.Equal("[ScreenEvaluation]", section.Title);
            Assert.Equal(31, section.Count);

            Assert.Equal(new Line("HelpText", "&BACK; Exit &START; Move On &MENULEFT;-&MENURIGHT; Change Page &SELECT; Snapshot"), section[Header.GetIndex(0)]);
            Assert.Equal(new Line("HelpText", "&BACK; Exit &START; Move On &MENULEFT;&MENURIGHT; Change Page &SELECT; Snapshot"), section[Header.GetIndex(21)]);

            var header0 = section.Keys.ElementAt(0);
            Assert.Equal("HelpText", header0.HeaderKey);
            Assert.Equal(0, header0.OccurenceIndex);
            Assert.Equal(0, header0.Index);

            var header21 = section.Keys.ElementAt(21);
            Assert.Equal("HelpText", header21.HeaderKey);
            Assert.Equal(1, header21.OccurenceIndex);
            Assert.Equal(21, header21.Index);
        }


        [Fact]
        public void WhenProvidedWithCommentsThenTheyAreAddedToTheNextLine()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\SectionWithComments.txt").ToList();

            var section = new SectionParser().ParseSection(lines);

            Assert.NotNull(section);
            Assert.Equal("[ScreenTestFonts]", section.Title);
            Assert.Equal(5, section.Count);
            foreach (var header in section.Keys)
            {
                Assert.Equal(header.HeaderKey, section[header].TranslationKey);
            }
            var index = 0;
            Assert.Equal(new Line("Text1", "WWWWWWWWWWWW", "# W is annoying:"), section[new Header("Text1", 0, index++)]);
            Assert.Equal(new Line("Text2", "The argument goes something like this: &oq;I refuse to prove that I exist,&cq; says God, &oq;for proof denies faith, and without faith I am nothing.&cq;::::&oq;But,&cq; says Man, &oq;The Babel fish is a dead giveaway, isn't it? It could not have evolved by chance. It proves you exist, and so therefore, by your own arguments, you don't. QED.&cq;::::&oq;Oh dear,&cq; says God, &oq;I hadn't thought of that,&cq; and promptly vanished in a puff of logic.", "# Realistic excerpt:"), section[new Header("Text2", 0, index++)]);
            Assert.Equal(new Line("Text3", "01234567890 ::100.00% 12345% 9:00 50/100", "# Numbers test:"), section[new Header("Text3", 0, index++)]);
            Assert.Equal(new Line("Text4", "$50% (Test) \"test's\" &oq;test's&cq;, #1! 1+2-3*4/5.6::3:00 a; b; 5<6 6>5  2^3=8 Why? test@example.com::[Foo] {bar} Press &START; 3|4 &oq;1_2_3&cq;", "# General symbol test, and test alignment against another font."), section[new Header("Text4", 0, index++)]);
            Assert.Equal(new Line("Text5", ""), section[new Header("Text5", 0, index++)]);
        }

        [Fact]
        public void WhenProvidedWithSlashSlashCommentsThenTheyAreAddedToTheNextLine()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\SectionWithVariousTypesOfComment.txt").ToList();

            var section = new SectionParser().ParseSection(lines);

            Assert.NotNull(section);
            Assert.Equal("[CustomDifficulty]", section.Title);
            Assert.Equal(14, section.Count);
            foreach (var header in section.Keys)
            {
                Assert.Equal(header.HeaderKey, section[header].TranslationKey);
            }
            Assert.Equal(new Line("Beginner", "Novice", "// いじるとバグりそう"), section[0]);
            Assert.Equal(new Line("Easy", "Easy"), section[1]);
            Assert.Equal(new Line("Freestyle", "Freestyle", "#"), section[8]);
            Assert.Equal(new Line("Challenge", "Expert", $"; Put all help text in this one group and have metrics look up the strings here.  That will help make sure\n; that strings are in a consistent style and that strings aren't duplicated."), section[4]);
            Assert.Equal(new Line("", "", "# Test comment"), section[10]);
            Assert.Equal(new Line("", "", ""), section[11]);
        }

        [Fact]
        public void WhenProvidedWithEmptyLineThenTheyAreAddedToTheSection()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\SectionWithEmptyLine.txt").ToList();

            var section = new SectionParser().ParseSection(lines);

            Assert.NotNull(section);
            Assert.Equal("[ScreenPracticeMenu]", section.Title);
            Assert.Equal(7, section.Count);
            foreach (var header in section.Keys)
            {
                Assert.Equal(header.HeaderKey, section[header].TranslationKey);
            }
            var index = 0;
            Assert.Equal(new Line("HeaderText", "Practice Songs"), section[new Header("HeaderText", 0, index++)]);
            Assert.Equal(new Line("", ""), section[new Header("", 0, index++)]);
            Assert.Equal(new Line("ExplanationGroup", "Select the song group."), section[new Header("ExplanationGroup", 0, index++)]);
            Assert.Equal(new Line("ExplanationSong", "Select the song you wish to practice."), section[new Header("ExplanationSong", 0, index++)]);
            Assert.Equal(new Line("ExplanationAction", "Press Start to practice!"), section[new Header("ExplanationAction", 0, index++)]);
            Assert.Equal(new Line("ExplanationStepsType", "Select the StepsType you want to practice."), section[new Header("ExplanationStepsType", 0, index++)]);
            Assert.Equal(new Line("ExplanationSteps", "Select a notechart."), section[new Header("ExplanationSteps", 0, index++)]);
        }

        [Fact]
        public void WhenThereIsACommentAtTheEndOfTheSectionItIsAdded()
        {
            var lines = File.ReadAllLines(".\\Input\\SectionParser\\SectionWithCommentAtTheEnd.txt").ToList();

            var section = new SectionParser().ParseSection(lines);
            Assert.Equal(9, section.Count);
            Assert.Equal("[OptionTitles]", section.Title);
            foreach (var header in section.Keys)
            {
                Assert.Equal(header.HeaderKey, section[header].TranslationKey);
            }
            Assert.Equal(new Line("", "", "#"), section[8]);
        }
    }
}
