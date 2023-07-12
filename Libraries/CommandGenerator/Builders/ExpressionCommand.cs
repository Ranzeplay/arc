using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.CommandGeneration.Mappings;
using Arc.Compiler.Shared.CommandGeneration.Relocation;
using Arc.Compiler.Shared.Parsing.Components;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Expression;
using Arc.Compiler.Shared.Parsing.Components.Function;
using Arc.CompilerCommandGenerator.Builders.Fragments;
using Arc.CompilerCommandGenerator.Extensions;
using Arc.CompilerCommandGenerator.Models;
using System.Text;

namespace Arc.CompilerCommandGenerator.Builders
{
    internal class ExpressionCommand
    {
        public static PartialGenerationResult Build(GenerationContext<SimpleExpression> source)
        {
            var result = new PartialGenerationResult();

            var postfixExpression = source.Component.ToPostfixExpression();

            foreach (var term in postfixExpression.Terms)
            {
                switch (term.TermType)
                {
                    case ExpressionTermType.Operator:
                        {
                            result.Commands.AddRange(MathCommand.FromOperator(term.GetOperator()!)!);
                            break;
                        }
                    case ExpressionTermType.Data:
                        {
                            var partialResult = BuildExpressionDataTerm(new GenerationContext<ExpressionDataTerm>(term.GetDataTerm()!,
                                source.LocalData,
                                source.GlobalData,
                                source.AvailableFunctions,
                                source.RelocationReferences,
                                source.PackageMetadata,
                                result.GeneratedConstants.Count));

                            if (partialResult != null)
                            {
                                result.Commands.AddRange(partialResult.Commands);
                                result.Combine(partialResult);
                            }

                            break;
                        }
                }
            }

            return result;
        }

        private static PartialGenerationResult? BuildExpressionDataTerm(GenerationContext<ExpressionDataTerm> source)
        {
            switch (source.Component.DataTermType)
            {
                case ExpressionDataTermType.Number:
                    return BuildNumberCommand(source.TransferToNewComponent(source.Component.GetNumber()!));
                case ExpressionDataTermType.String:
                    return BuildStringCommand(source.TransferToNewComponent(source.Component.GetString()!));
                case ExpressionDataTermType.DataAccessor:
                    var accessorGenerationSource = source.TransferToNewComponent(source.Component.GetDataAccessor()!);
                    var accessorSource = new DataAccessorSource(accessorGenerationSource);
                    return BuildDataAccessorCommand(source.TransferToNewComponent(accessorSource));
                case ExpressionDataTermType.FunctionCall:
                    return BuildFunctionCallCommand(source.TransferToNewComponent(source.Component.GetFunctionCall()!));
                default:
                    break;
            }

            return null;
        }

        private static PartialGenerationResult BuildNumberCommand(GenerationContext<string> source)
        {
            // Get number
            var numberObj = new NumberObject(source.Component);

            var commands = Utils.CombineLeadingCommand((byte)RootCommand.Stack, (byte)StackCommand.PushFromConstant).ToList();

            var constant = new GeneratedConstant(
                new DataType(
                    new Identifier(
                        Array.Empty<string>(),
                        "number"),
                    false),
                numberObj.ToPackageEncoding(source.PackageMetadata));

            var relocationDescriptor = RelocationTarget.NewConstant(0, commands.Count, 0);

            commands.AddRange(source.PackageMetadata.GenerateEmptyDataSlot());

            return new PartialGenerationResult(
                commands,
                null,
                new GeneratedConstant[1] { constant },
                new RelocationTarget[1] { relocationDescriptor });
        }

        private static PartialGenerationResult BuildStringCommand(GenerationContext<string> source)
        {
            // Generate string data
            var stringBytes = Encoding.UTF8.GetBytes(source.Component);
            var encodedString = source.PackageMetadata.BuildDataBlock(stringBytes);

            // Generate string length
            var stringLen = stringBytes.Length;
            var encodedLen = source.PackageMetadata.BuildDataBlock(BitConverter.GetBytes(stringLen));

            var constantData = encodedLen.Concat(encodedString);

            var constant = new GeneratedConstant(new DataType(
                new Identifier(Array.Empty<string>(), "string"),
                false),
                                                       constantData.ToArray());

            var commands = Utils.CombineLeadingCommand((byte)RootCommand.Stack, (byte)StackCommand.PushFromConstant).ToList();

            var relocationDescriptor = RelocationTarget.NewConstant(0, commands.Count, 0);

            commands.AddRange(source.PackageMetadata.GenerateEmptyDataSlot());

            return new PartialGenerationResult(
                commands,
                null,
                new GeneratedConstant[1] { constant },
                new RelocationTarget[1] { relocationDescriptor });
        }

        private static PartialGenerationResult BuildDataAccessorCommand(GenerationContext<DataAccessorSource> source)
        {
            var commands = Utils.CombineLeadingCommand((byte)RootCommand.Stack, (byte)StackCommand.PushFromObject).ToList();

            // Evaluate index expression first and put it to the top of the stack
            commands.AddRange(DataAccessorCommand.Build(source.TransferToNewComponent(source.Component.DataAccessor))!.Commands);

            // TODO: Check whether the the object is singleton or array element

            return new PartialGenerationResult(commands);
        }

        private static PartialGenerationResult? BuildFunctionCallCommand(GenerationContext<FunctionCallBase> source)
        {
            var commands = new List<byte>();

            var targetFunction = source.AvailableFunctions.FirstOrDefault(f => f.Identifier.Equals(source.Component.TargetFunctionIdentifier)) ?? throw new InvalidDataException("Target function not found!");
            var functionId = source.AvailableFunctions.IndexOf(targetFunction);

            // Evaluate each parameter
            // TODO: Add checks
            // The first argument goes to the top of the stack
            foreach (var callArg in source.Component.Arguments.Reverse())
            {
                var expr = Build(source.TransferToNewComponent(callArg.EvaluateExpression));
                if (expr != null)
                {
                    commands.AddRange(expr.Commands);
                }
                else
                {
                    throw new InvalidDataException("Encountered an expression that is not evaluable");
                }
            }

            // Push leading command
            commands.AddRange(Utils.CombineLeadingCommand((byte)RootCommand.Function, (byte)FunctionCommand.Enter));

            // Add function id
            var slot = source.PackageMetadata.GenerateFunctionIdData(functionId);
            commands.AddRange(slot);

            return new(commands);
        }
    }
}
