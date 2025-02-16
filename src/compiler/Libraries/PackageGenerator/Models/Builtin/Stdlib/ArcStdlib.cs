using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Builtin.Stdlib
{
    internal class ArcStdlib
    {
        public static ArcScopeTree GetTree()
        {
            var treeResult = ArcStdlibNamespace.GetTree();
            var tree = treeResult.Item1;
            var stdNamespace = treeResult.Item2;

            stdNamespace
                .AddChildChained(ArcStdlibConsole.GetNamespace())
                .AddChildChained(ArcStdlibCompilation.GetNamespace());

            return tree;
        }
    }
}
