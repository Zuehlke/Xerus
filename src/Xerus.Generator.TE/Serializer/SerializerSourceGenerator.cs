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
    using System.Text;
    using Interfaces;
    using Model;
    using Model.DataTypes;
    using TemplateEngine;

    /// <summary>
    /// Generator for the serializer source file
    /// </summary>
    public class SerializerSourceGenerator : TemplateEngine.Template
    {
        /// <summary>
        /// Generates the serializers for the specified data Types.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="dataTypes">The data types.</param>
        /// <returns>The source code as string</returns>
        public string Generate(IXerusModel model, IEnumerable<IXerusDataType> dataTypes, string name)
        {
            this.AppendLine(@"/******************************************************");
            this.AppendLine(@" *  " + name + ".cpp");
            this.AppendLine(@" *-----------------------------------------------------");
            this.AppendLine(@" * ");
            this.AppendLine(@" * ");
            this.AppendLine(@" *------------------------------------------------------");
            this.AppendLine(@" *      Serializers for the used data types");
            this.AppendLine(@" *-------------------------------------------------------");
            this.AppendLine(@" *");
            this.AppendLine(@" *******************************************************/");
            this.AppendLine();
            this.AppendLine("#include <" + name + ".h>");
            this.AppendLine();
            foreach (var xerusDataType in dataTypes)
            {
                this.AppendLine(GenerateSerializerDefinition(xerusDataType as dynamic, name));
                this.AppendLine(GenerateDeserializerDefinition(xerusDataType as dynamic, name));
            }

            return this.ToString();
        }

        /// <summary>
        /// Generates the deserializer code for an enum.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>Derializer code for enums</returns>
        private string GenerateDeserializerDefinition(XerusEnumType dataType, string serializerName)
        {
            string name = dataType.Name;

            int maxValue = 0;
            foreach (var xerusEnumMemberType in dataType.Members)
            {
                if (xerusEnumMemberType.DefinedValue > maxValue)
                {
                    maxValue = xerusEnumMemberType.DefinedValue;
                }
            }

            var tb = new TextBuilder();
            tb.AppendLine(string.Format(
@"/** Deserializer for data type {0}
*  @param data Buffer with the serialized data
*  @return Deserialized data type
*/", name));
            tb.AppendLine("uint16 " + serializerName + "::Bytes_To_{0}(const uint8 *const data, {0}& output )", name);
            tb.AppendLine("{");
            tb.PushIndent();

            if (maxValue > 65535)
            {
                tb.AppendLine("return Bytes_To_uint32(data, (uint32&) output);", name);
            }
            else if (maxValue > 255)
            {
                tb.AppendLine("return Bytes_To_uint16(data, (uint16&) output);", name);
            }
            else
            {
                tb.AppendLine("return Bytes_To_uint8(data, (uint8&) output);", name);
            }

            tb.PopIndent();
            tb.AppendLine("}");
            return tb.ToString();
        }

        /// <summary>
        /// Generates the serializer code for an enum
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>Serializer code for enums</returns>
        private string GenerateSerializerDefinition(XerusEnumType dataType, string serializerName)
        {
            string name = dataType.Name;

            int maxValue = 0;
            foreach (var xerusEnumMemberType in dataType.Members)
            {
                if (xerusEnumMemberType.DefinedValue > maxValue)
                {
                    maxValue = xerusEnumMemberType.DefinedValue;
                }
            }

            var tb = new TextBuilder();
            tb.AppendLine(string.Format(
@"/** Serializer for data type {0}
*  @param input Data type
*  @param data Buffer for the serialized data
*  @return Length of the serialized data in bytes
*/", name));
            tb.AppendLine("uint16 "+serializerName+"::{0}_To_Bytes(const {0} input, uint8 *const data)", name);
            tb.AppendLine("{");
            tb.PushIndent();

            if (maxValue > 65535)
            {
                tb.AppendLine("uint32_To_Bytes((const uint32)input, data);");
                tb.AppendLine("return 4;");
            }
            else if (maxValue > 255)
            {
                tb.AppendLine("uint16_To_Bytes((const uint16)input, data);");
                tb.AppendLine("return 2;");
            }
            else
            {
                tb.AppendLine("data[0] = input;");
                tb.AppendLine("return 1;");
            }
            tb.PopIndent();
            tb.AppendLine("}");
            return tb.ToString();
        }


        /// <summary>
        /// Generates the struct serializer code.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>Serializer code for structs</returns>
        private string GenerateSerializerDefinition(XerusStruct dataType, string serializerName)
        {
            string name = dataType.Name;
            var tb = new TextBuilder();
            tb.AppendLine(string.Format(
@"/** Serializer for data type {0}
*  @param input Data type
*  @param data Buffer for the serialized data
*  @return Length of the serialized data in bytes
*/", name));
            tb.AppendLine("uint16 "+serializerName+"::{0}_To_Bytes(const {0} input, uint8 *const data)", name);
            tb.AppendLine("{");
            tb.PushIndent();

            tb.AppendLine("int offset = 0;");

            foreach (var memberInfo in dataType.GetType().GetFields())
            {
                var fieldType = memberInfo.FieldType;

                if (fieldType.IsArray)
                {
                    var arrayRank = fieldType.GetArrayRank();
                    var value = memberInfo.GetValue(dataType);
                    if (value == null)
                    {
                        throw new ModelValidationException(string.Format("The dimension of the array {0}.{1} has to be specifed. e.g. DataType[,] Member1 = new DataType[5,2]", dataType.Name, memberInfo.Name));
                    }

                    var array = value as Array;
                    var indexString = new StringBuilder();

                    for (int currentRank = 0; currentRank < arrayRank; currentRank++)
                    {
                        tb.AppendLine("for (int index{0} = 0; index{0} < {1}; index{0}++)", currentRank, array.GetLength(currentRank));
                        tb.AppendLine("{");
                        tb.PushIndent();
                        indexString.Append(string.Format("[index{0}]", currentRank));
                    }

                    tb.AppendLine("offset+={0}_To_Bytes(input.{1}{2},data+offset);", fieldType.XerusTypeName(TargetLanguage.Cpp), memberInfo.Name, indexString.ToString());

                    for (int currentRank = 0; currentRank < arrayRank; currentRank++)
                    {
                        tb.PopIndent();
                        tb.AppendLine("}");
                    }

                }
                else
                {
                    tb.AppendLine("offset+={0}_To_Bytes(input.{1},data+offset);", fieldType.XerusTypeName(TargetLanguage.Cpp), memberInfo.Name);

                }
            }


            tb.AppendLine("return offset;");
            tb.PopIndent();
            tb.AppendLine("}");

            return tb.ToString();
        }

        /// <summary>
        /// Generates the struct deserializer code.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>Serializer code for structs</returns>
        private string GenerateDeserializerDefinition(XerusStruct dataType, string serializerName)
        {
            string name = dataType.Name;
            var tb = new TextBuilder();
            tb.AppendLine(string.Format(
@"/** Deserializer for data type {0}
*  @param input Data type
*  @param data Buffer with the serialized data
*  @return Deserialized data type
*/", name));
            tb.AppendLine("uint16 "+serializerName +"::Bytes_To_{0}(const uint8 *const data, {0} &output)", name);
            tb.AppendLine("{");
            tb.PushIndent();

            tb.AppendLine("int offset = 0;");

            foreach (var memberInfo in dataType.GetType().GetFields())
            {
                var fieldType = memberInfo.FieldType;

                if (fieldType.IsArray)
                {
                    var arrayRank = fieldType.GetArrayRank();
                    var value = memberInfo.GetValue(dataType);
                    if (value == null)
                    {
                        throw new ModelValidationException(string.Format("The dimension of the array {0}.{1} has to be specifed. e.g. DataType[,] Member1 = new DataType[5,2]", dataType.Name, memberInfo.Name));
                    }

                    var array = value as Array;
                    var indexString = new StringBuilder();

                    for (int currentRank = 0; currentRank < arrayRank; currentRank++)
                    {
                        tb.AppendLine("for (int index{0} = 0; index{0} < {1}; index{0}++)", currentRank, array.GetLength(currentRank));
                        tb.AppendLine("{");
                        tb.PushIndent();
                        indexString.Append(string.Format("[index{0}]", currentRank));
                    }

                    tb.AppendLine("offset+=Bytes_To_{0}(data+offset, output.{1}{2});", fieldType.XerusTypeName(TargetLanguage.Cpp), memberInfo.Name, indexString.ToString());

                    for (int currentRank = 0; currentRank < arrayRank; currentRank++)
                    {
                        tb.PopIndent();
                        tb.AppendLine("}");
                    }

                }
                else
                {
                    tb.AppendLine("offset+=Bytes_To_{0}(data+offset, output.{1});", fieldType.XerusTypeName(TargetLanguage.Cpp), memberInfo.Name);
                }
            }


            tb.AppendLine("return offset;");
            tb.PopIndent();
            tb.AppendLine("}");

            return tb.ToString();
        }


        /// <summary>
        /// Creates serializer for the specified data type.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>Serializer code for base types</returns>
        private string GenerateSerializerDefinition(XerusBaseType dataType, string serializerName)
        {
            string name = dataType.GetType().XerusTypeName(TargetLanguage.Cpp);

            var tb = new TextBuilder();
            tb.AppendLine(string.Format(
@"/** Serializer for data type {0}
*  @param input Data type
*  @param data Buffer for the serialized data
*  @return Length of the serialized data in bytes
*/", name));
            tb.AppendLine("uint16 "+serializerName+"::{0}_To_Bytes(const {0} input, uint8 *const data)", name);
            tb.AppendLine("{");
            tb.PushIndent();
            var languageSpecificTypeDefinition = dataType.GetType().TypeDefinition(TargetLanguage.Cpp);
            tb.AppendLine("*(({0}*)(data)) = input;", name);
            tb.AppendLine("return {0};", languageSpecificTypeDefinition.NumberOfBytes);
            tb.PopIndent();
            tb.AppendLine("}");
            return tb.ToString();
        }


        /// <summary>
        /// Creates deserializer for the specified data type.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>Serializer code for base types</returns>
        private string GenerateDeserializerDefinition(XerusBaseType dataType, string serializerName)
        {
            string name = dataType.GetType().XerusTypeName(TargetLanguage.Cpp);

            var tb = new TextBuilder();
            tb.AppendLine(string.Format(
@"/** Deserializer for data type {0}
*  @param data Buffer with the serialized data
*  @return Deserialized data type
*/", name));
            tb.AppendLine("uint16 "+serializerName+"::Bytes_To_{0}(const uint8 *const data, {0}& output)", name);
            tb.AppendLine("{");
            tb.PushIndent();
            var languageSpecificTypeDefinition = dataType.GetType().TypeDefinition(TargetLanguage.Cpp);
            tb.AppendLine("output = ( *((const {0}*)(data)) );", name);
            tb.AppendLine("return {0};", languageSpecificTypeDefinition.NumberOfBytes);
            tb.PopIndent();
            tb.AppendLine("}");
            return tb.ToString();
        }
    }
}