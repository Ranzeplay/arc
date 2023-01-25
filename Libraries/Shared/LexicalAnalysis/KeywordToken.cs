using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.LexicalAnalysis
{
    public enum KeywordToken
    {
        Number,       // number
        Char,         // char
        String,       // str
        Bool,         // bool
        Declare,      // decl
        Var,          // var
        Const,        // const
        Export,       // export
        Func,         // func
        If,           // if
        ElseIf,       // elif
        Else,         // else
        While,        // while
        Loop,         // loop
        Continue,     // continue
        Break,        // break
        Return,       // return
        Call,         // call
        Link,         // link
        None,         // none
        Any,          // any
        True,         // true
        False,        // false
        Group,        // group
        Implement,    // implement
        Get,          // get
        Set,          // set
        Method,       // method
        Field,        // field
        Default,      // default
        Self,         // self
        Invalid
    }
}
