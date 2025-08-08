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
        private static bool isTextureFile = false; //Used to determine if the texture is from a texture file or a track file
        private static bool isModelFile = false; //Used to determine if the texture is from a model file or a track file

        public static void populateTextures(byte[] assetData, ListBox listBox1, Label label1)
        {
            int textureCount = 0;
            ushort assetCount = BitConverter.ToUInt16(assetData, 0);
            for (int i = 0; i < assetCount; i++)
            {
                if (Assets.getAssetType(assetData, Assets.getAssetOffset(assetData, i)) == 0x0C)
                {
                    textureCount++;
                    listBox1.Items.Add($"Asset {(i + 1).ToString()}");
                }
            }
            label1.Text = $"Texture Files Found: {textureCount}";
        }

        public static void getAllTextures(byte[] trackData, ListBox listBox1, Label label1, String trackName)
        {
            if (trackData == null)
            {
                MessageBox.Show("Please select a track first!");
                return;
            }
            
            isTextureFile = trackName == "Texture"; 
            isModelFile = trackName == "Model";

            int textureCount = trackName == "Texture" ? BitConverter.ToUInt16(trackData, 0) : trackName == "Model"? BitConverter.ToUInt16(trackData, 8) :  BitConverter.ToUInt16(trackData, Tracks.getTrackTextureGroupOffset(trackData));
            for (int i = 0; i < textureCount; i++)
            {
                listBox1.Items.Add($"Texture {(i + 1).ToString()}");
            }
            label1.Text = $"Textures Found: {textureCount}";

        }


        public static void showImage(byte[] trackData, int textureIndex, uint paletteUID)
        {
            try
            {
                //for texture
                int offset = getTextureOffset(trackData, textureIndex);
                int width = BitConverter.ToInt16(trackData, offset + 16);
                int height = BitConverter.ToInt16(trackData, offset + 18);
                int size = BitConverter.ToInt16(trackData, offset + 14) * 0x10;

                int format = BitConverter.ToInt16(trackData, offset + 8);

                //for palette
                int paletteOffset = getPaletteOffset(trackData, paletteUID);
                if (paletteOffset == -1) //If palette not found, show error
                    return;
                int paletteSize = BitConverter.ToInt16(trackData, paletteOffset + 4);

                //data
                byte[] textureData = trackData.Skip(offset + 20).Take(size).ToArray();
                byte[] paletteData = trackData.Skip(paletteOffset + 8).Take(paletteSize).ToArray();

                TextureViewer viewer = new TextureViewer(textureData, paletteData, width, height, format);
                viewer.Show();
            }
            catch (Exception ex)
            {
                
            }
        }

        public static Bitmap showPreview(byte[] trackData, int textureIndex, uint paletteUID)
        {
            try
            {
                //for texture
                int offset = getTextureOffset(trackData, textureIndex);
                int width = BitConverter.ToInt16(trackData, offset + 16);
                int height = BitConverter.ToInt16(trackData, offset + 18);
                int size = BitConverter.ToInt16(trackData, offset + 14) * 0x10;

                int format = BitConverter.ToInt16(trackData, offset + 8);

                //for palette
                int paletteOffset = getPaletteOffset(trackData, paletteUID);
                if (paletteOffset == -1) //If palette not found, show error
                    return null;
                int paletteSize = BitConverter.ToInt16(trackData, paletteOffset + 4);

                //data
                byte[] textureData = trackData.Skip(offset + 20).Take(size).ToArray();
                byte[] paletteData = trackData.Skip(paletteOffset + 8).Take(paletteSize).ToArray();

                Bitmap prevImg = TextureViewer.GetTexture(textureData, paletteData, width, height, format);
                return prevImg;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static void changeTexture(byte[] trackData, int textureIndex, byte[] newTextureData)
        {
            try
            {
                //for texture
                int offset = getTextureOffset(trackData, textureIndex);
                int width = BitConverter.ToInt16(trackData, offset + 16);
                int height = BitConverter.ToInt16(trackData, offset + 18);
                int size = BitConverter.ToInt16(trackData, offset + 14) * 0x10;

                int format = BitConverter.ToInt16(trackData, offset + 8);

                //update data
                Array.Copy(newTextureData, 0, trackData, offset + 20, size);
                return;
            }
            catch (Exception ex)
            {
                return;
            }

        }
        public static void changePalette(byte[] trackData, uint paletteUID, byte[] newPaletteData)
        {
            try
            {
                //for palette
                int offset = getPaletteOffset(trackData, paletteUID);
                int size = BitConverter.ToInt16(trackData, offset + 4);

                //update data
                Array.Copy(newPaletteData, 0, trackData, offset + 8, size);
                return;
            }
            catch (Exception ex)
            {
                return;
            }

        }

        //public static void populateTextures(byte[] trackData, ListBox listBox1, Label label1, String trackName)
        //{
        //    if (trackData == null)
        //    {
        //        MessageBox.Show("Please select a track first!");
        //        return;
        //    }

        //    ushort textureCount = BitConverter.ToUInt16(trackData, Tracks.getTrackTextureGroupOffset(trackData));
        //    for (int i = 0; i < textureCount; i++)
        //    {
        //        listBox1.Items.Add($"Texture {(i + 1).ToString()}");
        //    }
        //    label1.Text = $"Textures For Track {trackName} Found: {textureCount}";

        //}

        public static int getTextureOffset(byte[] trackData,int textureIndex)
        {
            try
            {
                int trackGroupOffset = isTextureFile ? 0 : isModelFile ? Models.getTextureGroupOffset(trackData) : Tracks.getTrackTextureGroupOffset(trackData);
                int fileIndex = trackGroupOffset + 4; //+ 4 for the 4byte header file
                int size;
                int currentIndex = 0;
                for (int i = 0; currentIndex < textureIndex; i++)
                {
                    size = BitConverter.ToInt16(trackData, fileIndex + 14);
                    fileIndex += 20; //Go to end of Texture Entry Header. This now points to start of data
                    fileIndex += size * 0x10; //Go to end of Texture Data to get to next Texture Entry. This points to end of data, start of next texture header
                    currentIndex++;
                }
                if (fileIndex >= trackData.Length) //If fileIndex is out of bounds, return 0
                {
                    return -1;
                }
                return fileIndex;
            } catch
            {
                return -1; //Return -1 to indicate an error
            }
        }

        public static int getPaletteOffset(byte[] trackData, uint paletteUID)
        {
            try
            {
                int trackGroupOffset = isTextureFile ? 0 : isModelFile ? Models.getTextureGroupOffset(trackData) : Tracks.getTrackTextureGroupOffset(trackData);
                ushort textureCount = BitConverter.ToUInt16(trackData, trackGroupOffset);
                ushort paletteCount = BitConverter.ToUInt16(trackData, trackGroupOffset + 2);
                int fileIndex = getTextureOffset(trackData, textureCount); //Get Last texture offset/start of palette data
                if (fileIndex == -1) return -1; //Return -1 to indicate an error
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
            catch
            {
                return -1; //Return -1 to indicate an error
            }
        }

        public static uint getTextureUID(byte[] trackData, int textureIndex)
        {
            try
            {
                int offset = getTextureOffset(trackData, textureIndex);
                return offset==-1?0: BitConverter.ToUInt32(trackData, offset);
            }
            catch (Exception ex)
            {
                return 0; //Return 0 to indicate an error
            }
        }

        public static uint getPaletteUID(byte[] trackData, int textureIndex)
        {
            try
            {
                int offset = getTextureOffset(trackData, textureIndex);
                return BitConverter.ToUInt32(trackData, offset + 4);
            }
            catch (Exception ex)
            {
                return 0; //Return 0 to indicate an error
            }
        }

        public static string getTextureFormat(byte[] trackData, int textureIndex)
        {
            try
            {
                int offset = getTextureOffset(trackData, textureIndex);
                uint format = BitConverter.ToUInt16(trackData, offset + 8);
                return format == 0 ? "Unknown" : format == 1 ? "A3I5" : format == 2 ? "4-Color Palette" : format == 3 ? "16-Color Palette" : format == 4 ? "256-Color Palette" : format == 5 ? "4x4-Texel Compressed" : format == 6 ? "A5I3" : format == 7 ? "Direct Color (RGB5551)" : "Not Documented";
            }
            catch (Exception ex)
            {
                return "Unknown";
            }
        }

        public static uint getTextureFlags(byte[] trackData, int textureIndex)
        {
            try
            {
                int offset = getTextureOffset(trackData, textureIndex);
                uint flags = BitConverter.ToUInt16(trackData, offset + 10);
                return flags;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public static string getTextureDimensions(byte[] trackData, int textureIndex)
        {
            try
            {
                int offset = getTextureOffset(trackData, textureIndex);
                uint width = BitConverter.ToUInt16(trackData, offset + 16);
                uint height = BitConverter.ToUInt16(trackData, offset + 18);
                return $"{width} x {height}";
            }
            catch (Exception ex)
            {
                return "Unknown";
            }

        }

        public static int getPaletteSize(byte[] trackData, uint paletteUID)
        {
            try
            {
                int offset = getPaletteOffset(trackData, paletteUID);
                if (offset == -1) //If palette not found, show error
                {
                    MessageBox.Show("This asset has inconsistent file structure.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                int size = BitConverter.ToInt16(trackData, offset + 4);
                return size;
            }
            catch (Exception ex)
            {
                MessageBox.Show("This asset has inconsistent file structure.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return -1;
            }
        }
    }
}
