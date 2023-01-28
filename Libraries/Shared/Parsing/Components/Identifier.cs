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

        public override bool Equals(object? obj)
        {
            if (obj is not Identifier ident)
            {
                return false;
            }
            else
            {
                return ident.Scope == Scope && ident.Name == Name;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
