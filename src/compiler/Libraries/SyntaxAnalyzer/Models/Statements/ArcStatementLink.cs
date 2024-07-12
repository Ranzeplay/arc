using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementLink(ArcSourceCodeParser.Arc_stmt_linkContext context)
    {
        public ArcNamespaceIdentifier Identifier { get; set; } = new(context.arc_namespace_identifier());
    }
}
