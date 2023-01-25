using Arc.Compiler.Shared.LexicalAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Compilation
{
    public class TokenizedFile
    {
        public SourceFile SourceFile { get; }

        public Token[] Tokens { get; }

        public TokenizedFile(SourceFile sourceFile, Token[] tokens)
        {
            SourceFile = sourceFile;
            Tokens = tokens;
        }
    }
}
