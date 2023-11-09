using Arc.Compiler.CommandGenerator.Models;
using Arc.Compiler.Shared.Parsing.AST;

namespace Arc.Compiler.CommandGenerator.Builders
{
    internal class ActionBlockCommand
    {
        public static PartialGenerationResult Build(GenerationContext<ActionBlock> source)
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
                    case ASTNodeType.LoopContinue:
                        var @continue = LoopControlCommand.BuildContinueCommand(source.PackageMetadata)!;
                        result.Combine(@continue);
                        break;
                    case ASTNodeType.LoopBreak:
                        var @break = LoopControlCommand.BuildBreakCommand(source.PackageMetadata)!;
                        result.Combine(@break);
                        break;
                    case ASTNodeType.FunctionCall:
                        var funcCall = FunctionCallCommand.Build(source.TransferToNewComponent(action.GetFunctionCallBlock()!))!;
                        result.Combine(funcCall);
                        break;
                    case ASTNodeType.FunctionReturn:
                        var funcRet = FunctionReturnCommand.Build(source.TransferToNewComponent(action.GetFunctionReturnBlock()!));
                        result.Combine(funcRet);
                        break;
                    case ASTNodeType.ConditionalLoop:
                        var loop = ConditionalLoopCommand.Build(source.TransferToNewComponent(action.GetConditionalLoopBlock()!))!;
                        result.Combine(loop);
                        break;
                    case ASTNodeType.ConditionalExec:
                        var exec = ConditionalExecCommand.Build(source.TransferToNewComponent(action.GetConditionalExecBlock()!))!;
                        result.Combine(exec);
                        break;
                    case ASTNodeType.Invalid:
                        throw new NotImplementedException();
                }
            }

            return result;
        }
    }
}
