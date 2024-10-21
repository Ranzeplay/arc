using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal class FunctionCallGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcFunctionCall funcCall)
        {
            var result = new ArcPartialGenerationResult();

            var funcDeclarator = source.AccessibleSymbols
                .OfType<ArcFunctionDescriptor>()
                .FirstOrDefault(f => f.RawFullName == funcCall.Identifier.ToString());

            if (funcDeclarator != null)
            {
                throw new InvalidOperationException();
            }

            result.Append(new ArcFunctionCallInstruction(funcDeclarator.Id, funcCall.Arguments.Count()).Encode(source));

            return result;
        }
    }
}
