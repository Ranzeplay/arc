using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Expression
{
    public class ArcExpression : IArcTraceable<ArcSourceCodeParser.Arc_expressionContext>
    {
        public IEnumerable<ArcExpressionTerm> Terms { get; set; }
        
        public List<ArcExpression> SubExpressions { get; set; }
        
        public ArcOperator? Operator { get; set; }
        
        public bool IsWrapped { get; set; }
        
        public ArcDataValue? DataValue { get; set; }

        public ArcExpression(ArcSourceCodeParser.Arc_expressionContext context)
        {
            var terms = new List<ArcExpressionTerm>();

            SubExpressions = context.arc_expression()
                .Select(expr => new ArcExpression(expr))
                .ToList();
            IsWrapped = false;

            if (context.MULTIPLY() != null)
            {
                var exprBefore = SubExpressions.ElementAt(0);
                var exprAfter = SubExpressions.ElementAt(1);
                const ArcOperator op = ArcOperator.Multiply;
                Operator = op;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.DIVIDE() != null)
            {
                var exprBefore = SubExpressions.ElementAt(0);
                var exprAfter = SubExpressions.ElementAt(1);
                const ArcOperator op = ArcOperator.Divide;
                Operator = op;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.PLUS() != null)
            {
                var exprBefore = SubExpressions.ElementAt(0);
                var exprAfter = SubExpressions.ElementAt(1);
                const ArcOperator op = ArcOperator.Plus;
                Operator = op;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.MINUS() != null)
            {
                var exprBefore = SubExpressions.ElementAt(0);
                var exprAfter = SubExpressions.ElementAt(1);
                const ArcOperator op = ArcOperator.Minus;
                Operator = op;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.BITWISE_AND() != null)
            {
                var exprBefore = SubExpressions.ElementAt(0);
                var exprAfter = SubExpressions.ElementAt(1);
                const ArcOperator op = ArcOperator.BitwiseAnd;
                Operator = op;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.BITWISE_OR() != null)
            {
                var exprBefore = SubExpressions.ElementAt(0);
                var exprAfter = SubExpressions.ElementAt(1);
                const ArcOperator op = ArcOperator.BitwiseOr;
                Operator = op;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.BITWISE_XOR() != null)
            {
                var exprBefore = SubExpressions.ElementAt(0);
                var exprAfter = SubExpressions.ElementAt(1);
                const ArcOperator op = ArcOperator.BitwiseXor;
                Operator = op;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.BITWISE_NOT() != null)
            {
                var exprAfter = SubExpressions.ElementAt(0);
                const ArcOperator op = ArcOperator.BitwiseNot;
                Operator = op;

                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.LOGICAL_AND() != null)
            {
                var exprBefore = SubExpressions.ElementAt(0);
                var exprAfter = SubExpressions.ElementAt(1);
                var op = ArcOperator.LogicalAnd;
                Operator = op;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.LOGICAL_OR() != null)
            {
                var exprBefore = SubExpressions.ElementAt(0);
                var exprAfter = SubExpressions.ElementAt(1);
                const ArcOperator op = ArcOperator.LogicalOr;
                Operator = op;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.LOGICAL_NOT() != null)
            {
                var exprAfter = SubExpressions.ElementAt(0);
                const ArcOperator op = ArcOperator.LogicalNot;
                Operator = op;

                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.COMP_LT() != null)
            {
                var exprBefore = SubExpressions.ElementAt(0);
                var exprAfter = SubExpressions.ElementAt(1);
                const ArcOperator op = ArcOperator.LessThan;
                Operator = op;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.COMP_GT() != null)
            {
                var exprBefore = SubExpressions.ElementAt(0);
                var exprAfter = SubExpressions.ElementAt(1);
                const ArcOperator op = ArcOperator.GreaterThan;
                Operator = op;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.COMP_LTE() != null)
            {
                var exprBefore = SubExpressions.ElementAt(0);
                var exprAfter = SubExpressions.ElementAt(1);
                const ArcOperator op = ArcOperator.LessThanOrEqual;
                Operator = op;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.COMP_GTE() != null)
            {
                var exprBefore = SubExpressions.ElementAt(0);
                var exprAfter = SubExpressions.ElementAt(1);
                const ArcOperator op = ArcOperator.GreaterThanOrEqual;
                Operator = op;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.COMP_OBJ_EQ() != null)
            {
                var exprBefore = SubExpressions.ElementAt(0);
                var exprAfter = SubExpressions.ElementAt(1);
                const ArcOperator op = ArcOperator.ObjectEquals;
                Operator = op;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.COMP_OBJ_NEQ() != null)
            {
                var exprBefore = SubExpressions.ElementAt(0);
                var exprAfter = SubExpressions.ElementAt(1);
                const ArcOperator op = ArcOperator.ObjectNotEquals;
                Operator = op;

                terms.AddRange(exprBefore.Terms);
                terms.AddRange(exprAfter.Terms);
                terms.Add(new ArcExpressionTerm(op) { Context = context });
            }
            else if (context.arc_wrapped_expression() != null)
            {
                terms.AddRange(new ArcExpression(
                    context.arc_wrapped_expression().arc_expression())
                        .Terms
                        .Select(t => { t.Context = context; return t; })
                );

                SubExpressions = [new ArcExpression(context.arc_wrapped_expression().arc_expression())];
                IsWrapped = true;
            }
            else if (context.arc_data_value() != null)
            {
                var dv = ArcDataValue.FromTokens(context.arc_data_value());
                
                terms.Add(new ArcExpressionTerm(dv)
                {
                    Context = context.arc_data_value()
                });

                DataValue = dv;
            }
            else
            {
                throw new NotImplementedException();
            }


            Terms = terms;
            Context = context;
        }

        public ArcSourceCodeParser.Arc_expressionContext Context { get; }
    }
}
