using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data
{
    internal class ArcReferenceValue
    {
        public ReferenceValueType ValueType { get; set; }

        public enum ReferenceValueType
        {
            LocalVariable,
            FunctionReturnValue
        }

        public ArcSingleIdentifier? VariableIdentifier { get; set; }

        public ArcFunctionCall? FunctionCall { get; set; }
    }
}
