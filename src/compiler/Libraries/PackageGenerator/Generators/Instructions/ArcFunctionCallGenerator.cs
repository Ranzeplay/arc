using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal class ArcFunctionCallGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcFunctionCall funcCall)
        {
            var result = new ArcPartialGenerationResult();

            long funcId = Utils.GetFunctionId(source, funcCall);

            foreach (var arg in funcCall.Arguments.Reverse())
            {
                result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, arg.Expression));
            }

            result.Append(new ArcFunctionCallInstruction(funcId, funcCall.Arguments.Count()).Encode(source));

            return result;
        }
    }
}
