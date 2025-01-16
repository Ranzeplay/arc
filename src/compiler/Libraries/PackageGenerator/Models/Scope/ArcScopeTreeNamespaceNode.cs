namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeNamespaceNode(string name) : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.Namespace;

        public string Name { get; set; } = name;
    }
}
