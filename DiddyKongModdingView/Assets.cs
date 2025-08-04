using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiddyKongModdingView
{
    internal class Assets
    {

        public static void populateAssets(byte[] assetData, ListBox listBox1, Label label1)
        {
            ushort assetCount = BitConverter.ToUInt16(assetData, 0);
            for (int i = 0; i < assetCount; i++)
            {
                listBox1.Items.Add($"Asset {(i + 1).ToString()}");
            }
            label1.Text = $"Total Asset Files Found: {assetCount}";
        }

        public static ushort getAssetID(byte[] assetData, int assetIndex)
        {
            return BitConverter.ToUInt16(assetData, 4 + assetIndex * 2);
        }

        public static int getAssetOffset(byte[] assetData, int assetIndex)
        {
            ushort assetCount = BitConverter.ToUInt16(assetData, 0);
            return BitConverter.ToInt32(assetData, 6 + assetCount * 2 + assetIndex * 8);
        }

        public static string getAssetSize(byte[] assetData, int assetIndex)
        {
            ushort assetCount = BitConverter.ToUInt16(assetData, 0);
            return FormatSize(BitConverter.ToUInt32(assetData, 10 + assetCount * 2 + assetIndex * 8));
        }

        public static string getAssetCompressionType(byte[] assetData, int assetOffset)
        {
            byte compressionValue = assetData[assetOffset];
            return (compressionValue == 0) ? "No Compression" : (compressionValue == 1) ? "Huffman" : (compressionValue == 2) ? "LZ77" : (compressionValue == 3) ? "Unknown" : (compressionValue == 4) ? "RunLength" : "Not Documented";
        }

        public static string getUnknownAssetFlags(byte[] assetData, int assetOffset)
        {
            int byte1 = assetData[assetOffset + 1];
            int byte2 = assetData[assetOffset + 2];
            int byte3 = assetData[assetOffset + 3];
            string hexByte1 = byte1.ToString("X2");
            string hexByte2 = byte2.ToString("X2");
            string hexByte3 = byte3.ToString("X2");

            return $"0x{hexByte1}{hexByte2}{hexByte3}";
        }

        public static string getAssetStringType(byte[] assetData, int assetOffset, Button decompressBtn)
        {
            int byte2 = assetData[assetOffset + 2];

            string compType = getAssetCompressionType(assetData, assetOffset);

            bool canDecomp = compType == "LZ77" || compType == "No Compression"; //|| compType == "RunLength" || compType == "Huffman";
            
            decompressBtn.Text = compType == "No Compression" ? "Open" : "Decompress";

            switch (byte2)
            {
                case 0x18:
                    decompressBtn.Enabled = true && canDecomp;
                    return "Model";
                case 0x9A:
                    decompressBtn.Enabled = true && canDecomp;
                    return "Track";
                case 0x0C:
                    decompressBtn.Enabled = true && canDecomp;
                    return "Texture";
                default:
                    decompressBtn.Enabled = false;
                    return $"Unknown";
            }
        }

        public static int getAssetIntType(byte[] assetData, int assetOffset)
        {
            int byte2 = assetData[assetOffset + 2];

            switch (byte2)
            {
                case 0x18:
                    return 1;   // Model
                case 0x9A:
                    return 2;   // Track
                case 0x0C:
                    return 3;   // Texture
                default:
                    return 0;   // Unknown or unsupported type
            }
        }

        private static byte[] replaceAllAssetOffsets(byte[] assetData, byte[] modifiedFile, int assetIndex, int sizeDifference)
        {
            //MessageBox.Show($"Replacing offsets for asset index {assetIndex} with size difference {sizeDifference} bytes.");
            ushort assetCount = BitConverter.ToUInt16(assetData, 0);
            int assetOffsetOffset = 0;
            int sizeOffset = 0;

            int assetOffset = 0;

            for (int i = assetIndex+1; i < assetCount; i++)
            {
                assetOffsetOffset = 6 + assetCount * 2 + i * 8;
                sizeOffset = 10 + assetCount * 2 + i * 8;

                assetOffset = BitConverter.ToInt32(assetData,assetOffsetOffset);
                Int32 newOffset = assetOffset + sizeDifference; // Calculate the new offset
                byte[] offsetBytes = BitConverter.GetBytes((Int32)newOffset); // or UInt32 if unsigned
                //MessageBox.Show($"Replacing offsets for asset index {i + 1} with size difference {sizeDifference} bytes. With newOffset{newOffset} and old one {assetOffset}");
                Array.Copy(offsetBytes, 0, modifiedFile, assetOffsetOffset, 4);
            }
            string outputFile = $"assets_modded.bin";
            File.WriteAllBytes(outputFile, modifiedFile);
            return modifiedFile; // Return the modified file with updated offsets
        }

        public static byte[] replaceAssetData(byte[] assetData, int assetIndex, string type, string compType, byte[] newData)
        {
            int assetOffset = getAssetOffset(assetData, assetIndex);    //grab the asset offset includs the 8 byte header
            ushort assetCount = BitConverter.ToUInt16(assetData, 0);//the asset count is at the start of the file ?2305?
            int sizeOffset = 10 + assetCount * 2 + assetIndex * 8;
            uint assetSize = BitConverter.ToUInt32(assetData, sizeOffset);//used to get asset size compressed

            //grab header for stupid bug that i made where the header is not included in the asset data
            byte[] header = new byte[8];
            Array.Copy(assetData, assetOffset, header, 0, 8);
            newData = header.Concat(newData).ToArray(); // Concatenate the header with the new data

            // Split the original data into three parts
            byte[] before = new byte[assetOffset];
            Array.Copy(assetData, 0, before, 0, assetOffset);

            byte[] after = new byte[assetData.Length - (assetOffset + assetSize)];
            Array.Copy(assetData, assetOffset + assetSize, after, 0, after.Length);

            // Combine: before + new data + after
            byte[] modifiedFile = new byte[before.Length + newData.Length + after.Length];
            Buffer.BlockCopy(before, 0, modifiedFile, 0, before.Length);
            Buffer.BlockCopy(newData, 0, modifiedFile, before.Length, newData.Length);
            Buffer.BlockCopy(after, 0, modifiedFile, before.Length + newData.Length, after.Length);

            // Save result
            //File.WriteAllBytes(filePath, modifiedFile);
            byte[] sizeBytes = BitConverter.GetBytes((UInt32)newData.Length); // little-endian by default
            Array.Copy(sizeBytes, 0, modifiedFile, sizeOffset, 4); // write 4 bytes
            //MessageBox.Show($"Asset {assetIndex} replaced successfully. New size: {newData.Length} bytes. Old size: {(int)assetSize}");
            modifiedFile = replaceAllAssetOffsets(assetData, modifiedFile, assetIndex, (int)newData.Length - (int)assetSize); // Update offsets for all assets after the modified one
            //MessageBox.Show("Asset replaced successfully.");
            return modifiedFile; // Return the modified file with the replaced asset data
        }

        public static byte[] decompressAsset(byte[] assetData, int assetIndex, string type,string compType)
        {
            int assetOffset = getAssetOffset(assetData, assetIndex) + 8;// +8 to skip the header
            ushort assetCount = BitConverter.ToUInt16(assetData, 0);//the asset count is at the start of the file ?2305?
            uint assetSize = BitConverter.ToUInt32(assetData, 10 + assetCount * 2 + assetIndex * 8);//used to get asset size compressed
            byte[] result = new byte[assetSize];//reate a byte array of the size of the asset
            Array.Copy(assetData, assetOffset, result, 0, assetSize);//copy the asset data into the result array
            byte[] payload = FileDecompressor.Decompress_Handler(result, type, compType);
            return payload; // Return the decompressed data
        }

        public static byte[] recompressAsset(byte[] decompressedData, int assetIndex, string type, string compType)
        {
            byte[] newCompressedData = FileDecompressor.Compress_Handler(decompressedData, type, compType);
            //return newCompressedFile;
            byte [] newDecompressed = FileDecompressor.Decompress_Handler(newCompressedData, type, compType);
            bool match = decompressedData.SequenceEqual(newDecompressed);
            MessageBox.Show(match ? "Recompression successful!" : "Recompression failed! Data mismatch.");
            return newCompressedData; // Return the recompressed data
        }

        public static byte getAssetType(byte[] assetData, int assetOffset)
        {
            return assetData[assetOffset + 2];
        }
        public static string getAssetUncompressedSize(byte[] assetData, int assetOffset)
        {
            return FormatSize(BitConverter.ToUInt32(assetData, assetOffset + 4));
        }

        private static string FormatSize(long byteCount)
        {
            if (byteCount < 1000)
                return $"{byteCount} Bytes";
            double kb = byteCount / 1000.0;
            return $"{kb:F2} KB";
        }
    }
}
