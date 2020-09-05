using System;
using System.Collections.Generic;
using System.Text;

namespace TranslationToolKit.DataModel
{
    /// <summary>
    /// A class defining position info for Lines or Sections.
    /// Used in particular to track duplicate keys.
    /// </summary>
    public class Header : IEquatable<Header>
    {
        /// <summary>
        /// The Key identifying the object. In a perfectly formatted file, this would be enough.
        /// In real life, there can be duplicate keys.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// For 2 objects with the same key, track the order in which they appear.
        /// </summary>
        public int OccurenceIndex { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="occurenceIndex"></param>
        public Header(string key, int occurenceIndex)
        {
            Key = key;
            OccurenceIndex = occurenceIndex;
        }


        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return Equals(obj as Header);
        }

        /// <inheritdoc />
        public bool Equals(Header other)
        {
            return other != null &&
                   Key == other.Key &&
                   OccurenceIndex == other.OccurenceIndex;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(Key, OccurenceIndex);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[Key:{Key}|Index:{OccurenceIndex}]";
        }
    }
}
