using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Group
{
    public class ArcGroup : IArcTraceable<ArcSourceCodeParser.Arc_group_blockContext>, IArcLocatable
    {
        public ArcSingleIdentifier Identifier { get; set; }

        public IEnumerable<ArcAnnotation> Annotations { get; set; }

        public IEnumerable<ArcGroupField> Fields { get; set; }

        public IEnumerable<ArcGroupFunction> Functions { get; set; }

        public IEnumerable<ArcSingleIdentifier> GenericTypes { get; set; }

        public ArcSourceCodeParser.Arc_group_blockContext Context { get; }

        public ArcGroup(ArcSourceCodeParser.Arc_group_blockContext context)
        {
            Identifier = new(context.arc_single_identifier());
            Annotations = context.arc_annotation().Select(a => new ArcAnnotation(a));
            Fields = context.arc_wrapped_group_member().arc_group_member().ToList().FindAll(m => m.arc_group_field() != null).Select(f => new ArcGroupField(f.arc_group_field()));
            Functions = context.arc_wrapped_group_member().arc_group_member().ToList().FindAll(m => m.arc_group_function() != null).Select(f => new ArcGroupFunction(f.arc_group_function()));
            GenericTypes = context.arc_generic_declaration_wrapper()?.arc_single_identifier().Select(g => new ArcSingleIdentifier(g)) ?? Array.Empty<ArcSingleIdentifier>();
            Context = context;
        }

        public string GetSignature() => $"G{Identifier}";
    }
}
