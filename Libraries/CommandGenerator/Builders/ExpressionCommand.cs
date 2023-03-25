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

            var postfixExpression = Utils.ExpressionInfixToPostfix(source.ActionBlock);

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

                            if(partialResult != null)
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
            switch (source.ActionBlock.DataTermType)
            {
                case ExpressionDataTermType.Number:
                    return BuildNumberCommand(GenerationSource<string>.MigrateGenerationSource(source.ActionBlock.GetNumber()!, source));
                case ExpressionDataTermType.String:
                    return BuildStringCommand(GenerationSource<string>.MigrateGenerationSource(source.ActionBlock.GetString()!, source));
                case ExpressionDataTermType.DataAccessor:
                    throw new NotImplementedException();
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
            var numberObj = new NumberObject(source.ActionBlock);

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
            var stringBytes = Encoding.UTF8.GetBytes(source.ActionBlock);
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
    }
}
