using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiddyKongModdingView
{
    internal class TexturePaletteExtractor
    {

        //For 256 Color Palettes
        static Bitmap ConvertTo8BitIndexed(Bitmap original, int width, int height)
        {
            // Create 8-bit indexed bitmap
            Bitmap indexed = new Bitmap(original.Width, original.Height, PixelFormat.Format8bppIndexed);

            // Set palette (we’ll use grayscale here as an example)
            ColorPalette palette = indexed.Palette;
            for (int i = 0; i < 256; i++)
                palette.Entries[i] = Color.FromArgb(i, i, i);
            indexed.Palette = palette;

            // Draw original into 8-bit bitmap
            using (Graphics g = Graphics.FromImage(indexed))
            {
                g.DrawImage(original, 0, 0);
            }

            return indexed;
        }

        static byte[] GetIndexedPixelData(Bitmap indexed)
        {
            if (indexed.PixelFormat != PixelFormat.Format8bppIndexed)
                throw new InvalidOperationException("Bitmap is not 8bpp indexed.");

            Rectangle rect = new Rectangle(0, 0, indexed.Width, indexed.Height);
            BitmapData data = indexed.LockBits(rect, ImageLockMode.ReadOnly, indexed.PixelFormat);

            int stride = data.Stride;
            int height = data.Height;
            int width = data.Width;
            byte[] pixels = new byte[height * stride];

            System.Runtime.InteropServices.Marshal.Copy(data.Scan0, pixels, 0, pixels.Length);
            indexed.UnlockBits(data);

            return pixels;
        }

        static byte[] GetPalette(Bitmap indexed)
        {
            ColorPalette palette = indexed.Palette;
            byte[] raw = new byte[256 * 2]; // 256 entries, 2 bytes per color (for BGR555)

            for (int i = 0; i < 256; i++)
            {
                Color c = palette.Entries[i];

                // Convert RGB888 to BGR555 (used on GBA/NDS)
                ushort bgr555 =
                    (ushort)(((c.B >> 3) & 0x1F) |
                             ((c.G >> 3) & 0x1F) << 5 |
                             ((c.R >> 3) & 0x1F) << 10);

                raw[i * 2 + 0] = (byte)(bgr555 & 0xFF);
                raw[i * 2 + 1] = (byte)((bgr555 >> 8) & 0xFF);
            }

            return raw;
        }

        static byte[] Detile8Bit(byte[] tiledData, int width, int height)
        {
            int tileSize = 8;
            int tilesX = width / tileSize;
            int tilesY = height / tileSize;
            byte[] output = new byte[width * height];

            int tileIndex = 0;

            for (int ty = 0; ty < tilesY; ty++)
            {
                for (int tx = 0; tx < tilesX; tx++)
                {
                    int tileOffset = tileIndex * 64; // 8x8 = 64 bytes per tile
                    for (int y = 0; y < tileSize; y++)
                    {
                        for (int x = 0; x < tileSize; x++)
                        {
                            int outX = tx * tileSize + x;
                            int outY = ty * tileSize + y;
                            int outIndex = outY * width + outX;
                            output[outIndex] = tiledData[tileOffset + y * tileSize + x];
                        }
                    }
                    tileIndex++;
                }
            }

            return output;
        }

        static byte[] Detile4Bit(byte[] tiledData, int width, int height)
        {
            int tileSize = 8;
            int tilesX = width / tileSize;
            int tilesY = height / tileSize;
            byte[] output = new byte[width * height]; // 1 byte per pixel

            int tileIndex = 0;

            for (int ty = 0; ty < tilesY; ty++)
            {
                for (int tx = 0; tx < tilesX; tx++)
                {
                    int tileOffset = tileIndex * 32; // 8x8 pixels = 64 pixels = 32 bytes
                    for (int y = 0; y < tileSize; y++)
                    {
                        for (int x = 0; x < tileSize; x += 2)
                        {
                            int byteIndex = tileOffset + (y * 4) + (x / 2);
                            byte dataByte = tiledData[byteIndex];

                            byte pixel1 = (byte)(dataByte & 0x0F);       // low nibble
                            byte pixel2 = (byte)((dataByte >> 4) & 0x0F); // high nibble

                            int outX1 = tx * tileSize + x;
                            int outX2 = tx * tileSize + x + 1;
                            int outY = ty * tileSize + y;

                            output[outY * width + outX1] = pixel1;
                            output[outY * width + outX2] = pixel2;
                        }
                    }
                    tileIndex++;
                }
            }

            return output;
        }

        static byte[] DetileDirectColor(byte[] tiledData, int width, int height)
        {
            int tileSize = 8;
            int tilesX = width / tileSize;
            int tilesY = height / tileSize;
            byte[] output = new byte[width * height * 2]; // 2 bytes per pixel

            int tileIndex = 0;

            for (int ty = 0; ty < tilesY; ty++)
            {
                for (int tx = 0; tx < tilesX; tx++)
                {
                    int tileOffset = tileIndex * 128; // 8x8 pixels × 2 bytes = 128 bytes per tile
                    for (int y = 0; y < tileSize; y++)
                    {
                        for (int x = 0; x < tileSize; x++)
                        {
                            int outX = tx * tileSize + x;
                            int outY = ty * tileSize + y;
                            int outIndex = (outY * width + outX) * 2;

                            int inIndex = tileOffset + ((y * tileSize + x) * 2);
                            output[outIndex] = tiledData[inIndex];
                            output[outIndex + 1] = tiledData[inIndex + 1];
                        }
                    }
                    tileIndex++;
                }
            }

            return output;
        }



        public static byte[] texture_Returner(string inputPath, byte[] trackData, int textureIndex)
        {
            string textureFormat = Textures.getTextureFormat(trackData, textureIndex);
            int offset = Textures.getTextureOffset(trackData, textureIndex);
            int size = BitConverter.ToInt16(trackData, offset + 14) * 0x10;
            int width = BitConverter.ToInt16(trackData, offset + 16);
            int height = BitConverter.ToInt16(trackData, offset + 18);

            byte[] data = File.ReadAllBytes(inputPath);

            if (data.Length != size)
            {
                return null;
            }

            //return data;
            if (textureFormat == "4 Bit Indexed")
                return Detile4Bit(data, width, height);
            else if (textureFormat == "Direct Color (RGB5551)")
                return DetileDirectColor(data, width, height);
            else if (textureFormat == "256-Color Palette")
                return Detile8Bit(data, width,height);
            return data; // Default case, return as is
        }
        public static byte[] palette_Returner(string inputPath, byte[] trackData, uint paletteUID)
        {
            int paletteOffset = Textures.getPaletteOffset(trackData, paletteUID);
            int paletteSize = BitConverter.ToInt16(trackData, paletteOffset + 4);

            byte[] data = File.ReadAllBytes(inputPath);

            if (data.Length != paletteSize)
            {
                return null;
            }

            return data;
        }
    }
}
