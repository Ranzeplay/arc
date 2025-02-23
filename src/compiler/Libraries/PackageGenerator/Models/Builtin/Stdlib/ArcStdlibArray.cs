using Arc.Compiler.PackageGenerator.Models.Descriptors.Function;
using Arc.Compiler.PackageGenerator.Models.Descriptors;
using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Components;
using Arc.Compiler.SyntaxAnalyzer.Models.Data;

namespace Arc.Compiler.PackageGenerator.Models.Builtin.Stdlib
{
    class ArcStdlibArray
    {
        public static ArcScopeTreeNamespaceNode GetNamespace()
        {
            return (ArcScopeTreeNamespaceNode)
                new ArcScopeTreeNamespaceNode("Array")
                    .AddChildChained(
                        new ArcScopeTreeBuiltinFunction(
                            "CreateIntArray",
                            new ArcFunctionDescriptor()
                            {
                                Id = 0xc1,
                                Name = "CreateIntArray",
                                Accessibility = ArcAccessibility.Public,
                                Parameters = [],
                                ReturnValueType = new ArcDataDeclarationDescriptor()
                                {
                                    Type = ArcPersistentData.IntType,
                                    AllowNone = false,
                                    Dimension = 1,
                                    MemoryStorageType = ArcMemoryStorageType.Value,
                                    Mutability = ArcMutability.Variable,
                                }
                            }
                        )
                        { Id = 0xc1 }
                    )
                    .AddChildChained(
                        new ArcScopeTreeBuiltinFunction(
                            "PushIntArray",
                            new ArcFunctionDescriptor()
                            {
                                Id = 0xc2,
                                Name = "PushIntArray",
                                Accessibility = ArcAccessibility.Public,
                                Parameters = [
                                    new ArcParameterDescriptor()
                                    {
                                        DataType = new ArcDataDeclarationDescriptor()
                                        {
                                            Type = ArcPersistentData.IntType,
                                            AllowNone = false,
                                            Dimension = 0,
                                            MemoryStorageType = ArcMemoryStorageType.Value,
                                            Mutability = ArcMutability.Constant,
                                        },
                                        RawFullName = "i",
                                    }
                                ],
                                ReturnValueType = new ArcDataDeclarationDescriptor()
                                {
                                    Type = ArcPersistentData.NoneType,
                                    AllowNone = false,
                                    Dimension = 0,
                                    MemoryStorageType = ArcMemoryStorageType.Reference,
                                    Mutability = ArcMutability.Constant,
                                }
                            }
                        )
                        { Id = 0xc2 }
                    )
                    .AddChildChained(
                        new ArcScopeTreeBuiltinFunction(
                            "RemoveElementFromIntArray",
                            new ArcFunctionDescriptor()
                            {
                                Id = 0xc3,
                                Name = "RemoveElementFromIntArray",
                                Accessibility = ArcAccessibility.Public,
                                Parameters = [
                                    new ArcParameterDescriptor()
                                    {
                                        DataType = new ArcDataDeclarationDescriptor()
                                        {
                                            Type = ArcPersistentData.IntType,
                                            AllowNone = false,
                                            Dimension = 0,
                                            MemoryStorageType = ArcMemoryStorageType.Value,
                                            Mutability = ArcMutability.Constant,
                                        },
                                        RawFullName = "index",
                                    }
                                ],
                                ReturnValueType = new ArcDataDeclarationDescriptor()
                                {
                                    Type = ArcPersistentData.NoneType,
                                    AllowNone = false,
                                    Dimension = 0,
                                    MemoryStorageType = ArcMemoryStorageType.Reference,
                                    Mutability = ArcMutability.Constant,
                                }
                            }
                        )
                        { Id = 0xc3 }
                    )
                    .AddChildChained(
                        new ArcScopeTreeBuiltinFunction(
                            "GetIntArraySize",
                            new ArcFunctionDescriptor()
                            {
                                Id = 0xc4,
                                Name = "GetIntArraySize",
                                Accessibility = ArcAccessibility.Public,
                                Parameters = [],
                                ReturnValueType = new ArcDataDeclarationDescriptor()
                                {
                                    Type = ArcPersistentData.IntType,
                                    AllowNone = false,
                                    Dimension = 0,
                                    MemoryStorageType = ArcMemoryStorageType.Value,
                                    Mutability = ArcMutability.Constant,
                                }
                            }
                        )
                        { Id = 0xc4 }
                    );
        }
    }
}
