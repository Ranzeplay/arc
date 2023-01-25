using Arc.Compiler.Shared.Compilation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.LexicalAnalysis
{
    public class TokenPosition
    {
        public SourceFile SourceFile { get; }

        public int Position { get; }

        public int Length { get; }

        public TokenPosition(ref SourceFile sourceFile, int position, int length)
        {
            SourceFile = sourceFile;
            Position = position;
            Length = length;
        }

        public string getRaw()
        {
            return SourceFile.Content.Substring(Position, Length);
        }
    }
}
