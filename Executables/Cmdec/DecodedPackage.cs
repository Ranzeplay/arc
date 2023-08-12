using Arc.Compiler.Shared.CommandGeneration;

namespace Arc.Cmdec
{
    internal record DecodedPackage(PackageMetadata Metadata, IEnumerable<DecodedCommand> Commands, IEnumerable<DecodedFunctionEntry> FunctionTable, IEnumerable<DecodedConstant> ConstantTable)
    {
    }
}