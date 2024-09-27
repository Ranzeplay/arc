using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal class ArcDataSlot
    {
        public long Id { get; set; }

        public ArcDataDeclarator Declarator { get; set; }

        public long SlotId { get; set; }
    }
}
