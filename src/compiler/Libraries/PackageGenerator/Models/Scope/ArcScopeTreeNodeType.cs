namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    public enum ArcScopeTreeNodeType
    {
        Root,
        Namespace,
        Group,
        GroupFunction,
        GroupConstructor,
        GroupDestructor,
        GroupField,
        IndividualFunction,
        FunctionData,
        DataType,
        GenericType,
        Annotation,
        Enum,
        EnumMember
    }
}
