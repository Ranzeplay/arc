using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    internal class ArcGroupFieldDescriptor
    {
        public long Index { get; set; }

        public string RawFullName { get; set; }

        public ArcDataTypeDescriptor DataType { get; set; }

        public IEnumerable<ArcAnnotationDescriptor> Annotations { get; set; }

        public ArcAccessibility Accessibility { get; set; }
    }
}
