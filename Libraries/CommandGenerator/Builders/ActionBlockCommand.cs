using Arc.Compiler.Shared.Parsing.AST;
using Arc.CompilerCommandGenerator.Models;

namespace Arc.CompilerCommandGenerator.Builders
{
    internal class ActionBlockCommand
    {
        public static PartialGenerationResult? Build(GenerationContext<ActionBlock> source)
        {
            var result = new PartialGenerationResult();

            foreach (var action in source.Component.ASTNodes)
            {
                switch (action.NodeType)
                {
                    case ASTNodeType.DataAssignment:
                        var assignment = AssignmentCommand.Build(source.TransferToNewComponent(action.GetAssignmentBlock()!))!;
                        result.Combine(assignment);
                        break;
                    case ASTNodeType.DataDeclaration:
                        var declaration = DeclarationCommand.Build(source.TransferToNewComponent(action.GetDeclarationBlock()!))!;
                        result.Combine(declaration);
                        break;
                    case ASTNodeType.Invalid:
                    case ASTNodeType.FunctionCall:
                    case ASTNodeType.FunctionReturn:
                    case ASTNodeType.ConditionalLoop:
                    case ASTNodeType.ConditionalExec:
                    case ASTNodeType.LoopContinue:
                    case ASTNodeType.LoopBreak:
                        throw new NotImplementedException();
                }
            }

            return result;
        }
    }
}
