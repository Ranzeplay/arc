using Arc.Compiler.Shared.CommandGeneration;
using Arc.Compiler.Shared.Parsing.Components.Data;
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

        public IEnumerable<DataDeclarator> LocalData { get; }

        public IEnumerable<DataDeclarator> GlobalData { get; }

        public PackageMetadata PackageMetadata { get; }

        public long ConstantBeginIndex { get; }

        public GenerationSource(T component, IEnumerable<DataDeclarator> localData, IEnumerable<DataDeclarator> globalData, PackageMetadata packageMetadata, long constantBeginIndex = 0)
        {
            Component = component;
            LocalData = localData;
            GlobalData = globalData;
            PackageMetadata = packageMetadata;
            ConstantBeginIndex = constantBeginIndex;
        }

        public static GenerationSource<Ta> MigrateGenerationSource<Ta, To>(Ta actionBlock, GenerationSource<To> originalSource)
        {
            return new GenerationSource<Ta>(actionBlock, originalSource.LocalData, originalSource.GlobalData, originalSource.PackageMetadata, originalSource.ConstantBeginIndex);
        }
    }
}
