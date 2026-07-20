using Arc.Compiler.PackageGenerator.Ginkgo.Interface;
using Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;
using Arc.Compiler.PackageGenerator.Ginkgo.Scope;
using Arc.Compiler.PackageGenerator.Interfaces;
using Arc.Compiler.PackageGenerator.Models;

namespace Arc.Compiler.PackageGenerator.Ginkgo.Transformation;

public class ArcFunctionGenerationContext(
    ArcGinkgoUnitGenerationContext globalContext,
    ArcScopeTreeFunctionNodeBase currentNode,
    IEnumerable<IArcDataTypeProxy> linkedTypes,
    IEnumerable<IArcDataTypeProxy> linkedGenericTypes)
    : IArcGinkgoGenerationContext<ArcScopeTreeFunctionNodeBase>
{
    public TwoWayDictionary<ushort, string, ArcImplicitRegister> ImplicitRegisterRegistry { get; } = new();

    public void AddImplicitRegister(ArcImplicitRegister localObject)
    {
        ImplicitRegisterRegistry.Add(localObject.Id, localObject.Name, localObject);
    }

    public ushort NextImplicitRegisterId => (ushort)ImplicitRegisterRegistry.Count;

    public ArcImplicitRegister? GetImplicitRegister(string name)
    {
        return ImplicitRegisterRegistry[name];
    }

    public ArcImplicitRegister? GetImplicitRegister(ushort id)
    {
        return ImplicitRegisterRegistry[id];
    }

    public ArcGinkgoUnitGenerationContext GlobalContext { get; } = globalContext;

    public ArcScopeTreeFunctionNodeBase CurrentNode { get; } = currentNode;

    public IEnumerable<IArcDataTypeProxy> LinkedTypes { get; } = linkedTypes;

    public IEnumerable<IArcDataTypeProxy> LinkedGenericTypes { get; } = linkedGenericTypes;

    public string UnitName => GlobalContext.UnitName;
}