using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Shared.CommandGeneration.Relocation
{
    public class RelocationTarget
    {
        public long CommandLocation { get; set; }

        public int PlaceholderOffset { get; set; }

        public RelocationType RelocationType { get; set; }

        public RelativeRelocator? Relative { get; set; }

        public int ConstantId { get; set; }

        private FunctionDeclarator? Function { get; set; }

        public static RelocationTarget NewRelativeLocation(long commandLocation, int placeholderOffset, RelativeRelocator relative)
        {
            return new(RelocationType.RelativeLocation, commandLocation, placeholderOffset, relative, 0, null);
        }

        public static RelocationTarget NewConstant(long commandLocation, int placeholderOffset, int constantId)
        {
            return new(RelocationType.Constant, commandLocation, placeholderOffset, null, constantId, null);
        }

        public static RelocationTarget NewFunction(long commandLocation, int placeHolderOffset, FunctionDeclarator functionDeclarator)
        {
            return new(RelocationType.Function, commandLocation, placeHolderOffset, null, 0, functionDeclarator);
        }

        private RelocationTarget(RelocationType relocationType, long commandLocation, int placeHolderOffset, RelativeRelocator? relative, int constantId, FunctionDeclarator? function)
        {
            CommandLocation = commandLocation;
            PlaceholderOffset = placeHolderOffset;
            RelocationType = relocationType;
            Relative = relative;
            ConstantId = constantId;
            Function = function;
        }

        public RelativeRelocator? GetRelativeLocation()
        {
            return RelocationType == RelocationType.RelativeLocation ? Relative : null;
        }

        public int? GetConstantId()
        {
            return RelocationType == RelocationType.Constant ? ConstantId : null;
        }

        public FunctionDeclarator? GetFunctionDeclarator()
        {
            return RelocationType == RelocationType.Function ? Function : null;
        }


    }
}
