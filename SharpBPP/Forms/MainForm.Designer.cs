﻿namespace SharpBPP.Forms
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanelLeftSide = new System.Windows.Forms.FlowLayoutPanel();
            this.mapBox = new SharpMap.Forms.MapBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNone = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPan = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.gbxLayers = new System.Windows.Forms.GroupBox();
            this.treeViewLayers = new System.Windows.Forms.TreeView();
            this.menuStrip.SuspendLayout();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.flowLayoutPanelLeftSide.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.gbxLayers.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1006, 28);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 24);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(108, 26);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.BottomToolStripPanel
            // 
            this.toolStripContainer.BottomToolStripPanel.Controls.Add(this.statusStrip);
            // 
            // toolStripContainer.ContentPanel
            // 
            this.toolStripContainer.ContentPanel.Controls.Add(this.splitContainer);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(1006, 644);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 28);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(1006, 693);
            this.toolStripContainer.TabIndex = 1;
            this.toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            this.toolStripContainer.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // statusStrip
            // 
            this.statusStrip.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Location = new System.Drawing.Point(0, 0);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1006, 22);
            this.statusStrip.TabIndex = 0;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.flowLayoutPanelLeftSide);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.mapBox);
            this.splitContainer.Size = new System.Drawing.Size(1006, 644);
            this.splitContainer.SplitterDistance = 147;
            this.splitContainer.TabIndex = 1;
            // 
            // flowLayoutPanelLeftSide
            // 
            this.flowLayoutPanelLeftSide.AutoScroll = true;
            this.flowLayoutPanelLeftSide.AutoSize = true;
            this.flowLayoutPanelLeftSide.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelLeftSide.Controls.Add(this.gbxLayers);
            this.flowLayoutPanelLeftSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelLeftSide.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanelLeftSide.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanelLeftSide.Name = "flowLayoutPanelLeftSide";
            this.flowLayoutPanelLeftSide.Size = new System.Drawing.Size(147, 644);
            this.flowLayoutPanelLeftSide.TabIndex = 0;
            // 
            // mapBox
            // 
            this.mapBox.ActiveTool = SharpMap.Forms.MapBox.Tools.None;
            this.mapBox.BackColor = System.Drawing.Color.White;
            this.mapBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.mapBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapBox.FineZoomFactor = 10D;
            this.mapBox.Location = new System.Drawing.Point(0, 0);
            this.mapBox.MapQueryMode = SharpMap.Forms.MapBox.MapQueryType.LayerByIndex;
            this.mapBox.Name = "mapBox";
            this.mapBox.QueryGrowFactor = 5F;
            this.mapBox.QueryLayerIndex = 0;
            this.mapBox.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox.ShowProgressUpdate = false;
            this.mapBox.Size = new System.Drawing.Size(855, 644);
            this.mapBox.TabIndex = 0;
            this.mapBox.Text = "mapBox1";
            this.mapBox.WheelZoomMagnitude = -2D;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonNone,
            this.toolStripButtonPan,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(84, 27);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripButtonNone
            // 
            this.toolStripButtonNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonNone.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNone.Image")));
            this.toolStripButtonNone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNone.Name = "toolStripButtonNone";
            this.toolStripButtonNone.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonNone.Text = "None";
            this.toolStripButtonNone.Click += new System.EventHandler(this.toolStripButtonNone_ButtonClick);
            // 
            // toolStripButtonPan
            // 
            this.toolStripButtonPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonPan.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPan.Image")));
            this.toolStripButtonPan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPan.Name = "toolStripButtonPan";
            this.toolStripButtonPan.Size = new System.Drawing.Size(24, 24);
            this.toolStripButtonPan.Text = "Pan";
            this.toolStripButtonPan.Click += new System.EventHandler(this.toolStripButtonPan_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(24, 24);
            this.toolStripButton1.Text = "btnPostgreConnect";
            // 
            // gbxLayers
            // 
            this.gbxLayers.Controls.Add(this.treeViewLayers);
            this.gbxLayers.Location = new System.Drawing.Point(3, 3);
            this.gbxLayers.Name = "gbxLayers";
            this.gbxLayers.Size = new System.Drawing.Size(141, 100);
            this.gbxLayers.TabIndex = 0;
            this.gbxLayers.TabStop = false;
            this.gbxLayers.Text = "Layers";
            // 
            // treeViewLayers
            // 
            this.treeViewLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewLayers.Location = new System.Drawing.Point(3, 18);
            this.treeViewLayers.Name = "treeViewLayers";
            this.treeViewLayers.Size = new System.Drawing.Size(135, 79);
            this.treeViewLayers.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 721);
            this.Controls.Add(this.toolStripContainer);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainForm";
            this.Text = "SharpBPP";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.toolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer.ContentPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer.TopToolStripPanel.PerformLayout();
            this.toolStripContainer.ResumeLayout(false);
            this.toolStripContainer.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.flowLayoutPanelLeftSide.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbxLayers.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButtonNone;
        private SharpMap.Forms.MapBox mapBox;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripButton toolStripButtonPan;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelLeftSide;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.GroupBox gbxLayers;
        private System.Windows.Forms.TreeView treeViewLayers;
    }
}