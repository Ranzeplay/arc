using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    public class ArcNamespaceBlock :IArcTraceable<ArcSourceCodeParser.Arc_namespace_blockContext>
    {
        public ArcNamespaceIdentifier Identifier { get; set; }

        public IEnumerable<ArcGroup> Groups { get; set; }

        public IEnumerable<ArcBlockIndependentFunction> Functions { get; set; }
        
        public ArcSourceCodeParser.Arc_namespace_blockContext Context { get; }

        public ArcNamespaceBlock(ArcSourceCodeParser.Arc_namespace_blockContext context)
        {
            Identifier = new(context.arc_namespace_declarator().arc_namespace_identifier());
            Functions = context.arc_namespace_member().ToList()
                .FindAll(f => f.arc_function_block() != null)
                .Select(f => new ArcBlockIndependentFunction(f.arc_function_block()));
            Groups = context.arc_namespace_member().ToList()
                .FindAll(g => g.arc_group_block() != null)
                .Select(g => new ArcGroup(g.arc_group_block()));

            Context = context;
        }
    }
}
