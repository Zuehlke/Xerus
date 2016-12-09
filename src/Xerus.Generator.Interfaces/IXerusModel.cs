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
//   The model is filled with all the definitions and then processed by the generators
//   to produce the desired output.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Xerus.Interfaces
{
    using System.Collections.Generic;

    /// <summary>
    ///     The model is filled with all the definitions and then processed by the generators
    ///     to produce the desired output.
    /// </summary>
    public interface IXerusModel
    {
        #region Public Properties

        /// <summary>
        ///     Gets all data types.
        /// </summary>
        /// <value>
        ///     The data types.
        /// </value>
        IEnumerable<IXerusDataType> DataTypes { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds a data type.
        /// </summary>
        /// <param name="type">
        /// The data type.
        /// </param>
        void AddDataType(IXerusDataType type);

        /// <summary>
        /// Validates the model
        /// </summary>
        /// <param name="errors">The errors</param>
        /// <returns>true if no errors, otherwise false</returns>
        bool Validate(ref string errors);

        #endregion

    }
}