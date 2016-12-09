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

    /// <summary>
    ///     Enumeration data type
    /// </summary>
    public class XerusEnumType : XerusDataTypeBase
    {
    #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDataType"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        public XerusEnumType(string name)
        {
            this.Name = name;
            this.Members = new List<XerusEnumMemberType>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumDataType"/> class.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        public XerusEnumType(string name, string description)
        {
            this.Name = name;
            this.Description = description;
            this.Members = new List<XerusEnumMemberType>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets the members of the enumeration
        /// </summary>
        /// <value>
        ///     The elements.
        /// </value>
        public List<XerusEnumMemberType> Members { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Adds an member to the enumeration
        /// </summary>
        /// <param name="member">
        /// The member
        /// </param>
        /// <returns>
        /// The enumeration
        /// </returns>
        public XerusEnumType AddMember(XerusEnumMemberType member)
        {
            this.Members.Add(member);
            return this;
        }

        /// <summary>
        /// Adds the member.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="EnumDataType"/>.
        /// </returns>
        public XerusEnumType AddMember(string name, string description, int value)
        {
            this.Members.Add(new XerusEnumMemberType(name, description, value));
            return this;
        }

        /// <summary>
        /// Adds the member.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="EnumDataType"/>.
        /// </returns>
        public XerusEnumType AddMember(string name, int value)
        {
            this.Members.Add(new XerusEnumMemberType(name, value));
            return this;
        }

        /// <summary>
        /// Adds the member.
        /// </summary>
        /// <param name="name">
        /// The name.
        /// </param>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <returns>
        /// The <see cref="EnumDataType"/>.
        /// </returns>
        public XerusEnumType AddMember(string name, string description)
        {
            this.Members.Add(new XerusEnumMemberType(name, description));
            return this;
        }

        /// <summary>
        /// Sets the description of the enumeration
        /// </summary>
        /// <param name="description">
        /// The description.
        /// </param>
        /// <returns>
        /// The enumeration
        /// </returns>
        public XerusEnumType WithDescription(string description)
        {
            this.Description = description;
            return this;
        }

        #endregion

        /// <summary>
        /// Assign missing enumeration values automatically.
        /// </summary>
        public XerusEnumType AutoAssignValues()
        {
            int currentIndex = 0;
            foreach (var enumDataTypeMember in Members)
            {
                if (enumDataTypeMember.ValueDefined)
                {
                    currentIndex = enumDataTypeMember.DefinedValue+1;
                }
                else
                {
                    enumDataTypeMember.DefinedValue = currentIndex;
                    currentIndex++;
                }
            }
            return this;
        }

        /// <summary>
        /// Creates an EnumDataType from a C# enumeration.
        /// </summary>
        /// <param name="enumeration">The enumeration.</param>
        /// <returns>The created EnumDataType</returns>
        public static XerusEnumType CreateFromEnum(Type enumeration)
        {
            if (!enumeration.IsEnum)
            {
                throw new ArgumentException("Given parameter is not an enum");
            }

            var enumDataType = new XerusEnumType(enumeration.Name, enumeration.GetDocumentation());
            foreach (var value in Enum.GetValues(enumeration))
            {
                var memberInfos = enumeration.GetMember(value.ToString()).FirstOrDefault();
                var customAttributes = value.GetType().GetCustomAttributes();
                var definedValue = (int)value;
                string documentation = memberInfos.GetDocumentation();
                XerusEnumMemberType xerusEnumMemberType = new XerusEnumMemberType(value.ToString()).WithDefinedValue(definedValue).WithDescription(documentation);
                enumDataType.Members.Add(xerusEnumMemberType);
            }
            return enumDataType;
        }

    }
}