using Arc.Compiler.Shared.Parsing.Components.Data;

namespace Arc.Compiler.Shared.Parsing.Components.Group
{
    public record GroupField(DataDeclarator Declarator, GSBlock? Getter, GSBlock? Setter)
    {
    }
}