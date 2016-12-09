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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Xerus.Model.Attributes;

    /// <summary>
    /// Contains extension methods for data types to simplify the reflection handling
    /// </summary>
    public static class DataTypesExtensions
    {
        /// <summary>
        /// Gets the documentation.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static string GetDocumentation(this Type type)
        {
            string documentation = null;
            var documentationAttribute = type.GetCustomAttributes(typeof(DocumentationAttribute)).FirstOrDefault() as DocumentationAttribute;
            if (documentationAttribute != null)
            {
                documentation = documentationAttribute.Documentation;
            }
            return documentation;
        }

        /// <summary>
        /// Gets the documentation.
        /// </summary>
        /// <param name="memberInfo">The member information.</param>
        /// <returns></returns>
        public static string GetDocumentation(this MemberInfo memberInfo)
        {
            string documentation = null;
            if (memberInfo != null)
            {
                var documentationAttribute =
                    memberInfo.GetCustomAttributes(typeof(DocumentationAttribute)).FirstOrDefault() as
                    DocumentationAttribute;
                if (documentationAttribute != null)
                {
                    documentation = documentationAttribute.Documentation;
                }
            }
            return documentation;
        }

        /// <summary>
        /// Initializes the array with the given default value
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="value">The value.</param>
        public static void SetDefault(this Array array, object value)
        {
            List<int> currentIndex = new List<int>();
            for (int i = 0; i < array.Rank; i++)
            {
                currentIndex.Add(0);
            }
            ProcessIndex(0, currentIndex, value, array);
        }

        /// <summary>
        /// Initializes the given index of an array with the given value
        /// This method should only be used by SetDefault
        /// </summary>
        /// <param name="indexLevel">The index level.</param>
        /// <param name="currentIndex">Index of the current.</param>
        /// <param name="value">The value.</param>
        /// <param name="array">The array.</param>
        private static void ProcessIndex(int indexLevel, List<int> currentIndex, object value, Array array)
        {
            var upperBound = array.GetLength(indexLevel);
            for (currentIndex[indexLevel] = 0; currentIndex[indexLevel] < upperBound; currentIndex[indexLevel]++)
            {
                if (indexLevel != array.Rank - 1)
                {
                    ProcessIndex(indexLevel + 1, currentIndex, value, array);
                }
                else
                {
                    array.SetValue(value, currentIndex.ToArray());
                }
            }
        }
    }
}