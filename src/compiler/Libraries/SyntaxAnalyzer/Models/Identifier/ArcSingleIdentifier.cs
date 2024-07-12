using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    internal class ArcSingleIdentifier
    {
        public string Name { get; set; }

        public ArcSingleIdentifier(ArcSourceCodeParser.Arc_single_identifierContext context)
        {
            Name = context.IDENTIFIER().GetText();
        }
    }
}
