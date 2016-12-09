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
namespace Xerus.Generator.TE.DataLayer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Model;
    using Model.DataTypes;
    using TemplateEngine;

    /// <summary>
    /// Generates the DataTypes Header file
    /// </summary>
    public class DataTypesHeaderGenerator : Template
    {

        /// <summary>
        /// Generates the DataTypes.h file from the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public string Generate(IXerusModel model)
        {
            this.AppendLine(@"/******************************************************");
            this.AppendLine(@" *  DataTypes.h");
            this.AppendLine(@" *-----------------------------------------------------");
            this.AppendLine(@" * Generated data types");
            this.AppendLine(@" *-----------------------------------------------------");
            this.AppendLine(@" *");
            this.AppendLine(@" ******************************************************");
            this.AppendLine(@"*/");
            this.AppendLine();
            this.AppendLine(@"#ifndef __DataTypes_h__");
            this.AppendLine(@"#define __DataTypes_h__");
            this.AppendLine();
            this.AppendLine(@"typedef unsigned char       uint8;          //!< 8 bit unsigned integer");
            this.AppendLine(@"typedef unsigned short      uint16;         //!< 16 bit unsigned integer");
            this.AppendLine(@"typedef unsigned long       uint32;         //!< 32 bit unsigned integer");
            this.AppendLine();

            IEnumerable<IXerusDataType> sortedDataTypes = Sort(model.DataTypes);

            foreach (var dataType in sortedDataTypes)
            {
                this.Create(dataType);
                this.AppendLine();
            }
            this.AppendLine();
            this.AppendLine(@"#endif");
            return this.ToString();
        }

        private IEnumerable<IXerusDataType> Sort(IEnumerable<IXerusDataType> dataTypes)
        {
            List<IXerusDataType> types = new List<IXerusDataType>();
            foreach (var xerusDataType in dataTypes)
            {
                if (!(xerusDataType is XerusStruct))
                {
                    types.Add(xerusDataType);
                }
            }
            foreach (var xerusDataType in dataTypes.Where(t => t is XerusStruct))
            {
                ProcessStruct(types, xerusDataType);
            }

            return types;
        }

        private static void ProcessStruct(List<IXerusDataType> dataTypes, IXerusDataType xerusDataType)
        {
            foreach (var memberInfo in xerusDataType.GetType().GetFields())
            {
                if (typeof(XerusStruct).IsAssignableFrom(memberInfo.FieldType))
                {
                    var baseType = memberInfo.GetValue(xerusDataType) ?? Activator.CreateInstance(memberInfo.FieldType) as XerusStruct;

                    if (!dataTypes.Contains(baseType))
                    {
                        ProcessStruct(dataTypes, (IXerusDataType)baseType);
                    }

                }
            }

            if(dataTypes.Find(
                dataType => dataType.GetType() == xerusDataType.GetType()) == null)
            {
                dataTypes.Add(xerusDataType);
            }

        }

        /// <summary>
        /// Creates the type definition for the specified data type.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        private void Create(IXerusDataType dataType)
        {
            this.Create(dataType as dynamic);
        }

        /// <summary>
        /// Creates the type definition for the specified basic data type.
        /// </summary>
        /// <param name="basicDataType">Type of the basic data.</param>
        private void Create(XerusBaseType basicDataType)
        {
            // Basic data types are build into the language and so we do not need to define them here
        }

        /// <summary>
        /// Creates the the type definition for specified enumeration data type.
        /// </summary>
        /// <param name="enumDataType">Type of the enumeration data type.</param>
        private void Create(XerusEnumType enumDataType)
        {
            this.AddMultiLineDescription(enumDataType.Description);
            this.AppendLine("enum " + enumDataType.Name);
            this.AppendLine("{");
            this.PushIndent();
            for (var i = 0; i < enumDataType.Members.Count; i++)
            {
                var member = enumDataType.Members[i];
                var last = i == enumDataType.Members.Count - 1;
                this.Append(member.Name + " = " + member.DefinedValue);
                if (!last)
                {
                    this.Append(",");
                }

                this.AddSingleLineDescription(member.Description);
                this.AppendLine();
            }
            this.PopIndent();
            this.AppendLine("};");
        }

        /// <summary>
        /// Add multi line description if one was provided
        /// </summary>
        /// <param name="description">The description.</param>
        private void AddMultiLineDescription(string description)
        {
            if (!string.IsNullOrEmpty(description))
            {
                this.AppendLine("/**");
                this.AppendLine("  * " + description);
                this.AppendLine("  */");
            }
        }

        /// <summary>
        /// Add single line description if one was provided
        /// </summary>
        /// <param name="description">The description.</param>
        private void AddSingleLineDescription(string description)
        {
            if (!string.IsNullOrEmpty(description))
            {
                this.Append(" //!< " + description);
            }
        }


        /// <summary>
        /// Creates the the type definition for specified structure data type.
        /// </summary>
        /// <param name="structDataType">Type of the structure data.</param>
        /// <exception cref="ModelValidationException">If an error within the model exists</exception>
        private void Create(XerusStruct structDataType)
        {
            this.AddMultiLineDescription(structDataType.Description);
            this.AppendLine("struct " + structDataType.Name);
            this.AppendLine("{");
            this.PushIndent();
            foreach (var memberInfo in structDataType.GetType().GetFields())
            {
                var fieldType = memberInfo.FieldType;
                if (fieldType.IsArray)
                {
                    var memberType = fieldType.GetElementType().XerusTypeName(TargetLanguage.Cpp);
                    var arrayRank = fieldType.GetArrayRank();
                    var value = memberInfo.GetValue(structDataType);
                    if (value == null)
                    {
                        throw new ModelValidationException(string.Format("The dimension of the array {0}.{1} has to be specifed. e.g. DataType[,] Member1 = new DataType[5,2]", structDataType.Name, memberType));
                    }

                    this.Append(memberType + " " + memberInfo.Name);
                    for (int rank = 0; rank < arrayRank; rank++)
                    {
                        var array = value as Array;
                        this.Append(string.Format("[{0}]", array.GetLength(rank)));
                    }

                    this.Append(";");
                }
                else
                {
                    this.Append(memberInfo.FieldType.XerusTypeName(TargetLanguage.Cpp) + " " + memberInfo.Name + ";");
                }
                var documentation = memberInfo.GetDocumentation();
                if (documentation != null)
                {
                    this.AddSingleLineDescription(documentation);
                }
                this.AppendLine();

            }
            this.PopIndent();
            this.AppendLine("};");
        }


    }
}