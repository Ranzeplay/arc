using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Builtin.Stdlib
{
    internal class ArcStdlib
    {
        public static ArcScopeTree GetTree()
        {
            var tree = ArcStdlibNamespace.GetTree();
            tree.Root.AddChild(
                ArcStdlibConsole.GetNamespace()
            );

            return tree;
        }
    }
}
