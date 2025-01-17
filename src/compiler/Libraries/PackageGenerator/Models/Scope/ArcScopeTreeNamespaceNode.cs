using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Descriptors;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeNamespaceNode(string name) : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Namespace;

        public string Name { get; set; } = name;

        public override string GetSignature() => "+N" + Name;

        public override IEnumerable<ArcSymbolBase> GetSymbols() => [new ArcNamespaceDescriptor() { Name = Name }];
    }
}
