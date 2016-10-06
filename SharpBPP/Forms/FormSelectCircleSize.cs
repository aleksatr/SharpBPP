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
    public partial class FormSelectCircleSize : Form
    {
        public int CircleSize { get; set; }
        public FormSelectCircleSize()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            CircleSize = (int)numericSize.Value;
            this.Dispose();
        }
    }
}
