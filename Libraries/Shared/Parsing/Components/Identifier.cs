using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components
{
    public class Identifier
    {
        public string[] Scope { get; set; }

        public string Name { get; set; }

        public Identifier(string[] scope, string name)
        {
            Scope = scope;
            Name = name;
        }
    }
}
