using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Models.Builtin;

namespace Arc.Compiler.PackageGenerator.Models.Scope
{
    internal class ArcScopeTreeDataTypeNode(ArcTypeBase dataType) : ArcScopeTreeNodeBase
    {
        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.DataType;

        public bool IsInternal { get; set; } = dataType is ArcBaseType;

        public ArcTypeBase DataType { get; set; } = dataType;

        public override string SignatureAddend => "T" + DataType.FullName;

        public override IEnumerable<ArcSymbolBase> GetSymbols() => [DataType];

        public string ShortName => DataType.Identifier;

        public override string Name => DataType.Identifier;
    }
}
