using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.PackageGenerator.Models.Relocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    internal class ArcPartialGenerationResult
    {
        public IEnumerable<byte> GeneratedData { get; set; } = [];

        public IEnumerable<ArcRelocationTarget> RelocationTargets { get; set; } = [];

        public IEnumerable<ArcRelocationLabel> RelocationLabels { get; set; } = [];

        public IEnumerable<ArcDataSlot> DataSlots { get; set; } = [];
    }
}
