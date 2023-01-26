using Arc.Compiler.Shared.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Expression
{
    public class ExpressionTerm
    {
        public ExpressionTermType TermType { get; }

        private object? Target { get; }

        public ExpressionTerm(OperatorToken operatorToken)
        {
            TermType = ExpressionTermType.Operator;
            Target = operatorToken;
        }

        public ExpressionTerm(ExpressionDataTerm dataTerm)
        {
            TermType = ExpressionTermType.Data;
            Target = dataTerm;
        }

        public ExpressionTerm(ExpressionTermType termType)
        {
            TermType = termType;
            Target = null;
        }

        public OperatorToken? GetOperator()
        {
            if (TermType == ExpressionTermType.Operator)
            {
                return Target as OperatorToken;
            }
            else
            {
                return null;
            }
        }

        public ExpressionDataTerm? GetDataTerm()
        {
            if (TermType == ExpressionTermType.Data)
            {
                return Target as ExpressionDataTerm;
            }
            else
            {
                return null;
            }
        }
    }
}
