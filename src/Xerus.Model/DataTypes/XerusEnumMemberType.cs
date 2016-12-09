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
    public class XerusEnumMemberType
    {
           /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the defined value.
        /// </summary>
        /// <value>
        /// The defined value.
        /// </value>
        public int DefinedValue { get; set; }

        /// <summary>
        /// Gets a value indicating whether [value defined].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [value defined]; otherwise, <c>false</c>.
        /// </value>
        public bool ValueDefined { get; private set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDataTypeMember"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public XerusEnumMemberType(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDataTypeMember"/> class.
        /// </summary>
        /// <param name="definedValue">
        /// The defined value.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public XerusEnumMemberType(string name, int definedValue)
        {
            this.DefinedValue = definedValue;
            this.ValueDefined = true;
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDataTypeMember"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public XerusEnumMemberType(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDataTypeMember"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="definedValue">
        /// The defined value.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public XerusEnumMemberType(string name, string description, int definedValue)
        {
            this.Name = name;
            this.DefinedValue = definedValue;
            this.ValueDefined = true;
            this.Description = description;
        }

        /// <summary>
        ///  Sets the defined value of the enumeration element
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The enumeration element</returns>
        public XerusEnumMemberType WithDefinedValue(int value)
        {
            this.DefinedValue = value;
            this.ValueDefined = true;
            return this;
        }

        /// <summary>
        /// Sets the description of the enumeration element
        /// </summary>
        /// <param name="description">The description.</param>
        /// <returns>The enumeration element</returns>
        public XerusEnumMemberType WithDescription(string description)
        {
            this.Description = description;
            return this;
        }
    }
}