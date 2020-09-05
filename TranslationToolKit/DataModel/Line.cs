﻿using System;

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
    public sealed class Line : IEquatable<Line>
    {
        /// <summary>
        /// The name of the line we're translating
        /// Ex: for line Fitness=Fitness Mode, title is Fitness.
        /// </summary>
        public string Title { get; }

        /// <summary>
        /// A full line of text, it includes the title
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Comment may be above the line to translate, in which case, this field holds that.
        /// </summary>
        public string Comment { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="title"></param>
        /// <param name="text"></param>
        public Line(string title, string text, string comment = null)
        {
            Title = title;
            Text = text;
            Comment = comment;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if(obj == null)
            {
                return false;
            }
            if (obj is Line)
            {
                return Equals(obj as Line);
            }
            else return false;
        }

        /// <inheritdoc />
        public bool Equals(Line other)
        {
            if(other == null)
            {
                return false;
            }
            return string.Equals(Title, other.Title)
                && string.Equals(Text, other.Text)
                && string.Equals(Comment, other.Comment);                    
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return HashCode.Combine(Title, Text, Comment);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            if(string.IsNullOrEmpty(Comment))
            {

            }
            return $"{Comment}\n{Title}={Text}";
        }
    }
}
