using Arc.Compiler.PackageGenerator.Ginkgo.Interface;
using Arc.Compiler.PackageGenerator.Ginkgo.Scope;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;
using Microsoft.Extensions.Logging;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;

public class ArcDataTypeInstance
{
    public IArcDataTypeProxy Type { get; set; }
    
    public ushort Dimension { get; set; }
    
    public bool AllowNone { get; set; }
    
    public ArcMutability Mutability { get; set; }

    public static (ArcDataTypeInstance?, IEnumerable<ArcCompilationLogBase>) Create<T>(IArcGinkgoGenerationContext<T> ctx, ArcDataDeclarationDescriptor decl)
        where T : ArcScopeTreeNodeBase
    {
        // Find the type proxy for the data type
        var dt = ArcDataTypeHelper.GetDataTypeNode(ctx, decl.Type);

        if (dt is null)
        {
            return (null, [
                new ArcSourceLocatableLog(LogLevel.Error, 0, $"Data type \"{decl.Type.Identifier}\" not found", ctx.GlobalContext.UnitName, decl.SyntaxTree.Identifier)
            ]);
        }
        
        return (new ArcDataTypeInstance
        {
            Type = dt,
            Dimension = (ushort)decl.Dimension,
            AllowNone = decl.AllowNone,
            Mutability = decl.Mutability,
        }, []);
    }
    
    public static (ArcDataTypeInstance?, IEnumerable<ArcCompilationLogBase>) Create<T>(IArcGinkgoGenerationContext<T> ctx, ArcDataType dataType, ArcMutability mutability, ushort dimension, bool allowNone)
        where T : ArcScopeTreeNodeBase
    {
        // Find the type proxy for the data type
        var dt = ArcDataTypeHelper.GetDataType(ctx, dataType);

        if (dt is null)
        {
            return (null, [
                new ArcSourceUnlocatableLog(LogLevel.Error, 0, $"Data type \"{dataType.TypeName}\" not found", ctx.GlobalContext.UnitName)
            ]);
        }
        
        return (new ArcDataTypeInstance
        {
            Type = dt,
            Dimension = dimension,
            AllowNone = allowNone,
            Mutability = mutability,
        }, []);
    }
}