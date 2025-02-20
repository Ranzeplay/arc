using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using System.Data;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Components
{
    public enum ArcMemoryStorageType
    {
        Reference = 0x00,
        Value = 0x01,
        Invalid = 0xFF
    }

    public static class ArcMemoryStorageTypeUtils
    {
        public static ArcMemoryStorageType FromToken(ArcSourceCodeParser.Arc_mem_store_typeContext context)
        {
            if (context.KW_REFERENCE() != null) return ArcMemoryStorageType.Reference;
            if (context.KW_VALUE() != null) return ArcMemoryStorageType.Value;
            throw new InvalidConstraintException("Invalid memory storage type");
        }
    }
}
