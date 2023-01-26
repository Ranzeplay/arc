using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Expression
{
    /// <summary>
    /// The type of an expression term. OpenPriority is to lift the priority to the coming terms "(" token, ClosePriority is equal to ")" token
    /// </summary>
    public enum ExpressionTermType
    {
        Operator,
        Data,
        OpenPriority,
        ClosePriority
    }
}
