using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Group
{
    internal class ArcGroup
    {
        public ArcSingleIdentifier Identifier { get; set; }

        public IEnumerable<ArcAnnotation> Annotations { get; set; }

        public IEnumerable<ArcGroupField> Fields { get; set; }

        public IEnumerable<ArcGroupFunction> Functions { get; set; }

        public ArcGroup(ArcSourceCodeParser.Arc_group_blockContext source)
        {
            Identifier = new(source.arc_single_identifier());
            Annotations = source.arc_annotation().Select(a => new ArcAnnotation(a));
            Fields = source.arc_wrapped_group_member().arc_group_member().ToList().FindAll(m => m.arc_group_field() != null).Select(f => new ArcGroupField(f.arc_group_field()));
            Functions = source.arc_wrapped_group_member().arc_group_member().ToList().FindAll(m => m.arc_group_function() != null).Select(f => new ArcGroupFunction(f.arc_group_function()));
        }
    }
}
