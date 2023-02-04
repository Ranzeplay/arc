using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.Parsing.Components.Group
{
    public class GroupBlock
    {
        public Identifier Identifier { get; }

        public GroupField[] Fields { get; }

        public GroupMethod[] GroupMethods { get; }

        public GroupFunction[] Functions { get; }

        public GroupBlock(Identifier identifier, GroupField[] fields, GroupMethod[] groupMethods, GroupFunction[] functions)
        {
            Identifier = identifier;
            Fields = fields;
            GroupMethods = groupMethods;
            Functions = functions;
        }
    }
}
