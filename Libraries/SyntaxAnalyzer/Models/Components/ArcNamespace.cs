using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    internal class ArcNamespace(ArcSourceCodeParser.Arc_namespaceContext source)
    {
        public ArcScopedIdentifier Identifier { get; set; } = new ArcScopedIdentifier(source.arc_scoped_identifier());

        public IEnumerable<ArcGroup> Groups { get; set; }

        public IEnumerable<ArcBlockIndependentFunction> Functions { get; set; } = source.arc_function_block().Select(f => new ArcBlockIndependentFunction(f));
    }
}
