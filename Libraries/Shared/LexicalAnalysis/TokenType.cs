using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.LexicalAnalysis
{
    public enum TokenType
    {
        Identifier,
        Number,
        String,
        Keyword,
        Operator,
        Container,
        Whitespace,
        Comment,
        Semicolon
    }
}
