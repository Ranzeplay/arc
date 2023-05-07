using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.CompilerException
{
    public class ExceptionDescription
    {
        public ExceptionLevel Level { get; }

        public ExceptionZone Zone { get; }

        public int Code { get; } = 0;

        public string Description { get; }

        public string FullCode { get => $"{Level}-{Zone}{Code:0000}"; }

        public override string ToString()
        {
            return $"{FullCode} : {Description}";
        }

        public ExceptionDescription(ExceptionLevel level, ExceptionZone zone, int code, string? description = null)
        {
            Level = level;
            Zone = zone;
            Code = code;
            Description = description ?? "No description.";
        }
    }
}
