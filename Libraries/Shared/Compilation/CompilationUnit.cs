using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Compilation
{
    public class CompilationUnit
    {
        public TokenizedFile[] TokenizedFiles { get; }

        public CompilationUnit(TokenizedFile[] tokenizedFiles)
        {
            TokenizedFiles = tokenizedFiles;
        }
    }
}
