using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal static class FunctionGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcBlockIndependentFunction func, ArcNamespaceBlock ns)
        {
            var descriptor = GenerateDescripto(source, func.Declarator);

            var result = new ArcPartialGenerationResult
            {
                OtherSymbols = [descriptor]
            };

            var body = GenerateBody(source, func.Body);
            result.Append(body);

            result.RelocationLabels = [
                new() { Name = descriptor.RawFullName, Type = ArcRelocationLabelType.BeginFunction, Location = 0 },
                new() { Name = descriptor.RawFullName, Type = ArcRelocationLabelType.EndFunction, Location = result.GeneratedData.Count() }
            ];

            return result;
        }

        public static ArcPartialGenerationResult GenerateBody(ArcGenerationSource source, ArcFunctionBody body)
        {
            return SequentialExecutionGenerator.Generate(source, body);
        }

        public static ArcFunctionDescriptor GenerateDescripto(ArcGenerationSource source, ArcFunctionDeclarator declarator)
        {
            var signatureSource = source.ParentSignature;
            signatureSource.Locators = signatureSource.Locators.Append(declarator);

            var parameters = declarator.Arguments.Select(a => new ArcParameterDescriptor
            {
                DataType = new ArcDataDeclarationDescriptor
                {
                    TypeId = source.AccessibleSymbols
                        .First(u => u is ArcBaseType bt && bt.FullName == (a.DataType.PrimitiveType ?? ArcPrimitiveDataType.Infer).GetTypeName())
                        .Id,
                    AllowNone = false,
                    IsArray = a.DataType.IsArray,
                    MemoryStorageType = a.DataType.MemoryStorageType,
                },
                RawFullName = a.Identifier.Name,
            });
            var returnValueType = new ArcDataDeclarationDescriptor
            {
                TypeId = source.AccessibleSymbols
                        .First(u => u is ArcBaseType bt && bt.FullName == (declarator.ReturnType.PrimitiveType ?? ArcPrimitiveDataType.Infer).GetTypeName())
                        .Id,
                AllowNone = false,
                IsArray = declarator.ReturnType.IsArray,
                MemoryStorageType = declarator.ReturnType.MemoryStorageType,
            };

            return new ArcFunctionDescriptor
            {
                Name = signatureSource.GetSignature(),
                ReturnValueType = returnValueType,
                Parameters = parameters,
            };
        }
    }
}
