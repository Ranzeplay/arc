using Arc.Compiler.Shared.Parsing.Components.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Data
{
    public class DataAccessor
    {
        public DataAccessorType AccessorType { get; }

        public DataDeclarator DataDeclarator { get; }

        public SimpleExpression? IndexEvalExpression { get; }

        public DataAccessor(DataDeclarator dataDeclarator, SimpleExpression indexEvalExpression)
        {
            AccessorType = DataAccessorType.ArrayElement;
            DataDeclarator = dataDeclarator;
            IndexEvalExpression = indexEvalExpression;
        }

        public DataAccessor(DataDeclarator dataDeclarator)
        {
            AccessorType = DataAccessorType.Singleton;
            DataDeclarator = dataDeclarator;
            IndexEvalExpression = null;
        }
    }
}
