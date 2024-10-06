using Arc.Compiler.PackageGenerator.Models.Descriptors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.PackageGenerator.Models
{
    internal class ArcEncodeResult
    {
        public IEnumerable<byte> Data { get; set; }

        public IEnumerable<ArcRelocationDescriptor> RelocationDescriptors { get; set; }
    }
}
