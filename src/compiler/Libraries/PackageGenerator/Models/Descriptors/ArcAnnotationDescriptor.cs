using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Group;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    public class ArcAnnotationDescriptor : ArcSymbolBase
    {
        public ArcGroupDescriptor TargetGroup { get; set; }
    }
}
