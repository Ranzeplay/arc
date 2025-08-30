using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;
using Arc.Compiler.SyntaxAnalyzer.Models.Statements;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

public class ArcBlockExtendedConditionalLoop(ArcSourceCodeParser.Arc_stmt_forContext context) : ArcExecutionStepBase, IArcTraceable<ArcSourceCodeParser.Arc_stmt_forContext>
{
    public ArcStatementDeclaration Initializer { get; set; } = new(context.arc_stmt_decl());
    public ArcBlockSequentialExecution Body { get; set; } = new(context.arc_wrapped_function_body());
    public ArcExpression Condition { get; set; } = new(context.arc_expression());
    public ArcStatementAssign Iterator { get; set; } = new(context.arc_stmt_assign());
    public ArcSourceCodeParser.Arc_stmt_forContext Context { get; } = context;
}
