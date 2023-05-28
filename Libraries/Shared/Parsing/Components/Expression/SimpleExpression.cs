using Arc.Compiler.Shared.LexicalAnalysis;
using Arc.Compiler.Shared.Parsing.Components.Data;

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
    }
}
