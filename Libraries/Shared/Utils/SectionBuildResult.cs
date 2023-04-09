using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Utils
{
    public record SectionBuildResult<TSection>(TSection Section, int Length)
    {
    }
}
