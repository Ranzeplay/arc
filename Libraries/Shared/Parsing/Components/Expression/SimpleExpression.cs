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
        public ExpressionTerm[] Terms { get; }

        public DataType OutputDataType { get; }

        public SimpleExpression(ExpressionTerm[] terms, DataType outputDataType)
        {
            Terms = terms;
            OutputDataType = outputDataType;
        }
    }
}
