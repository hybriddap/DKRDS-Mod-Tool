using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiddyKongModdingView
{
    internal class Textures
    {
        public static void showImage(byte[] trackData, int textureIndex, uint paletteUID)
        {
            //for texture
            int offset = getTextureOffset(trackData, textureIndex);
            int width = BitConverter.ToInt16(trackData, offset + 16);
            int height = BitConverter.ToInt16(trackData, offset + 18);
            int size = BitConverter.ToInt16(trackData, offset + 14) * 0x10;

            int format = BitConverter.ToInt16(trackData, offset + 8);

            //for palette
            int paletteOffset = getPaletteOffset(trackData, paletteUID);
            int paletteSize = BitConverter.ToInt16(trackData, paletteOffset + 4);

            //data
            byte[] textureData = trackData.Skip(offset + 20).Take(size).ToArray();
            byte[] paletteData = trackData.Skip(paletteOffset + 8).Take(paletteSize).ToArray();

            TextureViewer viewer = new TextureViewer(textureData, paletteData, width, height, format);
            viewer.Show();
        }

        public static void populateTextures(byte[] trackData, ListBox listBox1, Label label1, String trackName)
        {
            if (trackData == null)
            {
                MessageBox.Show("Please select a track first!");
                return;
            }

            ushort textureCount = BitConverter.ToUInt16(trackData, Tracks.getTrackTextureGroupOffset(trackData));
            for (int i = 0; i < textureCount; i++)
            {
                listBox1.Items.Add($"Texture {(i + 1).ToString()}");
            }
            label1.Text = $"Textures For Track {trackName} Found: {textureCount}";

        }

        public static int getTextureOffset(byte[] trackData,int textureIndex)
        {
            int fileIndex = Tracks.getTrackTextureGroupOffset(trackData) + 4; //+ 4 for the 4byte header file
            int size;
            int currentIndex = 0;
            for (int i = 0; currentIndex < textureIndex; i++)
            {
                size = BitConverter.ToInt16(trackData, fileIndex + 14);
                fileIndex += 20; //Go to end of Texture Entry Header. This now points to start of data
                fileIndex += size * 0x10; //Go to end of Texture Data to get to next Texture Entry. This points to end of data, start of next texture header

                currentIndex++;
            }
            return fileIndex;
        }

        public static int getPaletteOffset(byte[] trackData,uint paletteUID)
        {
            ushort textureCount = BitConverter.ToUInt16(trackData, Tracks.getTrackTextureGroupOffset(trackData));
            ushort paletteCount = BitConverter.ToUInt16(trackData, Tracks.getTrackTextureGroupOffset(trackData) + 2);
            int fileIndex = getTextureOffset(trackData,textureCount); //Get Last texture offset/start of palette data

            uint curUID = 0;
            int size;
            for (int i = 0; i < paletteCount; i++)
            {
                curUID = BitConverter.ToUInt32(trackData, fileIndex);
                if (paletteUID == curUID)
                {
                    return fileIndex;
                }
                size = BitConverter.ToInt16(trackData, fileIndex + 4);
                fileIndex += 8; //to get to end of header
                fileIndex += size; //to get to end of palette data
            }
            return fileIndex;
        }

        public static uint getTextureUID(byte[] trackData, int textureIndex)
        {
            int offset = getTextureOffset(trackData, textureIndex);
            return BitConverter.ToUInt32(trackData, offset);
        }

        public static uint getPaletteUID(byte[] trackData, int textureIndex)
        {
            int offset = getTextureOffset(trackData, textureIndex);
            return BitConverter.ToUInt32(trackData, offset + 4);
        }

        public static string getTextureFormat(byte[] trackData, int textureIndex)
        {
            int offset = getTextureOffset(trackData, textureIndex);
            uint format = BitConverter.ToUInt16(trackData, offset + 8);
            return format == 0 ? "Unknown" : format == 1 ? "A3I5" : format == 2 ? "4-Color Palette" : format == 3 ? "16-Color Palette" : format == 4 ? "256-Color Palette" : format == 5 ? "4x4-Texel Compressed" : format == 6 ? "A5I3" : format == 7 ? "Direct Color (RGB5551)" : "Not Documented";
        }

        public static uint getTextureFlags(byte[] trackData, int textureIndex)
        {
            int offset = getTextureOffset(trackData, textureIndex);
            uint flags = BitConverter.ToUInt16(trackData, offset + 10);
            return flags;
        }

        public static string getTextureDimensions(byte[] trackData, int textureIndex)
        {
            int offset = getTextureOffset(trackData, textureIndex);
            uint width = BitConverter.ToUInt16(trackData, offset + 16);
            uint height = BitConverter.ToUInt16(trackData, offset + 18);
            return $"{width} x {height}";
        }

        public static int getPaletteSize(byte[] trackData, uint paletteUID)
        {
            int offset = getPaletteOffset(trackData, paletteUID);
            int size = BitConverter.ToInt16(trackData, offset + 4);
            return size;
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
