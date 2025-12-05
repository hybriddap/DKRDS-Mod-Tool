using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DiddyKongModdingView
{


    internal class TextureEncoder
    {
        static ushort RGB888toRGB555(Color c)
        {
            return (ushort)(
                ((c.R >> 3) << 0) |
                ((c.G >> 3) << 5) |
                ((c.B >> 3) << 10)
            );
        }

        private static bool ConvertToRGB5551(Bitmap bmp)
        {
            try
            {
                int width = bmp.Width;
                int height = bmp.Height;

                byte[] output = new byte[width * height * 2]; // 2 bytes per pixel
                int pos = 0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color c = bmp.GetPixel(x, y);

                        int r = c.R >> 3; // 5 bits
                        int g = c.G >> 3; // 5 bits
                        int b = c.B >> 3; // 5 bits

                        int a = (c.A > 127) ? 1 : 0; // 1-bit alpha

                        ushort rgb5551 =
                            (ushort)((r) |
                            (g << 5) |
                            (b << 10) |
                            (a << 15));

                        output[pos++] = (byte)(rgb5551 & 0xFF);
                        output[pos++] = (byte)((rgb5551 >> 8) & 0xFF);
                    }
                }
                File.WriteAllBytes("texture.bin", output);
                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Error trying to encode texture.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }

        public static void SaveToFile(byte[] data, string path)
        {
            File.WriteAllBytes(path, data);
        }

        private static bool encode_256_color(Bitmap bmp, int paletteCount, int width, int height)
        {
            try
            {
                // Quantize with Octree (matches and replaces PIL)
                OctreeQuantizer quantizer = new OctreeQuantizer();
                for (int y = 0; y < bmp.Height; y++)
                    for (int x = 0; x < bmp.Width; x++)
                        quantizer.AddColor(bmp.GetPixel(x, y));

                List<Color> palette = quantizer.GeneratePalette(paletteCount);

                // Fill missing entries
                while (palette.Count < paletteCount)
                    palette.Add(Color.Black);

                // Build Palette Binary File
                using (BinaryWriter bw = new BinaryWriter(File.Open("palette.bin", FileMode.Create)))
                {
                    foreach (var c in palette)
                    {
                        ushort rgb555 = RGB888toRGB555(c);
                        bw.Write(rgb555);
                    }
                }

                // Build indexed texture
                byte[] linear = new byte[width * height];

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color c = bmp.GetPixel(x, y);

                        // find nearest palette entry
                        int best = 0;
                        int bestDist = int.MaxValue;

                        for (int i = 0; i < paletteCount; i++)
                        {
                            int dr = c.R - palette[i].R;
                            int dg = c.G - palette[i].G;
                            int db = c.B - palette[i].B;
                            int dist = dr * dr + dg * dg + db * db;

                            if (dist < bestDist)
                            {
                                bestDist = dist;
                                best = i;
                            }
                        }

                        linear[y * width + x] = (byte)best;
                    }
                }

                // tile texture 8x8 for ds
                byte[] tiled = new byte[width * height];
                int index = 0;

                for (int ty = 0; ty < height; ty += 8)
                {
                    for (int tx = 0; tx < width; tx += 8)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            for (int x = 0; x < 8; x++)
                            {
                                int sx = tx + x;
                                int sy = ty + y;

                                tiled[index++] = linear[sy * width + sx];
                            }
                        }
                    }
                }

                File.WriteAllBytes("texture.bin", tiled);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error trying to encode 256-color texture.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private static bool encode_16_color(Bitmap bmp, int paletteCount,int width,int height)
        {
            try
            {
                // Quantize with Octree (matches and replaces PIL)
                OctreeQuantizer quantizer = new OctreeQuantizer();
                for (int y = 0; y < bmp.Height; y++)
                    for (int x = 0; x < bmp.Width; x++)
                        quantizer.AddColor(bmp.GetPixel(x, y));

                List<Color> palette = quantizer.GeneratePalette(paletteCount);

                // Fill missing entries
                while (palette.Count < paletteCount)
                    palette.Add(Color.Black);

                // Build Palette Binary File
                using (BinaryWriter bw = new BinaryWriter(File.Open("palette.bin", FileMode.Create)))
                {
                    foreach (var c in palette)
                    {
                        ushort rgb555 = RGB888toRGB555(c);
                        bw.Write(rgb555);
                    }
                }

                // Build indexed texture
                byte[] linear = new byte[(width * height + 1) / 2]; // each byte stores 2 pixels


                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color c = bmp.GetPixel(x, y);

                        int best = 0;
                        int bestDist = int.MaxValue;

                        for (int i = 0; i < paletteCount; i++)
                        {
                            int dr = c.R - palette[i].R;
                            int dg = c.G - palette[i].G;
                            int db = c.B - palette[i].B;
                            int dist = dr * dr + dg * dg + db * db;

                            if (dist < bestDist)
                            {
                                bestDist = dist;
                                best = i;
                            }
                        }

                        int idx = (y * width + x) / 2;
                        if ((x & 1) == 0)
                            linear[idx] = (byte)(best << 4); // high nibble
                        else
                            linear[idx] |= (byte)(best & 0x0F); // low nibble
                    }
                }


                // tile texture 8x8 for ds
                byte[] tiled = new byte[linear.Length];
                int index = 0;

                for (int ty = 0; ty < height; ty += 8)
                {
                    for (int tx = 0; tx < width; tx += 8)
                    {
                        for (int y = 0; y < 8; y++)
                        {
                            for (int x = 0; x < 8; x += 2)
                            {
                                int sx = tx + x;
                                int sy = ty + y;

                                int linearIndex1 = (sy * width + sx) / 2;
                                int linearIndex2 = (sy * width + sx + 1) / 2;

                                byte b1 = linear[linearIndex1];
                                byte b2 = linear[linearIndex2];

                                // Combine the two nibbles
                                byte combined = (byte)((b1 & 0xF0) | ((b2 & 0xF0) >> 4));
                                tiled[index++] = combined;
                            }
                        }
                    }
                }
                File.WriteAllBytes("texture.bin", tiled);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error trying to encode  16-color texture.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private static byte encodeA3I5(Color c)
        {
            // Convert alpha to 3 bits
            int alpha3 = (c.A * 7 / 255) & 0x7;

            // Convert intensity (grayscale) to 5 bits
            int intensity = (c.R * 31 / 255) & 0x1F; // R=G=B for grayscale

            return (byte)((alpha3 << 5) | intensity);
        }


        private static bool encode_A3I5(Bitmap bmp, int paletteCount, int width, int height)
        {
            try
            {
                OctreeQuantizer quantizer = new OctreeQuantizer();
                for (int y = 0; y < bmp.Height; y++)
                    for (int x = 0; x < bmp.Width; x++)
                        quantizer.AddColor(bmp.GetPixel(x, y));

                List<Color> palette = quantizer.GeneratePalette(paletteCount);

                // Fill missing entries
                while (palette.Count < paletteCount)
                    palette.Add(Color.Black);

                // Build Palette Binary File
                using (BinaryWriter bw = new BinaryWriter(File.Open("palette.bin", FileMode.Create)))
                {
                    foreach (var c in palette)
                    {
                        ushort rgb555 = RGB888toRGB555(c);
                        bw.Write(rgb555);
                    }
                }

                byte[] result = new byte[width * height];
                int index = 0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color c = bmp.GetPixel(x, y);
                        result[index++] = encodeA3I5(c);
                    }
                }

                File.WriteAllBytes("texture.bin", result);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error trying to encode A3I5 texture.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public static bool encodeImage(string inputFile, byte[] trackData, int textureIndex)
        {
            int paletteCount;
            string textureFormat = Textures.getTextureFormat(trackData, textureIndex);
            int offset = Textures.getTextureOffset(trackData, textureIndex);
            int size = BitConverter.ToInt16(trackData, offset + 14) * 0x10;
            int width = BitConverter.ToInt16(trackData, offset + 16);
            int height = BitConverter.ToInt16(trackData, offset + 18);

            if (inputFile == null)
                throw new Exception("There are no params");

            Bitmap bmp = new Bitmap(Image.FromFile(inputFile), new Size(width, height));

            if (textureFormat == "16-Color Palette")
                return encode_16_color(bmp, 16, width, height);
            else if (textureFormat == "Direct Color (RGB5551)")
                return ConvertToRGB5551(bmp);
            else if (textureFormat == "256-Color Palette")
                return encode_256_color(bmp, 256, width, height);
            else if (textureFormat == "A3I5")
                return encode_A3I5(bmp, 16, width, height);
            else return false;
        }
    }

}
