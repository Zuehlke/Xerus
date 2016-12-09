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
namespace Xerus.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Xerus.Interfaces;
    using Xerus.Model.DataTypes;

    /// <summary>
    /// Extensions methods to ease the handling of XerusDataTypes 
    /// </summary>
    public static class XerusDataTypeExtensions
    {
        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="targetLanguage">The target language.</param>
        /// <returns>The name of the type within the given target language</returns>
        public static string XerusTypeName(this Type fieldType, TargetLanguage targetLanguage)
        {
            string typeName = fieldType.Name;
            var typeDefinition = fieldType.TypeDefinition(targetLanguage);
            if (typeDefinition == null)
            {
                if (fieldType.IsArray)
                {
                    typeName = fieldType.GetElementType().Name;
                }
            }
            else
            {
                typeName = typeDefinition.Name;
            }
            return typeName;
        }

        /// <summary>
        /// Gets the language specific type definition
        /// </summary>
        /// <param name="fieldType">Type of the field.</param>
        /// <param name="targetLanguage">The target language.</param>
        /// <returns>The language specific type definition</returns>
        public static LanguageSpecificTypeDefinition TypeDefinition(this Type fieldType, TargetLanguage targetLanguage)
        {
            Type instanceType = fieldType;
            LanguageSpecificTypeDefinition definition = null;
            if (fieldType.HasElementType)
            {
                instanceType = fieldType.GetElementType();
            }
            var baseType = Activator.CreateInstance(instanceType) as XerusBaseType;
            if (baseType != null)
            {
                var languageSpecificTypeDefinition =
                    baseType.LanguageSpecificTypeDefinitions.Find(ts => ts.TargetLanguage == targetLanguage);
                if (languageSpecificTypeDefinition != null)
                {
                    definition = languageSpecificTypeDefinition;
                }
            }
            return definition;
        }

        /// <summary>
        /// Returns the base name of the given type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The base name</returns>
        public static string BaseName(this Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType().Name;
            }
            else
            {
                return type.Name;
            }
        }

        /// <summary>
        /// Creates the index paramters.
        /// </summary>
        /// <param name="indexations">The indexations.</param>
        /// <returns></returns>
        public static string CreateIndexParamters(this IEnumerable<XerusIndexElement> indexations)
        {
            var sb = new StringBuilder();
            uint index = 0;
            var first = true;

            if (indexations != null)
            {
                foreach (var indexation in indexations)
                {
                    if (!first)
                    {
                        sb.Append(", ");
                    }
                    else
                    {
                        first = false;
                    }

                    switch (indexation.IndexType)
                    {
                        case XerusIndexType.Enum:
                            sb.Append(indexation.IndexerType.Name);
                            break;
                        default:
                            sb.Append("uint16");
                            break;
                    }
                    sb.Append(string.Format(" index{0}", index));
                    index++;
                }
            }
            return sb.ToString();
        }

        public static uint GetNumberBytes(this Type type, TargetLanguage targetLanguage)
        {
            if (typeof(XerusBaseType).IsAssignableFrom(type))
            {
                return type.TypeDefinition(targetLanguage).NumberOfBytes;
            }
            if (typeof(XerusStruct).IsAssignableFrom(type))
            {
                var structType = (XerusStruct)Activator.CreateInstance(type);
                return GetNumberBytes(structType, targetLanguage);
            }
            if (typeof(XerusEnumType).IsAssignableFrom(type))
            {
                var enumType = (XerusEnumType)Activator.CreateInstance(type);
                return (uint)enumType.Members.Last().DefinedValue;
            }
           

            throw new Exception("This data type is not supported.");

        }


        private static uint GetNumberBytes(XerusStruct dataType, TargetLanguage targetLanguage)
        {
            uint factor = 1;
            uint size = 0;

            var fieldInfos = dataType.GetType().GetFields();
            foreach (var field in fieldInfos)
            {
                var value = field.GetValue(dataType);
                var fieldType = field.FieldType;


                if (fieldType.IsArray)
                {
                    var array = value as Array;
                    for (var rank = 0; rank < fieldType.GetArrayRank(); rank++)
                    {
                        if (array != null)
                        {
                            factor = factor * (uint)array.GetLength(rank);
                        }
                    }
                    size += GetNumberBytes(fieldType.GetElementType(), targetLanguage) * factor;
                    factor = 1;
                }
            }
            return size;

        }
    }
}
