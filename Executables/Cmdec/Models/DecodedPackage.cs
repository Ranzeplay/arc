using Arc.Compiler.Shared.CommandGeneration;

namespace Arc.Cmdec.Models
{
    internal record DecodedPackage(PackageMetadata Metadata, IEnumerable<DecodedCommand> Commands, IEnumerable<DecodedFunctionEntry> FunctionTable, IEnumerable<DecodedConstant> ConstantTable)
    {
    }
}