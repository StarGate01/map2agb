using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace map2agbimport.Compression
{
    public static class LzCompression
    {
        public const byte MAGIC = 0xF;

        /* binary reader extension probably */
        public static IEnumerable<byte> Decompress(this BinaryReader reader)
        {
            uint head = reader.ReadUInt32();
            if ((byte)(head & 0xFF) != MAGIC)
                throw new ArgumentException("could not read LZ structure, invalid magic number read", "reader");
            uint size = head >> 8;

            List<byte> output = new List<byte>((int)(size * 2));
            while (output.Count < size)
            {
                byte compound = reader.ReadByte();
                for (int i = 0; i < 8; ++i)
                {
                    bool compressed = (compound & 0x80 >> i) > 0;
                    if (compressed)
                    {
                        ushort lookup = reader.ReadUInt16();
                        //TODO: Implement compression
                    }
                    else
                        output.Add(reader.ReadByte());
                }
            }
            return output.GetRange(0, (int)size);

        }
    }
}
