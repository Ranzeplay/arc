using Antlr4.Runtime;
using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal static class ArcFunctionGenerator
    {
        public static ArcPartialGenerationResult Generate<T>(ArcGenerationSource source, ArcFunctionBase<T> func, bool withDeclarationDescriptor) where T : ParserRuleContext
        {
            var descriptor = GenerateDescriptor(source, func.Declarator);

            var result = new ArcPartialGenerationResult();

            if (withDeclarationDescriptor)
            {
                result.OtherSymbols.Add(descriptor);
            }

            // Add parameters to the local data slots
            foreach (var param in descriptor.Parameters)
            {
                source.LocalDataSlots.Add(new ArcDataSlot()
                {
                    Name = "Function param slot",
                    Declarator = param.Declarator,
                    SlotId = source.LocalDataSlots.Count
                });
            }

            var beginBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.BeginFunction, descriptor.RawFullName).Encode(source);
            var body = GenerateBody(source, func.Body);
            var endBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.EndFunction, descriptor.RawFullName).Encode(source);

            result.Append(beginBlockLabel);
            result.Append(body);
            result.Append(endBlockLabel);

            source.LocalDataSlots = [];

            return result;
        }

        public static ArcPartialGenerationResult GenerateBody(ArcGenerationSource source, ArcFunctionBody body)
        {
            return ArcSequentialExecutionGenerator.Generate(source, body);
        }

        public static ArcFunctionDescriptor GenerateDescriptor(ArcGenerationSource source, ArcFunctionDeclarator declarator)
        {
            var signatureSource = source.ParentSignature;
            signatureSource.Locators.Add(declarator);

            var parameters = declarator.Arguments.Select(a => new ArcParameterDescriptor
            {
                DataType = new ArcDataDeclarationDescriptor
                {
                    Type = source.GlobalScopeTree.Symbols
                        .OfType<ArcTypeBase>()
                        .First(bt => bt.FullName == a.DataType.TypeName || bt.Identifier == a.DataType.TypeName),
                    AllowNone = false,
                    IsArray = a.DataType.IsArray,
                    MemoryStorageType = a.DataType.MemoryStorageType,
                },
                RawFullName = a.Identifier.Name,
                Declarator = a.DataDeclarator,
            });
            var returnValueType = new ArcDataDeclarationDescriptor
            {
                Type = source.GlobalScopeTree.Symbols
                        .OfType<ArcTypeBase>()
                        .First(bt => bt.FullName == declarator.ReturnType.TypeName || bt.Identifier == declarator.ReturnType.TypeName),
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
