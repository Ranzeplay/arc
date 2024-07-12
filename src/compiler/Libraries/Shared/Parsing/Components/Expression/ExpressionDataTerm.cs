using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Shared.Parsing.Components.Expression
{
    public class ExpressionDataTerm
    {
        public ExpressionDataTermType DataTermType { get; }

        private object Target { get; }

        public ExpressionDataTerm(DataAccessor dataAccessor)
        {
            DataTermType = ExpressionDataTermType.DataAccessor;
            Target = dataAccessor;
        }

        public ExpressionDataTerm(FunctionCallBase functionCall)
        {
            DataTermType = ExpressionDataTermType.FunctionCall;
            Target = functionCall;
        }

        public ExpressionDataTerm(ExpressionDataTermType termType, string target)
        {
            if (termType == ExpressionDataTermType.String || termType == ExpressionDataTermType.Number)
            {
                DataTermType = termType;
                Target = target;
            }
            else
            {
                throw new ArgumentException("Please use other constructors!");
            }
        }

        public DataAccessor? GetDataAccessor()
        {
            if (DataTermType == ExpressionDataTermType.DataAccessor)
            {
                return Target as DataAccessor;
            }
            else
            {
                return null;
            }
        }

        public FunctionCallBase? GetFunctionCall()
        {
            if (DataTermType == ExpressionDataTermType.FunctionCall)
            {
                return Target as FunctionCallBase;
            }
            else
            {
                return null;
            }
        }

        public string? GetNumber()
        {
            if (DataTermType == ExpressionDataTermType.Number)
            {
                return Target as string;
            }
            else
            {
                return null;
            }
        }

        public string? GetString()
        {
            if (DataTermType == ExpressionDataTermType.String)
            {
                return Target as string;
            }
            else
            {
                return null;
            }
        }
    }
}
