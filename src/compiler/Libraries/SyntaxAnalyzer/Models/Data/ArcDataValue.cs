using Arc.Compiler.SyntaxAnalyzer.Generated.ANTLR;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Components.CallChain;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.Instant;

namespace Arc.Compiler.SyntaxAnalyzer.Models.Data;

public sealed record ArcEmptyDataValue;

public union ArcDataValue(ArcInstantValue, ArcDataType, ArcCallChain, ArcEnumAccessor, ArcLambdaExpression, ArcEmptyDataValue)
{
    public static ArcDataValue FromTokens(ArcSourceCodeParser.Arc_data_valueContext context)
    {
        if (context.arc_instant_value() is { } instant)
            return ArcInstantValue.FromTokens(instant);
        if (context.arc_type_value() is { } type)
            return new ArcDataType(type.arc_data_type());
        if (context.arc_call_chain() is { } chain)
            return new ArcCallChain(chain);
        if (context.arc_enum_accessor() is { } accessor)
            return new ArcEnumAccessor(accessor);
        if (context.arc_lambda_expression() is { } lambda)
            return new ArcLambdaExpression(lambda);
        return new ArcEmptyDataValue();
    }
}
