using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Ginkgo.Instruction;
using Arc.Compiler.PackageGenerator.Models.Descriptors;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Metadata;

public class ArcPackage
{
    public ArcPackageDescriptor PackageDescriptor { get; set; }
    
    public ArcSymbolTable SymbolTable { get; set; }
    
    public List<ArcGinkgoInstructionBase> Instructions { get; set; }
}