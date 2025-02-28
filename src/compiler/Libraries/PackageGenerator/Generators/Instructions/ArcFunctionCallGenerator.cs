using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal class ArcFunctionCallGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcFunctionCall funcCall, bool isSelfFunction, ArcScopeTreeGroupNode? searchUnderGroup = null)
        {
            var result = new ArcPartialGenerationResult();

            var funcId = ArcFunctionHelper.GetFunctionId(source, funcCall, searchUnderGroup);

            foreach (var arg in funcCall.Arguments)
            {
                result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, arg.Expression, true));
            }

            var totalArgs = funcCall.Arguments.Count() + (isSelfFunction ? 1 : 0);

            result.Append(new ArcFunctionCallInstruction(funcId, (uint)totalArgs).Encode(source));

            return result;
        }
    }
}
