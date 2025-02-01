using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Builtin.Stdlib
{
    internal class ArcStdlibNamespace
    {
        public static ArcScopeTree GetTree()
        {
            var tree = new ArcScopeTree();
            tree.Root.AddChild(
                new ArcScopeTreeNamespaceNode("Arc")
                    .AddChildChained(
                        new ArcScopeTreeNamespaceNode("Std")
                    )
            );
            return tree;
        }
    }
}
