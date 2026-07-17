namespace Arc.Compiler.PackageGenerator.Ginkgo.Scope;

public enum ArcScopeTreeNodeType
{
    Root,
    Namespace,
    Group,
    GroupFunction,
    GroupLifecycleFunction,
    GroupField,
    IndividualFunction,
    FunctionData,
    DataType,
    GenericType,
    Annotation,
    Enum,
    EnumMember,
    Lambda
}