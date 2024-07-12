using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    internal class ArcDataDeclarator(ArcSourceCodeParser.Arc_data_declaratorContext context)
    {
        public ArcMemoryStorageType MemoryStorageType { get; set; } = ArcMemoryStorageTypeUtils.FromToken(context.arc_data_type().arc_mem_store_type());

        public ArcMutability Mutability { get; set; } = ArcMutabilityUtils.FromToken(context.arc_mutability());

        public ArcSingleIdentifier Identifier { get; set; } = new(context.arc_single_identifier());

        public ArcDataType DataType { get; set; } = new ArcDataType(context.arc_data_type());
    }
}
