using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Utils
{
    public class SectionBuildResult<TSection>
    {
        public TSection? Section { get; }

        public int Length { get; }

        public SectionBuildResult(TSection? section, int length)
        {
            Section = section;
            Length = length;
        }
    }
}
