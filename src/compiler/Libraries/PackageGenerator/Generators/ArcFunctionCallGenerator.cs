using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal class ArcFunctionCallGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcFunctionCall funcCall)
        {
            var result = new ArcPartialGenerationResult();

            var funcDeclarator = source.AccessibleSymbols
                .OfType<ArcFunctionDescriptor>()
                .FirstOrDefault(f => f.RawFullName.StartsWith(funcCall.Identifier.AsFunctionIdentifier()));

            if (funcDeclarator == null)
            {
                throw new InvalidOperationException("Invalid function declarator");
            }

            foreach (var arg in funcCall.Arguments.Reverse())
            {
                result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, arg.Expression));
            }

            result.Append(new ArcFunctionCallInstruction(funcDeclarator.Id, funcCall.Arguments.Count()).Encode(source));

            return result;
        }
    }
}
