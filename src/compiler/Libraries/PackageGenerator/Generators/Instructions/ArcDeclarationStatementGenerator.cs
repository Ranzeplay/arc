using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;

namespace Arc.Compiler.PackageGenerator.Generators.Instructions
{
    internal class ArcDeclarationStatementGenerator
    {
        public static ArcPartialGenerationResult Generate(ArcStatementDeclaration decl, ArcGenerationSource source, ArcScopeTreeFunctionNodeBase fnNode)
        {
            var result = new ArcDeclarationInstruction(decl.DataDeclarator).Encode(source);
            result.SourceInformation.FunctionDataSlotMapping[fnNode.Id] = [];
            var slot = result.DataSlots.First();
            result.SourceInformation.FunctionDataSlotMapping[fnNode.Id][slot.SlotId] = slot.Name;

            if (decl.InitialValueExpression != null)
            {
                var expr = ArcExpressionEvaluationGenerator.GenerateEvaluationCommand(source, decl.InitialValueExpression);
                var assignment = new ArcPopToSlotInstruction(slot).Encode(source);

                result.Append(expr);
                result.Append(assignment);
            }

            return result;
        }
    }
}
