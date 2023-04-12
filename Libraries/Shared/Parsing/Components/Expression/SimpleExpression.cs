using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Expression
{
    public class SimpleExpression
    {
        /// <summary>
        /// Infix expression, should be converted to postfix expression in code generation
        /// </summary>
        public ExpressionTerm[] Terms { get; }

        public DataType? OutputDataType { get; private set; }

        public SimpleExpression(ExpressionTerm[] terms, DataType? outputDataType = null)
        {
            Terms = terms;
            OutputDataType = outputDataType;
        }

        public void AssignOutputDataType(DataType dataType)
        {
            OutputDataType = dataType;
        }

        public SimpleExpression ToPostfixExpression()
        {
            var operatorStack = new Stack<ExpressionTerm>();
            var result = new List<ExpressionTerm>();

            foreach (var term in Terms)
            {
                switch (term.TermType)
                {
                    case ExpressionTermType.Data:
                        {
                            // Push all terms into result directly (infix to postfix)
                            result.Add(term);
                            break;
                        }
                    case ExpressionTermType.Operator:
                        {
                            // The previous TermType::Priority must increased the priority level
                            while (operatorStack.TryPeek(out var r) && r.TermType == ExpressionTermType.Operator)
                            {
                                // Pop if operator priority is higher than current operator
                                if (TokenConstants.CalculationOperatorPriority[r.GetOperator()!.CalculationOperator] > TokenConstants.CalculationOperatorPriority[term.GetOperator()!.CalculationOperator])
                                {
                                    result.Add(operatorStack.Pop());
                                }
                                else
                                {
                                    break;
                                }
                            }

                            operatorStack.Push(term);

                            break;
                        }
                    case ExpressionTermType.OpenPriority:
                        {
                            operatorStack.Push(term);
                            break;
                        }
                    case ExpressionTermType.ClosePriority:
                        {
                            // Pop until bracket
                            while (operatorStack.TryPeek(out var r) && r.TermType == ExpressionTermType.Operator)
                            {
                                result.Add(operatorStack.Pop());

                            }

                            // Pop this bracket (it won't be transferred to result)
                            operatorStack.Pop();

                            break;
                        }
                    default:
                        {
                            throw new NotImplementedException("Invalid ExpressionTermType");
                        }
                }
            }

            // Push all operators remained
            result.AddRange(operatorStack.ToArray());

            return new(result.ToArray(), OutputDataType);
        }
    }
}
