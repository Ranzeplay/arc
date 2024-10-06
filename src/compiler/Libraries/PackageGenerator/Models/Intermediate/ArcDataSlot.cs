using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal class ArcDataSlot : ArcSymbolBase
    {
        public ArcDataDeclarator Declarator { get; set; }

        public long SlotId { get; set; }
    }
}
