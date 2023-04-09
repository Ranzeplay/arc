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
    public record GenerationSource<T>(T Component, List<DataDeclarator> LocalData, List<DataDeclarator> GlobalData, List<FunctionDeclarator> AvailableFunctions, PackageMetadata PackageMetadata, long ConstantBeginIndex = 0)
    {
        public static GenerationSource<Tc> MigrateGenerationSource<Tc, To>(Tc component, GenerationSource<To> originalSource)
        {
            return new GenerationSource<Tc>(component, originalSource.LocalData, originalSource.GlobalData, originalSource.AvailableFunctions, originalSource.PackageMetadata, originalSource.ConstantBeginIndex);
        }
    }
}
