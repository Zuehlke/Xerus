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
    using System.Collections.Generic;
    using Interfaces;
    using Model;
    using Model.DataTypes;
    using TemplateEngine;

    /// <summary>
    /// Generator for the serializer header file
    /// </summary>
    public class SerializerHeaderGenerator : Template
    {

        /// <summary>
        /// Generates the serializers for the specified data Types.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="dataTypes">The data types.</param>
        /// <returns>The source code as string</returns>
        public string Generate(IXerusModel model, List<IXerusDataType> dataTypes, string name)
        {

            this.AppendLine(@"/******************************************************");
            this.AppendLine(@" *  "+ name +".h");
            this.AppendLine(@" *-----------------------------------------------------");
            this.AppendLine(@" * ");
            this.AppendLine(@" * ");
            this.AppendLine(@" *------------------------------------------------------");
            this.AppendLine(@" *      Serializers for the used data types");
            this.AppendLine(@" *-------------------------------------------------------");
            this.AppendLine(@" *");
            this.AppendLine(@" *******************************************************/");
            this.AppendLine();
            this.AppendLine("#include <DataTypes.h>");
            this.AppendLine();
            this.AppendLine("#ifndef __"+name+"_h__");
            this.AppendLine("#define __" + name + "_h__");
            this.AppendLine();
            this.AppendLine("class " + name);
            this.AppendLine("{");
            this.PushIndent();
            this.AppendLine("public:");
            foreach (var xerusDataType in dataTypes)
            {
                this.AppendLine(GenerateSerializerSignature(xerusDataType as dynamic));
                this.AppendLine(GenerateDeserializerSignature(xerusDataType as dynamic));
            }
            this.PopIndent();
            this.AppendLine("};");
            this.AppendLine("#endif");
            return this.ToString();

        }

        /// <summary>
        /// Generates the deserializer code for an enum.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>Derializer code for enums</returns>
        private string GenerateDeserializerSignature(XerusEnumType dataType)
        {
            string name = dataType.Name;

            var tb = new TextBuilder();
            tb.AppendLine(string.Format(@"/** Deserializer for data type {0}
*  @param data Buffer with the serialized data
*  @param output Data type
*  @return  Length of the data in bytes
*/", name));
            tb.AppendLine("static uint16 Bytes_To_{0}(const uint8 *const data, {0}& output );", name);
            return tb.ToString();
        }

        /// <summary>
        /// Generates the Enumeration serializer code
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>Serializer code for enums</returns>
        private string GenerateSerializerSignature(XerusEnumType dataType)
        {
            string name = dataType.Name;
            var tb = new TextBuilder();
            tb.AppendLine(string.Format(@"/** Serializer for data type {0}
*  @param input Data type
*  @param data Buffer for the serialized data
*  @return Length of the serialized data in bytes
*/", name));
            tb.AppendLine("static uint16 {0}_To_Bytes(const {0} input, uint8 *const data);", name);
            return tb.ToString();
        }

        /// <summary>
        /// Generates the struct serializer code.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>Serializer code for structs</returns>
        private string GenerateSerializerSignature(XerusStruct dataType)
        {
            string name = dataType.Name;
            var tb = new TextBuilder();
            tb.AppendLine(string.Format(@"/** Serializer for data type {0}
*  @param input Data type
*  @param data Buffer for the serialized data
*  @return Length of the serialized data in bytes
*/", name));
            tb.AppendLine("static uint16 {0}_To_Bytes(const {0} input, uint8 *const data);", name);
            return tb.ToString();
        }

        /// <summary>
        /// Generates the struct deserializer code.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>Serializer code for structs</returns>
        private string GenerateDeserializerSignature(XerusStruct dataType)
        {
            string name = dataType.Name;
            var tb = new TextBuilder();
            tb.AppendLine(string.Format(@"/** Deserializer for data type {0}
*  @param data Buffer with the serialized data
*  @param output Data type
*  @return  Length of the data in bytes
*/", name));
            tb.AppendLine("static uint16 Bytes_To_{0}(const uint8 *const data, {0} &output);", name);
            return tb.ToString();
        }

        /// <summary>
        /// Creates serializer for the specified data type.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns></returns>

        private string GenerateSerializerSignature(XerusBaseType dataType)
        {
            string name = dataType.GetType().XerusTypeName(TargetLanguage.Cpp);

            var tb = new TextBuilder();
            tb.AppendLine(string.Format(@"/** Serializer for data type {0}
*  @param input Data type
*  @param data Buffer for the serialized data
*  @return Length of the serialized data in bytes
*/", name));
            tb.AppendLine("static uint16 {0}_To_Bytes(const {0} input, uint8 *const data);", name);
            return tb.ToString();
        }


        /// <summary>
        /// Creates deserializer for the specified data type.
        /// </summary>
        /// <param name="dataType">Type of the data.</param>
        /// <returns>Serializer code for base types</returns>
        private string GenerateDeserializerSignature(XerusBaseType dataType)
        {
            string name = dataType.GetType().XerusTypeName(TargetLanguage.Cpp);

            var tb = new TextBuilder();
            tb.AppendLine(string.Format(@"/** Deserializer for data type {0}
*  @param data Buffer with the serialized data
*  @param output Data type
*  @return  Length of the data in bytes
*/", name));
            tb.AppendLine("static uint16 Bytes_To_{0}(const uint8 *const data, {0}& output);", name);
            return tb.ToString();
        }
    }
}