using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.Parsing.Components.Data;
using Arc.Compiler.Shared.Parsing.Components.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.CompilerCommandGenerator.Models
{
    public record GenerationContext<T>(T Component,
                                       List<DataDeclarator> LocalData,
                                       List<DataDeclarator> GlobalData,
                                       List<FunctionDeclarator> AvailableFunctions,
                                       PackageMetadata PackageMetadata,
                                       long ConstantBeginIndex = 0)
    {
        public GenerationContext<Tc> TransferToNewComponent<Tc>(Tc component)
        {
            return new GenerationContext<Tc>(component, LocalData, GlobalData, AvailableFunctions, PackageMetadata, ConstantBeginIndex);
        }
    }
}
