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
            label1.Text = $"Assets Found: {assetCount}";
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
            byte byte1 = assetData[assetOffset + 1];
            byte byte2 = assetData[assetOffset + 2];
            byte byte3 = assetData[assetOffset + 3];
            return $"{byte1 + byte2 + byte3}";
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
