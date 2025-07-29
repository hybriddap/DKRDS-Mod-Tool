using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiddyKongModdingView
{
    internal class Models
    {
        public static bool earlierVersion = false; //Used to determine if the model is from an earlier version of the game or not

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

        public static string getUnknownVarious(byte[] modelData)
        {
            int value = BitConverter.ToUInt16(modelData, 2);
            string binary = Convert.ToString(value & 0xFF, 2).PadLeft(8, '0');
            return binary;
        }

        public static int getNumberModels(byte[] modelData)
        {
            return BitConverter.ToUInt16(modelData, 4);
        }

        public static int getNumberTextures(byte[] modelData)
        {
            return BitConverter.ToUInt16(modelData, 8);
        }

        public static int getModelGroupOffset(byte[] modelData)
        {
            if (BitConverter.ToInt32(modelData, 0x18) != 0)
            {
                earlierVersion = true;
                return BitConverter.ToInt32(modelData, 0x18);
            }
            else
            {
                earlierVersion = false;
                return BitConverter.ToInt32(modelData, 0x1C);
            }
        }

        public static int getTextureGroupOffset(byte[] modelData)
        {
            if (earlierVersion)
            {
                return BitConverter.ToInt32(modelData, 0x1C);
            }
            else
            {
                return BitConverter.ToInt32(modelData, 0x20);
            }
        }

        public static int getTextureReferenceTableOffset(byte[] modelData)
        {
            if (earlierVersion)
            {
                return BitConverter.ToInt32(modelData, 0x20);
            }
            else
            {
                return BitConverter.ToInt32(modelData, 0x24);
            }
        }
    }
}
