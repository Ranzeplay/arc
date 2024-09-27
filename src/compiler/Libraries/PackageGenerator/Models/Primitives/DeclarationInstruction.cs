using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Intermediate;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.PackageGenerator.Models.Primitives
{
    internal class DeclarationInstruction(ArcDataDeclarator dataDecl) : IArcPrimitiveInstruction
    {
        public byte[] Opcode => [0x01];

        public ArcDataDeclarator DataDeclarator { get; } = dataDecl;

        public ArcGenerationResult Encode<T>(ArcGenerationSource<T> source)
        {
            var id = new Random().NextInt64();
            var slot = new ArcDataSlot
            {
                Id = id,
                Declarator = DataDeclarator,
                SlotId = source.Symbols.Where(x => x.Value is ArcDataSlot).Count(),
            };

            return new ArcGenerationResult
            {
                GeneratedData = Opcode
                    .Concat([
                        DataDeclarator.DataType.MemoryStorageType == ArcMemoryStorageType.Value ? (byte)0x01 : (byte)0x00,
                        DataDeclarator.DataType.IsArray ? (byte)0x01 : (byte)0x00
                    ])
                    .Concat(BitConverter.GetBytes((long) 0)),
                Symbols = new() { { id, DataDeclarator } },
            };
        }
    }
}
