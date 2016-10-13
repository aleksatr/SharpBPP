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
using GeoAPI.CoordinateSystems;
using GeoAPI.CoordinateSystems.Transformations;
using SharpMap.Rendering.Symbolizer;
using NetTopologySuite.Geometries;
using NetTopologySuite.Utilities;
using SharpMap.Forms;
using SharpMap.Data;
using System.Collections.ObjectModel;
using SharpMap.Data.Providers;
using GeoAPI.Geometries;

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
        private LayerCollection resultLayerCollection;
        private FilterProcessor filterProcessor;
        private VectorLayer _tmpLayer;
        private MapDrawingState _state;
        private bool featureInfo;
        private bool mouseClickActive;

        public MapBox Box
        {
            get
            {
                return this.mapBox;
            }
        }

        public MainForm()
        {
            InitializeComponent();
            _connectionStrings = ConfigurationManager.ConnectionStrings;
            _appSettings = ConfigurationManager.AppSettings;
            mouseClickActive = false;
            dataProcessor = new DataProcessor();
            _state = MapDrawingState.Default;

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
            mapBox.ActiveTool = MapBox.Tools.None;
        }

        private void toolStripButtonPan_Click(object sender, EventArgs e)
        {
            mapBox.ActiveTool = MapBox.Tools.Pan;
            mapBox.MouseClick -= mapBox_MouseClick;
            mouseClickActive = false;
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

            if (_tmpLayer != null && !_tmpLayer.IsDisposed)
            {
                _tmpLayer.Dispose();
                _tmpLayer = null;
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
            treeViewLayers.TreeViewNodeSorter = new TreeViewNodeComparer(mapBox.Map.Layers);
        }

        ~MainForm()
        {
            FreeMap();
        }

        private void treeViewLayers_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (e.Action != TreeViewAction.Unknown)
            {
                treeViewLayers.SelectedNode = e.Node;
                if (e.Node.Parent != null)
                {
                    //handle child nodes click
                    ApplyFilter(e.Node);
                }
                else if (e.Node.Checked) //root nodes click (layer on/off)
                {
                    mapBox.Map.Layers.Add(_layerCollection.Where(l => l.LayerName == e.Node.Text).First());

                    var layerLabel = _labelLayers.Where(l => l.LayerName == e.Node.Text + "_label").FirstOrDefault();

                    if (layerLabel != null)
                        mapBox.Map.Layers.Add(layerLabel);

                    treeViewLayers.Sort();
                    treeViewLayers.SelectedNode = e.Node;
                }
                else
                {
                    mapBox.Map.Layers.Remove(_layerCollection.Where(l => l.LayerName == e.Node.Text).First());

                    var layerLabel = _labelLayers.Where(l => l.LayerName == e.Node.Text + "_label").FirstOrDefault();

                    if (layerLabel != null)
                        mapBox.Map.Layers.Remove(layerLabel);
                }

                if (_tmpLayer != null && mapBox.Map.Layers.Contains(_tmpLayer))
                {
                    mapBox.Map.Layers.Remove(_tmpLayer);
                    if (!_tmpLayer.IsDisposed)
                        _tmpLayer.Dispose();
                    _tmpLayer.Dispose();
                    _tmpLayer = null;
                }

                mapBox.Refresh();
                mapBox.Invalidate();
            }
        }

        private void ApplyFilter(TreeNode node)
        {
            VectorLayer layerToFilter = (VectorLayer)_layerCollection.Where(l => l.LayerName == node.Parent.Text).FirstOrDefault();
            SharpMap.Data.Providers.PostGIS postgisProvider = (SharpMap.Data.Providers.PostGIS)layerToFilter.DataSource;

            StringBuilder sb = new StringBuilder();
            List<TreeNode> checkedNodes = node.Parent.Nodes.Cast<TreeNode>().Where(c => c.Checked).ToList();

            if (checkedNodes.Count == 0)
            {
                sb.Append("true = false");
            }
            else if (checkedNodes.Count == 1)
            {
                sb.Append(node.Parent.Tag.ToString() + " = '" + checkedNodes.First().Text + "'");
            }
            else
            {
                sb.Append(node.Parent.Tag.ToString() + " in ('");
                sb.Append(string.Join("', '", checkedNodes.Select(n => n.Text).ToArray()));
                sb.Append("')");
            }

            postgisProvider.DefinitionQuery = sb.ToString();

            mapBox.Refresh();
        }

        private void btnLabels_Click(object sender, EventArgs e)
        {
            List<string> layerAttributes;
            if (treeViewLayers.SelectedNode != null && treeViewLayers.SelectedNode.Parent == null)
            {
                if (treeViewLayers.SelectedNode.Checked)
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

                        if (_tmpLayer != null && mapBox.Map.Layers.Contains(_tmpLayer))
                        {
                            mapBox.Map.Layers.Remove(_tmpLayer);
                            _tmpLayer.Dispose();
                            _tmpLayer = null;
                        }

                        mapBox.Refresh();
                    }
                }
                else
                {
                    MessageBox.Show("Check Layer in TreeView to change label!");
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
                else if (dataProcessor.Layers.ContainsKey(layer) && dataProcessor.Layers[layer].Type.Contains("POLYGON"))
                {
                    styleChooser = new FormStyleChooser(layer.Style.Outline.Width);
                    if (styleChooser.ShowDialog() == DialogResult.OK)
                        SetPolygonStyle(layer, styleChooser);
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
                int size = (int)styleChooser.PointSize;
                Bitmap b = new Bitmap(size, size);
                using (Graphics g = Graphics.FromImage((Image)b))
                {
                    g.DrawImage(styleChooser.CustomImage, 0, 0, size, size);
                }
                layer.Style.Symbol = b;
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

        private void btnCreateSubnodes_Click(object sender, EventArgs e)
        {
            List<string> layerAttributes;
            TreeNode node = treeViewLayers.SelectedNode;

            if (node != null && node.Parent == null)
            {
                if (node.Checked)
                {
                    layerAttributes = dataProcessor.GetAllLayerAttributes(node.Text);
                    layerAttributes.Insert(0, "None");
                    string selectedAttribute = MainFormHelper.DialogCombo("Subcategories", "Select category:", layerAttributes);

                    if (selectedAttribute != null)
                    {
                        node.Nodes.Clear();

                        if (selectedAttribute != "None")
                        {
                            List<object> distinctCategories = dataProcessor.GetAllDistinctValues(node.Text, selectedAttribute);

                            TreeNode[] childNodes = new TreeNode[distinctCategories.Count];

                            for (int i = 0; i < childNodes.Length; ++i)
                            {
                                childNodes[i] = new TreeNode(distinctCategories[i].ToString());
                                childNodes[i].Tag = distinctCategories[i];
                                childNodes[i].Checked = true;
                            }

                            node.Nodes.AddRange(childNodes);
                            node.Tag = selectedAttribute;

                            VectorLayer layerToExpand = (VectorLayer)_layerCollection.Where(l => l.LayerName == node.Text).FirstOrDefault();
                            SharpMap.Data.Providers.PostGIS postgisProvider = layerToExpand.DataSource as SharpMap.Data.Providers.PostGIS;

                            if (postgisProvider != null)
                                postgisProvider.DefinitionQuery = "";


                            node.Expand();
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Check Layer in TreeView to create subcategories!");
                }
            }
            else
            {
                MessageBox.Show("Please select layer from TreeView!");
            }
        }

        private void btnFilterLayer_Click(object sender, EventArgs e)
        {
            List<string> layerAttributes;
            if (treeViewLayers.SelectedNode != null && treeViewLayers.SelectedNode.Parent == null)
            {
                if (treeViewLayers.SelectedNode.Checked)
                {
                    layerAttributes = dataProcessor.GetAllLayerAttributes(treeViewLayers.SelectedNode.Text);
                    layerAttributes.Insert(0, "None");
                    string filter;
                    bool likeOperation;
                    string selectedAttribute = MainFormHelper.FilterDialogWithRadio("Filter", "Select Attribute:", layerAttributes, out filter, out likeOperation);

                    if (selectedAttribute != null)
                    {
                        if (_tmpLayer != null)
                        {
                            mapBox.Map.Layers.Remove(_tmpLayer);
                            if (!_tmpLayer.IsDisposed)
                                _tmpLayer.Dispose();
                            _tmpLayer = null;
                        }

                        if (selectedAttribute != "None")
                        {
                            var baseLayer = _layerCollection.Where(l => l.LayerName == treeViewLayers.SelectedNode.Text).FirstOrDefault();
                            _tmpLayer = dataProcessor.CreateFilteredLayer(baseLayer as VectorLayer, selectedAttribute, filter, likeOperation);

                            mapBox.Map.Layers.Add(_tmpLayer);
                            _tmpLayer.CoordinateTransformation = _ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84, ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);
                            _tmpLayer.ReverseCoordinateTransformation = _ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator, ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);
                        }

                        mapBox.Refresh();
                    }
                }
                else
                {
                    MessageBox.Show("Check Layer in TreeView to filter!");
                }
            }
            else
            {
                MessageBox.Show("Please select layer from TreeView!");
            }
        }

        private void btnZUp_Click(object sender, EventArgs e)
        {
            TreeNode node = treeViewLayers.SelectedNode;

            if (node != null && node.Parent == null)
            {
                if (node.Checked)
                {
                    VectorLayer selectedLayer = (VectorLayer)mapBox.Map.Layers.Where(l => l.LayerName == node.Text).FirstOrDefault();
                    int maxIndex = mapBox.Map.Layers.Count - 1;
                    int currentIndex = mapBox.Map.Layers.IndexOf(selectedLayer);

                    if (currentIndex < maxIndex)
                    {
                        mapBox.Map.Layers.RemoveAt(currentIndex);
                        mapBox.Map.Layers.Insert(currentIndex + 1, selectedLayer);
                        mapBox.Refresh();

                        treeViewLayers.Sort();
                        treeViewLayers.SelectedNode = node;
                    }
                }
                else
                {
                    MessageBox.Show("Check Layer in TreeView to change Z-order!");
                }
            }
            else
            {
                MessageBox.Show("Please select layer from TreeView!");
            }
        }

        private void btnZDown_Click(object sender, EventArgs e)
        {
            TreeNode node = treeViewLayers.SelectedNode;

            if (node != null && node.Parent == null)
            {
                if (node.Checked)
                {
                    VectorLayer selectedLayer = (VectorLayer)mapBox.Map.Layers.Where(l => l.LayerName == node.Text).FirstOrDefault();
                    int currentIndex = mapBox.Map.Layers.IndexOf(selectedLayer);

                    if (currentIndex > 0)
                    {
                        mapBox.Map.Layers.RemoveAt(currentIndex);
                        mapBox.Map.Layers.Insert(currentIndex - 1, selectedLayer);
                        mapBox.Refresh();

                        treeViewLayers.Sort();
                        treeViewLayers.SelectedNode = node;
                    }
                }
                else
                {
                    MessageBox.Show("Check Layer in TreeView to change Z-order!");
                }
            }
            else
            {
                MessageBox.Show("Please select layer from TreeView!");
            }
        }

        private void mapBox_MouseMove(GeoAPI.Geometries.Coordinate worldPos, MouseEventArgs imagePos)
        {
            lblCoordinate.Text = "Coordinate: " + worldPos.X + ", " + worldPos.Y;
        }

        private void mapBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (featureInfo)
            {
                txtFeatureInfo.Text = GetFeatureInfo(e.Location);
                btnFeatureInfo.Checked = false;
            }
            else
            {
                CircleFiltering(e.Location);
            }

            mapBox.MouseClick -= mapBox_MouseClick;
            mouseClickActive = false;
        }

        private string GetFeatureInfo(System.Drawing.Point location)
        {
            return new FilterProcessor(this, mapBox.Map.Zoom/300.0).GetFeatureInfo(location, mapBox.Map.Layers);
        }

        private void CircleFiltering(System.Drawing.Point location)
        {
            if (filterProcessor == null)
                return;

            if (resultLayerCollection != null)
            {
                foreach (Layer layer in resultLayerCollection)
                {
                    mapBox.Map.Layers.Remove(layer);
                    layer.Dispose();
                }
            }
            resultLayerCollection = filterProcessor.CrateResultLayerCollection(mapBox.Map.Layers, location);

            mapBox.Map.Layers.Clear();
            mapBox.Map.Layers.AddCollection(resultLayerCollection);

            mapBox.Refresh();
            mapBox.Invalidate();
        }

        private void btnDrawCircle_Click(object sender, EventArgs e)
        {
            FormSelectCircleSize fscs = new FormSelectCircleSize();
            if (fscs.ShowDialog() == DialogResult.OK)
            {
                if (filterProcessor != null)
                {
                    PopulateMap();
                    filterProcessor.Dispose();
                }
                filterProcessor = new FilterProcessor(this, fscs.CircleSize);
            }
            featureInfo = false;
            if (!mouseClickActive)
                mapBox.MouseClick += mapBox_MouseClick;
            mouseClickActive = true;
            mapBox.ActiveTool = MapBox.Tools.None;
        }

        private void mapBox_GeometryDefined(IGeometry geometry)
        {
            //hande draw point for Routing
            if(_state == MapDrawingState.RouteChooseA)
            {
                dataProcessor._routeStartEnd[0] = (NetTopologySuite.Geometries.Point) geometry;
                _state = MapDrawingState.RouteChooseB;
                lblInfo.Text = "Please select end point...";
                return;
            }  else if (_state == MapDrawingState.RouteChooseB)
            {
                dataProcessor._routeStartEnd[1] = (NetTopologySuite.Geometries.Point)geometry;
                lblInfo.Text = "";
                mapBox.ActiveTool = MapBox.Tools.Pan;
                _state = MapDrawingState.Default;
                btnRoute.Checked = false;

                //remove previous route from Layers
                var _existingRoute = mapBox.Map.Layers.Where(l => l.LayerName == "RouteAtoB").FirstOrDefault();
                if (_existingRoute != null)
                {
                    _layerCollection.Remove(_existingRoute);
                    mapBox.Map.Layers.Remove(_existingRoute);
                    TreeNode _existingNode = null;

                    foreach(TreeNode node in treeViewLayers.Nodes)
                        if(node.Text == "RouteAtoB")
                        {
                            _existingNode = node;
                            break;
                        }

                    if (_existingNode != null)
                        treeViewLayers.Nodes.Remove(_existingNode);

                    (_existingRoute as VectorLayer).Dispose();
                }

                //add new route layer
                VectorLayer routeLayer = dataProcessor.CreateRouteLayer();
                _layerCollection.Add(routeLayer);
                mapBox.Map.Layers.Add(routeLayer);
                var newNode = new TreeNode("RouteAtoB");
                newNode.Checked = true;
                treeViewLayers.Nodes.Insert(0, newNode);
                mapBox.Refresh();
                return;
            }

            if (resultLayerCollection != null)
            {
                resultLayerCollection.Clear();
            }

            if (filterProcessor != null)
            {
                PopulateMap();
                filterProcessor.Dispose();
            }
            filterProcessor = new FilterProcessor(this);

            resultLayerCollection = filterProcessor.PolygonFiltering(mapBox.Map.Layers, geometry);

            mapBox.Map.Layers.Clear();
            mapBox.Map.Layers.AddCollection(resultLayerCollection);

            mapBox.Refresh();
            mapBox.Invalidate();
        }

        private void btnPolygon_Click(object sender, EventArgs e)
        {
            mapBox.ActiveTool = MapBox.Tools.DrawPolygon;
        }

        private void btnFeatureInfo_Click(object sender, EventArgs e)
        {
            featureInfo = true;
            btnFeatureInfo.Checked = true;
            if (!mouseClickActive)
                mapBox.MouseClick += mapBox_MouseClick;
            mouseClickActive = true;
            mapBox.ActiveTool = MapBox.Tools.None;
        }

        private void btnRoute_Click(object sender, EventArgs e)
        {
            mapBox.ActiveTool = MapBox.Tools.DrawPoint;
            btnRoute.Checked = true;
            _state = MapDrawingState.RouteChooseA;   
            lblInfo.Text = "Please select starting point...";
        }

        private void btnGeomFilter_Click(object sender, EventArgs e)
        {
            FormGeomFilter fgf = new FormGeomFilter(mapBox.Map.Layers);
            if (fgf.ShowDialog() == DialogResult.OK)
            {
                IList<IGeometry> geometries;
               
            }
        }
    }
}
