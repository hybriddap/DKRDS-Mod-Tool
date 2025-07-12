using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiddyKongModdingView
{
    internal static class Decompressor
    {
        private static byte[] LZ77(byte[] data)
        {
            if (data[0] != 0x10)
                throw new InvalidOperationException("Not a valid LZ77 (type 0x10) compressed file.");

            int decompressedLength = data[1] | (data[2] << 8) | (data[3] << 16);
            byte[] result = new byte[decompressedLength];

            int inOffset = 4;
            int outOffset = 0;

            while (outOffset < decompressedLength)
            {
                if (inOffset >= data.Length)
                    throw new IndexOutOfRangeException("Unexpected end of compressed data.");

                byte flag = data[inOffset++];
                for (int i = 0; i < 8; i++)
                {
                    if ((flag & (0x80 >> i)) == 0)
                    {
                        // Uncompressed byte
                        if (inOffset >= data.Length)
                            throw new IndexOutOfRangeException("Unexpected end of data.");
                        result[outOffset++] = data[inOffset++];
                    }
                    else
                    {
                        // Compressed block
                        if (inOffset + 1 >= data.Length)
                            throw new IndexOutOfRangeException("Unexpected end of data.");
                        int byte1 = data[inOffset++];
                        int byte2 = data[inOffset++];

                        int disp = ((byte1 & 0xF) << 8) | byte2;
                        int len = (byte1 >> 4) + 3;

                        int copySource = outOffset - (disp + 1);
                        for (int j = 0; j < len; j++)
                        {
                            if (copySource < 0 || copySource >= result.Length)
                                throw new IndexOutOfRangeException("Copy source out of bounds.");
                            result[outOffset++] = result[copySource++];
                        }
                    }

                    if (outOffset >= decompressedLength)
                        break;
                }
            }

            return result;
        }

        public static byte[] LZ77_Decompress(byte[] data)
        {
            string outputFile = "track_decompressed.bin";

            byte[] output = LZ77(data);

            //File.WriteAllBytes(outputFile, output);
            MessageBox.Show("Track file decompression complete.");
            return output;
        }
    }
}
