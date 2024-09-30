using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal class ArcDataSlot
    {
        public long Id { get; set; }

        public ArcDataDeclarator Declarator { get; set; }

        public long SlotId { get; set; }
    }
}
