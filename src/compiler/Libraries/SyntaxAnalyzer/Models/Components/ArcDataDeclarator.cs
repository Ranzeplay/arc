using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    public class ArcDataDeclarator : IArcTraceable<ArcSourceCodeParser.Arc_data_declaratorContext>, IArcLocatable
    {
        public ArcMemoryStorageType MemoryStorageType { get; set; }

        public ArcMutability Mutability { get; set; }

        public ArcSingleIdentifier Identifier { get; set; }

        public ArcDataType DataType { get; set; }

        public ArcSourceCodeParser.Arc_data_declaratorContext Context => RegularContext!;

        public ArcSourceCodeParser.Arc_data_declaratorContext? RegularContext { get; }
        public ArcSourceCodeParser.Arc_self_data_declaratorContext? SelfContext { get; }

        public ArcDataDeclarator(ArcSourceCodeParser.Arc_data_declaratorContext context)
        {
            MemoryStorageType = ArcMemoryStorageTypeUtils.FromToken(context.arc_data_type().arc_mem_store_type());
            Mutability = ArcMutabilityUtils.FromToken(context.arc_mutability());
            Identifier = new(context.arc_single_identifier());
            DataType = new ArcDataType(context.arc_data_type());
            RegularContext = context;
        }

        public ArcDataDeclarator(ArcSourceCodeParser.Arc_self_data_declaratorContext context)
        {
            MemoryStorageType = ArcMemoryStorageTypeUtils.FromToken(context.arc_data_type().arc_mem_store_type());
            Mutability = ArcMutabilityUtils.FromToken(context.arc_mutability());
            Identifier = new(context.arc_self_wrapper());
            DataType = new ArcDataType(context.arc_data_type());
            SelfContext = context;
        }

        public string GetSignature() => $"{DataType.GetSignature()}@{Identifier.Name}";
    }
}
