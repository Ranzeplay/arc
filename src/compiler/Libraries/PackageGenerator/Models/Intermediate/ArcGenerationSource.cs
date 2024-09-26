using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.PackageGenerator.Models.Intermediate
{
    internal class ArcGenerationSource<T>
    {
        public T Value { get; set; }

        public Dictionary<long, object> Symbols { get; set; } = [];
    }
}
