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
    public partial class FormStyleChooser : Form
    {
        public Color? OutlineColor { get; private set; }
        public float PenSize { get; private set; }
        public float PointSize { get; private set; }
        public Color? FillColor { get; private set; }
        public Image CustomImage { get; private set; }

        public FormStyleChooser(float penSize)
        {
            InitializeComponent();
            numericPenSize.Value = (decimal)penSize;
        }

        public FormStyleChooser(float penSize, float pointSize)
            :this(penSize)
        {
            numericPointSize.Value = (decimal)pointSize;
            numericPointSize.Visible = true;
            label2.Text = "Choose Point size and color";
        }

        private Color? ChooseColor()
        {
            ColorDialog colorDialog = new ColorDialog();

            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                return colorDialog.Color;
            }
            return null;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            PenSize = (float)numericPenSize.Value;
            PointSize = (float)numericPointSize.Value;
            this.Dispose();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void btnChooseImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "PNG (*.png)|*.png|JPEG(*.jpg)|*.jpg|BMP(*.bmp)|*.bmp";
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                CustomImage = Image.FromFile(fileDialog.FileName);
                txtFilePath.Text = fileDialog.FileName;
            }
        }

        private void btnOutline_Click(object sender, EventArgs e)
        {
            OutlineColor = ChooseColor();
        }

        private void btnFill_Click(object sender, EventArgs e)
        {
            FillColor = ChooseColor();
        }
    }
}
