using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Builtin.Stdlib
{
    class ArcStdlibCompilation
    {
        public static ArcScopeTreeNamespaceNode GetNamespace()
        {
            return (ArcScopeTreeNamespaceNode)
                new ArcScopeTreeNamespaceNode("Compilation")
                    .AddChildren(ArcAnnotationHelper.GenerateEmptyAnnotation("Entrypoint", 0xb1, 0xb11))
                    .AddChildren(ArcAnnotationHelper.GenerateEmptyAnnotation("Export", 0xb2, 0xb21))
                    .AddChildren(ArcAnnotationHelper.GenerateEmptyAnnotation("Getter", 0xb3, 0xb31))
                    .AddChildren(ArcAnnotationHelper.GenerateEmptyAnnotation("Setter", 0xb4, 0xb41))
                    .AddChildren(ArcAnnotationHelper.GenerateEmptyAnnotation("Constructor", 0xb5, 0xb51))
                    .AddChildren(ArcAnnotationHelper.GenerateEmptyAnnotation("Destructor", 0xb6, 0xb61));
        }
    }
}
