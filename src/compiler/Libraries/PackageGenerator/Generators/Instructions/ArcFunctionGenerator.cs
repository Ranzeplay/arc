using Antlr4.Runtime;
using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Microsoft.Extensions.Logging;

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
                    SlotId = (ulong)source.LocalDataSlots.Count
                });
            }

            var relocationLayer = Guid.NewGuid();

            var beginBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.BeginFunction, node.Signature, relocationLayer).Encode(source);
            var body = GenerateBody(source, func.Body, node);
            var endBlockLabel = new ArcLabellingInstruction(ArcRelocationLabelType.EndFunction, node.Signature, relocationLayer).Encode(source);

            result.Append(beginBlockLabel);
            result.Append(body);
            result.Append(endBlockLabel);

            source.LocalDataSlots = [];
            node.DataCount = result.TotalGeneratedDataSlotCount + node.Parameters.Count();

            return result;
        }

        public static ArcPartialGenerationResult GenerateBody(ArcGenerationSource source, ArcFunctionBody body, ArcScopeTreeFunctionNodeBase fnNode)
        {
            return ArcSequentialExecutionGenerator.Generate(source, body, fnNode);
        }

        public static (T, IEnumerable<ArcCompilationLogBase>) GenerateDescriptor<T>(ArcGenerationSource source, ArcFunctionDeclarator declarator) where T : ArcScopeTreeFunctionNodeBase, new()
        {
            var logs = new List<ArcCompilationLogBase>();

            var signatureSource = source.ParentSignature;
            signatureSource.Locators.Add(declarator);

            var parameters = declarator.Arguments.Select(a =>
            {
                var paramDataType = ArcDataTypeHelper.GetDataTypeNode(source, a.DataType);
                if (paramDataType == null)
                {
                    logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, $"Data type '{a.DataType}' not found", source.Name, a.DataType.Context));
                }

                return new ArcParameterDescriptor
                {
                    DataType = new ArcDataDeclarationDescriptor
                    {
                        Type = paramDataType?.DataType ?? ArcBaseType.Placeholder(),
                        AllowNone = false,
                        Dimension = a.DataType.Dimension,
                        MemoryStorageType = a.DataType.MemoryStorageType,
                        SyntaxTree = a.DataDeclarator,
                    },
                    RawFullName = a.Identifier.Name,
                };
            });

            var returnDataType = ArcDataTypeHelper.GetDataTypeNode(source, declarator.ReturnType);
            if (returnDataType == null)
            {
                logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, $"Data type '{declarator.ReturnType}' not found", source.Name, declarator.ReturnType.Context));
            }

            var returnValueType = new ArcDataDeclarationDescriptor
            {
                Type = ArcDataTypeHelper.GetDataTypeNode(source, declarator.ReturnType)?.DataType ?? ArcBaseType.Placeholder(),
                AllowNone = false,
                Dimension = declarator.ReturnType.Dimension,
                MemoryStorageType = declarator.ReturnType.MemoryStorageType,
            };

            var annotations = declarator.Annotations
                .ToDictionary(
                    a => ArcAnnotationHelper.FindAnnotationNode(source, a),
                    a => a.CallArguments.Select(ca => ca.Expression)
                );

            return (
                new()
                {
                    ReturnValueType = returnValueType,
                    Parameters = parameters,
                    Annotations = annotations,
                },
                logs
            );
        }
    }
}
