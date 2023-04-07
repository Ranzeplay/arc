using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Expression;
using Arc.CompilerCommandGenerator.Extensions;
using Arc.CompilerCommandGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.CompilerCommandGenerator.Builders
{
    internal class ExpressionCommand
    {
        public static PartialGenerationResult? BuildSimpleExpression(GenerationSource<SimpleExpression> source)
        {
            var result = new PartialGenerationResult();

            var commands = new List<byte>();

            var postfixExpression = Utils.ExpressionInfixToPostfix(source.Component);

            foreach (var term in postfixExpression.Terms)
            {
                switch (term.TermType)
                {
                    case ExpressionTermType.Operator:
                        {
                            commands.AddRange(MathCommand.FromOperator(term.GetOperator()!)!);
                            break;
                        }
                    case ExpressionTermType.Data:
                        {
                            var partialResult = BuildExpressionDataTerm(new GenerationSource<ExpressionDataTerm>(term.GetDataTerm()!, source.LocalData, source.GlobalData, source.PackageMetadata, result.GeneratedConstants.Count()));

                            if (partialResult != null)
                            {
                                commands.AddRange(partialResult.Commands);
                                result.GeneratedConstants = Enumerable.Concat(result.GeneratedConstants, partialResult.GeneratedConstants);
                            }

                            break;
                        }
                }
            }

            result.Commands = commands.ToArray();
            return result;
        }

        private static PartialGenerationResult? BuildExpressionDataTerm(GenerationSource<ExpressionDataTerm> source)
        {
            switch (source.Component.DataTermType)
            {
                case ExpressionDataTermType.Number:
                    return BuildNumberCommand(GenerationSource<string>.MigrateGenerationSource(source.Component.GetNumber()!, source));
                case ExpressionDataTermType.String:
                    return BuildStringCommand(GenerationSource<string>.MigrateGenerationSource(source.Component.GetString()!, source));
                case ExpressionDataTermType.DataAccessor:
                    return BuildDataAccessorCommand(GenerationSource<DataAccessorSource>.MigrateGenerationSource<DataAccessorSource, ExpressionDataTerm>(new(GenerationSource<DataAccessor>.MigrateGenerationSource(source.Component.GetDataAccessor()!, source)), source));
                case ExpressionDataTermType.FunctionCall:
                    throw new NotImplementedException();
                default:
                    break;
            }

            return null;
        }

        private static PartialGenerationResult BuildNumberCommand(GenerationSource<string> source)
        {
            // Get number
            var numberObj = new NumberObject(source.Component);

            var commands = Utils.CombineLeadingCommand((byte)RootCommand.Stack, (byte)StackCommand.PushFromConstant).ToList();
            var slot = Utils.GenerateSlotData(source.ConstantBeginIndex, source.PackageMetadata);
            commands.AddRange(slot);

            return new PartialGenerationResult(
                commands.ToArray(),
                null,
                new GeneratedConstant[1]
                {
                    new GeneratedConstant(source.ConstantBeginIndex,
                                          new DataType(new Identifier(Array.Empty<string>(), "number"), false),
                                                       numberObj.ToPackageEncoding(source.PackageMetadata))
                });
        }

        private static PartialGenerationResult BuildStringCommand(GenerationSource<string> source)
        {
            // Generate string data
            var stringBytes = Encoding.UTF8.GetBytes(source.Component);
            var encodedString = Utils.BuildDataBlock(stringBytes, source.PackageMetadata);

            // Generate string length
            var stringLen = stringBytes.Length;
            var encodedLen = Utils.BuildDataBlock(BitConverter.GetBytes(stringLen), source.PackageMetadata);

            var constantData = encodedLen.Concat(encodedString);

            var commands = Utils.CombineLeadingCommand((byte)RootCommand.Stack, (byte)StackCommand.PushFromConstant).ToList();
            var slot = Utils.GenerateSlotData(source.ConstantBeginIndex, source.PackageMetadata);
            commands.AddRange(slot);

            return new PartialGenerationResult(
                commands.ToArray(),
                null,
                new GeneratedConstant[1]
                {
                    new GeneratedConstant(source.ConstantBeginIndex,
                                          new DataType(new Identifier(Array.Empty<string>(), "string"), false),
                                                       constantData.ToArray())
                });
        }

        private static PartialGenerationResult BuildDataAccessorCommand(GenerationSource<DataAccessorSource> source)
        {
            var commands = Utils.CombineLeadingCommand((byte)RootCommand.Stack, (byte)StackCommand.PushFromObject).ToList();

            // Evaluate index expression first and put it to the top of the stack
            if (source.Component.DataAccessor.AccessorType == DataAccessorType.ArrayElement)
            {
                var indexExpression = BuildSimpleExpression(GenerationSource<SimpleExpression>.MigrateGenerationSource(source.Component.DataAccessor.IndexEvalExpression!, source));
                if (indexExpression != null)
                {
                    commands.AddRange(indexExpression.Commands);
                }
            }

            switch (source.Component.Origin)
            {
                case DataAccessorSource.AccessorOrigin.Local:
                    commands.Add(0x00);
                    break;
                case DataAccessorSource.AccessorOrigin.Global:
                    commands.Add(0x01);
                    break;
            }

            var slot = Utils.GenerateSlotData(source.Component.Slot, source.PackageMetadata);
            commands.AddRange(slot);

            // TODO: Check whether the the object is singleton or array element

            return new PartialGenerationResult(commands.ToArray());
        }
    }
}
