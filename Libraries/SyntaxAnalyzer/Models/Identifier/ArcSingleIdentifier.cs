using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Identifier
{
    internal class ArcSingleIdentifier : IIdentifier
    {
        public string Name { get => Names.ElementAt(0); set => Names = [value]; }
    }
}
