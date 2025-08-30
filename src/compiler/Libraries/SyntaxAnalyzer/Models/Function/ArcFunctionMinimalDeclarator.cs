using Arc.Compiler.SyntaxAnalyzer.Interfaces;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Identifier;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Function;

public abstract class ArcFunctionMinimalDeclarator : IArcLocatable
{
    public IEnumerable<ArcFunctionArgument> Arguments { get; set; }

    public IEnumerable<ArcSingleIdentifier> GenericTypes { get; set; }

    public ArcDataType ReturnType { get; set; }
    
    public abstract string GetSignature();
}
