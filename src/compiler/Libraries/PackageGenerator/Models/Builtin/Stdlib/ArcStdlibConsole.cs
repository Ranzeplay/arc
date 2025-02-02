using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;

namespace Arc.Compiler.PackageGenerator.Models.Builtin.Stdlib
{
    internal class ArcStdlibConsole
    {
        public static ArcScopeTreeNamespaceNode GetNamespace()
        {
            return (ArcScopeTreeNamespaceNode)
                new ArcScopeTreeNamespaceNode("Console")
                    .AddChildChained(
                        new ArcScopeTreeBuiltinFunction(
                            "PrintString",
                            new ArcFunctionDescriptor()
                            {
                                Id = 0xa1,
                                Name = "PrintString",
                                Accessibility = ArcAccessibility.Public,
                                Parameters = [
                                    new ArcParameterDescriptor()
                                    {
                                        DataType = new ArcDataDeclarationDescriptor()
                                        {
                                            Type = ArcPersistentData.StringType,
                                            AllowNone = false,
                                            IsArray = false,
                                            MemoryStorageType = ArcMemoryStorageType.Reference,
                                            Mutability = ArcMutability.Constant,
                                        },
                                        RawFullName = "s",
                                    }
                                ],
                                ReturnValueType = new ArcDataDeclarationDescriptor()
                                {
                                    Type = ArcPersistentData.NoneType,
                                    AllowNone = false,
                                    IsArray = false,
                                    MemoryStorageType = ArcMemoryStorageType.Reference,
                                    Mutability = ArcMutability.Constant,
                                }
                            }
                        )
                        { Id = 0xa1 }
                    )
                    .AddChildChained(
                        new ArcScopeTreeBuiltinFunction(
                            "ReadString",
                            new ArcFunctionDescriptor()
                            {
                                Id = 0xa2,
                                Name = "ReadString",
                                Accessibility = ArcAccessibility.Public,
                                ReturnValueType = new ArcDataDeclarationDescriptor()
                                {
                                    Type = ArcPersistentData.StringType,
                                    AllowNone = false,
                                    IsArray = false,
                                    MemoryStorageType = ArcMemoryStorageType.Value,
                                    Mutability = ArcMutability.Constant,
                                }
                            }
                        )
                        { Id = 0xa2 }
                    );
        }
    }
}
