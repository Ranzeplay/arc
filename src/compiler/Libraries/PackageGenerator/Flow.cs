using Arc.Compiler.PackageGenerator.Models;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Primitives;
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
                result.Append(GenerateFunction(result.GenerateSource<(ArcNamespaceBlock, ArcBlockIndependentFunction)>((ns, fn))));
            }

            return result;
        }

        public static ArcGenerationResult GenerateFunction(ArcGenerationSource<(ArcNamespaceBlock, ArcBlockIndependentFunction)> source)
        {
            var ns = source.Value.Item1;
            var func = source.Value.Item2;

            var signature = $"{ns.Identifier}+{func.Declarator.Identifier.Name}@{string.Join('&', func.Declarator.Arguments.Select(a => a.DataType))}";
            var parameters = func.Declarator.Arguments.Select(a => new ArcParameterDescriptor
            {
                DataType = new ArcDataTypeDescriptor
                {
                    TypeId = source.Symbols.First(u => u.Value is ArcBaseType bt && bt.FullName == (a.DataType.PrimitiveType ?? ArcPrimitiveDataType.Infer).GetTypeName()).Key,
                    AllowNone = false,
                    IsArray = a.DataType.IsArray,
                    MemoryStorageType = a.DataType.MemoryStorageType,
                },
                RawFullName = a.Identifier.Name,
            });
            var returnValueType = new ArcDataTypeDescriptor
            {
                TypeId = source.Symbols
                        .First(u => u.Value is ArcBaseType bt && bt.FullName == (func.Declarator.ReturnType.PrimitiveType ?? ArcPrimitiveDataType.Infer).GetTypeName())
                        .Key,
                AllowNone = false,
                IsArray = func.Declarator.ReturnType.IsArray,
                MemoryStorageType = func.Declarator.ReturnType.MemoryStorageType,
            };

            var descriptor = new ArcFunctionDescriptor
            {
                Id = new Random().Next(),
                RawFullName = signature,
                ReturnValueType = returnValueType,
                Parameters = parameters,
            };

            var result = new ArcGenerationResult
            {
                Symbols = new()
                {
                    { descriptor.Id, descriptor },
                },
            };

            var body = GenerateFunctionBody(source.Migrate(func.Body));
            result.Append(body);

            result.Labels = [
                new() { Id = new Random().Next(), Name = descriptor.RawFullName, Type = ArcLabelType.BeginFunction, Position = 0 },
                new() { Id = new Random().Next(), Name = descriptor.RawFullName, Type = ArcLabelType.EndFunction, Position = result.GeneratedData.Count() }
            ];

            return result;
        }

        public static ArcGenerationResult GenerateFunctionBody(ArcGenerationSource<ArcFunctionBody> source)
        {
            return GenerateSequentialExecutionFlow(source.Migrate((ArcBlockSequentialExecution) source.Value));
        }

        public static ArcGenerationResult GenerateSequentialExecutionFlow(ArcGenerationSource<ArcBlockSequentialExecution> source)
        {
            var result = new ArcGenerationResult();

            foreach (var step in source.Value.ExecutionSteps)
            {
                var stepResult = new ArcGenerationResult();
                switch (step)
                {
                    case ArcStatementDeclaration decl:
                        {
                            stepResult = new DeclarationInstruction(decl.DataDeclarator).Encode(source);
                            break;
                        }
                    case ArcStatementAssign assign:
                        {
                            var exprResult = ExpressionEvaluator.GenerateEvaluationCommand(source.Migrate(assign.Expression));
                            stepResult.Append(exprResult);

                            // Pop top element to the target
                            var targetSymbol = source.Symbols.First(x => x.Value is ArcDataSlot ds && ds.Declarator.Identifier.Name == assign.Identifier.Name).Value as ArcDataSlot;
                            stepResult.Append(new PopToSlotInstruction(targetSymbol).Encode(source));
                            break;
                        }
                    case ArcBlockIf ifBlock:
                        {
                            stepResult = ConditionBlockGenerator.Encode(source.Migrate(ifBlock));
                            break;
                        }
                    case ArcBlockConditionalLoop conditionalLoop:
                        {
                            stepResult = ConditionLoopBlockGenerator.Encode(source.Migrate(conditionalLoop));
                            break;
                        }
                    default:
                        throw new NotImplementedException();
                }

                source.Merge(stepResult);
                result.Append(stepResult);
            }

            result.ClearDataSlots();
            return result;
        }
    }
}
