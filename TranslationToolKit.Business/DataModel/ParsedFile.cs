using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace TranslationToolKit.Business.DataModel
{
    /// <summary>
    /// A class representing a file parsed into sections.
    /// </summary>
    public class ParsedFile : IEnumerable<KeyValuePair<Header, Section>>
    {
        /// <summary>
        /// If there is text before the first section, we consider that to be a header
        /// and we store it separately.
        /// </summary>
        public string FileHeader { get; set;  }

        /// <summary>
        /// List of all sections in the file
        /// </summary>
        private SortedDictionary<Header, Section> Sections { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public ParsedFile()
        {
            FileHeader = "";
            Sections = new SortedDictionary<Header, Section>();
        }

        /// <summary>
        /// Add a section to the file.
        /// </summary>
        /// <param name="line">the line to be added</param>
        /// <param name="index">index of this section within the file</param>
        public void AddSection(Section section, int index)
        {
            int occurenceIndex = -1;
            if (Sections.Any(x => x.Key.HeaderKey.Equals(section.Title)))
            {
                occurenceIndex = Sections.Where(x => x.Key.HeaderKey.Equals(section.Title))
                                      .Max(x => x.Key.OccurenceIndex);
            }
            Sections.Add(new Header(section.Title, 0, index), section);
        }

        #region Implementing various interfaces to allow checking the data, but not modifying list without using the proper add methods.

        /// <summary>
        /// An operator that allows to fetch sections, 
        /// but not modify them without using the proper methods.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Section this[Header i]
        {
            get { return Sections[i]; }
        }

        /// <summary>
        /// An operator that allows to fetch sections, 
        /// but not modify them without using the proper methods.
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Section this[int i]
        {
            get { return Sections[Header.GetIndex(i)]; }
        }

        /// <inheritdoc />
        public IEnumerator<KeyValuePair<Header, Section>> GetEnumerator()
        {
            return Sections.GetEnumerator();
        }

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return Sections.GetEnumerator();
        }

        /// <summary>
        /// Count of sections in the file.
        /// </summary>
        public int Count => Sections.Count;

        /// <summary>
        /// Headers of the sections.
        /// </summary>
        public SortedDictionary<Header, Section>.KeyCollection Keys => Sections.Keys;

        #endregion Implementing various interfaces to allow checking the data, but not modifying list
    }
}
