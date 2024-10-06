using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    internal class ArcGenerationScope : ArcSymbolBase, IArcLocatable
    {
        public WeakReference<ArcGenerationScope> Parent { get; set; }

        public IEnumerable<ArcGenerationScope> Children { get; set; } = [];

        public IEnumerable<ArcFunctionDescriptor> FunctionDescriptors { get; set; } = [];

        public IEnumerable<ArcGroupDescriptor> GroupDescriptors { get; set; } = [];

        public IEnumerable<ArcRelocationDescriptor> RelocationDescriptors { get; set; } = [];
    }
}
