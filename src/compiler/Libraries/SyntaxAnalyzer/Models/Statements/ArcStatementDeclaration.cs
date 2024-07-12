using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Statements
{
    internal class ArcStatementDeclaration(ArcSourceCodeParser.Arc_stmt_declContext context) : ArcDataDeclarator(context.arc_data_declarator())
    {
    }
}
