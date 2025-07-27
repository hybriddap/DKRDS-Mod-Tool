using DiddyKongModdingView.Properties;

namespace DiddyKongModdingView
{
    partial class BackBtn
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button1 = new Button();
            listBox1 = new ListBox();
            label1 = new Label();
            label2 = new Label();
            label4 = new Label();
            label3 = new Label();
            AssetOffset = new TextBox();
            AssetID = new TextBox();
            AssetSize = new TextBox();
            label6 = new Label();
            label5 = new Label();
            CompressionType = new TextBox();
            UncompressedSize = new TextBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            label8 = new Label();
            UnknownFlags = new TextBox();
            label21 = new Label();
            AssetType = new TextBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            AssetsBtn = new Button();
            TracksBtn = new Button();
            ModelsBtn = new Button();
            TexturesBtn = new Button();
            label7 = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            TextureGroupOffset = new TextBox();
            NumberOfTextures = new TextBox();
            TrackSectionGroupOffset = new TextBox();
            NumberOfSections = new TextBox();
            TrackName = new TextBox();
            FileSize = new TextBox();
            label14 = new Label();
            label13 = new Label();
            label12 = new Label();
            label11 = new Label();
            label9 = new Label();
            label10 = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            TextureDimensions = new TextBox();
            TextureFlags = new TextBox();
            TextureFormat = new TextBox();
            PaletteUID = new TextBox();
            TextureUID = new TextBox();
            PaletteSize = new TextBox();
            label15 = new Label();
            label16 = new Label();
            label17 = new Label();
            label18 = new Label();
            label19 = new Label();
            label20 = new Label();
            ViewTextureBtn = new Button();
            DecompressButton = new Button();
            button2 = new Button();
            TextureDataBtn = new Button();
            pictureBox1 = new PictureBox();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(618, 426);
            button1.Name = "button1";
            button1.Size = new Size(154, 23);
            button1.TabIndex = 1;
            button1.Text = "Open assets.bin File";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(12, 84);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(231, 334);
            listBox1.TabIndex = 2;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 430);
            label1.Name = "label1";
            label1.Size = new Size(139, 15);
            label1.TabIndex = 3;
            label1.Text = "Total Asset Files Found: 0";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 3);
            label2.Name = "label2";
            label2.Size = new Size(52, 15);
            label2.TabIndex = 4;
            label2.Text = "Asset ID:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 91);
            label4.Name = "label4";
            label4.Size = new Size(73, 15);
            label4.TabIndex = 6;
            label4.Text = "Asset Offset:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 148);
            label3.Name = "label3";
            label3.Size = new Size(61, 15);
            label3.TabIndex = 7;
            label3.Text = "Asset Size:";
            // 
            // AssetOffset
            // 
            AssetOffset.Location = new Point(146, 94);
            AssetOffset.Name = "AssetOffset";
            AssetOffset.ReadOnly = true;
            AssetOffset.Size = new Size(94, 23);
            AssetOffset.TabIndex = 11;
            // 
            // AssetID
            // 
            AssetID.Location = new Point(146, 6);
            AssetID.Name = "AssetID";
            AssetID.ReadOnly = true;
            AssetID.Size = new Size(63, 23);
            AssetID.TabIndex = 12;
            // 
            // AssetSize
            // 
            AssetSize.Location = new Point(146, 151);
            AssetSize.Name = "AssetSize";
            AssetSize.ReadOnly = true;
            AssetSize.Size = new Size(94, 23);
            AssetSize.TabIndex = 13;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 196);
            label6.Name = "label6";
            label6.Size = new Size(108, 15);
            label6.TabIndex = 14;
            label6.Text = "Compression Type:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 246);
            label5.Name = "label5";
            label5.Size = new Size(112, 15);
            label5.TabIndex = 15;
            label5.Text = "Uncompressed Size:";
            // 
            // CompressionType
            // 
            CompressionType.Location = new Point(146, 199);
            CompressionType.Name = "CompressionType";
            CompressionType.ReadOnly = true;
            CompressionType.Size = new Size(94, 23);
            CompressionType.TabIndex = 16;
            // 
            // UncompressedSize
            // 
            UncompressedSize.Location = new Point(146, 249);
            UncompressedSize.Name = "UncompressedSize";
            UncompressedSize.ReadOnly = true;
            UncompressedSize.Size = new Size(94, 23);
            UncompressedSize.TabIndex = 17;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble;
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(AssetOffset, 1, 2);
            tableLayoutPanel1.Controls.Add(label4, 0, 2);
            tableLayoutPanel1.Controls.Add(label2, 0, 0);
            tableLayoutPanel1.Controls.Add(AssetID, 1, 0);
            tableLayoutPanel1.Controls.Add(label8, 0, 6);
            tableLayoutPanel1.Controls.Add(UnknownFlags, 1, 6);
            tableLayoutPanel1.Controls.Add(UncompressedSize, 1, 5);
            tableLayoutPanel1.Controls.Add(label5, 0, 5);
            tableLayoutPanel1.Controls.Add(label6, 0, 4);
            tableLayoutPanel1.Controls.Add(CompressionType, 1, 4);
            tableLayoutPanel1.Controls.Add(label3, 0, 3);
            tableLayoutPanel1.Controls.Add(AssetSize, 1, 3);
            tableLayoutPanel1.Controls.Add(label21, 0, 1);
            tableLayoutPanel1.Controls.Add(AssetType, 1, 1);
            tableLayoutPanel1.Location = new Point(258, 84);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 7;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 47.55245F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 52.44755F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 47F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 32F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
            tableLayoutPanel1.Size = new Size(283, 337);
            tableLayoutPanel1.TabIndex = 19;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(6, 281);
            label8.Name = "label8";
            label8.Size = new Size(91, 15);
            label8.TabIndex = 18;
            label8.Text = "Unknown Flags:";
            // 
            // UnknownFlags
            // 
            UnknownFlags.Location = new Point(146, 284);
            UnknownFlags.Name = "UnknownFlags";
            UnknownFlags.ReadOnly = true;
            UnknownFlags.Size = new Size(94, 23);
            UnknownFlags.TabIndex = 19;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(6, 45);
            label21.Name = "label21";
            label21.Size = new Size(66, 15);
            label21.TabIndex = 20;
            label21.Text = "Asset Type:";
            // 
            // AssetType
            // 
            AssetType.Location = new Point(146, 48);
            AssetType.Name = "AssetType";
            AssetType.ReadOnly = true;
            AssetType.Size = new Size(94, 23);
            AssetType.TabIndex = 21;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(AssetsBtn);
            flowLayoutPanel1.Controls.Add(TracksBtn);
            flowLayoutPanel1.Controls.Add(ModelsBtn);
            flowLayoutPanel1.Controls.Add(TexturesBtn);
            flowLayoutPanel1.Location = new Point(12, 47);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(231, 31);
            flowLayoutPanel1.TabIndex = 20;
            // 
            // AssetsBtn
            // 
            AssetsBtn.Enabled = false;
            AssetsBtn.Location = new Point(3, 3);
            AssetsBtn.Name = "AssetsBtn";
            AssetsBtn.Size = new Size(41, 23);
            AssetsBtn.TabIndex = 0;
            AssetsBtn.Text = "All";
            AssetsBtn.UseVisualStyleBackColor = true;
            AssetsBtn.Click += AssetsBtn_Click;
            // 
            // TracksBtn
            // 
            TracksBtn.Enabled = false;
            TracksBtn.Location = new Point(50, 3);
            TracksBtn.Name = "TracksBtn";
            TracksBtn.Size = new Size(48, 23);
            TracksBtn.TabIndex = 1;
            TracksBtn.Text = "Tracks";
            TracksBtn.UseVisualStyleBackColor = true;
            TracksBtn.Click += TracksBtn_Click;
            // 
            // ModelsBtn
            // 
            ModelsBtn.Enabled = false;
            ModelsBtn.Location = new Point(104, 3);
            ModelsBtn.Name = "ModelsBtn";
            ModelsBtn.Size = new Size(55, 23);
            ModelsBtn.TabIndex = 3;
            ModelsBtn.Text = "Models";
            ModelsBtn.UseVisualStyleBackColor = true;
            ModelsBtn.Click += ModelsBtn_Click;
            // 
            // TexturesBtn
            // 
            TexturesBtn.Enabled = false;
            TexturesBtn.Location = new Point(165, 3);
            TexturesBtn.Name = "TexturesBtn";
            TexturesBtn.Size = new Size(58, 23);
            TexturesBtn.TabIndex = 2;
            TexturesBtn.Text = "Textures";
            TexturesBtn.UseVisualStyleBackColor = true;
            TexturesBtn.Click += TexturesBtn_Click;
            // 
            // label7
            // 
            label7.Anchor = AnchorStyles.Top;
            label7.AutoSize = true;
            label7.Font = new Font("Yu Gothic", 19.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(278, 38);
            label7.Name = "label7";
            label7.Size = new Size(240, 35);
            label7.TabIndex = 21;
            label7.Text = "DKR DS Mod Tool";
            label7.TextAlign = ContentAlignment.TopCenter;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(TextureGroupOffset, 1, 4);
            tableLayoutPanel2.Controls.Add(NumberOfTextures, 1, 3);
            tableLayoutPanel2.Controls.Add(TrackSectionGroupOffset, 1, 2);
            tableLayoutPanel2.Controls.Add(NumberOfSections, 1, 1);
            tableLayoutPanel2.Controls.Add(TrackName, 1, 0);
            tableLayoutPanel2.Controls.Add(FileSize, 1, 5);
            tableLayoutPanel2.Controls.Add(label14, 0, 0);
            tableLayoutPanel2.Controls.Add(label13, 0, 1);
            tableLayoutPanel2.Controls.Add(label12, 0, 2);
            tableLayoutPanel2.Controls.Add(label11, 0, 3);
            tableLayoutPanel2.Controls.Add(label9, 0, 5);
            tableLayoutPanel2.Controls.Add(label10, 0, 4);
            tableLayoutPanel2.Location = new Point(258, 84);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 6;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 47.55245F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 52.44755F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 49F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableLayoutPanel2.Size = new Size(283, 337);
            tableLayoutPanel2.TabIndex = 22;
            tableLayoutPanel2.Visible = false;
            // 
            // TextureGroupOffset
            // 
            TextureGroupOffset.Location = new Point(146, 242);
            TextureGroupOffset.Name = "TextureGroupOffset";
            TextureGroupOffset.ReadOnly = true;
            TextureGroupOffset.Size = new Size(94, 23);
            TextureGroupOffset.TabIndex = 17;
            // 
            // NumberOfTextures
            // 
            NumberOfTextures.Location = new Point(146, 174);
            NumberOfTextures.Name = "NumberOfTextures";
            NumberOfTextures.ReadOnly = true;
            NumberOfTextures.Size = new Size(94, 23);
            NumberOfTextures.TabIndex = 16;
            // 
            // TrackSectionGroupOffset
            // 
            TrackSectionGroupOffset.Location = new Point(146, 117);
            TrackSectionGroupOffset.Name = "TrackSectionGroupOffset";
            TrackSectionGroupOffset.ReadOnly = true;
            TrackSectionGroupOffset.Size = new Size(94, 23);
            TrackSectionGroupOffset.TabIndex = 11;
            // 
            // NumberOfSections
            // 
            NumberOfSections.Location = new Point(146, 59);
            NumberOfSections.Name = "NumberOfSections";
            NumberOfSections.ReadOnly = true;
            NumberOfSections.Size = new Size(94, 23);
            NumberOfSections.TabIndex = 13;
            // 
            // TrackName
            // 
            TrackName.Location = new Point(146, 6);
            TrackName.Name = "TrackName";
            TrackName.ReadOnly = true;
            TrackName.Size = new Size(94, 23);
            TrackName.TabIndex = 12;
            // 
            // FileSize
            // 
            FileSize.Location = new Point(146, 294);
            FileSize.Name = "FileSize";
            FileSize.ReadOnly = true;
            FileSize.Size = new Size(94, 23);
            FileSize.TabIndex = 19;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(6, 3);
            label14.Name = "label14";
            label14.Size = new Size(73, 15);
            label14.TabIndex = 18;
            label14.Text = "Track Name:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(6, 56);
            label13.Name = "label13";
            label13.Size = new Size(115, 15);
            label13.TabIndex = 4;
            label13.Text = "Number of Sections:";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(6, 114);
            label12.Name = "label12";
            label12.Size = new Size(116, 30);
            label12.TabIndex = 7;
            label12.Text = "Track Section Group Offset:";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(6, 171);
            label11.Name = "label11";
            label11.Size = new Size(114, 15);
            label11.TabIndex = 6;
            label11.Text = "Number of Textures:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(6, 291);
            label9.Name = "label9";
            label9.Size = new Size(51, 15);
            label9.TabIndex = 15;
            label9.Text = "File Size:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(6, 239);
            label10.Name = "label10";
            label10.Size = new Size(119, 15);
            label10.TabIndex = 14;
            label10.Text = "Texture Group Offset:";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.CellBorderStyle = TableLayoutPanelCellBorderStyle.InsetDouble;
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(TextureDimensions, 1, 4);
            tableLayoutPanel3.Controls.Add(TextureFlags, 1, 3);
            tableLayoutPanel3.Controls.Add(TextureFormat, 1, 2);
            tableLayoutPanel3.Controls.Add(PaletteUID, 1, 1);
            tableLayoutPanel3.Controls.Add(TextureUID, 1, 0);
            tableLayoutPanel3.Controls.Add(PaletteSize, 1, 5);
            tableLayoutPanel3.Controls.Add(label15, 0, 0);
            tableLayoutPanel3.Controls.Add(label16, 0, 1);
            tableLayoutPanel3.Controls.Add(label17, 0, 2);
            tableLayoutPanel3.Controls.Add(label18, 0, 3);
            tableLayoutPanel3.Controls.Add(label19, 0, 5);
            tableLayoutPanel3.Controls.Add(label20, 0, 4);
            tableLayoutPanel3.Location = new Point(258, 84);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 6;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 47.55245F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 52.44755F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 49F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
            tableLayoutPanel3.Size = new Size(283, 337);
            tableLayoutPanel3.TabIndex = 23;
            tableLayoutPanel3.Visible = false;
            // 
            // TextureDimensions
            // 
            TextureDimensions.Location = new Point(146, 242);
            TextureDimensions.Name = "TextureDimensions";
            TextureDimensions.ReadOnly = true;
            TextureDimensions.Size = new Size(94, 23);
            TextureDimensions.TabIndex = 17;
            // 
            // TextureFlags
            // 
            TextureFlags.Location = new Point(146, 174);
            TextureFlags.Name = "TextureFlags";
            TextureFlags.ReadOnly = true;
            TextureFlags.Size = new Size(94, 23);
            TextureFlags.TabIndex = 16;
            // 
            // TextureFormat
            // 
            TextureFormat.Location = new Point(146, 117);
            TextureFormat.Name = "TextureFormat";
            TextureFormat.ReadOnly = true;
            TextureFormat.Size = new Size(94, 23);
            TextureFormat.TabIndex = 11;
            // 
            // PaletteUID
            // 
            PaletteUID.Location = new Point(146, 59);
            PaletteUID.Name = "PaletteUID";
            PaletteUID.ReadOnly = true;
            PaletteUID.Size = new Size(94, 23);
            PaletteUID.TabIndex = 13;
            // 
            // TextureUID
            // 
            TextureUID.Location = new Point(146, 6);
            TextureUID.Name = "TextureUID";
            TextureUID.ReadOnly = true;
            TextureUID.Size = new Size(94, 23);
            TextureUID.TabIndex = 12;
            // 
            // PaletteSize
            // 
            PaletteSize.Location = new Point(146, 294);
            PaletteSize.Name = "PaletteSize";
            PaletteSize.ReadOnly = true;
            PaletteSize.Size = new Size(94, 23);
            PaletteSize.TabIndex = 19;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(6, 3);
            label15.Name = "label15";
            label15.Size = new Size(70, 15);
            label15.TabIndex = 18;
            label15.Text = "Texture UID:";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(6, 56);
            label16.Name = "label16";
            label16.Size = new Size(68, 15);
            label16.TabIndex = 4;
            label16.Text = "Palette UID:";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(6, 114);
            label17.Name = "label17";
            label17.Size = new Size(89, 15);
            label17.TabIndex = 7;
            label17.Text = "Texture Format:";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(6, 171);
            label18.Name = "label18";
            label18.Size = new Size(37, 15);
            label18.TabIndex = 6;
            label18.Text = "Flags:";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(6, 291);
            label19.Name = "label19";
            label19.Size = new Size(69, 15);
            label19.TabIndex = 15;
            label19.Text = "Palette Size:";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(6, 239);
            label20.Name = "label20";
            label20.Size = new Size(113, 30);
            label20.TabIndex = 14;
            label20.Text = "Texture Dimensions (WxH):";
            // 
            // ViewTextureBtn
            // 
            ViewTextureBtn.Location = new Point(345, 426);
            ViewTextureBtn.Name = "ViewTextureBtn";
            ViewTextureBtn.Size = new Size(106, 23);
            ViewTextureBtn.TabIndex = 24;
            ViewTextureBtn.Text = "View Texture";
            ViewTextureBtn.UseVisualStyleBackColor = true;
            ViewTextureBtn.Visible = false;
            ViewTextureBtn.Click += ViewTextureBtn_Click;
            // 
            // DecompressButton
            // 
            DecompressButton.Enabled = false;
            DecompressButton.Location = new Point(345, 427);
            DecompressButton.Name = "DecompressButton";
            DecompressButton.Size = new Size(106, 23);
            DecompressButton.TabIndex = 25;
            DecompressButton.Text = "Decompress";
            DecompressButton.UseVisualStyleBackColor = true;
            DecompressButton.Click += DecompressButton_Click;
            // 
            // button2
            // 
            button2.Enabled = false;
            button2.Location = new Point(258, 427);
            button2.Name = "button2";
            button2.Size = new Size(58, 23);
            button2.TabIndex = 26;
            button2.Text = "Back";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // TextureDataBtn
            // 
            TextureDataBtn.Location = new Point(345, 426);
            TextureDataBtn.Name = "TextureDataBtn";
            TextureDataBtn.Size = new Size(106, 23);
            TextureDataBtn.TabIndex = 27;
            TextureDataBtn.Text = "Texture Data";
            TextureDataBtn.UseVisualStyleBackColor = true;
            TextureDataBtn.Visible = false;
            TextureDataBtn.Click += TextureDataBtn_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(547, 84);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(225, 225);
            pictureBox1.TabIndex = 28;
            pictureBox1.TabStop = false;
            pictureBox1.Visible = false;
            // 
            // BackBtn
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 461);
            Controls.Add(pictureBox1);
            Controls.Add(TextureDataBtn);
            Controls.Add(button2);
            Controls.Add(ViewTextureBtn);
            Controls.Add(label7);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(label1);
            Controls.Add(listBox1);
            Controls.Add(button1);
            Controls.Add(tableLayoutPanel3);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(DecompressButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = Resources.DKRDS_icon_Diddy;
            MaximizeBox = false;
            Name = "BackBtn";
            Text = "Diddy Kong Mod Tool";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
        private ListBox listBox1;
        private Label label1;
        private Label label2;
        private Label label4;
        private Label label3;
        private TextBox AssetOffset;
        private TextBox AssetID;
        private TextBox AssetSize;
        private Label label6;
        private Label label5;
        private TextBox CompressionType;
        private TextBox UncompressedSize;
        private TableLayoutPanel tableLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button AssetsBtn;
        private Button TracksBtn;
        private Button TexturesBtn;
        private Label label7;
        private Label label8;
        private TextBox UnknownFlags;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox TextureGroupOffset;
        private Label label9;
        private TextBox NumberOfTextures;
        private Label label10;
        private TextBox TrackSectionGroupOffset;
        private Label label11;
        private TextBox NumberOfSections;
        private Label label12;
        private Label label13;
        private TextBox TrackName;
        private Label label14;
        private TextBox FileSize;
        private TableLayoutPanel tableLayoutPanel3;
        private TextBox TextureDimensions;
        private TextBox TextureFlags;
        private TextBox TextureFormat;
        private TextBox PaletteUID;
        private TextBox TextureUID;
        private TextBox PaletteSize;
        private Label label15;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label20;
        private Button ViewTextureBtn;
        private Label label21;
        private TextBox AssetType;
        private Button DecompressButton;
        private Button ModelsBtn;
        private Button button2;
        private Button TextureDataBtn;
        private PictureBox pictureBox1;
    }
}
