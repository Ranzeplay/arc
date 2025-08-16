using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    public class ArcNamespaceBlock : IArcTraceable<ArcSourceCodeParser.Arc_namespace_blockContext>, IArcLocatable
    {
        public ArcNamespaceIdentifier Identifier { get; set; }

        public IEnumerable<ArcGroup> Groups { get; set; }

        public IEnumerable<ArcBlockIndependentFunction> Functions { get; set; }

        public IEnumerable<ArcBlockEnum> Enums { get; set; }

        public ArcSourceCodeParser.Arc_namespace_blockContext Context { get; }

        public ArcNamespaceBlock(ArcSourceCodeParser.Arc_namespace_blockContext context)
        {
            Identifier = new(context.arc_namespace_declarator().arc_namespace_identifier());
            Functions = context.arc_namespace_member()
                .Where(f => f.arc_function_block() != null)
                .Select(f => new ArcBlockIndependentFunction(f.arc_function_block()));
            Groups = context.arc_namespace_member()
                .Where(g => g.arc_group_block() != null)
                .Select(g => new ArcGroup(g.arc_group_block()));
            Enums = context.arc_namespace_member()
                .Where(m => m.arc_enum_declarator() != null)
                .Select(e => new ArcBlockEnum(e.arc_enum_declarator()));

            Context = context;
        }

        public string GetSignature()
        {
            return string.Join(':', Identifier.Namespace ?? []);
        }
    }
}
