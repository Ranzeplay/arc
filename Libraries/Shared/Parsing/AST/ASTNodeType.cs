namespace Arc.Compiler.Shared.Parsing.AST
{
    public enum ASTNodeType
    {
        Invalid,
        DataAssignment,
        DataDeclaration,
        FunctionCall,
        FunctionReturn,
        ConditionalLoop,
        ConditionalExec,
        UnconditionalLoop,
        LoopContinue,
        LoopBreak
    }
}
