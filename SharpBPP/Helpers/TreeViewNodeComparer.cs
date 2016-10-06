using SharpMap.Layers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpBPP.Helpers
{
    public class TreeViewNodeComparer : IComparer, IComparer<TreeNode>
    {
        private LayerCollection _layerCollection;

        public TreeViewNodeComparer(LayerCollection layerCollection)
        {
            _layerCollection = layerCollection;
        }

        public int Compare(object x, object y)
        {
            return Compare((TreeNode) x, (TreeNode) y);
        }

        public int Compare(TreeNode x, TreeNode y)
        {
            if (x.Parent != null || y.Parent != null)
                return 0;

            var layerX = _layerCollection.Where(l => l.LayerName == x.Text).FirstOrDefault();
            var layerY = _layerCollection.Where(l => l.LayerName == y.Text).FirstOrDefault();

            return _layerCollection.IndexOf(layerY) - _layerCollection.IndexOf(layerX);
        }
    }
}
