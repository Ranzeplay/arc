using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Shared.CommandGeneration.Relocation
{
    public class RelocationTarget
    {
        public long CommandLocation { get; set; }

        public int PlaceholderOffset { get; set; }

        public RelocationType RelocationType { get; set; }

        public long RelativeLocation { get; set; }

        public int ConstantId { get; set; }

        private FunctionDeclarator? Function { get; set; }

        public static RelocationTarget NewRelativeLocation(long commandLocation, int placeholderOffset, long relativeLocation)
        {
            return new(RelocationType.RelativeLocation, commandLocation, placeholderOffset, relativeLocation, 0, null);
        }

        public static RelocationTarget NewConstant(long commandLocation, int placeholderOffset, int constantId)
        {
            return new(RelocationType.Constant, commandLocation, placeholderOffset, 0, constantId, null);
        }

        public static RelocationTarget NewFunction(long commandLocation, int placeHolderOffset, FunctionDeclarator functionDeclarator)
        {
            return new(RelocationType.Function, commandLocation, placeHolderOffset, 0, 0, functionDeclarator);
        }

        private RelocationTarget(RelocationType relocationType, long commandLocation, int placeHolderOffset, long relativeLocation, int constantId, FunctionDeclarator? function)
        {
            CommandLocation = commandLocation;
            PlaceholderOffset = placeHolderOffset;
            RelocationType = relocationType;
            RelativeLocation = relativeLocation;
            ConstantId = constantId;
            Function = function;
        }

        public long? GetRelativeLocation()
        {
            return RelocationType == RelocationType.RelativeLocation ? RelativeLocation : null;
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
