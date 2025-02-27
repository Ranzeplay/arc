using Antlr4.Runtime;
using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal static class ArcFunctionGenerator
    {
        public static ArcPartialGenerationResult Generate<TSyntax, TFunctionNode>(ArcGenerationSource source, ArcScopeTreeFunctionNodeBase node, ArcFunctionBase<TSyntax> func)
            where TSyntax : ParserRuleContext
            where TFunctionNode : ArcScopeTreeFunctionNodeBase, new()
        {
            var result = new ArcPartialGenerationResult();

            // Add parameters to the local data slots
            foreach (var param in node.Parameters)
            {
                source.LocalDataSlots.Add(new ArcDataSlot()
                {
                    Name = "Function param slot",
                    DeclarationDescriptor = param.DataType,
                    SlotId = source.LocalDataSlots.Count
                });
            }

            var beginBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.BeginFunction, node.Signature).Encode(source);
            var body = GenerateBody(source, func.Body);
            var endBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.EndFunction, node.Signature).Encode(source);

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

        public static T GenerateDescriptor<T>(ArcGenerationSource source, ArcFunctionDeclarator declarator) where T : ArcScopeTreeFunctionNodeBase, new()
        {
            var signatureSource = source.ParentSignature;
            signatureSource.Locators.Add(declarator);

            var parameters = declarator.Arguments.Select(a => new ArcParameterDescriptor
            {
                DataType = new ArcDataDeclarationDescriptor
                {
                    Type = ArcDataTypeHelper.GetDataTypeNode(source, a.DataType).DataType,
                    AllowNone = false,
                    Dimension = a.DataType.Dimension,
                    MemoryStorageType = a.DataType.MemoryStorageType,
                    SyntaxTree = a.DataDeclarator,
                },
                RawFullName = a.Identifier.Name,
            });
            var returnValueType = new ArcDataDeclarationDescriptor
            {
                Type = ArcDataTypeHelper.GetDataTypeNode(source, declarator.ReturnType).DataType,
                AllowNone = false,
                Dimension = declarator.ReturnType.Dimension,
                MemoryStorageType = declarator.ReturnType.MemoryStorageType,
            };

            var annotations = declarator.Annotations
                .ToDictionary(
                    a => ArcAnnotationHelper.FindAnnotationNode(source, a),
                    a => a.CallArguments.Select(ca => ca.Expression)
                );

            return new()
            {
                ReturnValueType = returnValueType,
                Parameters = parameters,
                Annotations = annotations,
            };
        }
    }
}
