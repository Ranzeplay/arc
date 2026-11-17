using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal static class ArcSequentialExecutionGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcBlockSequentialExecution seqExec, ArcScopeTreeFunctionNodeBase fnNode)
        {
            var result = new ArcPartialGenerationResult();

            foreach (var step in seqExec.ExecutionSteps)
            {
                var stepResult = step switch
                {
                    ArcStatementDeclaration decl => ArcDeclarationStatementGenerator.Generate(decl, source, fnNode),
                    ArcStatementAssign assign => assign.Generate(source, fnNode),
                    ArcBlockIf ifBlock => ArcConditionBlockGenerator.Encode(source, ifBlock, fnNode),
                    ArcBlockConditionalLoop conditionalLoop => ArcConditionLoopBlockGenerator.EncodeWhileLoop(source, conditionalLoop, fnNode),
                    ArcStatementReturn ret => ArcSequenceReturnGenerator.Generate(source, ret, fnNode),
                    ArcStatementBreak => ArcLoopControlGenerator.GenerateBreak(source),
                    ArcStatementContinue => ArcLoopControlGenerator.GenerateContinue(source),
                    ArcStatementCall call => GenerateCall(source, call, fnNode),
                    ArcStatementThrow statement => GenerateThrow(source, statement, fnNode),
                    ArcBlockExtendedConditionalLoop forLoop => ArcConditionLoopBlockGenerator.EncodeForLoop(source, forLoop, fnNode),
                    ArcBlockLoop => throw new NotSupportedException("Unconditional loops are not implemented by the stack backend"),
                    null => throw new ArgumentNullException(nameof(step)),
                };

                source.Merge(stepResult);
                result.Append(stepResult);
            }

            result.DataSlots = [];
            return result;
        }

        private static ArcPartialGenerationResult GenerateCall(ArcGenerationSource source, ArcStatementCall call, ArcScopeTreeFunctionNodeBase fnNode)
        {
            var stepResult = new ArcPartialGenerationResult();
            if (call.Value is not ArcFunctionCall &&
                !(call.Value is ArcCallChain chain && chain.Terms.LastOrDefault()?.Value is ArcFunctionCall))
            {
                stepResult.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Expected a function call clause", source.Name, call.Context));
                return stepResult;
            }

            ulong? funcId;
            IEnumerable<ArcCompilationLogBase> logs;
            if (call.Value is ArcFunctionCall functionCall)
            {
                stepResult = ArcFunctionCallGenerator.Generate(source, functionCall, true, fnNode);
                (funcId, logs) = ArcFunctionHelper.GetFunctionId(source, functionCall);
            }
            else if (call.Value is ArcCallChain callChain)
            {
                (stepResult, funcId) = ArcCallChainGenerator.GenerateWithFinalCalledFunctionId(source, callChain, fnNode);
                logs = [];
            }
            else throw new InvalidOperationException("Uninitialized statement call");

            // Discard the result of the function call if any
            stepResult.Logs.AddRange(logs);
            var function = source.GlobalScopeTree
                .FlattenedNodes
                .OfType<ArcScopeTreeFunctionNodeBase>()
                .FirstOrDefault(f => f.Id == funcId);

            if (function == null)
            {
                stepResult.Logs.Add(new ArcSourceLocatableLog(LogLevel.Error, 0, "Function not found", source.Name, call.Context));
            }

            if (function?.ReturnValueType.Type.TypeId != 0)
            {
                stepResult.Append(new ArcDiscardStackTopInstruction().Encode(source));
            }
            return stepResult;
        }

        private static ArcPartialGenerationResult GenerateThrow(ArcGenerationSource source, ArcStatementThrow statement, ArcScopeTreeFunctionNodeBase fnNode)
        {
            var result = ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, statement.Expression, fnNode);
            result.Append(new ArcThrowInstruction().Encode(source));
            return result;
        }
    }
}
