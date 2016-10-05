﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Collections.Specialized;
using SharpMap.Layers;
using SharpBPP.Entities;
using SharpBPP.DataAccess;

namespace SharpBPP.Forms
{
    public partial class MainForm : Form
    {
        private ConnectionStringSettingsCollection _connectionStrings;
        private NameValueCollection _appSettings;
        private LayerCollection _layerCollection = new LayerCollection();
        private DataProcessor dataProcessor;

        public MainForm()
        {
            InitializeComponent();
            _connectionStrings = ConfigurationManager.ConnectionStrings;
            _appSettings = ConfigurationManager.AppSettings;

            dataProcessor = new DataProcessor();

            PopulateMap();
        }

        private void PopulateMap(LayerCollection layersToUse = null)
        {
            mapBox.Map.Layers.Clear();
            mapBox.Map.BackgroundLayer.Clear();

            if (_layerCollection != null && _layerCollection.Count > 0)
                FreeMap();

            if (layersToUse == null)
                _layerCollection = dataProcessor.CreateLayers();
            else
                _layerCollection = layersToUse;

            mapBox.Map.Layers.AddCollection(_layerCollection);

            ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctFact = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();

            foreach (VectorLayer layer in _layerCollection)
            {
                layer.CoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84, ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);
                layer.ReverseCoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator, ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);
            }

            if (_layerCollection.Count > 0)
                mapBox.Map.ZoomToExtents();

            mapBox.Refresh();

            mapBox.Map.BackgroundLayer.Add(dataProcessor.CreateBackgroundLayer());

            //populate treeView
            PopulateTreeView();

            //pan is selected by default
            mapBox.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
        }

        private void toolStripButtonNone_ButtonClick(object sender, EventArgs e)
        {
            mapBox.ActiveTool = SharpMap.Forms.MapBox.Tools.None;
        }

        private void toolStripButtonPan_Click(object sender, EventArgs e)
        {
            mapBox.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void FreeMap()
        {
            foreach (VectorLayer layer in _layerCollection)
            {
                if (layer != null && !layer.IsDisposed)
                    layer.Dispose();
            }
        }
        private void btnPostgreConnect_Click(object sender, EventArgs e)
        {
            FormSelectLayers form = new FormSelectLayers(dataProcessor.GetAllLayers());
            if (form.ShowDialog() == DialogResult.OK)
            {
                List<LayerRecord> records = form.SelectedLayerRecords;
                PopulateMap(dataProcessor.CreateLayers(records));

            }
        }

        private void PopulateTreeView()
        {
            TreeNode[] childNodes = new TreeNode[_layerCollection.Count];

            //check or uncheck, depending on old values
            for (int i = 0; i < childNodes.Length; ++i)
            {
                childNodes[i] = new TreeNode(_layerCollection[i].LayerName);
                childNodes[i].Checked = true;
            }

            treeViewLayers.Nodes.Clear();
            treeViewLayers.Nodes.AddRange(childNodes);
        }

        private void CheckAllChildNodes(TreeNode treeNode, bool nodeChecked)
        {
            foreach (TreeNode node in treeNode.Nodes)
            {
                node.Checked = nodeChecked;
                if (node.Nodes.Count > 0)
                {
                    // If the current node has child nodes, call the CheckAllChildsNodes method recursively.
                    this.CheckAllChildNodes(node, nodeChecked);
                }
            }
        }

        ~MainForm()
        {
            FreeMap();
        }

        private void treeViewLayers_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                //if (e.Node.Nodes.Count > 0)
                //{
                //    this.CheckAllChildNodes(e.Node, e.Node.Checked);
                //}
                if (e.Node.Checked)
                {
                    mapBox.Map.Layers.Add(_layerCollection.Where(l => l.LayerName == e.Node.Text).First());
                }
                else
                {
                    mapBox.Map.Layers.Remove(_layerCollection.Where(l => l.LayerName == e.Node.Text).First());
                }

                treeViewLayers.SelectedNode = e.Node;

                mapBox.Refresh();
            }
        }

        private void btnSetStyle_Click(object sender, EventArgs e)
        {
            if (treeViewLayers.SelectedNode == null)
            {
                MessageBox.Show("Please select layer.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            VectorLayer layer = mapBox.Map.Layers.Where(x => x.LayerName == treeViewLayers.SelectedNode.Text).FirstOrDefault() as VectorLayer;

            if (layer != null)
            {
                FormStyleChooser styleChooser;
                

                if (dataProcessor.Layers.ContainsKey(layer) && dataProcessor.Layers[layer].Type == "POINT")
                {
                    styleChooser = new FormStyleChooser(layer.Style.Outline.Width, layer.Style.PointSize);
                    if(styleChooser.ShowDialog() == DialogResult.OK)
                        SetPointStyle(layer, styleChooser);
                }
                else
                {
                    styleChooser = new FormStyleChooser(layer.Style.Outline.Width);
                    if (styleChooser.ShowDialog() == DialogResult.OK)
                        SetLineStyle(layer, styleChooser);
                }              
                mapBox.Refresh();
            }
        }
        private void SetPointStyle(VectorLayer layer, FormStyleChooser styleChooser)
        {
            if (styleChooser.FillColor.HasValue)
            {
                layer.Style.PointColor = new SolidBrush(styleChooser.FillColor.Value);
            }
            layer.Style.EnableOutline = true;
            layer.Style.PointSize = styleChooser.PointSize;
            if (styleChooser.CustomImage != null)
            {
                layer.Style.Symbol = styleChooser.CustomImage;
            }
            if (styleChooser.OutlineColor.HasValue)
            {
                layer.Style.EnableOutline = true;
                layer.Style.Outline.Width = styleChooser.PenSize;
                layer.Style.Outline.Color = styleChooser.OutlineColor.Value;
            }

            if (styleChooser.CustomImage != null)
            {
                layer.Style.Symbol = styleChooser.CustomImage;
            }
        }

        private void SetLineStyle(VectorLayer layer, FormStyleChooser styleChooser)
        {
            if (styleChooser.FillColor.HasValue)
            {
                layer.Style.Line.Color = styleChooser.FillColor.Value;
            }
            layer.Style.EnableOutline = true;
            layer.Style.Outline.Width = styleChooser.PenSize;
            if (styleChooser.OutlineColor.HasValue)
            {
                layer.Style.Outline.Color = styleChooser.OutlineColor.Value;
            }
            if (styleChooser.CustomImage != null)
            {
                layer.Style.Symbol = styleChooser.CustomImage;
            }
        }

        private void SetPolygonStyle(VectorLayer layer, FormStyleChooser styleChooser)
        {
            if (styleChooser.FillColor.HasValue)
            {
                layer.Style.Fill = new SolidBrush(styleChooser.FillColor.Value);
            }
            layer.Style.EnableOutline = true;
            layer.Style.Outline.Width = styleChooser.PenSize;
            if (styleChooser.OutlineColor.HasValue)
            {
                layer.Style.Outline.Color = styleChooser.OutlineColor.Value;
            }
            if (styleChooser.CustomImage != null)
            {
                layer.Style.Symbol = styleChooser.CustomImage;
            }
        }
    }
}
