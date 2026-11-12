using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Expression;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Scope;

public abstract class ArcScopeTreeFunctionNodeBase : ArcScopeTreeNodeBase, IArcAccessible
{
    public IEnumerable<ArcParameterDescriptor> Parameters { get; set; } = [];

    public ArcDataDeclarationDescriptor ReturnValueType { get; set; }

    public Dictionary<Models.Scope.ArcScopeTreeAnnotationNode, IEnumerable<ArcExpression>> Annotations { get; set; } = [];

    public ArcAccessibility Accessibility { get; set; }

    public IEnumerable<ArcScopeTreeGenericTypeNode> GenericTypes => GetChildren<ArcScopeTreeGenericTypeNode>();

    public long EntrypointPos { get; set; }

    public long BlockLength { get; set; }

    public long DataCount { get; set; }

    public IEnumerable<ArcScopeTreeLambdaNode> LambdaExpressions => GetChildren<ArcScopeTreeLambdaNode>();

    public ArcPartialGenerationResult GenerationResult { get; set; }

    public bool IsSelfFunction =>
        Parameters.Any() && Parameters.ElementAt(0).DataType.SyntaxTree.SelfContext != null;
}
