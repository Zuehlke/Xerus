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
using System;

namespace Xerus.Model.IntrinsicTypes
{
    using System.Collections.Generic;
    using System.Globalization;

    using Xerus.Interfaces;
    using Xerus.Model.Attributes;
    using Xerus.Model.DataTypes;

    public static class IntrinsicTypes
    {
        /// <summary>
        /// Returns a list of all intrinsic types
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<XerusBaseType> GetTypes()
        {
            var types = new List<XerusBaseType>();
            types.Add(new XerusFloat());
            types.Add(new XerusUInt8());
            types.Add(new XerusUInt16());
            types.Add(new XerusUInt32());
            types.Add(new XerusBool());
            return types;
        }
    }

    [Documentation("Boolean value")]
    public class XerusBool : XerusBaseType
    {
        public XerusBool()
        {
            this.AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("bool").InLanguage(TargetLanguage.Cpp).WithDefaultValue("false").WithNumberOfBytes(1))
   .AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("bool").InLanguage(TargetLanguage.CSharp).WithDefaultValue("false").WithNumberOfBytes(1))
  .AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("Boolean").InLanguage(TargetLanguage.Documentation));
        }

        public XerusBool(bool value)
            :this()
        {
            this.Value = value.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// The xerus float.
    /// </summary>
    [Documentation("Floating point value")]
    public class XerusFloat : XerusBaseType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XerusFloat"/> class.
        /// </summary>
        public XerusFloat()
        {
            this.AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("float").InLanguage(TargetLanguage.Cpp).UsesSuffix("f").WithDefaultValue("0").WithNumberOfBytes(4))
               .AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("Single").InLanguage(TargetLanguage.CSharp).UsesSuffix("F").WithDefaultValue("0").WithNumberOfBytes(4))
              .AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("Float").InLanguage(TargetLanguage.Documentation));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XerusFloat"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public XerusFloat(float value)
            : this()
        {
            this.Value = value.ToString(CultureInfo.InvariantCulture);
        }
    }

    /// <summary>
    /// The xerus UInt32.
    /// </summary>
    [Documentation("Unsigned Int 32 value")]
    public class XerusUInt32 : XerusBaseType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XerusChar"/> class.
        /// </summary>
        public XerusUInt32()
        {
            this.AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("UInt32").InLanguage(TargetLanguage.CSharp).WithDefaultValue("0").WithNumberOfBytes(4))
                .AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("uint32").InLanguage(TargetLanguage.Cpp).WithDefaultValue("0").WithNumberOfBytes(4))
                .AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("UInt32").InLanguage(TargetLanguage.Documentation));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XerusChar"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public XerusUInt32(int value)
            : this()
        {
            this.Value = value.ToString(CultureInfo.InvariantCulture);
        }

    }

    /// <summary>
    /// The xerus UInt32.
    /// </summary>
    [Documentation("Unsigned Int 16 value")]
    public class XerusUInt16 : XerusBaseType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XerusChar"/> class.
        /// </summary>
        public XerusUInt16()
        {
            this.AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("UInt16").InLanguage(TargetLanguage.CSharp).WithDefaultValue("0").WithNumberOfBytes(2))
                .AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("uint16").InLanguage(TargetLanguage.Cpp).WithDefaultValue("0").WithNumberOfBytes(2))
                .AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("UInt16").InLanguage(TargetLanguage.Documentation));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XerusChar"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public XerusUInt16(int value)
            : this()
        {
            this.Value = value.ToString(CultureInfo.InvariantCulture);
        }

    }

    /// <summary>
    /// The xerus UInt32.
    /// </summary>
    [Documentation("Unsigned Int 8 value")]
    public class XerusUInt8 : XerusBaseType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XerusChar"/> class.
        /// </summary>
        public XerusUInt8()
        {
            this.AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("Byte").InLanguage(TargetLanguage.CSharp).WithDefaultValue("0").WithNumberOfBytes(1))
                .AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("uint8").InLanguage(TargetLanguage.Cpp).WithDefaultValue("0").WithNumberOfBytes(1))
                .AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("UInt8").InLanguage(TargetLanguage.Documentation));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XerusUInt8"/> class.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public XerusUInt8(int value)
            : this()
        {
            this.Value = value.ToString(CultureInfo.InvariantCulture);
        }

    }


    /// <summary>
    /// The xerus UInt32.
    /// </summary>
    [Documentation("void")]
    public class XerusVoid : XerusBaseType
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XerusChar"/> class.
        /// </summary>
        public XerusVoid()
        {
            this.AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("void").InLanguage(TargetLanguage.CSharp).WithNumberOfBytes(0))
                .AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("void").InLanguage(TargetLanguage.Cpp).WithNumberOfBytes(0))
                .AddTypeDefinition(new LanguageSpecificTypeDefinition().NamedAs("void").InLanguage(TargetLanguage.Documentation));
        }

    }
}