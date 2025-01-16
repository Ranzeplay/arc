namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public abstract class ArcScopeTreeNodeBase
    {
        public abstract ArcScopeTreeNodeType NodeType { get; }

        public ICollection<ArcScopeTreeNodeBase> Children { get; set; } = [];

        public virtual ArcScopeTreeNodeBase Parent { get; set; }

        public ArcScopeTreeNodeBase AddChild(ArcScopeTreeNodeBase child)
        {
            child.Parent = this;
            Children.Add(child);
            return child;
        }

        public ArcScopeTreeNodeBase AddChildren(IEnumerable<ArcScopeTreeNodeBase> children)
        {
            foreach (var child in children)
            {
                AddChild(child);
            }
            return this;
        }
    }
}
