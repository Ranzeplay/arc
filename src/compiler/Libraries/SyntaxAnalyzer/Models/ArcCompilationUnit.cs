using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;

namespace Arc.Compiler.SyntaxAnalyzer.Models
{
    internal class ArcCompilationUnit(ArcSourceCodeParser.Arc_compilation_unitContext source)
    {
        public IEnumerable<ArcStatementLink> LinkedSymbols { get; set; } = source.arc_stmt_link().Select(stmt => new ArcStatementLink(stmt));

        public ArcNamespaceBlock Namespace { get; set; } = new ArcNamespaceBlock(source.arc_namespace_block());
    }
}
