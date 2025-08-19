using Arc.Compiler.PackageGenerator.Models.Scope;
using Arc.Compiler.SyntaxAnalyzer.Models.Blocks;

namespace Arc.Compiler.PackageGenerator.Generators;

public static class ArcEnumGenerator
{
    public static ArcScopeTreeEnumNode GenerateEnum(ArcBlockEnum syntaxTree)
    {
        var node = new ArcScopeTreeEnumNode
        {
            SyntaxTree = syntaxTree,
            Children = []
        };

        var members = syntaxTree.Members
            .Select(m => new ArcScopeTreeEnumMemberNode
            {
                SyntaxTree = m,
            });

        node.AddChildren(members);

        return node;
    }
}