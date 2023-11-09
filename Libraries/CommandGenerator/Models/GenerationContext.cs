using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.CommandGeneration.Relocation;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.CommandGenerator.Models
{
    public record GenerationContext<T>(T Component,
                                       List<DataDeclarator> LocalData,
                                       List<DataDeclarator> GlobalData,
                                       List<FunctionDeclarator> AvailableFunctions,
                                       List<GeneratedConstant> GeneratedConstants,
                                       List<RelocationReference> RelocationReferences,
                                       PackageMetadata PackageMetadata,
                                       long ConstantBeginIndex = 0)
    {
        public GenerationContext<Tc> TransferToNewComponent<Tc>(Tc component)
        {
            return new GenerationContext<Tc>(component, LocalData, GlobalData, AvailableFunctions, GeneratedConstants, RelocationReferences, PackageMetadata, ConstantBeginIndex);
        }
    }
}
