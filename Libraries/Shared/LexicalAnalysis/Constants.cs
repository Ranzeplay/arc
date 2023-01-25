using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.LexicalAnalysis
{
    public class Constants
    {
        public static readonly Dictionary<KeywordToken, string> KeywordMappings = new()
        {
            { KeywordToken.Declare,   "decl" },
            { KeywordToken.Number,    "number" },
            { KeywordToken.Char,      "char" },
            { KeywordToken.String,    "string" },
            { KeywordToken.Bool,      "bool" },
            { KeywordToken.Var,       "var" },
            { KeywordToken.Const,     "const" },
            { KeywordToken.Export,    "export" },
            { KeywordToken.Func,      "func" },
            { KeywordToken.If,        "if" },
            { KeywordToken.ElseIf,    "elif" },
            { KeywordToken.Else,      "else" },
            { KeywordToken.While,     "while" },
            { KeywordToken.Loop,      "loop" },
            { KeywordToken.Continue,  "continue" },
            { KeywordToken.Break,     "break" },
            { KeywordToken.Return,    "return" },
            { KeywordToken.Call,      "call" },
            { KeywordToken.Link,      "link" },
            { KeywordToken.None,      "none" },
            { KeywordToken.Any,       "any" },
            { KeywordToken.True,      "true" },
            { KeywordToken.False,     "false" },
            { KeywordToken.Group,     "group" },
            { KeywordToken.Implement, "impl" },
            { KeywordToken.Get,       "get" },
            { KeywordToken.Set,       "set" },
            { KeywordToken.Method,    "method" },
            { KeywordToken.Field,     "field" },
            { KeywordToken.Default,   "default" },
            { KeywordToken.Self,      "self" }
        };
    }
}
