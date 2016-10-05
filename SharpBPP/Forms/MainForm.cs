using System;
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
using SharpBPP.Helpers;
using ProjNet.CoordinateSystems.Transformations;

namespace SharpBPP.Forms
{
    public partial class MainForm : Form
    {
        private ConnectionStringSettingsCollection _connectionStrings;
        private NameValueCollection _appSettings;
        private LayerCollection _layerCollection = new LayerCollection();
        private LayerCollection _labelLayers = new LayerCollection();
        private DataProcessor dataProcessor;
        private bool _showBackgroundLayer = true;
        private CoordinateTransformationFactory _ctFact = new CoordinateTransformationFactory();


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

            _labelLayers.Clear();

            mapBox.Map.Layers.AddCollection(_layerCollection);
            mapBox.Map.Layers.AddCollection(_labelLayers);

            foreach (VectorLayer layer in _layerCollection)
            {
                layer.CoordinateTransformation = _ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84, ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);
                layer.ReverseCoordinateTransformation = _ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator, ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);
            }

            foreach (LabelLayer label in _labelLayers)
            {
                label.CoordinateTransformation = _ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84, ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);
                label.ReverseCoordinateTransformation = _ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator, ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);
            }

            if (_layerCollection.Count > 0)
                mapBox.Map.ZoomToExtents();

            mapBox.Refresh();

            if (_showBackgroundLayer)
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

            foreach (LabelLayer label in _labelLayers)
            {
                if (label != null && !label.IsDisposed)
                    label.Dispose();
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
                treeViewLayers.SelectedNode = e.Node;
                if (e.Node.Parent != null)
                {
                    //handle child nodes click
                }
                else if (e.Node.Checked) //root nodes click (layer on/off)
                {
                    mapBox.Map.Layers.Add(_layerCollection.Where(l => l.LayerName == e.Node.Text).First());

                    var layerLabel = _labelLayers.Where(l => l.LayerName == e.Node.Text + "_label").FirstOrDefault();

                    if (layerLabel != null)
                        mapBox.Map.Layers.Add(layerLabel);
                }
                else
                {
                    mapBox.Map.Layers.Remove(_layerCollection.Where(l => l.LayerName == e.Node.Text).First());

                    var layerLabel = _labelLayers.Where(l => l.LayerName == e.Node.Text + "_label").FirstOrDefault();

                    if (layerLabel != null)
                        mapBox.Map.Layers.Remove(layerLabel);
                }

                mapBox.Refresh();
                mapBox.Invalidate() ;
            }
        }

        private void btnLabels_Click(object sender, EventArgs e)
        {
            List<string> layerAttributes;
            if (treeViewLayers.SelectedNode != null && treeViewLayers.SelectedNode.Parent == null)
            {
                layerAttributes = dataProcessor.GetAllLayerAttributes(treeViewLayers.SelectedNode.Text);
                layerAttributes.Insert(0, "None");
                string selectedAttribute = MainFormHelper.DialogCombo("Labels", "Select Attribute:", layerAttributes);

                if (selectedAttribute != null)
                {
                    var existingLabel = _labelLayers.Where(l => l.LayerName == treeViewLayers.SelectedNode.Text + "_label").FirstOrDefault();
                    if (existingLabel != null)
                    {
                        _labelLayers.Remove(existingLabel);
                        mapBox.Map.Layers.Remove(existingLabel);
                        (existingLabel as LabelLayer).Dispose();
                    }

                    if (selectedAttribute != "None")
                    {
                        var newLabel = dataProcessor.CreateLabelLayer(_layerCollection.Where(l => l.LayerName == treeViewLayers.SelectedNode.Text).FirstOrDefault() as VectorLayer, selectedAttribute);
                        _labelLayers.Add(newLabel);

                        if (mapBox.Map.Layers.Contains(_layerCollection.Where(l => l.LayerName == treeViewLayers.SelectedNode.Text).FirstOrDefault()))
                            mapBox.Map.Layers.Add(newLabel);

                        newLabel.CoordinateTransformation = _ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84, ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);
                        newLabel.ReverseCoordinateTransformation = _ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator, ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);
                    }

                    mapBox.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select layer from TreeView!");
            }
        }

        private void btnToggleBackground_Click(object sender, EventArgs e)
        {
            _showBackgroundLayer = !_showBackgroundLayer;

            if (_showBackgroundLayer)
                mapBox.Map.BackgroundLayer.Add(dataProcessor.CreateBackgroundLayer());
            else
                mapBox.Map.BackgroundLayer.Clear();

            mapBox.Refresh();
            mapBox.Invalidate();
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
                    if (styleChooser.ShowDialog() == DialogResult.OK)
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
