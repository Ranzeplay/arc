using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arc.Compiler.Shared.CommandGeneration
{
    public class PackageMetadata
    {
        public byte PackageType { get; }

        public byte DataAlignment { get; }

        public byte DataSlotAlignment { get; }

        public byte AddressAlignment { get; }

        public byte EntryFunctionId { get; }

        public byte DataSectionSize { get; }

        public PackageMetadata(byte packageType, byte dataAlignment, byte dataSlotAlignment, byte addressAlignment, byte entryFunctionId, byte dataSectionSize)
        {
            PackageType = packageType;
            DataAlignment = dataAlignment;
            DataSlotAlignment = dataSlotAlignment;
            AddressAlignment = addressAlignment;
            EntryFunctionId = entryFunctionId;
            DataSectionSize = dataSectionSize;
        }
    }
}
