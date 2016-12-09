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
namespace Xerus.Interfaces
{
    using System;
    using System.Collections.Generic;

    public interface IXerusIndexList : IEnumerable<XerusIndexElement>
    {
        /// <summary>
        /// Gets the number of entires
        /// </summary>
        /// <value>
        /// The number of entries.
        /// </value>
        int Count { get; }
    }

    /// <summary>
    /// The different index types
    /// </summary>
    public enum XerusIndexType
    {
        Enum,
        Value,
    }

    /// <summary>
    /// Index Element
    /// </summary>
    public class XerusIndexElement
    {
        /// <summary>
        /// Gets or sets the type of the index.
        /// </summary>
        /// <value>
        /// The type of the index.
        /// </value>
        public XerusIndexType IndexType { get; set; }
        /// <summary>
        /// Gets or sets the numeric value.
        /// </summary>
        /// <value>
        /// The numeric value.
        /// </value>
        public int NumericValue { get; set; }
        /// <summary>
        /// Gets or sets the enum value.
        /// </summary>
        /// <value>
        /// The enum value.
        /// </value>
        public Type IndexerType { get; set; }
    }
}