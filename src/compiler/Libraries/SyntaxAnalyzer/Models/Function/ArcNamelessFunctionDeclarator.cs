namespace Arc.Compiler.SyntaxAnalyzer.Models.Function;

public class ArcNamelessFunctionDeclarator : ArcFunctionMinimalDeclarator
{
    public string SignaturePrefix { get; set; }
    
    public override string GetSignature() => $"{SignaturePrefix}@{string.Join('&', Arguments.Select(a => a.DataType.GetSignature()))}*{ReturnType.GetSignature()}";
}
