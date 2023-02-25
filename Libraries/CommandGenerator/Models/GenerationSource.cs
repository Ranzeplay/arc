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
        public T ActionBlock { get; }

        public IEnumerable<DataDeclarator> LocalData { get; }

        public IEnumerable<DataDeclarator> GlobalData { get; }

        public PackageMetadata PackageMetadata { get; }

        public GenerationSource(T actionBlock, IEnumerable<DataDeclarator> localData, IEnumerable<DataDeclarator> globalData, PackageMetadata packageMetadata)
        {
            ActionBlock = actionBlock;
            LocalData = localData;
            GlobalData = globalData;
            PackageMetadata = packageMetadata;
        }
    }
}
