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
    using System.Collections.Generic;

    using Xerus.Interfaces;

    /// <summary>
    /// Base type definiton for xerus types
    /// </summary>
    public class XerusBaseType : XerusDataTypeBase
    {
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the language specific type definitions.
        /// </summary>
        /// <value>
        /// The language specific type definitions.
        /// </value>
        public List<LanguageSpecificTypeDefinition> LanguageSpecificTypeDefinitions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XerusBaseType"/> class.
        /// </summary>
        public XerusBaseType()
        {
            LanguageSpecificTypeDefinitions = new List<LanguageSpecificTypeDefinition>();
        }

        /// <summary>
        /// Adds the type definition.
        /// </summary>
        /// <param name="typeDefinition">The type definition.</param>
        /// <returns>The data type</returns>
        public XerusBaseType AddTypeDefinition(LanguageSpecificTypeDefinition typeDefinition)
        {
            LanguageSpecificTypeDefinitions.Add(typeDefinition);
            return this;
        }
    }
}