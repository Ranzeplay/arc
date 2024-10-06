using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    internal class ArcGenerationSource
    {
        public WeakReference<ArcGenerationContext> ContextReference { get; }

        public IEnumerable<ArcDataSlot> LocalDataSlots { get; } = [];

        public IEnumerable<ArcSymbolBase> AccessibleSymbols { get; } = [];
    }
}
