using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    internal class ArcDataDeclarator
    {
        public ArcParameterType ParameterType { get; set; }

        public ArcReassignability Reassignability { get; set; }

        public ArcSingleIdentifier Identifier { get; set; }

        public ArcDataType DataType { get; set; }
    }
}
