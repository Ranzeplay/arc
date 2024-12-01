using Arc.Compiler.SyntaxAnalyzer.Interfaces;

namespace Arc.Compiler.PackageGenerator.Models.Generation
{
    public class ArcSignature : IArcLocatable
    {
        public IEnumerable<IArcLocatable> Locators { get; set; } = [];

        public string GetSignature() => string.Join('+', Locators.Select(locatable => locatable.GetSignature()));
    }
}
