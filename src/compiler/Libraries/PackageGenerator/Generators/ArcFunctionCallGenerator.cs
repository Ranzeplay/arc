using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;

namespace Arc.Compiler.PackageGenerator.Generators
{
    internal class ArcFunctionCallGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcGenerationSource source, ArcFunctionCall funcCall)
        {
            var result = new ArcPartialGenerationResult();

            long funcId = 0;
            if (funcCall.Identifier.Namespace != null && funcCall.Identifier.Namespace.Any())
            {
                var funcDeclarator = source.AccessibleSymbols
                    .OfType<ArcFunctionDescriptor>()
                    .FirstOrDefault(f => f.RawFullName.StartsWith(funcCall.Identifier.AsFunctionIdentifier()))
                    ?? throw new InvalidOperationException("Invalid function declarator");
                funcId = funcDeclarator.Id;
            }
            else
            {
                var funcNode = source.CurrentNode
                    .Root
                    .GetSpecificChild<ArcScopeTreeIndividualFunctionNode>(n => n.SyntaxTree.Declarator.Identifier.Name == funcCall.Identifier.Name, true)
                    ?? throw new InvalidOperationException("Invalid function node");
                funcId = funcNode.Descriptor.Id;
            }

            foreach (var arg in funcCall.Arguments.Reverse())
            {
                result.Append(ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, arg.Expression));
            }

            result.Append(new ArcFunctionCallInstruction(funcId, funcCall.Arguments.Count()).Encode(source));

            return result;
        }
    }
}
