using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    internal class ArcGenerationScope : ArcSymbolBase
    {
        public WeakReference<ArcGenerationScope> Parent { get; set; }

        public IEnumerable<ArcGenerationScope> Children { get; set; } = [];

        public IEnumerable<ArcFunctionDescriptor> FunctionDescriptors { get; set; } = [];

        public IEnumerable<ArcGroupDescriptor> GroupDescriptors { get; set; } = [];

        public IEnumerable<ArcRelocationTarget> RelocationTargets { get; set; } = [];
    }
}
