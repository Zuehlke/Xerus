// --------------------------------------------------------------------------------------------------------------------
// <copyright>
// Copyright (c) 2016 Zühlke Engineering GmbH
// Permission is hereby granted, free of charge, to any person 
// obtaining a copy of this software and associated
// documentation files (the "Software"), to deal in the Software
// without restriction, including without limitation the rights to
// use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to
// whom the Software is furnished to do so, subject to the
// following conditions:
// The above copyright notice and this permission notice shall
// be included in all copies or substantial portions of the
// Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE
// OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   The text builder.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xerus.TemplateEngine
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    ///     The text builder.
    /// </summary>
    public class TextBuilder
    {
        #region Fields

        /// <summary>
        ///     The underlying StringBuilder which stores the text
        /// </summary>
        private readonly StringBuilder stringBuilder = new StringBuilder();

        /// <summary>
        ///     Current level of indentation
        /// </summary>
        private uint indentationLevel = 0;

        /// <summary>
        /// Indicates if this is a new line
        /// </summary>
        private bool startOfNewLine = true;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Appends the specified text.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        public void Append(string text)
        {
             var correctedText = this.HandleIndentation(text);
             this.stringBuilder.Append(correctedText);

        }


        /// <summary>
        /// Appends the specified text.
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        public void Append(string text, params object[] args)
        {
            var correctedText = this.HandleIndentation(text);
            this.stringBuilder.Append(string.Format(correctedText, args));

        }

        /// <summary>
        ///     Appends the line.
        /// </summary>
        public void AppendLine()
        {
            this.Append(Environment.NewLine);
        }

        /// <summary>
        /// Appends the line.
        /// </summary>
        /// <param name="line">
        /// The line.
        /// </param>
        public void AppendLine(string line)
        {
            this.Append(line + Environment.NewLine);
        }

        /// <summary>
        /// Appends the line.
        /// </summary>
        /// <param name="line">
        /// The line.
        /// </param>
        public void AppendLine(string line, params object[] args)
        {
            this.Append(string.Format(line,args) + Environment.NewLine);
        }

        /// <summary>
        ///     Reduces the indentation level by one
        /// </summary>
        public void PopIndent()
        {
            this.PopIndent(1);
        }

        /// <summary>
        ///     Increases the indentation level by one
        /// </summary>
        public void PushIndent()
        {
            this.PushIndent(1);
        }

        /// <summary>
        ///     The to string.
        /// </summary>
        /// <returns>
        ///     The <see cref="string" />.
        /// </returns>
        public override string ToString()
        {
            return this.stringBuilder.ToString();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fixes the indentation of the given string according to the current indentation level
        /// </summary>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <returns>
        /// The fixed text
        /// </returns>
        private string HandleIndentation(string text)
        {
            StringBuilder sb = new StringBuilder();
            string indentation = string.Empty;
            for (int i = 0; i < indentationLevel; i++)
            {
                indentation += "\t";
            }

            using (StringReader sr = new StringReader(text))
            {
                string line = string.Empty;
                while((line = sr.ReadLine())!=null)
                {
                    if (startOfNewLine)
                    {
                        sb.AppendLine(indentation + line);
                    }
                    else
                    {
                        sb.AppendLine(line);
                    }
                }   
            }

            //Remove last line break if text doesn't end with a line break
            startOfNewLine = text.EndsWith(Environment.NewLine);
            if (!startOfNewLine && sb.Length>=Environment.NewLine.Length)
            {
                sb.Remove(sb.Length - Environment.NewLine.Length, Environment.NewLine.Length);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Decreases the indentation level by the given level
        /// </summary>
        /// <param name="level">
        /// The level.
        /// </param>
        public void PopIndent(uint level)
        {
            if (level <= this.indentationLevel)
            {
                this.indentationLevel -= level;
            }
            else
            {
                this.indentationLevel = 0;
            }
        }

        /// <summary>
        /// Increases the indentation level by the given level
        /// </summary>
        /// <param name="level">
        /// The level.
        /// </param>
        public void PushIndent(uint level)
        {
            this.indentationLevel += level;
        }

        #endregion
    }
}