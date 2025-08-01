using DiddyKongModdingView.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DiddyKongModdingView
{
    internal class TextureViewer : Form
    {
        private Bitmap bmp;
        public TextureViewer(byte[] textureData, byte[] paletteData, int width, int height, int type)
        {
            InitializeComponent();
            // Decode texture into Bitmap
            Bitmap bitmap = GetTexture(textureData, paletteData, width, height, type);
            bmp = bitmap;
            // Set up PictureBox
            PictureBox pictureBox = new PictureBox
            {
                Image = bitmap,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill,
            };

            this.Controls.Add(pictureBox);
            this.ClientSize = new Size(width * 8, height * 8); // Scale it up
            this.Text = "DKR Texture Viewer";
            this.Icon = Resources.DKRDS_icon_Diddy;
        }

        public static Bitmap GetTexture(byte[] textureData, byte[] paletteData, int width, int height, int type)
        {
            // Decode texture into Bitmap
            Bitmap bitmap = null;
            switch (type)
            {
                case 0: //unknown
                    //MessageBox.Show("Unknown Type");
                    break;
                case 1: //A3I5
                    bitmap = DecodeA3I5IndexedTexture(textureData, paletteData, width, height);
                    break;
                case 2: //4 - color
                    bitmap = Decode2bppIndexedTexture(textureData, paletteData, width, height);
                    break;
                case 3: //16 - color
                    bitmap = Decode4bppIndexedTexture(textureData, paletteData, width, height);
                    break;
                case 4: //256 - color
                    bitmap = DecodeIndexedTextureRGB555(textureData, paletteData, width, height);
                    break;
                case 5: //4x4 texel compressed (unused?)
                    //MessageBox.Show("Not Implemented");
                    break;
                case 6: //A5I3
                    bitmap = DecodeA5I3IndexedTexture(textureData, paletteData, width, height);
                    break;
                case 7: //Direct color (RGB5551)
                    bitmap = DecodeRGB5551Texture(textureData, width, height);
                    break;
                default:
                    //MessageBox.Show("Not Implemented");
                    break;
            }
            return bitmap;
        }

        private static Bitmap DecodeRGB5551Texture(byte[] texture, int width, int height)
        {
            if (texture.Length != width * height * 2)
                throw new ArgumentException("Texture size must be width * height * 2 for RGB5551 format.");

            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int offset = (y * width + x) * 2;
                    ushort value = BitConverter.ToUInt16(texture, offset); // Little-endian by default

                    int r = (value >> 10) & 0x1F;
                    int g = (value >> 5) & 0x1F;
                    int b = value & 0x1F;
                    int a = (value >> 15) & 0x01;

                    // Scale 5-bit RGB to 0–255
                    r = (r * 255) / 31;
                    g = (g * 255) / 31;
                    b = (b * 255) / 31;
                    a = a == 1 ? 255 : 0;

                    bmp.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }

            return bmp;
        }


        private static Bitmap DecodeA5I3IndexedTexture(byte[] texture, byte[] palette, int width, int height)
        {
            if (palette.Length != 8 * 2)
                throw new ArgumentException("Expected 16 bytes for 8-color RGB555 palette.");
            if (texture.Length != width * height)
                throw new ArgumentException("Texture size doesn't match width * height.");

            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte b = texture[y * width + x];

                    int alpha5 = (b >> 3) & 0x1F; // bits 3-7
                    int index = b & 0x07;         // bits 0-2

                    // Scale alpha (0–31) to 0–255
                    int alpha = (alpha5 * 255) / 31;

                    ushort color = BitConverter.ToUInt16(palette, index * 2);
                    int r = (color & 0x1F) << 3;
                    int g = ((color >> 5) & 0x1F) << 3;
                    int bgr = ((color >> 10) & 0x1F) << 3;

                    bmp.SetPixel(x, y, Color.FromArgb(alpha, r, g, bgr));
                }
            }

            return bmp;
        }


        private static Bitmap DecodeA3I5IndexedTexture(byte[] texture, byte[] palette, int width, int height)
        {
            if (palette.Length != 32 * 2)
                throw new ArgumentException("Expected 64 bytes for 32-color RGB555 palette.");
            if (texture.Length != width * height)
                throw new ArgumentException("Texture size doesn't match width * height.");

            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    byte b = texture[y * width + x];

                    int index = b & 0x1F; // bits 0-4
                    int alpha3 = (b >> 5) & 0x07; // bits 5-7

                    // Scale alpha from 0–7 to 0–255
                    int alpha = (alpha3 * 255) / 7;

                    ushort color = BitConverter.ToUInt16(palette, index * 2);
                    int r = (color & 0x1F) << 3;
                    int g = ((color >> 5) & 0x1F) << 3;
                    int bgr = ((color >> 10) & 0x1F) << 3;

                    bmp.SetPixel(x, y, Color.FromArgb(alpha, r, g, bgr));
                }
            }

            return bmp;
        }


        private static Color DecodeRGB555Color(byte[] palette, int index)
        {
            ushort color = BitConverter.ToUInt16(palette, index * 2);

            int r = (color & 0x1F) << 3;
            int g = ((color >> 5) & 0x1F) << 3;
            int b = ((color >> 10) & 0x1F) << 3;

            return Color.FromArgb(r, g, b);
        }

        private static Bitmap Decode2bppIndexedTexture(byte[] texture, byte[] palette, int width, int height)
        {
            if (palette.Length != 4 * 2)
                throw new ArgumentException("Expected 8 bytes for 4-color RGB555 palette.");

            int expectedSize = (width * height) / 4; // 4 pixels per byte
            if (texture.Length != expectedSize)
                throw new ArgumentException("Texture size doesn't match width * height for 2bpp.");

            Bitmap bmp = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int byteIndex = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x += 4)
                {
                    byte b = texture[byteIndex++];

                    for (int i = 0; i < 4; i++)
                    {
                        int index = (b >> (i * 2)) & 0x03; // extract 2 bits
                        Color color = DecodeRGB555Color(palette, index);

                        if (x + i < width) // edge safety
                            bmp.SetPixel(x + i, y, color);
                    }
                }
            }

            return bmp;
        }


        private static Bitmap Decode4bppIndexedTexture(byte[] texture, byte[] palette, int width, int height)
        {
            if (palette.Length != 16 * 2)
                throw new ArgumentException("Expected 32 bytes for 16-color RGB555 palette.");

            int expectedSize = (width * height) / 2; // Two pixels per byte
            if (texture.Length != expectedSize)
                throw new ArgumentException("Texture size doesn't match width * height for 4bpp.");

            Bitmap bmp = new Bitmap(width, height);
            int byteIndex = 0;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x += 2)
                {
                    byte b = texture[byteIndex++];

                    int index1 = b & 0x0F;        // Lower 4 bits
                    int index2 = (b >> 4) & 0x0F; // Upper 4 bits

                    // Draw first pixel
                    Color c1 = DecodeRGB555Color(palette, index1);
                    bmp.SetPixel(x, y, c1);

                    // Draw second pixel
                    Color c2 = DecodeRGB555Color(palette, index2);
                    bmp.SetPixel(x + 1, y, c2);
                }
            }

            return bmp;
        }

        private void InitializeComponent()
        {
            saveImage = new Button();
            SuspendLayout();
            // 
            // saveImage
            // 
            saveImage.Dock = DockStyle.Top;
            saveImage.Location = new Point(0, 0);
            saveImage.Name = "saveImage";
            saveImage.Size = new Size(284, 23);
            saveImage.TabIndex = 0;
            saveImage.Text = "Export Image";
            saveImage.UseVisualStyleBackColor = true;
            saveImage.Click += saveImage_Click;
            // 
            // TextureViewer
            // 
            ClientSize = new Size(284, 261);
            Controls.Add(saveImage);
            Name = "TextureViewer";
            ResumeLayout(false);

        }

        private static Bitmap DecodeIndexedTextureRGB555(byte[] texture, byte[] palette, int width, int height)
        {
            if (palette.Length != 256 * 2)
                throw new ArgumentException("Expected 512 bytes for RGB555 palette.");

            if (texture.Length != width * height)
                throw new ArgumentException("Texture size doesn't match width * height.");

            Bitmap bmp = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = texture[y * width + x];

                    // Each color is 2 bytes
                    ushort color = BitConverter.ToUInt16(palette, index * 2);

                    int r = (color & 0x1F) << 3;        // 5 bits red
                    int g = ((color >> 5) & 0x1F) << 3; // 5 bits green
                    int b = ((color >> 10) & 0x1F) << 3; // 5 bits blue

                    bmp.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }

            return bmp;
        }

        private void saveImage_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "Save Image As";
                saveDialog.Filter = "PNG Image|*.png|JPEG Image|*.jpg|Bitmap Image|*.bmp";
                saveDialog.DefaultExt = "png";
                saveDialog.AddExtension = true;
                saveDialog.FileName = "image";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveDialog.FileName;
                    string ext = Path.GetExtension(filePath).ToLower();

                    // Choose format based on extension
                    ImageFormat format = ImageFormat.Png;
                    if (ext == ".jpg") format = ImageFormat.Jpeg;
                    else if (ext == ".bmp") format = ImageFormat.Bmp;

                    bmp.Save(filePath, format);
                    MessageBox.Show("Image saved to:\n" + filePath);
                }
            }
        }

        private Button saveImage;
    }
}
