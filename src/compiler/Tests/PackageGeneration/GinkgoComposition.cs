using Arc.Compiler.PackageGenerator.Ginkgo.Instruction;
using Arc.Compiler.PackageGenerator.Ginkgo.Metadata.Data;
using Arc.Compiler.PackageGenerator.Ginkgo.Scope;
using Arc.Compiler.PackageGenerator.Ginkgo.Transformation;
using Arc.Compiler.PackageGenerator.Models.Relocation;

namespace Arc.Compiler.Tests.PackageGeneration;

public class GinkgoComposition
{
    [Test]
    public void ScopeNodeIdentityWorksInHashCollections()
    {
        var first = new ArcScopeTreeNamespaceNode("first") { Id = 42 };
        var same = new ArcScopeTreeNamespaceNode("same") { Id = 42 };
        var other = new ArcScopeTreeNamespaceNode("other") { Id = 43 };
        var dictionary = new Dictionary<ArcScopeTreeNodeBase, string> { [first] = "value" };

        Assert.Multiple(() =>
        {
            Assert.That(same, Is.EqualTo(first));
            Assert.That(same.GetHashCode(), Is.EqualTo(first.GetHashCode()));
            Assert.That(new[] { first, same, other }.Distinct().Count(), Is.EqualTo(2));
            Assert.That(dictionary[same], Is.EqualTo("value"));
        });
    }

    [Test]
    public void AppendingInstructionsPreservesOutputAndIncludesMetadata()
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        var output = Register(1);
        result.SetOutput(output);
        var child = Call(2);
        child.RelocationLabels.Add(ArcRelocationLabel.Create());
        result.AppendInstructions(child);

        Assert.Multiple(() =>
        {
            Assert.That(result.OutImplicitRegister, Is.SameAs(output));
            Assert.That(result.Instructions, Is.EqualTo(child.Instructions));
            Assert.That(result.RelocationLabels, Is.EqualTo(child.RelocationLabels));
        });
        result.StoreOutput(Register(3));
        Assert.That(result.Instructions.Last(), Is.TypeOf<ArcDataCopyInstruction>());
        Assert.That(((ArcControlFlowInvokeInstruction)child.Instructions.Single()).Dest, Is.EqualTo(2));
    }

    [Test]
    public void ForwardingValuesRedirectsOnlyTheFinalProducingCall()
    {
        var result = new ArcGinkgoInstructionGenerationResult();
        var argument = Call(1);
        var value = Call(2);
        result.AppendInstructions(argument);
        result.AppendValue(value);
        result.StoreOutput(Register(3));

        Assert.Multiple(() =>
        {
            Assert.That(result.Instructions.Count, Is.EqualTo(2));
            Assert.That(((ArcControlFlowInvokeInstruction)argument.Instructions.Single()).Dest, Is.EqualTo(1));
            Assert.That(((ArcControlFlowInvokeInstruction)value.Instructions.Single()).Dest, Is.EqualTo(3));
            Assert.That(result.OutImplicitRegister!.Id, Is.EqualTo(3));
        });
    }

    [Test]
    public void ForwardingVoidClearsThePreviousValue()
    {
        var result = Call(1);
        result.AppendValue(new ArcGinkgoInstructionGenerationResult());
        Assert.That(result.OutImplicitRegister, Is.Null);
        Assert.Throws<InvalidOperationException>(() => result.StoreOutput(Register(2)));
    }

    [Test]
    public void ConstructorOutputDoesNotRedirectTheVoidLifecycleCall()
    {
        var invoke = ArcControlFlowInvokeInstruction.Normal;
        var constructor = new ArcGinkgoInstructionGenerationResult { Instructions = [invoke] };
        constructor.SetOutput(Register(0));
        var result = Call(1);
        result.AppendValue(constructor);
        result.StoreOutput(Register(2));

        Assert.That(invoke.Dest, Is.Zero);
        var copy = (ArcDataCopyInstruction)result.Instructions.Last();
        Assert.That(copy.Src, Is.Zero);
        Assert.That(copy.Dest, Is.EqualTo(2));
    }

    [Test]
    public void AnInterveningInstructionPreventsCallRedirection()
    {
        var result = Call(1);
        var invoke = (ArcControlFlowInvokeInstruction)result.Instructions.Single();
        result.Instructions.Add(new ArcNoOperationInstruction());
        result.StoreOutput(Register(2));

        Assert.That(invoke.Dest, Is.EqualTo(1));
        Assert.That(result.Instructions.Last(), Is.TypeOf<ArcDataCopyInstruction>());
    }

    [Test]
    public void SettingANewOutputClearsThePreviousProducer()
    {
        var result = Call(1);
        result.SetOutput(Register(1));
        result.StoreOutput(Register(2));
        Assert.That(((ArcControlFlowInvokeInstruction)result.Instructions.First()).Dest, Is.EqualTo(1));
        Assert.That(result.Instructions.Last(), Is.TypeOf<ArcDataCopyInstruction>());
    }

    private static ArcGinkgoInstructionGenerationResult Call(ushort id)
    {
        var invoke = ArcControlFlowInvokeInstruction.Normal;
        invoke.Dest = id;
        var result = new ArcGinkgoInstructionGenerationResult { Instructions = [invoke] };
        result.SetOutput(Register(id), invoke);
        return result;
    }

    private static ArcImplicitRegister Register(ushort id) =>
        new(id, new ArcDataTypeInstance(), ArcImplicitRegisterType.LocalData);
}
