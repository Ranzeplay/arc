using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    internal abstract class IIdentifier
    {
        protected IEnumerable<string> Names { get; set; } = [];
    }
}
