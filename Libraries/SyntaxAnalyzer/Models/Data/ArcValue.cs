using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data
{
    internal class ArcValue : ArcExpressionTermBase
    {
        public ArcValueType ValueType { get; set; }

        public enum ArcValueType
        {
            Instant,
            Reference,
            None
        }

        public ArcInstantValueBase? InstantValue { get; set; }

        public ArcReferenceValue? ReferenceValue { get; set; }
    }
}
