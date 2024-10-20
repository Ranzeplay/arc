using Arc.Compiler.PackageGenerator.Generators;
using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using Arc.Compiler.SyntaxAnalyzer.Models;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;

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
                result.Append(GenerateFunction(result.GenerateSource(), ns, fn));
            }

            return result;
        }

        public static ArcPartialGenerationResult GenerateFunction(ArcGenerationSource source, ArcNamespaceBlock ns, ArcBlockIndependentFunction func)
        {
            var signature = $"{ns.Identifier}+{func.Declarator.Identifier.Name}@{string.Join('&', func.Declarator.Arguments.Select(a => a.DataType))}";
            var parameters = func.Declarator.Arguments.Select(a => new ArcParameterDescriptor
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
                        .First(u => u is ArcBaseType bt && bt.FullName == (func.Declarator.ReturnType.PrimitiveType ?? ArcPrimitiveDataType.Infer).GetTypeName())
                        .Id,
                AllowNone = false,
                IsArray = func.Declarator.ReturnType.IsArray,
                MemoryStorageType = func.Declarator.ReturnType.MemoryStorageType,
            };

            var descriptor = new ArcFunctionDescriptor
            {
                Name = signature,
                ReturnValueType = returnValueType,
                Parameters = parameters,
            };

            var result = new ArcPartialGenerationResult
            {
                OtherSymbols = [descriptor]
            };

            var body = GenerateFunctionBody(source, func.Body);
            result.Append(body);

            result.RelocationLabels = [
                new() { Name = descriptor.RawFullName, Type = ArcRelocationLabelType.BeginFunction, Location = 0 },
                new() { Name = descriptor.RawFullName, Type = ArcRelocationLabelType.EndFunction, Location = result.GeneratedData.Count() }
            ];

            return result;
        }

        public static ArcPartialGenerationResult GenerateFunctionBody(ArcGenerationSource source, ArcFunctionBody body)
        {
            return GenerateSequentialExecutionFlow(source, body);
        }

        public static ArcPartialGenerationResult GenerateSequentialExecutionFlow(ArcGenerationSource source, ArcBlockSequentialExecution seqExec)
        {
            var result = new ArcPartialGenerationResult();

            foreach (var step in seqExec.ExecutionSteps)
            {
                var stepResult = new ArcPartialGenerationResult();
                switch (step)
                {
                    case ArcStatementDeclaration decl:
                        {
                            stepResult = new ArcDeclarationInstruction(decl.DataDeclarator).Encode(source);
                            break;
                        }
                    case ArcStatementAssign assign:
                        {
                            var exprResult = ExpressionEvaluator.GenerateEvaluationCommand(source, assign.Expression);
                            stepResult.Append(exprResult);

                            // Pop top element to the target
                            var targetSymbol = source.LocalDataSlots.First(ds => ds.Declarator.Identifier.Name == assign.Identifier.Name);
                            stepResult.Append(new ArcPopToSlotInstruction(targetSymbol).Encode(source));
                            break;
                        }
                    case ArcBlockIf ifBlock:
                        {
                            stepResult = ConditionBlockGenerator.Encode(source, ifBlock);
                            break;
                        }
                    case ArcBlockConditionalLoop conditionalLoop:
                        {
                            stepResult = ConditionLoopBlockGenerator.Encode(source, conditionalLoop);
                            break;
                        }
                    default:
                        throw new NotImplementedException();
                }

                source.Merge(stepResult);
                result.Append(stepResult);
            }

            result.DataSlots = [];
            return result;
        }
    }
}
