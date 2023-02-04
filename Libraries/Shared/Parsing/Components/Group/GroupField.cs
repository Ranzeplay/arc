using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Data;

namespace Arc.Compiler.Shared.Parsing.Components.Group
{
    public class GroupField
    {
        public DataDeclarator Declarator { get; }

        public GSBlock? Getter { get; }

        public GSBlock? Setter { get; }

        public GroupField(DataDeclarator declarator, GSBlock? getter, GSBlock? setter)
        {
            Declarator = declarator;
            Getter = getter;
            Setter = setter;
        }
    }
} 