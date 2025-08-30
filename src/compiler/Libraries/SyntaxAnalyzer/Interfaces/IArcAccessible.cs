using Arc.Compiler.SyntaxAnalyzer.Models.Components;

namespace Arc.Compiler.SyntaxAnalyzer.Interfaces;

public interface IArcAccessible
{
    public ArcAccessibility Accessibility { get; set; }
}
