using System.Text.RegularExpressions;

namespace Arc.Compiler.Shared.LexicalAnalysis
{
    public partial class TokenConstants
    {
        public static readonly Dictionary<KeywordToken, string> KeywordMappings = new()
        {
            { KeywordToken.Declare,   "decl" },
            { KeywordToken.Number,    "number" },
            { KeywordToken.Char,      "char" },
            { KeywordToken.String,    "string" },
            { KeywordToken.Bool,      "bool" },
            { KeywordToken.Var,       "var" },
            { KeywordToken.Const,     "const" },
            { KeywordToken.Export,    "export" },
            { KeywordToken.Func,      "func" },
            { KeywordToken.If,        "if" },
            { KeywordToken.ElseIf,    "elif" },
            { KeywordToken.Else,      "else" },
            { KeywordToken.While,     "while" },
            { KeywordToken.Loop,      "loop" },
            { KeywordToken.Continue,  "continue" },
            { KeywordToken.Break,     "break" },
            { KeywordToken.Return,    "return" },
            { KeywordToken.Call,      "call" },
            { KeywordToken.Link,      "link" },
            { KeywordToken.None,      "none" },
            { KeywordToken.Any,       "any" },
            { KeywordToken.True,      "true" },
            { KeywordToken.False,     "false" },
            { KeywordToken.Group,     "group" },
            { KeywordToken.Implement, "impl" },
            { KeywordToken.Get,       "get" },
            { KeywordToken.Set,       "set" },
            { KeywordToken.Method,    "method" },
            { KeywordToken.Field,     "field" },
            { KeywordToken.Default,   "default" },
            { KeywordToken.Self,      "self" }
        };

        public static readonly Dictionary<ContainerToken, string> ContainerMappings = new()
        {
            { ContainerToken.Brace,         "{" },
            { ContainerToken.AntiBrace,     "}" },
            { ContainerToken.Bracket,       "(" },
            { ContainerToken.AntiBracket,   ")" },
            { ContainerToken.Index,         "["},
            { ContainerToken.AntiIndex,     "]" },
        };

        public static readonly Dictionary<OperatorTokenType, string> RootOperatorMappings = new()
        {
            { OperatorTokenType.Assignment, "=" },
            { OperatorTokenType.Scope,      "::" },
            { OperatorTokenType.Comma,      "," },
            { OperatorTokenType.Dot,        "." },
        };

        public static readonly Dictionary<CalculationOperatorType, string> CalculationOperatorMappings = new()
        {
            { CalculationOperatorType.Addition,       "+" },
            { CalculationOperatorType.Subtraction,    "-" },
            { CalculationOperatorType.Multiply,       "*" },
            { CalculationOperatorType.Division,       "/" },
            { CalculationOperatorType.Modulo,         "%" },
        };

        public static readonly Dictionary<RelationOperatorType, string> RelationOperatorMappings = new()
        {
            { RelationOperatorType.GreaterOrEqual, ">=" },
            { RelationOperatorType.LessOrEqual,    "<=" },
            { RelationOperatorType.NotEqual,       "<>" },
            { RelationOperatorType.Equal,          "==" },
            { RelationOperatorType.Greater,        ">" },
            { RelationOperatorType.Less,           "<" },
        };

        public static readonly Dictionary<LogicalOperatorType, string> LogicalOperatorMappings = new()
        {
            { LogicalOperatorType.And, "&&" },
            { LogicalOperatorType.Or,  "||" },
            { LogicalOperatorType.Not, "!" },
        };

        public static readonly Dictionary<CalculationOperatorType, int> CalculationOperatorPriority = new()
        {
            { CalculationOperatorType.Addition,       1 },
            { CalculationOperatorType.Subtraction,    1 },
            { CalculationOperatorType.Multiply,       2 },
            { CalculationOperatorType.Division,       2 },
            { CalculationOperatorType.Modulo,         2 },
        };

        public static readonly Dictionary<OperatorTokenType, int> OperatorPriority = new()
        {
            { OperatorTokenType.Calculation, 3 },
            { OperatorTokenType.Relation, 2 },
            { OperatorTokenType.Logical, 1 },
        };

        public static readonly char SemicolonToken = ';';

        public static readonly string CommentLeadingSequence = "//";

        public static readonly char QuoteContainer = '\"';

        // Just follow the suggestion from compiler
        [GeneratedRegex("^([_a-zA-Z][_a-zA-Z0-9]{0,80})")]
        private static partial Regex IdentifierRegexGen();
        public static readonly Regex IdentifierRegex = IdentifierRegexGen();

        [GeneratedRegex("^([+-]?\\d+(\\.?\\d+)?)")]
        private static partial Regex NumberRegexGen();
        public static readonly Regex NumberRegex = NumberRegexGen();
    }
}
