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
namespace Xerus
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using Autofac;
    using Interfaces;
    using Model;
    using Model.Attributes;
    using Model.DataTypes;

    using SerializerGenerator = Xerus.Generator.TE.DataLayer.SerializerGenerator;

    /// <summary>
    ///  Xerus Main
    /// </summary>
    class Program
    {
        private enum ErrorCodes
        {
            Success,
            MissingArguments,
            FileDoesNotExists,
            FailedToLoadModel,
            ValidationFailed,
            GenerationFailed
        }

        /// <summary>
        /// Entry point for the program
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static int Main(string[] args)
        {


            Console.WriteLine("Xerus Code Generator v{0}", Assembly.GetExecutingAssembly().GetName().Version);
            Console.WriteLine();
            try
            {
                if (args.Length < 1)
                {
                    Console.WriteLine("Missing arguments.");
                    Console.WriteLine("You have to specify the assembly which contains the model e.g. Xerus Xerus.Example.dll");
                    return (int)ErrorCodes.MissingArguments;
                }
                var assemblyName = args[0];
                if (!File.Exists(assemblyName))
                {
                    Console.WriteLine("Specified model assembly \"{0}\" does not exists.", assemblyName);
                    return (int)ErrorCodes.FileDoesNotExists;
                }

                var directoryName = "";
                if (args.Length > 1)
                {
                    directoryName = args[1];
                    if (!Directory.Exists(directoryName))
                    {
                        Directory.CreateDirectory(directoryName);
                    }
                }

                Assembly modelAssembly = null;
                try
                {
                    modelAssembly = Assembly.LoadFrom(assemblyName);
                }
                catch (Exception)
                {

                    return (int)ErrorCodes.FailedToLoadModel;
                }

                var model = CreateModelOutOfAssembly(modelAssembly);
                var validationErrors = string.Empty;

                Console.Write("Validate model ... ");
                if (model.Validate(ref validationErrors))
                {
                    if (!string.IsNullOrEmpty(validationErrors))
                    {
                        Console.WriteLine();
                        Console.WriteLine(validationErrors);
                    }
                    else
                    {
                        Console.WriteLine("Done");
                    }

                    Console.Write("Create Data Types ... ");
                    

                    Console.Write("Create Serializer ... ");
                    var serializerGenerator = new SerializerGenerator();
                    serializerGenerator.Generate(model, directoryName, "VehicleInterfaceSerializer");
                    Console.WriteLine("Done");


                }
                else
                {
                    Console.WriteLine("Failed");
                    Console.WriteLine(validationErrors);
                    return (int)ErrorCodes.ValidationFailed;
                }

                return (int)ErrorCodes.Success;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Generation Failed: {0}", ex.Message);
                return (int) ErrorCodes.GenerationFailed;
            }
        }




        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <returns>The model</returns>
        private static IXerusModel CreateModelOutOfAssembly(Assembly assembly)
        {
            IXerusModel model = new XerusModel();

            var builder = new ContainerBuilder();
            builder.RegisterAssemblyTypes(assembly)
                .Where(t => typeof(IModelDefinition).IsAssignableFrom(t)).AsSelf().As<IModelDefinition>().SingleInstance();
            var container = builder.Build();


            foreach (var type in assembly.DefinedTypes)
            {
                if (type.IsEnum)
                {
                    foreach (var attribute in type.CustomAttributes)
                    {
                        if (attribute.AttributeType == typeof(XerusEnumAttribute))
                        {
                            model.AddDataType(XerusEnumType.CreateFromEnum(type));
                        }
                    }
                }
                if (typeof(IXerusDataType).IsAssignableFrom(type))
                {
                    var instance = Activator.CreateInstance(type) as IXerusDataType;
                    model.AddDataType(instance);
                }
            }

            var modelDefinitions = container.Resolve<IEnumerable<IModelDefinition>>();
            foreach (var modelDefinition in modelDefinitions)
            {
                modelDefinition.Define(model);
            }


            return model;
        }

    }
}
