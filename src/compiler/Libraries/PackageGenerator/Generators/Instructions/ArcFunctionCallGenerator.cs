using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal class ArcFunctionCallGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcFunctionCall funcCall, bool isSelfFunction, bool considerLambda, ArcScopeTreeFunctionNodeBase baseFn, ArcScopeTreeGroupNode? searchUnderGroup = null)
        {
            var result = new ArcPartialGenerationResult();

            foreach (var arg in funcCall.Arguments)
            {
                result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, arg.Expression, baseFn));
            }
            var totalArgs = funcCall.Arguments.Count() + (isSelfFunction ? 1 : 0);
            
            ulong funcId = 0;
            
            // We check if the function identifier is a local data slot, which indicates a lambda
            var lambdaDataSlot = source.LocalDataSlots.FirstOrDefault(lds => lds.Name.Equals(funcCall.Identifier.Name));
            if (considerLambda && funcCall.Identifier.Namespace == null && lambdaDataSlot != null)
            {
                // We keep funcId as 0 so that the function call instruction knows to call a lambda from a data slot
                // Then it will read symbol from stack top
                result.Append(new ArcLoadDataToStackInstruction(new (ArcDataSourceType.DataSlot, lambdaDataSlot.SlotId, false)).Encode(source));
            }
            else
            {
                var (actualFuncId, actualLogs) = ArcFunctionHelper.GetFunctionId(source, funcCall, searchUnderGroup);
                if (actualLogs.Any())
                {
                    result.Logs.AddRange(actualLogs);
                }
                funcId = actualFuncId;
            }

            result.Append(new ArcFunctionCallInstruction(funcId, (uint)totalArgs, funcCall.SpecializedGenericTypes).Encode(source));

            return result;
        }
        
        // Make sure that the function symbol is already on stack top
        public static ArcPartialGenerationResult GenerateLambdaCall(ArcGenerationSource source, ArcFunctionCall funcCall, ArcScopeTreeFunctionNodeBase baseFn)
        {
            var result = new ArcPartialGenerationResult();

            foreach (var arg in funcCall.Arguments)
            {
                result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, arg.Expression, baseFn));
            }

            result.Append(new ArcFunctionCallInstruction(0, (uint)funcCall.Arguments.Count(), funcCall.SpecializedGenericTypes).Encode(source));

            return result;
        }
        
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcFunctionCall funcCall, ArcScopeTreeGroupFunctionNode funcNode, bool isSelfFunction, ArcScopeTreeGroupNode? searchUnderGroup = null)
        {
            var result = new ArcPartialGenerationResult();

            foreach (var arg in funcCall.Arguments)
            {
                result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, arg.Expression, funcNode));
            }

            var totalArgs = funcCall.Arguments.Count() + (isSelfFunction ? 1 : 0);

            result.Append(new ArcFunctionCallInstruction(funcNode.Id, (uint)totalArgs, funcCall.SpecializedGenericTypes).Encode(source));

            return result;
        }
    }
}
