using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementLink(ArcSourceCodeParser.Arc_stmt_linkContext context) : IArcTraceable<ArcSourceCodeParser.Arc_stmt_linkContext>
    {
        public ArcNamespaceIdentifier Identifier { get; set; } = new(context.arc_namespace_identifier());
        public ArcSourceCodeParser.Arc_stmt_linkContext Context { get; } = context;
    }
}
