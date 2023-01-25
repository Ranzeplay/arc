using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.LexicalAnalysis
{
    public class OperatorToken
    {
        public OperatorTokenType Type { get; }

        public CalculationOperatorType CalculationOperator { get; }

        public RelationOperatorType RelationOperator { get; }

        public LogicalOperatorType LogicalOperator { get; }

        public OperatorToken(CalculationOperatorType calculationOperator)
        {
            Type = OperatorTokenType.Calculation;
            CalculationOperator = calculationOperator;
        }

        public OperatorToken(RelationOperatorType relationOperator)
        {
            Type = OperatorTokenType.Relation;
            RelationOperator = relationOperator;
        }

        public OperatorToken(LogicalOperatorType logicalOperator)
        {
            Type = OperatorTokenType.Logical;
            LogicalOperator = logicalOperator;
        }

        public OperatorToken(OperatorTokenType type)
        {
            Type = type;
        }
    }

    public enum OperatorTokenType
    {
        Invalid,
        // Root type
        Calculation,
        Relation,
        Logical,
        // Absolute type
        Assignment, // =
        Scope,      // ::
        Comma,      // ,
        Dot,        // .
    }

    public enum CalculationOperatorType
    {
        Invalid,
        Addition,       // +
        Subtraction,    // -
        Multiply,       // *
        Division,       // /
        Modulo,         // %,
    }

    public enum RelationOperatorType
    {
        Invalid,
        Greater,            // >
        GreaterOrEqual,     // >=
        Less,               // <
        LessOrEqual,        // <=
        NotEqual,           // <>
        Equal,              // ==,
    }

    public enum LogicalOperatorType
    {
        Invalid,
        Not, // !
        And, // &&
        Or,  // ||
    }
}
