using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Builtin;
using Arc.Compiler.PackageGenerator.Models.Intermediate;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Scope;

public class ArcScopeTreeDataTypeNode(ArcDataTypeType typeType, ArcTypeBase dataType, string shortName) : ArcScopeTreeNodeBase, IArcDataTypeProxy
{
    public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.DataType;

    public bool IsInternal { get; set; } = dataType is ArcBaseType;

    public ArcDataTypeType ArcDataTypeType { get; set; } = typeType;

    public ArcTypeBase DataType { get; set; } = dataType;

    public string ShortName { get; set; } = shortName;

    public ArcScopeTreeGroupNode? ComplexTypeGroup { get; set; }

    public ArcScopeTreeEnumNode? EnumType { get; set; }

    public override string SignatureAddend => "T" + ShortName;

    public override string Name => DataType.Identifier;

    public string TypeSignature => Signature;

    public ulong ProxyTypeId => Id;

    public ulong ActualTypeId => Id;

    public ArcTypeBase ResolvedType => DataType;

    public static ArcScopeTreeDataTypeNode Placeholder() => new(ArcDataTypeType.Primitive, ArcBaseType.Placeholder(), "INVALID");
}
