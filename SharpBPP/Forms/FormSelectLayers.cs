using SharpBPP.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpBPP.Forms
{
    public partial class FormSelectLayers : Form
    {
        public List<LayerRecord> SelectedLayerRecords { get; private set; } 

        public FormSelectLayers()
        {
            InitializeComponent();
        }

        public FormSelectLayers(List<LayerRecord> layerRecords)
            :this()
        {
            this.SelectedLayerRecords = layerRecords;
            this.dataGridView.DataSource = layerRecords;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            List<LayerRecord> selectedRecords = new List<LayerRecord>();
            for (int i = 0; i < SelectedLayerRecords.Count; i++)
            {
                if (dataGridView.Rows[i].Selected)
                {
                    selectedRecords.Add(SelectedLayerRecords[i]);
                }
            }

            SelectedLayerRecords = selectedRecords;
            this.DialogResult = DialogResult.OK;
            this.Dispose();
        }
    }
}
