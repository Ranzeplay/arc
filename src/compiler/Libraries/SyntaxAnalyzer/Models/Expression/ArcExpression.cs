using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Expression
{
    internal class ArcExpression
    {
        public IEnumerable<ArcExpressionTerm> Terms { get; set; }

        public ArcExpression(ArcSourceCodeParser.Arc_expressionContext context)
        {
            var terms = new List<ArcExpressionTerm>();

            if (context.MULTIPLY() != null)
            {
                var exprBefore = new ArcExpression(context.arc_expression(0));
                var exprAfter = new ArcExpression(context.arc_expression(1));
                var op = ArcOperator.Multiply;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.DIVIDE() != null)
            {
                var exprBefore = new ArcExpression(context.arc_expression(0));
                var exprAfter = new ArcExpression(context.arc_expression(1));
                var op = ArcOperator.Divide;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.PLUS() != null)
            {
                var exprBefore = new ArcExpression(context.arc_expression(0));
                var exprAfter = new ArcExpression(context.arc_expression(1));
                var op = ArcOperator.Plus;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.MINUS() != null)
            {
                var exprBefore = new ArcExpression(context.arc_expression(0));
                var exprAfter = new ArcExpression(context.arc_expression(1));
                var op = ArcOperator.Minus;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.BITWISE_AND() != null)
            {
                var exprBefore = new ArcExpression(context.arc_expression(0));
                var exprAfter = new ArcExpression(context.arc_expression(1));
                var op = ArcOperator.BitwiseAnd;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.BITWISE_OR() != null)
            {
                var exprBefore = new ArcExpression(context.arc_expression(0));
                var exprAfter = new ArcExpression(context.arc_expression(1));
                var op = ArcOperator.BitwiseOr;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.BITWISE_XOR() != null)
            {
                var exprBefore = new ArcExpression(context.arc_expression(0));
                var exprAfter = new ArcExpression(context.arc_expression(1));
                var op = ArcOperator.BitwiseXor;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.BITWISE_NOT() != null)
            {
                var exprAfter = new ArcExpression(context.arc_expression(0));
                var op = ArcOperator.BitwiseNot;

                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.LOGICAL_AND() != null)
            {
                var exprBefore = new ArcExpression(context.arc_expression(0));
                var exprAfter = new ArcExpression(context.arc_expression(1));
                var op = ArcOperator.LogicalAnd;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.LOGICAL_OR() != null)
            {
                var exprBefore = new ArcExpression(context.arc_expression(0));
                var exprAfter = new ArcExpression(context.arc_expression(1));
                var op = ArcOperator.LogicalOr;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.LOGICAL_NOT() != null)
            {
                var exprAfter = new ArcExpression(context.arc_expression(0));
                var op = ArcOperator.LogicalNot;

                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.COMP_LT() != null)
            {
                var exprBefore = new ArcExpression(context.arc_expression(0));
                var exprAfter = new ArcExpression(context.arc_expression(1));
                var op = ArcOperator.LessThan;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.COMP_GT() != null)
            {
                var exprBefore = new ArcExpression(context.arc_expression(0));
                var exprAfter = new ArcExpression(context.arc_expression(1));
                var op = ArcOperator.GreaterThan;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.COMP_LTE() != null)
            {
                var exprBefore = new ArcExpression(context.arc_expression(0));
                var exprAfter = new ArcExpression(context.arc_expression(1));
                var op = ArcOperator.LessThanOrEqual;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.COMP_GTE() != null)
            {
                var exprBefore = new ArcExpression(context.arc_expression(0));
                var exprAfter = new ArcExpression(context.arc_expression(1));
                var op = ArcOperator.GreaterThanOrEqual;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.COMP_OBJ_EQ() != null)
            {
                var exprBefore = new ArcExpression(context.arc_expression(0));
                var exprAfter = new ArcExpression(context.arc_expression(1));
                var op = ArcOperator.ObjectEquals;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.COMP_OBJ_NEQ() != null)
            {
                var exprBefore = new ArcExpression(context.arc_expression(0));
                var exprAfter = new ArcExpression(context.arc_expression(1));
                var op = ArcOperator.ObjectNotEquals;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op));
            }
            else if (context.arc_wrapped_expression() != null)
            {
                terms.AddRange(new ArcExpression(context.arc_wrapped_expression().arc_expression()).Terms);
            }
            else if (context.arc_data_value() != null)
            {
                terms.Add(new ArcExpressionTerm(ArcDataValue.FromTokens(context.arc_data_value())));
            }
            else
            {
                throw new NotImplementedException();
            }


            Terms = terms;
        }
    }
}
