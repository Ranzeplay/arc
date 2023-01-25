using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.LexicalAnalysis
{
    public enum KeywordToken
    {
        KwNumber,       // number
        KwChar,         // char
        KwStr,          // str
        KwBool,         // bool
        KwDeclare,      // decl
        KwVar,          // var
        KwConst,        // const
        KwExport,       // export
        KwFunc,         // func
        KwIf,           // if
        KwElseIf,       // elif
        KwElse,         // else
        KwWhile,        // while
        KwLoop,         // loop
        KwContinue,     // continue
        KwBreak,        // break
        KwReturn,       // return
        KwCall,         // call
        KwLink,         // link
        KwNone,         // none
        KwAny,          // any
        KwTrue,         // true
        KwFalse,        // false
        KwGroup,        // group
        KwImplement,    // implement
        KwGet,          // get
        KwSet,          // set
        KwMethod,       // method
        KwField,        // field
        KwDefault,      // default
        KwSelf,         // self
        Invalid
    }
}
