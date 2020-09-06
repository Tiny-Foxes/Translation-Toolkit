using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TranslationToolKit.DataModel
{
    /// <summary>
    /// Represents a section in the translation line.
    /// The section has a title, and a list of lines.
    /// </summary>
    public class Section : IEnumerable<KeyValuePair<Header, Line>>
    {
        /// <summary>
        /// The comment on top of a section
        /// </summary>
        public string SectionComment { get; set; }

        /// <summary>
        /// Title of the section. Ex: [ScreenPracticeMenu]
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// List of lines in the section
        /// </summary>
        private SortedDictionary<Header, Line> Lines { get; }

        private int emptyLineOccurences = 0;
        /// <summary>
        /// The generated suffix for empty lines
        /// </summary>
        private int EmptyLineOccurences => emptyLineOccurences++;

        public Section()
        {
            Title = "";
            SectionComment = "";
            Lines = new SortedDictionary<Header, Line>();
        }

        /// <summary>
        /// When file has an empty line, keep track of it with a generated header name,
        /// so we can retranscribe it later on (translation sections may be divided in blocks)
        /// </summary>
        /// <param name="index">index of this line within the section</param>
        public void AddEmptyLine(int index, string comment)
        {
            var header = new Header("", EmptyLineOccurences, index);
            Lines.Add(header, new Line(string.Empty, string.Empty, comment));
        }

        /// <summary>
        /// Add a line to the section.
        /// </summary>
        /// <param name="line">the line to be added</param>
        /// <param name="index">index of this line within the section</param>
        public void AddLine(Line line, int index)
        {
            int occurenceIndex = -1;
            if (Lines.Any(x => x.Key.HeaderKey.Equals(line.TranslationKey)))
            {
                occurenceIndex = Lines.Where(x => x.Key.HeaderKey.Equals(line.TranslationKey))
                                      .Max(x => x.Key.OccurenceIndex);
            }
            Lines.Add(new Header(line.TranslationKey, ++occurenceIndex, index), line);
        }

        #region Implementing various interfaces to allow checking the data, but not modifying list without using the proper add methods.

        /// <summary>
        /// An operator that allows to fetch lines, 
        /// but not modify them without using the proper methods.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Line this[Header i]
        {
            get { return Lines[i]; }
        }

        /// <summary>
        /// An operator that allows to fetch lines, 
        /// but not modify them without using the proper methods.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Line this[int i]
        {
            get { return Lines[Header.GetIndex(i)]; }
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<Header, Line>> GetEnumerator()
        {
            return Lines.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Lines.GetEnumerator();
        }

        /// <summary>
        /// Count of lines in the section.
        /// </summary>
        public int Count => Lines.Count;

        /// <summary>
        /// Headers of the lines.
        /// </summary>
        public SortedDictionary<Header, Line>.KeyCollection Keys => Lines.Keys;

        #endregion Implementing various interfaces to allow checking the data, but not modifying list
    }
}
