using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;

namespace Arc.Compiler.PackageGenerator.Models.Descriptors
{
    public class ArcDataTypeDescriptor
    {
        public long TypeId { get; set; }

        public bool IsArray { get; set; }

        public bool AllowNone { get; set; }

        public ArcMutability Mutability { get; set; }

        public ArcMemoryStorageType MemoryStorageType { get; set; }
    }
}