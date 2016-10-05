namespace SharpBPP.Forms
{
    partial class FormStyleChooser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnFill = new System.Windows.Forms.Button();
            this.btnOutline = new System.Windows.Forms.Button();
            this.btnChooseImage = new System.Windows.Forms.Button();
            this.numericPenSize = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.numericPointSize = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericPenSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPointSize)).BeginInit();
            this.SuspendLayout();
            // 
            // btnFill
            // 
            this.btnFill.Location = new System.Drawing.Point(325, 48);
            this.btnFill.Name = "btnFill";
            this.btnFill.Size = new System.Drawing.Size(91, 23);
            this.btnFill.TabIndex = 0;
            this.btnFill.Text = "Fill Color";
            this.btnFill.UseVisualStyleBackColor = true;
            this.btnFill.Click += new System.EventHandler(this.btnFill_Click);
            // 
            // btnOutline
            // 
            this.btnOutline.Location = new System.Drawing.Point(325, 25);
            this.btnOutline.Name = "btnOutline";
            this.btnOutline.Size = new System.Drawing.Size(91, 23);
            this.btnOutline.TabIndex = 1;
            this.btnOutline.Text = "Outline Color";
            this.btnOutline.UseVisualStyleBackColor = true;
            this.btnOutline.Click += new System.EventHandler(this.btnOutline_Click);
            // 
            // btnChooseImage
            // 
            this.btnChooseImage.Location = new System.Drawing.Point(325, 71);
            this.btnChooseImage.Name = "btnChooseImage";
            this.btnChooseImage.Size = new System.Drawing.Size(91, 23);
            this.btnChooseImage.TabIndex = 2;
            this.btnChooseImage.Text = "Choose Image";
            this.btnChooseImage.UseVisualStyleBackColor = true;
            this.btnChooseImage.Click += new System.EventHandler(this.btnChooseImage_Click);
            // 
            // numericPenSize
            // 
            this.numericPenSize.DecimalPlaces = 2;
            this.numericPenSize.Location = new System.Drawing.Point(199, 28);
            this.numericPenSize.Name = "numericPenSize";
            this.numericPenSize.Size = new System.Drawing.Size(120, 20);
            this.numericPenSize.TabIndex = 3;
            this.numericPenSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(158, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Choose Outline Width and Color";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Choose Fill Color";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Choose Custom Symbol";
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(15, 102);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(341, 102);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // txtFilePath
            // 
            this.txtFilePath.Enabled = false;
            this.txtFilePath.Location = new System.Drawing.Point(133, 74);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(186, 20);
            this.txtFilePath.TabIndex = 9;
            // 
            // numericPointSize
            // 
            this.numericPointSize.DecimalPlaces = 2;
            this.numericPointSize.Location = new System.Drawing.Point(199, 51);
            this.numericPointSize.Name = "numericPointSize";
            this.numericPointSize.Size = new System.Drawing.Size(120, 20);
            this.numericPointSize.TabIndex = 10;
            this.numericPointSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericPointSize.Visible = false;
            // 
            // FormStyleChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 134);
            this.Controls.Add(this.numericPointSize);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericPenSize);
            this.Controls.Add(this.btnChooseImage);
            this.Controls.Add(this.btnOutline);
            this.Controls.Add(this.btnFill);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormStyleChooser";
            this.Text = "Styles";
            ((System.ComponentModel.ISupportInitialize)(this.numericPenSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericPointSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnFill;
        private System.Windows.Forms.Button btnOutline;
        private System.Windows.Forms.Button btnChooseImage;
        private System.Windows.Forms.NumericUpDown numericPenSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.NumericUpDown numericPointSize;
    }
}