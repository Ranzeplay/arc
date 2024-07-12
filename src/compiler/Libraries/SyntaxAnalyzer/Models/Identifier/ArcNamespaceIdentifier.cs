using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    internal class ArcNamespaceIdentifier
    {
        public IEnumerable<string>? Namespace { get; set; }

        public ArcNamespaceIdentifier(ArcSourceCodeParser.Arc_namespace_identifierContext context)
        {
            Namespace = context.IDENTIFIER().Select(i => i.GetText());
        }
    }
}
