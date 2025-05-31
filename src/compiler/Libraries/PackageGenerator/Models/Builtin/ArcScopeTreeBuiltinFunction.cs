using Arc.Compiler.PackageGenerator.Models.Scope;

namespace Arc.Compiler.PackageGenerator.Models.Builtin
{
    internal class ArcScopeTreeBuiltinFunction(string name) : ArcScopeTreeFunctionNodeBase
    {
        public override string Name => name;

        public override ArcScopeTreeNodeType NodeType => ArcScopeTreeNodeType.IndividualFunction;

        public override string SignatureAddend => "F" + name;
    }
}
