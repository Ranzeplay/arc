using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;

namespace Arc.Compiler.SyntaxAnalyzer.Models
{
    internal class ArcCompilationUnit(ArcSourceCodeParser.Arc_compilation_unitContext context)
    {
        public IEnumerable<ArcStatementLink> LinkedSymbols { get; set; } = context.arc_stmt_link().Select(stmt => new ArcStatementLink(stmt));

        public ArcNamespaceBlock Namespace { get; set; } = new ArcNamespaceBlock(context.arc_namespace_block());
    }
}
