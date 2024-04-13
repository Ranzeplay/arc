using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Function;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    internal class ArcAnnotation(ArcSourceCodeParser.Arc_annotationContext source)
    {
        public ArcScopedIdentifier Identifier { get; set; } = new ArcScopedIdentifier(source.arc_scoped_identifier());

        public IEnumerable<ArcFunctionCallArgument> CallArguments { get; set; } = source.arc_call_args().arc_expression().Select(arg => new ArcFunctionCallArgument(arg));
    }
}
