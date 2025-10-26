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
        public static ArcPartialGenerationResult GenerateFunction<TSyntax, TFunctionNode, TDeclarator>(ArcGenerationSource source, ArcScopeTreeFunctionNodeBase node, ArcFunctionBase<TSyntax, TDeclarator> func, bool saveGenResult = false)
            where TSyntax : ParserRuleContext
            where TFunctionNode : ArcScopeTreeFunctionNodeBase, new()
            where TDeclarator : ArcFunctionMinimalDeclarator
        {
            var result = new ArcPartialGenerationResult();

            // Add parameters to the local data slots
            foreach (var param in node.Parameters)
            {
                source.LocalDataSlots.Add(new ArcDataSlot()
                {
                    Name = param.RawFullName,
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

            if (saveGenResult)
            {
                node.GenerationResult = result;
            }

            return result;
        }

        public static ArcPartialGenerationResult GenerateBody(ArcGenerationSource source, ArcFunctionBody body, ArcScopeTreeFunctionNodeBase fnNode)
        {
            return ArcSequentialExecutionGenerator.Generate(source, body, fnNode);
        }

        public static (T, IEnumerable<ArcCompilationLogBase>) GenerateDescriptor<T>(ArcGenerationSource source, ArcFunctionMinimalDeclarator declarator) where T : ArcScopeTreeFunctionNodeBase, new()
        {
            var logs = new List<ArcCompilationLogBase>();

            var signatureSource = source.ParentSignature;
            signatureSource.Locators.Add(declarator);
            
            var genericTypes = declarator.GenericTypes?
                .Select(g => new ArcScopeTreeGenericTypeNode { Identifier = g.Name })
                ?? [];
            source.GenericTypes = source.GenericTypes.Concat(genericTypes);
            
            var parameters = declarator.Arguments.Select(a =>
            {
                var paramDataTypeProxy = ArcDataTypeHelper.GetDataType(source, a.DataType);
                if (paramDataTypeProxy == null)
                {
                    logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, $"Data type '{a.DataType}' not found", source.Name, a.DataType.Context));
                    return new ArcParameterDescriptor();
                }

                var paramDataType = ArcDataTypeHelper.GetDataTypeNode(source, paramDataTypeProxy.ResolvedType);

                return new ArcParameterDescriptor
                {
                    DataType = new ArcDataDeclarationDescriptor
                    {
                        Type = paramDataType?.DataType ?? ArcBaseType.Placeholder(),
                        AllowNone = false,
                        Dimension = a.DataType.Dimension,
                        SyntaxTree = a.DataDeclarator,
                    },
                    RawFullName = a.Identifier.Name,
                };
            });

            var returnTypeProxy = ArcDataTypeHelper.GetDataType(source, declarator.ReturnType);
            if (returnTypeProxy == null)
            {
                logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, $"Data type '{declarator.ReturnType}' not found", source.Name, declarator.ReturnType.Context));
                return (default!, logs);
            }

            var returnType = ArcDataTypeHelper.GetDataTypeNode(source, returnTypeProxy.ResolvedType)?.DataType;
            var returnValueType = new ArcDataDeclarationDescriptor
            {
                Type = returnType ?? ArcBaseType.Placeholder(),
                AllowNone = false,
                Dimension = declarator.ReturnType.Dimension,
            };
            
            var descriptor = new T
            {
                ReturnValueType = returnValueType,
                Parameters = parameters,
            };
            
            if (declarator is ArcNamedFunctionDeclarator d)
            {
                var annotations = d.Annotations
                    .ToDictionary(
                        a => ArcAnnotationHelper.FindAnnotationNode(source, a),
                        a => a.CallArguments.Select(ca => ca.Expression)
                    );
                
                descriptor.Annotations = annotations;
            }
            
            descriptor.AddChildren(genericTypes);

            return (descriptor, logs);
        }
    }
}
