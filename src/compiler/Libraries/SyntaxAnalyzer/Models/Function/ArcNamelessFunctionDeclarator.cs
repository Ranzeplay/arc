namespace Arc.Compiler.SyntaxAnalyzer.Models.Function;

public class ArcNamelessFunctionDeclarator : ArcFunctionMinimalDeclarator
{
    public string Remark { get; set; }
    
    public override string GetSignature() => Remark;
}
