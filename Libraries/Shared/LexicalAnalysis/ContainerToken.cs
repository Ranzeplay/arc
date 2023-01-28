using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.LexicalAnalysis
{
    public enum ContainerToken
    {
        Invalid,
        Brace,       // {
        AntiBrace,   // }
        Bracket,     // (
        AntiBracket, // )
        Index,       // [
        AntiIndex    // ]
    }
}
