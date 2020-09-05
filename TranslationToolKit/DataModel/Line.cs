using System;

namespace TranslationToolKit.DataModel
{
    /// <summary>
    /// Represents a line of data from the translation file.
    /// Note: This may not represent a file line, as we also include any comment above the actual text line.
    /// 
    /// Ex:
    /// # This is the button label for Fitness Mode
    /// Fitness=Fitness Mode
    /// 
    /// Title: 
    /// "Fitness"
    /// Text: 
    /// "Fitness=Fitness Mode"
    /// Comment:
    /// "# This is the button label for Fitness Mode"
    /// </summary>
    public sealed class Line : IEquatable<Line?>
    {
        /// <summary>
        /// The name of the line we're translating
        /// Ex: for line Fitness=Fitness Mode, title is Fitness.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// The Value 
        /// </summary>
        public string Value { get; }

        /// <summary>
        /// Comment may be above the line to translate, in which case, this field holds that.
        /// </summary>
        public string Comment { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public Line(string title, string text, string comment = "")
        {
            Key = title;
            Value = text;
            Comment = comment;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if(string.IsNullOrEmpty(Comment))
            {
                return $"{Key}={Value}";
            }
            return $"{Comment}\n{Key}={Value}";
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            return Equals(obj as Line);
        }

        /// <inheritdoc />
        public bool Equals(Line? other)
        {
            return other != null &&
                   Key == other.Key &&
                   Value == other.Value &&
                   Comment == other.Comment;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(Key, Value, Comment);
        }
    }
}
