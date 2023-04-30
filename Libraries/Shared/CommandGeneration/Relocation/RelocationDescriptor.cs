using Arc.Compiler.Shared.Parsing.Components.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.CommandGeneration.Relocation
{
    public class RelocationDescriptor
    {
        public long CommandLocation { get; set; }

        public RelocationType RelocationType { get; set; }

        public long RelativeLocation { get; set; }

        public int ConstantId { get; set; }

        private FunctionDeclarator? Function { get; set; }

        public RelocationDescriptor(long commandLocation, long relativeLocation)
        {
            CommandLocation = commandLocation;

            RelativeLocation = relativeLocation;
            ConstantId = 0;
            Function = null;
        }

        public RelocationDescriptor(long commandLocation, int constantId)
        {
            CommandLocation = commandLocation;

            RelativeLocation = 0;
            ConstantId = constantId;
            Function = null;
        }

        public RelocationDescriptor(long commandLocation, FunctionDeclarator functionDeclarator)
        {
            CommandLocation = commandLocation;

            RelativeLocation = 0;
            ConstantId = 0;
            Function = functionDeclarator;
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
