using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType
{
    public class ArcComplexDataType(ArcSourceCodeParser.Arc_flexible_identifierContext context) : IArcTraceable<ArcSourceCodeParser.Arc_flexible_identifierContext>
    {
        public ArcFlexibleIdentifier Identifier { get; set; } = new(context);

        public ArcSourceCodeParser.Arc_flexible_identifierContext Context { get; } = context;
    }
}
