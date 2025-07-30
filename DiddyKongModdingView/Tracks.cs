using DiddyKongModdingView.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
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
            label1.Text = $"Track Files Found: {trackCount}";
        }

        //public static byte[] decompressTrack(byte[] assetData, int assetIndex,string compType)
        //{
        //    int assetOffset = Assets.getAssetOffset(assetData, assetIndex) + 8;//the asset offset
        //    ushort assetCount = BitConverter.ToUInt16(assetData, 0);//the asset count is at the start of the file ?2305?
        //    uint assetSize = BitConverter.ToUInt32(assetData, 10 + assetCount * 2 + assetIndex * 8);//used to get asset size
        //    byte[] result = new byte[assetSize];//reate a byte array of the size of the asset
        //    Array.Copy(assetData, assetOffset, result, 0, assetSize);//copy the asset data into the result array
        //    return FileDecompressor.Decompress_Handler(result,"track",compType);
        //}

        public static string getTrackName(int assetID)
        {
            var assembly = Assembly.GetExecutingAssembly();
            string resourceName = "DiddyKongModdingView.data.json";
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                Console.WriteLine("Embedded resource not found.");
                return assetID.ToString();
            }
            using StreamReader reader = new StreamReader(stream);
            string json = reader.ReadToEnd();



            var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json);
            string hexValue = $"0x{assetID.ToString("X4")}"; // Convert assetID to hex format
            if (data.TryGetValue("trackNames", out var trackNames) &&
            trackNames.TryGetValue(hexValue, out var trackName))
            {
                return trackName;
            }
            return assetID.ToString();
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
