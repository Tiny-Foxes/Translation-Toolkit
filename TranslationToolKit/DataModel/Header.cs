using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace TranslationToolKit.DataModel
{
    /// <summary>
    /// A class defining position info for Lines or Sections.
    /// Used in particular to track duplicate keys.
    /// </summary>
    public class Header : IEquatable<Header?>, IComparable<Header>
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
        /// The Index of this line within a file.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="occurenceIndex"></param>
        public Header(string key, int occurenceIndex, int index)
        {
            Key = key;
            OccurenceIndex = occurenceIndex;
            Index = index;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[Key:{Key}|OccurrenceIndex:{OccurenceIndex}|Index:{Index}]";
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return Equals(obj as Header);
        }

        /// <inheritdoc />
        public bool Equals(Header? other)
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

        /// <summary>
        /// Implementing IComparable to order within our dictionary.
        /// We order by index.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo([AllowNull] Header other)
        {
            if(other == null)
            {
                return -1;
            }
            return Index - other.Index;
        }
    }
}
