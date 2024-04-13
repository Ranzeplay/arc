using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Group
{
    internal class ArcGroupMemberBase
    {
        public ArcGroupMemberType MemberType { get; set; }

        public enum ArcGroupMemberType
        {
            Field,
            Method,
            Function
        }

        public ArcGroupField? Field { get; set; }

        public ArcGroupMethod? Method { get; set; }

        public ArcGroupFunction? Function { get; set; }
    }
}
