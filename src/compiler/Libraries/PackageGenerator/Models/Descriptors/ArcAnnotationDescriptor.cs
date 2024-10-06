using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    internal class ArcAnnotationDescriptor : ArcSymbolBase
    {
        public ArcGroupDescriptor TargetGroup { get; set; }
    }
}
