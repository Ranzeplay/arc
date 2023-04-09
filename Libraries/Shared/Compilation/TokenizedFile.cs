using Arc.Compiler.Shared.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Compilation
{
    public record TokenizedFile(SourceFile SourceFile, Token[] Tokens)
    {
    }
}
