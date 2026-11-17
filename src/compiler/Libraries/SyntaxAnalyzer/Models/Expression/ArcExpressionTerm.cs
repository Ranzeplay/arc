using Antlr4.Runtime;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Expression;

public union ArcExpressionTermValue(ArcDataValue, ArcOperator);

public class ArcExpressionTerm(ArcExpressionTermValue value)
{
    public ArcExpressionTermValue Value { get; } = value;

    public ParserRuleContext Context { get; set; } = null!;
}
