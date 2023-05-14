using Arc.Compiler.Shared.Parsing.Components.Function;

namespace Arc.Compiler.Shared.CommandGeneration.Relocation
{
    public class RelocationDescriptor
    {
        public long CommandLocation { get; set; }

        public RelocationType RelocationType { get; set; }

        public long RelativeLocation { get; set; }

        public int ConstantId { get; set; }

        private FunctionDeclarator? Function { get; set; }

        public static RelocationDescriptor NewRelativeLocation(long commandLocation, long relativeLocation)
        {
            return new(RelocationType.RelativeLocation, commandLocation, relativeLocation, 0, null);
        }

        public static RelocationDescriptor NewConstant(long commandLocation, int constantId)
        {
            return new(RelocationType.Constant, commandLocation, 0, constantId, null);
        }

        public RelocationDescriptor NewFunction(long commandLocation, FunctionDeclarator functionDeclarator)
        {
            return new(RelocationType.Function, commandLocation, 0, 0, functionDeclarator);
        }

        private RelocationDescriptor(RelocationType relocationType, long commandLocation, long relativeLocation, int constantId, FunctionDeclarator? function)
        {
            CommandLocation = commandLocation;
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
