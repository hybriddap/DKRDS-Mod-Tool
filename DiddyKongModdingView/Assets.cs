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
        private static byte[] decompressedData;

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

        public static byte[] decompressAsset(byte[] assetData, int assetIndex, string type,string compType)
        {
            int assetOffset = getAssetOffset(assetData, assetIndex) + 8;//the asset offset
            ushort assetCount = BitConverter.ToUInt16(assetData, 0);//the asset count is at the start of the file ?2305?
            uint assetSize = BitConverter.ToUInt32(assetData, 10 + assetCount * 2 + assetIndex * 8);//used to get asset size
            byte[] result = new byte[assetSize];//reate a byte array of the size of the asset
            Array.Copy(assetData, assetOffset, result, 0, assetSize);//copy the asset data into the result array
            byte[] payload = FileDecompressor.Decompress_Handler(result, type, compType);
            decompressedData = payload;
            return payload; // Return the decompressed data
        }

        public static byte[] recompressAsset(byte[] assetData, int assetIndex, string type, string compType)
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
