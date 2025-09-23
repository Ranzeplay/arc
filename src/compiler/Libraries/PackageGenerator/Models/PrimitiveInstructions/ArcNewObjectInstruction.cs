using Arc.Compiler.PackageGenerator.Base;
using Arc.Compiler.PackageGenerator.Helpers;
using Arc.Compiler.PackageGenerator.Models.Generation;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Group;

namespace Arc.Compiler.PackageGenerator.Models.PrimitiveInstructions
{
    internal class ArcNewObjectInstruction(ArcScopeTreeDataTypeNode dataType, IEnumerable<ArcDataType> specializedGenericTypes, ArcScopeTreeLifecycleFunctionNode constructorFunction) : ArcPrimitiveInstructionBase
    {
        public override byte[] Opcode => [0x42];

        public ArcScopeTreeDataTypeNode DataType { get; set; } = dataType;
        
        public ArcScopeTreeLifecycleFunctionNode ConstructorFunction { get; set; } = constructorFunction;

        public IEnumerable<ArcDataType> SpecializedGenericTypes { get; set; } = specializedGenericTypes;

        public override ArcPartialGenerationResult Encode(ArcGenerationSource source)
        {
            var specializedGenericTypeId = SpecializedGenericTypes.Select(t => ArcDataTypeHelper.GetDataType(source, t)?.ResolvedType.TypeId ?? 0);

            return new ArcPartialGenerationResult
            {
                GeneratedData = [
                    .. Opcode,
                    .. BitConverter.GetBytes(DataType.Id),
                    
                    .. BitConverter.GetBytes(specializedGenericTypes.LongCount()),
                    .. specializedGenericTypeId.SelectMany(BitConverter.GetBytes),
                    
                    .. BitConverter.GetBytes(ConstructorFunction.Id)
                ],
            };
        }
    }
}
