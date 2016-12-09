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
using System.IO;
using Xerus.Interfaces;

namespace Xerus.Generator.TE.DataLayer
{
    public class DataTypesGenerator : IGenerator
    {
        /// <summary>
        /// Gets the target language for which the generator produces code
        /// </summary>
        /// <value>
        /// The target language.
        /// </value>
        public TargetLanguage TargetLanguage
        {
            get
            {
                return TargetLanguage.Cpp;
            }
        }

        /// <summary>
        /// Generates the code for the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        public void Generate(IXerusModel model, string baseDirectory)
        {
            //GenerateSourcePart data types
            using (var streamWriter = File.CreateText(baseDirectory + "\\DataTypes.h"))
            {
                var dataTypesHeader = new DataTypesHeaderGenerator();
                streamWriter.Write(dataTypesHeader.Generate(model));
            }

        }
    }
}