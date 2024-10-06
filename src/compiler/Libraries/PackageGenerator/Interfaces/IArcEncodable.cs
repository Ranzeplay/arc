using Arc.Compiler.PackageGenerator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.PackageGenerator.Interfaces
{
    internal interface IArcEncodable
    {
        public byte[] OpCode { get; }

        public ArcEncodeResult Encode();
    }
}
