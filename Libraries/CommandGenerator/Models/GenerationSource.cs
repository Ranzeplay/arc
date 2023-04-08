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
    public class GenerationSource<T>
    {
        public T Component { get; }

        public List<DataDeclarator> LocalData { get; }

        public List<DataDeclarator> GlobalData { get; }

        public List<FunctionDeclarator> AvailableFunctions { get; }

        public PackageMetadata PackageMetadata { get; }

        public long ConstantBeginIndex { get; }

        public GenerationSource(T component, List<DataDeclarator> localData, List<DataDeclarator> globalData, List<FunctionDeclarator> availableFunctions, PackageMetadata packageMetadata, long constantBeginIndex = 0)
        {
            Component = component;
            LocalData = localData;
            GlobalData = globalData;
            AvailableFunctions = availableFunctions;
            PackageMetadata = packageMetadata;
            ConstantBeginIndex = constantBeginIndex;
        }

        public static GenerationSource<Tc> MigrateGenerationSource<Tc, To>(Tc component, GenerationSource<To> originalSource)
        {
            return new GenerationSource<Tc>(component, originalSource.LocalData, originalSource.GlobalData, originalSource.AvailableFunctions, originalSource.PackageMetadata, originalSource.ConstantBeginIndex);
        }
    }
}
