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

        public byte[] GenerateFunctionIdData(long slot)
        {
            var slotBytes = BitConverter.GetBytes(slot).ToArray();
            Array.Resize(ref slotBytes, AddressAlignment);
            Array.Reverse(slotBytes);

            return slotBytes;
        }

        public byte[] GenerateSlotData(long slot)
        {
            var slotBytes = BitConverter.GetBytes(slot).ToArray();
            Array.Resize(ref slotBytes, DataSlotAlignment);
            Array.Reverse(slotBytes);

            return slotBytes;
        }

        public byte[] BuildDataBlock(byte[] encodedData)
        {
            var result = new List<byte>();

            var data = encodedData.ToList();
            if (data.Count % DataSectionSize > 0)
            {
                data.InsertRange(0, new byte[data.Count % DataSectionSize]);
            }

            result.Add((byte)(data.Count / DataSectionSize));
            result.AddRange(data);

            return result.ToArray();
        }
    }
}
