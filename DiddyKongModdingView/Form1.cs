using System.Drawing;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DiddyKongModdingView
{
    public partial class Form1 : Form
    {

        private byte[] fileData;
        private byte[] trackData;

        public Form1()
        {
            InitializeComponent();
        }

        private void OpenFileAndReadBytes()
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    string path = ofd.FileName;

                    fileData = File.ReadAllBytes(path);

                    ushort assetCount = BitConverter.ToUInt16(fileData, 0);

                    MessageBox.Show($"File Successfully Read.");
                    label1.Text = $"Assets Found: {assetCount}";

                    populateAssets();

                    TracksBtn.Enabled = true;
                    OptionsBtn.Enabled = true;
                }
            }
        }

        private void populateAssets()
        {
            ushort assetCount = BitConverter.ToUInt16(fileData, 0);
            for (int i = 0; i < assetCount; i++)
            {
                listBox1.Items.Add($"Asset {(i + 1).ToString()}");
            }
            label1.Text = $"Assets Found: {assetCount}";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            OpenFileAndReadBytes();
        }

        private ushort getAssetID(int assetIndex)
        {
            return BitConverter.ToUInt16(fileData, 4 + assetIndex * 2);
        }

        private int getAssetOffset(int assetIndex)
        {
            ushort assetCount = BitConverter.ToUInt16(fileData, 0);
            return BitConverter.ToInt32(fileData, 6 + assetCount * 2 + assetIndex * 8);
        }

        private string getAssetSize(int assetIndex)
        {
            ushort assetCount = BitConverter.ToUInt16(fileData, 0);
            return FormatSize(BitConverter.ToUInt32(fileData, 10 + assetCount * 2 + assetIndex * 8));
        }

        private string getAssetCompressionType(int assetOffset)
        {
            byte compressionValue = fileData[assetOffset];
            return (compressionValue == 0) ? "No Compression" : (compressionValue == 1) ? "Huffman" : (compressionValue == 2) ? "LZ77" : (compressionValue == 3) ? "Unknown" : (compressionValue == 4) ? "RunLength" : "Not Documented";
        }

        private string getUnknownAssetFlags(int assetOffset)
        {
            byte byte1 = fileData[assetOffset + 1];
            byte byte2 = fileData[assetOffset + 2];
            byte byte3 = fileData[assetOffset + 3];
            return $"{byte1 + byte2 + byte3}";
        }

        private byte getAssetType(int assetOffset)
        {
            return fileData[assetOffset + 2];
        }
        private string getAssetUncompressedSize(int assetOffset)
        {
            return FormatSize(BitConverter.ToUInt32(fileData, assetOffset + 4));
        }

        private uint getTrackSections()
        {
            return BitConverter.ToUInt32(trackData, 0);
        }

        private uint getTrackSectionGroupOffset()
        {
            return BitConverter.ToUInt32(trackData, 4);
        }

        private uint getTrackNoTextures()
        {
            return BitConverter.ToUInt32(trackData, 24);
        }

        private int getTrackTextureGroupOffset()
        {
            return BitConverter.ToInt32(trackData, 28);
        }

        private string getTrackFileSize()
        {
            return FormatSize(BitConverter.ToUInt32(trackData, 52));
        }

        private void populateTracks()
        {
            int trackCount = 0;
            ushort assetCount = BitConverter.ToUInt16(fileData, 0);
            for (int i = 0; i < assetCount; i++)
            {
                if (getAssetType(getAssetOffset(i)) == 0x9A)
                {
                    trackCount++;
                    listBox1.Items.Add($"Asset {(i + 1).ToString()}");
                }
            }
            label1.Text = $"Tracks Found: {trackCount}";
        }

        private void decompressTrack(int assetIndex)
        {
            int assetOffset = getAssetOffset(assetIndex) + 8;
            ushort assetCount = BitConverter.ToUInt16(fileData, 0);
            uint assetSize = BitConverter.ToUInt32(fileData, 10 + assetCount * 2 + assetIndex * 8);
            byte[] result = new byte[assetSize];
            Array.Copy(fileData, assetOffset, result, 0, assetSize);
            trackData = Decompressor.LZ77_Decompress(result);
        }

        private void populateTextures()
        {
            if (trackData == null)
            {
                MessageBox.Show("Please select a track first!");
                return;
            }

            ushort textureCount = BitConverter.ToUInt16(trackData, getTrackTextureGroupOffset());
            for (int i = 0; i < textureCount; i++)
            {
                listBox1.Items.Add($"Texture {(i + 1).ToString()}");
            }
            label1.Text = $"Textures For Track {TrackName.Text} Found: {textureCount}";

        }

        private int getTextureOffset(int textureIndex)
        {
            int fileIndex = getTrackTextureGroupOffset() + 4; //+ 4 for the 4byte header file
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

        private int getPaletteOffset(uint paletteUID)
        {
            ushort textureCount = BitConverter.ToUInt16(trackData, getTrackTextureGroupOffset());
            ushort paletteCount = BitConverter.ToUInt16(trackData, getTrackTextureGroupOffset() + 2);
            int fileIndex = getTextureOffset(textureCount); //Get Last texture offset/start of palette data

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

        private uint getTextureUID(int textureIndex)
        {
            int offset = getTextureOffset(textureIndex);
            return BitConverter.ToUInt32(trackData, offset);
        }

        private uint getPaletteUID(int textureIndex)
        {
            int offset = getTextureOffset(textureIndex);
            return BitConverter.ToUInt32(trackData, offset + 4);
        }

        private string getTextureFormat(int textureIndex)
        {
            int offset = getTextureOffset(textureIndex);
            uint format = BitConverter.ToUInt16(trackData, offset + 8);
            return format == 0 ? "Unknown" : format == 1 ? "A3I5" : format == 2 ? "4-Color Palette" : format == 3 ? "16-Color Palette" : format == 4 ? "256-Color Palette" : format == 5 ? "4x4-Texel Compressed" : format == 6 ? "A5I3" : format == 7 ? "Direct Color (RGB5551)" : "Not Documented";
        }

        private uint getTextureFlags(int textureIndex)
        {
            int offset = getTextureOffset(textureIndex);
            uint flags = BitConverter.ToUInt16(trackData, offset + 10);
            return flags;
        }

        private string getTextureDimensions(int textureIndex)
        {
            int offset = getTextureOffset(textureIndex);
            uint width = BitConverter.ToUInt16(trackData, offset + 16);
            uint height = BitConverter.ToUInt16(trackData, offset + 18);
            return $"{width} x {height}";
        }

        private int getPaletteSize(uint paletteUID)
        {
            int offset = getPaletteOffset(paletteUID);
            int size = BitConverter.ToInt16(trackData, offset + 4);
            return size;
        }

        private void showImage(int textureIndex, uint paletteUID)
        {
            //for texture
            int offset = getTextureOffset(textureIndex);
            int width = BitConverter.ToInt16(trackData, offset + 16);
            int height = BitConverter.ToInt16(trackData, offset + 18);
            int size = BitConverter.ToInt16(trackData, offset + 14) * 0x10;
            
            int format = BitConverter.ToInt16(trackData, offset + 8);

            //for palette
            int paletteOffset = getPaletteOffset(paletteUID);
            int paletteSize = BitConverter.ToInt16(trackData, paletteOffset + 4);

            //data
            byte[] textureData = trackData.Skip(offset + 20).Take(size).ToArray();
            byte[] paletteData = trackData.Skip(paletteOffset + 8).Take(paletteSize).ToArray();

            TextureViewer viewer = new TextureViewer(textureData, paletteData, width, height, format);
            viewer.Show();
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int assetOffset = getAssetOffset(listBox1.SelectedIndex);
            AssetID.Text = getAssetID(listBox1.SelectedIndex).ToString();
            AssetOffset.Text = $"{assetOffset.ToString()} Decimal";
            AssetSize.Text = $"{getAssetSize(listBox1.SelectedIndex).ToString()}";
            CompressionType.Text = $"{getAssetCompressionType(assetOffset)}";
            UncompressedSize.Text = $"{getAssetUncompressedSize(assetOffset).ToString()}";
            UnknownFlags.Text = $"{getUnknownAssetFlags(assetOffset)}";

            //For Tracks
            if (TracksBtn.Enabled == false)
            {
                int assetIndex = Int32.Parse(listBox1.SelectedItem.ToString().Split()[1]) - 1;
                int assetID = getAssetID(assetIndex);
                decompressTrack(assetIndex);
                TrackName.Text = assetID == 534 ? "Ancient Lake" : assetID.ToString();
                NumberOfSections.Text = $"{getTrackSections().ToString()} Sections";
                TrackSectionGroupOffset.Text = $"{getTrackSectionGroupOffset().ToString()} Decimal";
                NumberOfTextures.Text = $"{getTrackNoTextures().ToString()} Textures";
                TextureGroupOffset.Text = $"{getTrackTextureGroupOffset().ToString()} Decimal";
                FileSize.Text = $"{getTrackFileSize().ToString()}";
            }

            //For Textures
            if (OptionsBtn.Enabled == false)
            {
                int textureIndex = listBox1.SelectedIndex;
                TextureUID.Text = $"{getTextureUID(textureIndex)}";
                PaletteUID.Text = $"{getPaletteUID(textureIndex)}";
                TextureFormat.Text = getTextureFormat(textureIndex);
                TextureFlags.Text = $"{getTextureFlags(textureIndex)}";
                TextureDimensions.Text = getTextureDimensions(textureIndex);
                PaletteSize.Text = $"{getPaletteSize(getPaletteUID(textureIndex))}";
            }
        }

        private static string FormatSize(long byteCount)
        {
            if (byteCount < 1000)
                return $"{byteCount} Bytes";
            double kb = byteCount / 1000.0;
            return $"{kb:F2} KB";
        }

        private void AssetsBtn_Click(object sender, EventArgs e)
        {
            AssetsBtn.Enabled = false;
            TracksBtn.Enabled = true;
            OptionsBtn.Enabled = true;

            ViewTextureBtn.Visible = false;

            listBox1.Items.Clear();
            populateAssets();
            tableLayoutPanel1.Visible = true;
            tableLayoutPanel2.Visible = false;
            tableLayoutPanel3.Visible = false;
        }

        private void TracksBtn_Click(object sender, EventArgs e)
        {
            TracksBtn.Enabled = false;
            AssetsBtn.Enabled = true;
            OptionsBtn.Enabled = true;

            ViewTextureBtn.Visible = false;

            listBox1.Items.Clear();
            populateTracks();
            tableLayoutPanel1.Visible = false;
            tableLayoutPanel2.Visible = true;
            tableLayoutPanel3.Visible = false;
        }

        private void OptionsBtn_Click(object sender, EventArgs e)
        {
            OptionsBtn.Enabled = false;
            TracksBtn.Enabled = true;
            AssetsBtn.Enabled = true;

            listBox1.Items.Clear();
            tableLayoutPanel1.Visible = false;
            tableLayoutPanel2.Visible = false;
            tableLayoutPanel3.Visible = true;

            ViewTextureBtn.Visible = true;

            populateTextures();
        }

        private void ViewTextureBtn_Click(object sender, EventArgs e)
        {
            if (trackData == null || OptionsBtn.Enabled==true || TextureUID.Text==null)
            {
                MessageBox.Show("Please select a texture first!");
                return;
            }

            int textureIndex = listBox1.SelectedIndex;
            showImage(textureIndex, getPaletteUID(textureIndex));
        }
    }
}
