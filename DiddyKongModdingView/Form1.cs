using System.Drawing;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DiddyKongModdingView
{
    public partial class BackBtn : Form
    {

        private byte[] fileData;
        private byte[] trackData;

        public BackBtn()
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
                    TexturesBtn.Enabled = true;
                    ModelsBtn.Enabled = true;
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
            int assetIndex = Int32.Parse(listBox1.SelectedItem.ToString().Split()[1]) - 1;
            int assetOffset = Assets.getAssetOffset(fileData, assetIndex);
            AssetID.Text = Assets.getAssetID(fileData, assetIndex).ToString();
            AssetType.Text = Assets.getAssetStringType(fileData, assetOffset, DecompressButton);
            AssetOffset.Text = $"{assetOffset.ToString()} Decimal";
            AssetSize.Text = $"{Assets.getAssetSize(fileData, assetIndex).ToString()}";
            CompressionType.Text = $"{Assets.getAssetCompressionType(fileData, assetOffset)}";
            UncompressedSize.Text = $"{Assets.getAssetUncompressedSize(fileData, assetOffset).ToString()}";
            UnknownFlags.Text = $"{Assets.getUnknownAssetFlags(fileData, assetOffset)}";

            goBackFunction();

            //For Textures
            if (tableLayoutPanel3.Visible)
            {
                int textureIndex = listBox1.SelectedIndex;
                TextureUID.Text = $"{Textures.getTextureUID(trackData, textureIndex)}";
                PaletteUID.Text = $"{Textures.getPaletteUID(trackData, textureIndex)}";
                TextureFormat.Text = Textures.getTextureFormat(trackData, textureIndex);
                TextureFlags.Text = $"{Textures.getTextureFlags(trackData, textureIndex)}";
                TextureDimensions.Text = Textures.getTextureDimensions(trackData, textureIndex);
                PaletteSize.Text = $"{Textures.getPaletteSize(trackData, Textures.getPaletteUID(trackData, textureIndex))}";
                ViewTextureBtn.Visible = true;

                //Set preview image
                Bitmap prevImg = Textures.showPreview(trackData, textureIndex, Textures.getPaletteUID(trackData, textureIndex));
                pictureBox1.Image = prevImg;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void AssetsBtn_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            TextureDataBtn.Visible = false;

            AssetsBtn.Enabled = false;
            TracksBtn.Enabled = true;
            TexturesBtn.Enabled = true;
            ModelsBtn.Enabled = true;

            DecompressButton.Enabled = false;
            DecompressButton.Visible = true;

            pictureBox1.Image = null; // Clear previous image
            pictureBox1.Visible = false;


            ViewTextureBtn.Visible = false;

            listBox1.Items.Clear();
            Assets.populateAssets(fileData, listBox1, label1);

            tableLayoutPanel1.Visible = true;
            tableLayoutPanel2.Visible = false;
            tableLayoutPanel3.Visible = false;
            tableLayoutPanel4.Visible = false;
        }

        private void TracksBtn_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            TextureDataBtn.Visible = false;

            TracksBtn.Enabled = false;
            AssetsBtn.Enabled = true;
            TexturesBtn.Enabled = true;
            ModelsBtn.Enabled = true;

            DecompressButton.Enabled = false;
            DecompressButton.Visible = true;

            pictureBox1.Image = null; // Clear previous image
            pictureBox1.Visible = false;

            ViewTextureBtn.Visible = false;

            listBox1.Items.Clear();
            Tracks.populateTracks(fileData, listBox1, label1);

            tableLayoutPanel1.Visible = true;
            tableLayoutPanel2.Visible = false;
            tableLayoutPanel3.Visible = false;
            tableLayoutPanel4.Visible = false;
        }

        //Textures Button Click
        private void TexturesBtn_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            TextureDataBtn.Visible = false;

            TexturesBtn.Enabled = false;
            TracksBtn.Enabled = true;
            AssetsBtn.Enabled = true;
            ModelsBtn.Enabled = true;

            DecompressButton.Enabled = false;
            DecompressButton.Visible = true;

            pictureBox1.Image = null; // Clear previous image
            pictureBox1.Visible = false;

            ViewTextureBtn.Visible = false;

            listBox1.Items.Clear();
            Textures.populateTextures(fileData, listBox1, label1);

            tableLayoutPanel1.Visible = true;
            tableLayoutPanel2.Visible = false;
            tableLayoutPanel3.Visible = false;
            tableLayoutPanel4.Visible = false;

            //ViewTextureBtn.Visible = true;
        }

        private void ModelsBtn_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            TextureDataBtn.Visible = false;

            TexturesBtn.Enabled = true;
            TracksBtn.Enabled = true;
            AssetsBtn.Enabled = true;
            ModelsBtn.Enabled = false;

            DecompressButton.Enabled = false;
            DecompressButton.Visible = true;

            pictureBox1.Image = null; // Clear previous image
            pictureBox1.Visible = false;

            ViewTextureBtn.Visible = false;

            listBox1.Items.Clear();
            Models.populateModels(fileData, listBox1, label1);

            tableLayoutPanel1.Visible = true;
            tableLayoutPanel2.Visible = false;
            tableLayoutPanel3.Visible = false;
            tableLayoutPanel4.Visible = false;
        }

        private void DecompressButton_Click(object sender, EventArgs e)
        {
            int assetIndex = Int32.Parse(listBox1.SelectedItem.ToString().Split()[1]) - 1;
            int assetOffset = Assets.getAssetOffset(fileData, assetIndex);
            string compType = Assets.getAssetCompressionType(fileData, assetOffset);
            string assetType = Assets.getAssetStringType(fileData, assetOffset, DecompressButton);
            byte[] data=Assets.decompressAsset(fileData, assetIndex, assetType,compType);
            trackData = data;

            button2.Enabled = true;
            DecompressButton.Enabled = false;
            DecompressButton.Visible = false;

            switch (assetType)
            {
                case "Track":
                    tableLayoutPanel1.Visible = false;
                    tableLayoutPanel2.Visible = true;
                    tableLayoutPanel3.Visible = false;
                    tableLayoutPanel4.Visible = false;
                    TextureDataBtn.Visible = true;
                    showTrackData(data);
                    break;
                case "Texture":
                    tableLayoutPanel1.Visible = false;
                    tableLayoutPanel2.Visible = false;
                    tableLayoutPanel3.Visible = true;
                    tableLayoutPanel4.Visible = false;
                    ViewTextureBtn.Visible = false;
                    showTextureData(data);
                    pictureBox1.Image = null; // Clear previous image
                    pictureBox1.Visible = true;
                    break;
                case "Model":
                    tableLayoutPanel1.Visible = false;
                    tableLayoutPanel2.Visible = false;
                    tableLayoutPanel3.Visible = false;
                    tableLayoutPanel4.Visible = true;
                    TextureDataBtn.Visible = true;
                    showModelData(data);
                    break;
                default:
                    MessageBox.Show("Asset decompression not supported for this type.");
                    break;
            }
        }

        private void ViewTextureBtn_Click(object sender, EventArgs e)
        {
            if (trackData == null || TextureUID.Text == null)
            {
                MessageBox.Show("Please select a texture first!");
                return;
            }
            int textureIndex = Int32.Parse(listBox1.SelectedItem.ToString().Split()[1]) - 1;
            Textures.showImage(trackData, textureIndex, Textures.getPaletteUID(trackData, textureIndex));
        }

        private void goBackFunction(bool fromBtn=false)
        {
            if (tableLayoutPanel1.Visible || tableLayoutPanel3.Visible && !fromBtn)
                return;

            pictureBox1.Image = null; // Clear previous image
            pictureBox1.Visible = false;

            button2.Enabled = false;
            DecompressButton.Visible = true;
            TextureDataBtn.Visible = false;
            ViewTextureBtn.Visible = false;

            tableLayoutPanel1.Visible = true;
            tableLayoutPanel2.Visible = false;
            tableLayoutPanel3.Visible = false;
            tableLayoutPanel4.Visible = false;

            listBox1.Items.Clear();
            if (TexturesBtn.Enabled == false)
                Textures.populateTextures(fileData, listBox1, label1);
            else if (TracksBtn.Enabled == false)
                Tracks.populateTracks(fileData, listBox1, label1);
            else if (AssetsBtn.Enabled == false)
                Assets.populateAssets(fileData, listBox1, label1);
            else if (ModelsBtn.Enabled == false)
                Models.populateModels(fileData, listBox1, label1);
        }

        private void showTrackData(byte[] trackData)
        {
            int assetIndex = Int32.Parse(listBox1.SelectedItem.ToString().Split()[1]) - 1;
            int assetID = Assets.getAssetID(fileData, assetIndex);
            TrackName.Text = assetID == 534 ? "Ancient Lake" : assetID.ToString();
            NumberOfSections.Text = $"{Tracks.getTrackSections(trackData).ToString()} Sections";
            TrackSectionGroupOffset.Text = $"{Tracks.getTrackSectionGroupOffset(trackData).ToString()} Decimal";
            NumberOfTextures.Text = $"{Tracks.getTrackNoTextures(trackData).ToString()} Textures";
            TextureGroupOffset.Text = $"{Tracks.getTrackTextureGroupOffset(trackData).ToString()} Decimal";
            FileSize.Text = $"{Tracks.getTrackFileSize(trackData).ToString()}";
        }

        private void showModelData(byte[] modelData)
        {
            int assetIndex = Int32.Parse(listBox1.SelectedItem.ToString().Split()[1]) - 1;
            int assetID = Assets.getAssetID(fileData, assetIndex);
            NoOfModels.Text = $"{Models.getNumberModels(modelData).ToString()}";
            NoOfTexturesModel.Text = $"{Models.getNumberTextures(modelData).ToString()}";
            ModelGroup.Text = $"{Models.getModelGroupOffset(modelData).ToString()} Decimal";
            TextureGroupModels.Text = $"{Models.getTextureGroupOffset(modelData).ToString()} Decimal";
            TextureRefTable.Text = $"{Models.getTextureReferenceTableOffset(modelData).ToString()} Decimal";
            UnknownVarFlags.Text = $"{Models.getUnknownVarious(modelData).ToString()}";
        }

        private void showTextureData(byte[] textureData, string name="Texture")
        {
            listBox1.Items.Clear();
            Textures.getAllTextures(textureData, listBox1, label1, name);
            pictureBox1.Image = null; // Clear previous image
            pictureBox1.Visible = true;
        }

        //Back Button Click
        private void button2_Click(object sender, EventArgs e)
        {
            goBackFunction(true);
        }

        private void TextureDataBtn_Click(object sender, EventArgs e)
        {
            bool isModelFile = tableLayoutPanel4.Visible;
            tableLayoutPanel1.Visible = false;
            tableLayoutPanel2.Visible = false;
            tableLayoutPanel3.Visible = true;
            tableLayoutPanel4.Visible = false;
            TextureDataBtn.Visible = false;
            ViewTextureBtn.Visible = true;
            if (isModelFile)
                showTextureData(trackData, "Model");
            else
                showTextureData(trackData,TrackName.Text);
        }
    }
}
