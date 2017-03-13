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
        public const byte MAGIC = 0x10;

        /* binary reader extension probably */
        public static IEnumerable<byte> ReadLzArray(this BinaryReader reader)
        {
            uint head = reader.ReadUInt32();
            if ((byte)(head & 0xFF) != MAGIC)
                throw new ArgumentException("could not read LZ structure, invalid magic number read", "reader");
            uint size = head >> 8;

            List<byte> output = new List<byte>((int)(size + 16));
            while (output.Count < size)
            {
                byte compound = reader.ReadByte();
                for (int i = 0; i < 8; ++i)
                {
                    bool compressed = (compound & (0x80 >> i)) > 0;
                    if (compressed)
                    {
                        ushort lookup = reader.ReadUInt16();

                        ushort displace = (ushort)(((lookup >> 0x8) | ((lookup & 0xF) << 0x8)) + 1);
                        byte length = (byte)(((lookup >> 4) & 0xF) + 3);

                        List<byte> backBuffer = new List<byte>(length);

                        int index = output.Count - displace;
                        while (backBuffer.Count < length)
                        {
                            backBuffer.Add(output[index]);
                            index++;

                            if (index >= output.Count)
                                index = output.Count - displace;
                        }
                        output.AddRange(backBuffer);
                    }
                    else
                        output.Add(reader.ReadByte());
                }
            }
            return output.Take((int)size);

        }
    }
}
