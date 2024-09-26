using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.PackageGenerator
{
    internal class Flow
    {
        public static ArcGeneratorContext GenerateUnit(ArcCompilationUnit compilationUnit)
        {
            var ns = compilationUnit.Namespace;
            var result = new ArcGeneratorContext();
            result.LoadPrimitiveTypes();
            foreach (var fn in compilationUnit.Namespace.Functions)
            {
                result.Append(GenerateFunction(result.GenerateSource<(ArcNamespaceBlock, ArcBlockIndependentFunction)>((ns, fn))));
            }

            return result;
        }

        public static ArcGenerationResult GenerateFunction(ArcGenerationSource<(ArcNamespaceBlock, ArcBlockIndependentFunction)> source)
        {
            var ns = source.Value.Item1;
            var func = source.Value.Item2;

            var signature = $"{ns.Identifier}+{func.Declarator.Identifier.Name}@{string.Join('&', func.Declarator.Arguments.Select(a => a.DataType))}";
            var descriptor = new ArcFunctionDescriptor
            {
                Id = new Random().Next(),
                RawFullName = signature,
                ReturnValueType = new ArcDataTypeDescriptor
                {
                    TypeId = source.Symbols
                        .First(u =>
                            {
                                if (u.Value is ArcBaseType bt)
                                {
                                    return bt.FullName == ArcPrimitiveDataTypeUtils.GetTypeName(func.Declarator.ReturnType.PrimitiveType ?? ArcPrimitiveDataType.Infer);
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        )
                        .Key,
                    AllowNone = false,
                    IsArray = func.Declarator.ReturnType.IsArray,
                    MemoryStorageType = func.Declarator.ReturnType.MemoryStorageType,
                }
            };

            var body = GenerateFunctionBody(func.Declarator.Arguments, func.Body);

            var result = new ArcGenerationResult
            {
                Symbols = new()
                {
                    { descriptor.Id, descriptor },
                },
            }.Append(body);

            result.Labels = [
                new() { Id = new Random().Next(), Name = descriptor.RawFullName, Type = ArcLabelType.BeginFunction, Position = 0 },
                new() { Id = new Random().Next(), Name = descriptor.RawFullName, Type = ArcLabelType.EndFunction, Position = result.GeneratedData.Count() }
            ];

            return result;
        }

        public static ArcGenerationResult GenerateFunctionBody(IEnumerable<ArcFunctionArgument> arguments, ArcFunctionBody body)
        {
            var result = new ArcGenerationResult();
            return result;
        }
    }
}
