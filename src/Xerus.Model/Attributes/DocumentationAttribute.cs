﻿// --------------------------------------------------------------------------------------------------------------------
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
namespace Xerus.Model.Attributes
{
    using System;

    /// <summary>
    /// Attribute to provide a documentation for an element
    /// </summary>
    public class DocumentationAttribute : Attribute
    {
        /// <summary>
        /// Gets the documentation.
        /// </summary>
        /// <value>
        /// The documentation.
        /// </value>
        public string Documentation { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentationAttribute"/> class.
        /// </summary>
        /// <param name="documentation">
        /// The documentation.
        /// </param>
        public DocumentationAttribute(string documentation)
        {
            this.Documentation = documentation;
        }
    }
}