using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementDeclaration(ArcSourceCodeParser.Arc_declaration_stmtContext context) : ArcDataDeclarator(context.arc_data_declaration())
    {
    }
}
