using Arc.Compiler.Shared.Parsing.AST;
using Arc.Compiler.Shared.Parsing.Components.Function;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Compilation
{
    public class CompilationUnit
    {
        public List<TokenizedFile> TokenizedFiles { get; set; } = new();

        public List<ASTNode> ASTNodes { get; set; } = new();

        public List<FunctionDeclarator> DeclaredFunctions { get; set; } = new();
    }
}
