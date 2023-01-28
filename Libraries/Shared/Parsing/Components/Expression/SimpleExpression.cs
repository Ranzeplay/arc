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
            this.OutputDataType = dataType;
        }
    }
}
