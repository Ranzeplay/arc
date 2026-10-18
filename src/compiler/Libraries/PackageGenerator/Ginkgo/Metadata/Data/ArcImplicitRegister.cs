using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Logging;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;
using Arc.Compiler.SyntaxAnalyzer.Models.Data.DataType;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;

public class ArcImplicitRegister
{
    public ushort Id { get; set; }
    
    public string Name { get; set; }

    public ArcDataTypeInstance Instance { get; set; }
    
    public ArcImplicitRegisterType Type { get; set; }

    private static string RandomName => "$" + Guid.NewGuid().ToString("N")[..8];

    public ArcImplicitRegister(ushort id, string name, ArcDataTypeInstance instance, ArcImplicitRegisterType type)
    {
        Id = id;
        Name = name;
        Instance = instance;
        Type = type;
    }

    public ArcImplicitRegister(ushort id, ArcDataTypeInstance instance, ArcImplicitRegisterType type)
    {
        Id = id;
        Name = RandomName;
        Instance = instance;
        Type = type;
    }

    public static (ArcImplicitRegister?, IEnumerable<ArcCompilationLogBase>) Create(ArcFunctionGenerationContext ctx, ArcDataDeclarator decl, ArcImplicitRegisterType type)
    {
        var (instance, logs) = ArcDataTypeInstance.Create(ctx, decl.DataType, decl.Mutability,
            checked((ushort)decl.DataType.Dimension), false);
        return instance is null ? (null, logs) :
            (new ArcImplicitRegister(ctx.NextImplicitRegisterId, decl.Identifier.Name, instance, type), logs);
    }
    
    public static (ArcImplicitRegister?, IEnumerable<ArcCompilationLogBase>) Create(ArcFunctionGenerationContext ctx, ArcDataDeclarationDescriptor decl, ArcImplicitRegisterType type)
    {
        var (dti, logs) = ArcDataTypeInstance.Create(ctx, decl);
        if (dti == null)
        {
            return (null, logs);
        }
        
        return (new ArcImplicitRegister(ctx.NextImplicitRegisterId, decl.SyntaxTree.Identifier.Name, dti, type), logs);
    }
    
    public static (ArcImplicitRegister?, IEnumerable<ArcCompilationLogBase>) CreateAnonymous(ArcFunctionGenerationContext ctx, ArcDataType dataType, ArcMutability mutability, ArcImplicitRegisterType type)
    {
        var (instance, logs) = ArcDataTypeInstance.Create(ctx, dataType, mutability,
            checked((ushort)dataType.Dimension), false);
        return instance is null ? (null, logs) : CreateAnonymous(ctx, instance, type);
    }
    
    public static (ArcImplicitRegister?, IEnumerable<ArcCompilationLogBase>) CreateAnonymous(ArcFunctionGenerationContext ctx, ArcDataTypeInstance instance, ArcImplicitRegisterType type)
    {
        return (new ArcImplicitRegister(ctx.NextImplicitRegisterId, instance, type), []);
    }
    
    public static (ArcImplicitRegister?, IEnumerable<ArcCompilationLogBase>) CreateAnonymous(ArcFunctionGenerationContext ctx, ArcDataDeclarator decl, ArcImplicitRegisterType type)
    {
        return CreateAnonymous(ctx, decl.DataType, decl.Mutability, type);
    }
    
    public static (ArcImplicitRegister?, IEnumerable<ArcCompilationLogBase>) CreateAnonymous(ArcFunctionGenerationContext ctx, ArcDataDeclarationDescriptor decl, ArcImplicitRegisterType type)
    {
        var (dti, logs) = ArcDataTypeInstance.Create(ctx, decl);
        if (dti == null)
        {
            return (null, logs);
        }
        
        return (new ArcImplicitRegister(ctx.NextImplicitRegisterId, dti, type), logs);
    }
}
