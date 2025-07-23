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

                    Assets.populateAssets(fileData, listBox1, label1);

                    TracksBtn.Enabled = true;
                    OptionsBtn.Enabled = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            OpenFileAndReadBytes();
        }

        private void listBox1_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            int assetOffset = Assets.getAssetOffset(fileData, listBox1.SelectedIndex);
            AssetID.Text = Assets.getAssetID(fileData,listBox1.SelectedIndex).ToString();
            AssetOffset.Text = $"{assetOffset.ToString()} Decimal";
            AssetSize.Text = $"{Assets.getAssetSize(fileData, listBox1.SelectedIndex).ToString()}";
            CompressionType.Text = $"{Assets.getAssetCompressionType(fileData, assetOffset)}";
            UncompressedSize.Text = $"{Assets.getAssetUncompressedSize(fileData, assetOffset).ToString()}";
            UnknownFlags.Text = $"{Assets.getUnknownAssetFlags(fileData, assetOffset)}";

            //For Tracks
            if (TracksBtn.Enabled == false)
            {
                int assetIndex = Int32.Parse(listBox1.SelectedItem.ToString().Split()[1]) - 1;
                int assetID = Assets.getAssetID(fileData, assetIndex);
                trackData = Tracks.decompressTrack(fileData,assetIndex);
                TrackName.Text = assetID == 534 ? "Ancient Lake" : assetID.ToString();
                NumberOfSections.Text = $"{Tracks.getTrackSections(trackData).ToString()} Sections";
                TrackSectionGroupOffset.Text = $"{Tracks.getTrackSectionGroupOffset(trackData).ToString()} Decimal";
                NumberOfTextures.Text = $"{Tracks.getTrackNoTextures(trackData).ToString()} Textures";
                TextureGroupOffset.Text = $"{Tracks.getTrackTextureGroupOffset(trackData).ToString()} Decimal";
                FileSize.Text = $"{Tracks.getTrackFileSize(trackData).ToString()}";
            }

            //For Textures
            if (OptionsBtn.Enabled == false)
            {
                int textureIndex = listBox1.SelectedIndex;
                TextureUID.Text = $"{Textures.getTextureUID(trackData,textureIndex)}";
                PaletteUID.Text = $"{Textures.getPaletteUID(trackData, textureIndex)}";
                TextureFormat.Text = Textures.getTextureFormat(trackData, textureIndex);
                TextureFlags.Text = $"{Textures.getTextureFlags(trackData, textureIndex)}";
                TextureDimensions.Text = Textures.getTextureDimensions(trackData, textureIndex);
                PaletteSize.Text = $"{Textures.getPaletteSize(trackData, Textures.getPaletteUID(trackData, textureIndex))}";
            }
        }

        private void AssetsBtn_Click(object sender, EventArgs e)
        {
            AssetsBtn.Enabled = false;
            TracksBtn.Enabled = true;
            OptionsBtn.Enabled = true;

            ViewTextureBtn.Visible = false;

            listBox1.Items.Clear();
            Assets.populateAssets(fileData,listBox1,label1);
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
            Tracks.populateTracks(fileData,listBox1,label1);
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

            Textures.populateTextures(trackData,listBox1,label1,TrackName.Text);
        }

        private void ViewTextureBtn_Click(object sender, EventArgs e)
        {
            if (trackData == null || OptionsBtn.Enabled==true || TextureUID.Text==null)
            {
                MessageBox.Show("Please select a texture first!");
                return;
            }

            int textureIndex = listBox1.SelectedIndex;
            Textures.showImage(trackData,textureIndex, Textures.getPaletteUID(trackData, textureIndex));
        }
    }
}
