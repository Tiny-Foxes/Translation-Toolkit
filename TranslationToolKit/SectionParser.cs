using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TranslationToolKit.DataModel;

namespace TranslationToolKit
{
    public static class SectionParser
    {
        public enum MyEnum
        {

        }
        public static Section ParseSection(IList<string> lines)
        {
            if(lines == null || lines.Count == 0)
            {
                throw new ArgumentException("Tried to parse section but provided lines list is either null or empty", nameof(lines));
            }
            var section = new Section();

            var titleLine = lines.FirstOrDefault(x => x.StartsWith("["));
            section.Title = titleLine ?? throw new ArgumentException("Tried to parse section but no section title found", nameof(lines));

            string comment = null;
            foreach(var line in lines.Skip(lines.IndexOf(titleLine) +1))
            {
                ProcessLine(line.TrimStart(), section, ref comment);
            }
            return section;
        }

        private static void ProcessLine(string line, Section section, ref string comment)
        {
            if(string.IsNullOrWhiteSpace(line))
            {
                section.AddEmptyLine();
                return;
            }

            int index = line.IndexOf('=');
            if (line.StartsWith("#"))
            {
                if (comment == null)
                {
                    comment = line;
                }
                else
                {
                    comment += line;
                }
            }
            else if (index >= 1)
            {
                var substring = line.Substring(0, index);
                section.Lines.Add(substring, new Line(substring, line, comment));
                comment = null;
            }
        }
    }
}
