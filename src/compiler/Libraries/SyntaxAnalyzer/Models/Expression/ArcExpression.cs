using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Expression;

public sealed record ArcValueExpression(ArcDataValue Value);
public sealed record ArcUnaryExpression(ArcOperator Operator, ArcExpression Operand);
public sealed record ArcBinaryExpression(ArcOperator Operator, ArcExpression Left, ArcExpression Right);
public sealed record ArcWrappedExpression(ArcExpression Expression);
public union ArcExpressionShape(ArcValueExpression, ArcUnaryExpression, ArcBinaryExpression, ArcWrappedExpression);

public class ArcExpression : IArcTraceable<ArcSourceCodeParser.Arc_expressionContext>
{
    public ArcExpressionShape Shape { get; }

    public IEnumerable<ArcExpressionTerm> Terms { get; }

    public ArcSourceCodeParser.Arc_expressionContext Context { get; }

    public ArcExpression(ArcSourceCodeParser.Arc_expressionContext context)
    {
        Context = context;
        if (context.LOGICAL_NOT() is not null || context.BITWISE_NOT() is not null)
        {
            var operand = context.arc_wrapped_expression() is { } wrapped
                ? new ArcExpression(wrapped.arc_expression())
                : new ArcExpression(ArcDataValue.FromTokens(context.arc_data_value()), context);
            var op = context.LOGICAL_NOT() is not null ? ArcOperator.LogicalNot : ArcOperator.BitwiseNot;
            Shape = new ArcUnaryExpression(op, operand);
            Terms = [.. operand.Terms, new ArcExpressionTerm(op) { Context = context }];
            return;
        }

        ArcOperator? binary = context.MULTIPLY() is not null ? ArcOperator.Multiply :
            context.DIVIDE() is not null ? ArcOperator.Divide :
            context.PLUS() is not null ? ArcOperator.Plus :
            context.MINUS() is not null ? ArcOperator.Minus :
            context.MODULO() is not null ? ArcOperator.Modulus :
            context.BITWISE_AND() is not null ? ArcOperator.BitwiseAnd :
            context.BITWISE_OR() is not null ? ArcOperator.BitwiseOr :
            context.BITWISE_XOR() is not null ? ArcOperator.BitwiseXor :
            context.BITWISE_LSHIFT() is not null ? ArcOperator.ShiftLeft :
            context.BITWISE_RSHIFT() is not null ? ArcOperator.ShiftRight :
            context.LOGICAL_AND() is not null ? ArcOperator.LogicalAnd :
            context.LOGICAL_OR() is not null ? ArcOperator.LogicalOr :
            context.COMP_LT() is not null ? ArcOperator.LessThan :
            context.COMP_GT() is not null ? ArcOperator.GreaterThan :
            context.COMP_LTE() is not null ? ArcOperator.LessThanOrEqual :
            context.COMP_GTE() is not null ? ArcOperator.GreaterThanOrEqual :
            context.COMP_OBJ_EQ() is not null ? ArcOperator.ObjectEquals :
            context.COMP_OBJ_NEQ() is not null ? ArcOperator.ObjectNotEquals :
            context.COMP_REF_EQ() is not null ? ArcOperator.ReferenceEquals :
            context.COMP_REF_NEQ() is not null ? ArcOperator.ReferenceNotEquals : null;
        if (binary is { } binaryOperator)
        {
            var operands = context.arc_expression();
            var left = new ArcExpression(operands[0]);
            var right = new ArcExpression(operands[1]);
            Shape = new ArcBinaryExpression(binaryOperator, left, right);
            Terms = [.. left.Terms, .. right.Terms, new ArcExpressionTerm(binaryOperator) { Context = context }];
        }
        else if (context.arc_wrapped_expression() is { } wrapped)
        {
            var expression = new ArcExpression(wrapped.arc_expression());
            Shape = new ArcWrappedExpression(expression);
            Terms = expression.Terms.Select(term => { term.Context = context; return term; }).ToList();
        }
        else if (context.arc_data_value() is { } data)
        {
            var value = ArcDataValue.FromTokens(data);
            Shape = new ArcValueExpression(value);
            Terms = [new ArcExpressionTerm(value) { Context = data }];
        }
        else
        {
            throw new NotImplementedException();
        }
    }

    private ArcExpression(ArcDataValue value, ArcSourceCodeParser.Arc_expressionContext context)
    {
        Context = context;
        Shape = new ArcValueExpression(value);
        Terms = [new ArcExpressionTerm(value) { Context = context }];
    }
}
