namespace Arc.Compiler.Shared.Parsing.Components.Group
{
    public record GroupBlock(Identifier Identifier, GroupField[] Fields, GroupMethod[] GroupMethods, GroupFunction[] Functions)
    {
    }
}
