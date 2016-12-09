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
// --------------------------------------------------------------------------------------------------------------------
namespace Xerus.Model.DataTypes
{
    using Xerus.Interfaces;

    /// <summary>
    /// Language specific type for the definition
    /// </summary>
    public class LanguageSpecificTypeDefinition
    {
        /// <summary>
        /// Gets or sets the target language.
        /// </summary>
        /// <value>
        /// The target language.
        /// </value>
        public TargetLanguage TargetLanguage { get; set; }
        /// <summary>
        /// Gets or sets the name in the specified language
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the suffix.
        /// </summary>
        /// <value>
        /// The suffix.
        /// </value>
        public string Suffix { get; set; }

        /// <summary>
        /// Gets or sets the default value.
        /// </summary>
        /// <value>
        /// The default value.
        /// </value>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Gets or sets the number of bytes of the data type.
        /// </summary>
        /// <value>
        /// The bytes.
        /// </value>
        public uint NumberOfBytes { get; set; }

        /// <summary>
        /// Sets the name of the data type in the specified language
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The LanguageSpecificTypeDefinition</returns>
        public LanguageSpecificTypeDefinition NamedAs(string name)
        {
            this.Name = name;
            return this;
        }

        /// <summary>
        /// Defines the language
        /// </summary>
        /// <param name="targetLanguage">The language.</param>
        /// <returns>The LanguageSpecificTypeDefinition</returns>
        public LanguageSpecificTypeDefinition InLanguage(TargetLanguage targetLanguage)
        {
            this.TargetLanguage = targetLanguage;
            return this;
        }

        /// <summary>
        /// Sets the used suffix in the specified language
        /// </summary>
        /// <param name="suffix">The suffix.</param>
        /// <returns>The LanguageSpecificTypeDefinition</returns>
        public LanguageSpecificTypeDefinition UsesSuffix(string suffix)
        {
            this.Suffix = suffix;
            return this;
        }

        /// <summary>
        /// Withes the default value.
        /// </summary>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The LanguageSpecificTypeDefinition</returns>
        public LanguageSpecificTypeDefinition WithDefaultValue(string defaultValue)
        {
            this.DefaultValue = defaultValue;
            return this;
        }


        /// <summary>
        /// Uses the given number of bytes
        /// </summary>
        /// <param name="numberOfBytes">The number of bytes.</param>
        /// <returns>The LanguageSpecificTypeDefinition</returns>
        public LanguageSpecificTypeDefinition WithNumberOfBytes(uint numberOfBytes)
        {
            this.NumberOfBytes = numberOfBytes;
            return this;
        }
    }
}