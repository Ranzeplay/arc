using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    class ArcScopeTreeAnnotationNode(ArcAnnotationDescriptor descriptor) : ArcScopeTreeNodeBase
    {
        public override string Name => Descriptor.Name;

        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Annotation;

        public ArcAnnotationDescriptor Descriptor { get; set; } = descriptor;

        public override string SignatureAddend => "A" + Descriptor.GroupShortName;

        public override IEnumerable<ArcSymbolBase> GetSymbols() => [Descriptor];
    }
}
