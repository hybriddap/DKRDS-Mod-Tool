using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiddyKongModdingView
{
    internal class Models
    {
        public static void populateModels(byte[] assetData, ListBox listBox1, Label label1)
        {
            int modelCount = 0;
            ushort assetCount = BitConverter.ToUInt16(assetData, 0);
            for (int i = 0; i < assetCount; i++)
            {
                if (Assets.getAssetType(assetData, Assets.getAssetOffset(assetData, i)) == 0x18)
                {
                    modelCount++;
                    listBox1.Items.Add($"Asset {(i + 1).ToString()}");
                }
            }
            label1.Text = $"Model Files Found: {modelCount}";
        }
    }
}
