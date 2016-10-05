namespace SharpBPP.Forms
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
            this.tableLayoutPanelLeft = new System.Windows.Forms.TableLayoutPanel();
            this.gbxLayers = new System.Windows.Forms.GroupBox();
            this.treeViewLayers = new System.Windows.Forms.TreeView();
            this.mapBox = new SharpMap.Forms.MapBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButtonNone = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonPan = new System.Windows.Forms.ToolStripButton();
            this.btnPostgreConnect = new System.Windows.Forms.ToolStripButton();
            this.btnSetStyle = new System.Windows.Forms.ToolStripButton();
            this.btnLabels = new System.Windows.Forms.ToolStripButton();
            this.btnToggleBackground = new System.Windows.Forms.ToolStripButton();
            this.btnCreateSubnodes = new System.Windows.Forms.ToolStripButton();
            this.menuStrip.SuspendLayout();
            this.toolStripContainer.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer.ContentPanel.SuspendLayout();
            this.toolStripContainer.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.tableLayoutPanelLeft.SuspendLayout();
            this.gbxLayers.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1005, 28);
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
            this.toolStripContainer.ContentPanel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.toolStripContainer.ContentPanel.Size = new System.Drawing.Size(1005, 644);
            this.toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer.Location = new System.Drawing.Point(0, 28);
            this.toolStripContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.toolStripContainer.Name = "toolStripContainer";
            this.toolStripContainer.Size = new System.Drawing.Size(1005, 693);
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
            this.statusStrip.Size = new System.Drawing.Size(1005, 22);
            this.statusStrip.TabIndex = 0;
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.tableLayoutPanelLeft);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.mapBox);
            this.splitContainer.Size = new System.Drawing.Size(1005, 644);
            this.splitContainer.SplitterDistance = 179;
            this.splitContainer.SplitterWidth = 5;
            this.splitContainer.TabIndex = 1;
            // 
            // tableLayoutPanelLeft
            // 
            this.tableLayoutPanelLeft.ColumnCount = 1;
            this.tableLayoutPanelLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelLeft.Controls.Add(this.gbxLayers, 0, 0);
            this.tableLayoutPanelLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelLeft.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelLeft.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.tableLayoutPanelLeft.Name = "tableLayoutPanelLeft";
            this.tableLayoutPanelLeft.RowCount = 2;
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelLeft.Size = new System.Drawing.Size(179, 644);
            this.tableLayoutPanelLeft.TabIndex = 0;
            // 
            // gbxLayers
            // 
            this.gbxLayers.Controls.Add(this.treeViewLayers);
            this.gbxLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbxLayers.Location = new System.Drawing.Point(5, 5);
            this.gbxLayers.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.gbxLayers.Name = "gbxLayers";
            this.gbxLayers.Padding = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.gbxLayers.Size = new System.Drawing.Size(169, 312);
            this.gbxLayers.TabIndex = 0;
            this.gbxLayers.TabStop = false;
            this.gbxLayers.Text = "Layers";
            // 
            // treeViewLayers
            // 
            this.treeViewLayers.CheckBoxes = true;
            this.treeViewLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewLayers.FullRowSelect = true;
            this.treeViewLayers.Location = new System.Drawing.Point(5, 20);
            this.treeViewLayers.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.treeViewLayers.Name = "treeViewLayers";
            this.treeViewLayers.ShowNodeToolTips = true;
            this.treeViewLayers.Size = new System.Drawing.Size(159, 287);
            this.treeViewLayers.TabIndex = 0;
            this.treeViewLayers.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeViewLayers_AfterCheck);
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
            this.mapBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.mapBox.Name = "mapBox";
            this.mapBox.QueryGrowFactor = 5F;
            this.mapBox.QueryLayerIndex = 0;
            this.mapBox.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox.ShowProgressUpdate = false;
            this.mapBox.Size = new System.Drawing.Size(821, 644);
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
            this.btnPostgreConnect,
            this.btnSetStyle,
            this.btnLabels,
            this.btnToggleBackground,
            this.btnCreateSubnodes});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(739, 27);
            this.toolStrip1.TabIndex = 0;
            // 
            // toolStripButtonNone
            // 
            this.toolStripButtonNone.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonNone.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonNone.Image")));
            this.toolStripButtonNone.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonNone.Name = "toolStripButtonNone";
            this.toolStripButtonNone.Size = new System.Drawing.Size(49, 24);
            this.toolStripButtonNone.Text = "None";
            this.toolStripButtonNone.Click += new System.EventHandler(this.toolStripButtonNone_ButtonClick);
            // 
            // toolStripButtonPan
            // 
            this.toolStripButtonPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonPan.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonPan.Image")));
            this.toolStripButtonPan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonPan.Name = "toolStripButtonPan";
            this.toolStripButtonPan.Size = new System.Drawing.Size(36, 24);
            this.toolStripButtonPan.Text = "Pan";
            this.toolStripButtonPan.Click += new System.EventHandler(this.toolStripButtonPan_Click);
            // 
            // btnPostgreConnect
            // 
            this.btnPostgreConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPostgreConnect.Image = ((System.Drawing.Image)(resources.GetObject("btnPostgreConnect.Image")));
            this.btnPostgreConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPostgreConnect.Name = "btnPostgreConnect";
            this.btnPostgreConnect.Size = new System.Drawing.Size(104, 24);
            this.btnPostgreConnect.Text = "PostGIS layers";
            this.btnPostgreConnect.Click += new System.EventHandler(this.btnPostgreConnect_Click);
            // 
            // btnSetStyle
            // 
            this.btnSetStyle.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSetStyle.Image = ((System.Drawing.Image)(resources.GetObject("btnSetStyle.Image")));
            this.btnSetStyle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetStyle.Name = "btnSetStyle";
            this.btnSetStyle.Size = new System.Drawing.Size(109, 24);
            this.btnSetStyle.Text = "Set Layer Style";
            this.btnSetStyle.Click += new System.EventHandler(this.btnSetStyle_Click);
            // 
            // btnLabels
            // 
            this.btnLabels.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLabels.Image = ((System.Drawing.Image)(resources.GetObject("btnLabels.Image")));
            this.btnLabels.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLabels.Name = "btnLabels";
            this.btnLabels.Size = new System.Drawing.Size(94, 24);
            this.btnLabels.Text = "Layer Labels";
            this.btnLabels.Click += new System.EventHandler(this.btnLabels_Click);
            // 
            // btnToggleBackground
            // 
            this.btnToggleBackground.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnToggleBackground.Image = ((System.Drawing.Image)(resources.GetObject("btnToggleBackground.Image")));
            this.btnToggleBackground.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnToggleBackground.Name = "btnToggleBackground";
            this.btnToggleBackground.Size = new System.Drawing.Size(142, 24);
            this.btnToggleBackground.Text = "Toggle Background";
            this.btnToggleBackground.Click += new System.EventHandler(this.btnToggleBackground_Click);
            // 
            // btnCreateSubnodes
            // 
            this.btnCreateSubnodes.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCreateSubnodes.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateSubnodes.Image")));
            this.btnCreateSubnodes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCreateSubnodes.Name = "btnCreateSubnodes";
            this.btnCreateSubnodes.Size = new System.Drawing.Size(154, 24);
            this.btnCreateSubnodes.Text = "Create Subcategories";
            this.btnCreateSubnodes.Click += new System.EventHandler(this.btnCreateSubnodes_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 721);
            this.Controls.Add(this.toolStripContainer);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
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
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.tableLayoutPanelLeft.ResumeLayout(false);
            this.gbxLayers.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
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
        private System.Windows.Forms.ToolStripButton btnPostgreConnect;
        private System.Windows.Forms.GroupBox gbxLayers;
        private System.Windows.Forms.TreeView treeViewLayers;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelLeft;
        private System.Windows.Forms.ToolStripButton btnLabels;
        private System.Windows.Forms.ToolStripButton btnToggleBackground;
        private System.Windows.Forms.ToolStripButton btnSetStyle;
        private System.Windows.Forms.ToolStripButton btnCreateSubnodes;
    }
}