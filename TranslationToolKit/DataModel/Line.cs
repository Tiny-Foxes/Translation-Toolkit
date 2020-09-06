using System;
using System.Reflection;

namespace TranslationToolKit.DataModel
{
    /// <summary>
    /// Represents a line of data from the translation file.
    /// Note: This may not represent a single file line, as we also include any comment above the actual text line.
    /// 
    /// Ex:
    /// # This is the button label for Fitness Mode
    /// Fitness=Fitness Mode
    /// 
    /// TranslationKey: 
    /// "Fitness"
    /// TranslatedValue: 
    /// "Fitness=Fitness Mode"
    /// Comment:
    /// "# This is the button label for Fitness Mode"
    /// </summary>
    public sealed class Line : IEquatable<Line?>
    {
        /// <summary>
        /// The name of the line we're translating
        /// Ex: for line Fitness=Fitness Mode, TranslationKey is "Fitness".
        /// </summary>
        public string TranslationKey { get; }

        /// <summary>
        /// The translated value.
        /// Ex: for line Fitness=Fitness Mode, TranslatedValue is "Fitness Mode".
        /// </summary>
        public string TranslatedValue { get; }

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
            TranslationKey = title;
            TranslatedValue = text;
            Comment = comment;
        }

        /// <summary>
        /// returns a proper representation of the line, ready to be displayed/written.
        /// </summary>
        /// <returns></returns>
        public string DisplayString
        {
            get
            {
                if (string.IsNullOrEmpty(TranslationKey))
                {
                    if (string.IsNullOrEmpty(Comment))
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return $"{Comment}{EnvironmentConstants.EndOfLine}";
                    }
                }
                if (string.IsNullOrEmpty(Comment))
                {
                    return $"{TranslationKey}={TranslatedValue}";
                }
                return $"{Comment}{EnvironmentConstants.EndOfLine}{TranslationKey}={TranslatedValue}";
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return DisplayString;
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
                   TranslationKey == other.TranslationKey &&
                   TranslatedValue == other.TranslatedValue &&
                   Comment == other.Comment;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(TranslationKey, TranslatedValue, Comment);
        }
    }
}
