using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    internal class ArcAnnotationDescriptor
    {
        public long Id { get; set; }

        public ArcFunctionDescriptor TargetAnnotationFunction { get; set; }
    }
}
