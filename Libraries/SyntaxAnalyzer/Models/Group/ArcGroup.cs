using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Group
{
    internal class ArcGroup
    {
        public ArcSingleIdentifier Identifier { get; set; }

        public ICollection<ArcAnnotation> Annotations { get; set; }

        public ICollection<ArcGroupMemberBase> Members { get; set; }
    }
}
