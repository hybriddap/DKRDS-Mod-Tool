using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiddyKongModdingView
{
    internal class Tracks
    {
        public static void populateTracks(byte[] assetData, ListBox listBox1,Label label1)
        {
            int trackCount = 0;
            ushort assetCount = BitConverter.ToUInt16(assetData, 0);
            for (int i = 0; i < assetCount; i++)
            {
                if (Assets.getAssetType(assetData, Assets.getAssetOffset(assetData, i)) == 0x9A)
                {
                    trackCount++;
                    listBox1.Items.Add($"Asset {(i + 1).ToString()}");
                }
            }
            label1.Text = $"Tracks Found: {trackCount}";
        }

        public static byte[] decompressTrack(byte[] assetData, int assetIndex)
        {
            int assetOffset = Assets.getAssetOffset(assetData, assetIndex) + 8;
            ushort assetCount = BitConverter.ToUInt16(assetData, 0);
            uint assetSize = BitConverter.ToUInt32(assetData, 10 + assetCount * 2 + assetIndex * 8);
            byte[] result = new byte[assetSize];
            Array.Copy(assetData, assetOffset, result, 0, assetSize);
            return Decompressor.LZ77_Decompress(result);
        }

        public static uint getTrackSections(byte[] trackData)
        {
            return BitConverter.ToUInt32(trackData, 0);
        }

        public static uint getTrackSectionGroupOffset(byte[] trackData)
        {
            return BitConverter.ToUInt32(trackData, 4);
        }

        public static uint getTrackNoTextures(byte[] trackData)
        {
            return BitConverter.ToUInt32(trackData, 24);
        }

        public static int getTrackTextureGroupOffset(byte[] trackData)
        {
            return BitConverter.ToInt32(trackData, 28);
        }

        public static string getTrackFileSize(byte[] trackData)
        {
            return FormatSize(BitConverter.ToUInt32(trackData, 52));
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
