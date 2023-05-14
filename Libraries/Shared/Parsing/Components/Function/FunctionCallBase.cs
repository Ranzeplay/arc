namespace Arc.Compiler.Shared.Parsing.Components.Function
{
    public record FunctionCallBase(Identifier TargetFunctionIdentifier, FunctionArgument[] Arguments)
    {
    }
}
