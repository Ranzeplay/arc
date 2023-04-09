using Arc.Compiler.Shared.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.LexicalAnalysis
{
    public record TokenPosition(SourceFile SourceFile, int Position, int Length)
    {
        public string RawText
        {
            get
            {
                return SourceFile.Content.Substring(Position, Length);
            }
        }
    }
}
