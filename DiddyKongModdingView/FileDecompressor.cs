using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiddyKongModdingView
{
    internal static class FileDecompressor
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

        private static byte[] Run_Length(byte[] input)
        {
            List<byte> output = new List<byte>();
            int i = 0;

            while (i < input.Length)
            {
                byte control = input[i++];

                if ((control & 0x80) == 0)
                {
                    // Literal run: copy next (control + 1) bytes
                    int count = (control & 0x7F) + 1;
                    if (i + count > input.Length)
                        throw new InvalidDataException("Unexpected end of input during literal run.");

                    for (int j = 0; j < count; j++)
                        output.Add(input[i++]);
                }
                else
                {
                    // Repeat run: repeat next byte ((control & 0x7F) + 3) times
                    int count = (control & 0x7F) + 3;
                    if (i >= input.Length)
                        throw new InvalidDataException("Unexpected end of input during repeat run.");

                    byte value = input[i++];
                    for (int j = 0; j < count; j++)
                        output.Add(value);
                }
            }

            return output.ToArray();
        }




        public static byte[] Decompress_Handler(byte[] data,string assetType,string compressionType)
        {
            string outputFile = $"{assetType}_decompressed.bin";
            byte[] output;

            if (compressionType == "No Compression")
            {
                File.WriteAllBytes(outputFile, data);
                output = data;
            }
            else if (compressionType == "Huffman")
            {
                File.WriteAllBytes(outputFile, data);
                output = data;
            }
            else if (compressionType == "RunLength")
            {
                output = Run_Length(data);
                File.WriteAllBytes(outputFile, output);
                MessageBox.Show($"{assetType} file decompression complete.");
            }
            else if (compressionType == "LZ77")
            {
                output = LZ77(data);
                File.WriteAllBytes(outputFile, output);
                MessageBox.Show($"{assetType} file decompression complete.");
            }
            else
            {
                output = null;
            }
            return output;
        }
    }
}
