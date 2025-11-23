using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace DiddyKongModdingView
{

    //Class to replace PIL from Python
    //Essentially class to encode palette data.
    internal class OctreeQuantizer
    {
        private class OctreeNode
        {
            public bool IsLeaf;
            public int PixelCount;
            public int Red, Green, Blue;
            public OctreeNode[] Children = new OctreeNode[8];
            public OctreeNode Parent;
            public int PaletteIndex;

            public void AddColor(int r, int g, int b, int level)
            {
                PixelCount++;
                Red += r;
                Green += g;
                Blue += b;

                if (level == 8)
                {
                    IsLeaf = true;
                    return;
                }

                int index =
                    ((r >> (7 - level)) & 1) << 2 |
                    ((g >> (7 - level)) & 1) << 1 |
                    ((b >> (7 - level)) & 1);

                if (Children[index] == null)
                {
                    Children[index] = new OctreeNode { Parent = this };
                }

                Children[index].AddColor(r, g, b, level + 1);
            }

            public void CollectLeaves(List<OctreeNode> leaves)
            {
                if (IsLeaf)
                    leaves.Add(this);
                else
                    foreach (var child in Children)
                        child?.CollectLeaves(leaves);
            }
        }

        private OctreeNode root = new OctreeNode();

        public void AddColor(Color c)
        {
            root.AddColor(c.R, c.G, c.B, 0);
        }

        public List<Color> GeneratePalette(int maxColors)
        {
            List<OctreeNode> leaves = new List<OctreeNode>();
            root.CollectLeaves(leaves);

            while (leaves.Count > maxColors)
            {
                leaves.Sort((a, b) => a.PixelCount.CompareTo(b.PixelCount));
                OctreeNode smallest = leaves[0];

                OctreeNode parent = smallest.Parent;
                parent.IsLeaf = true;
                leaves.RemoveAt(0);

                int r = 0, g = 0, b = 0, total = 0;

                foreach (var child in parent.Children)
                {
                    if (child != null)
                    {
                        r += child.Red;
                        g += child.Green;
                        b += child.Blue;
                        total += child.PixelCount;
                    }
                }

                parent.Red = r;
                parent.Green = g;
                parent.Blue = b;
                parent.PixelCount = total;
            }

            List<Color> palette = new List<Color>();
            int index = 0;

            foreach (var leaf in leaves)
            {
                int r = leaf.Red / leaf.PixelCount;
                int g = leaf.Green / leaf.PixelCount;
                int b = leaf.Blue / leaf.PixelCount;

                leaf.PaletteIndex = index++;

                palette.Add(Color.FromArgb(r, g, b));
            }

            return palette;
        }
    }
}
