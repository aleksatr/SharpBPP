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

namespace SharpBPP.Forms
{
    public partial class MainForm : Form
    {
        private ConnectionStringSettingsCollection _connectionStrings;
        private NameValueCollection _appSettings;
        private LayerCollection _layerCollection = new LayerCollection();

        public MainForm()
        {
            InitializeComponent();
            _connectionStrings = ConfigurationManager.ConnectionStrings;
            _appSettings = ConfigurationManager.AppSettings;

            PopulateMap();
        }

        private void PopulateMap()
        {
            if (_layerCollection != null && _layerCollection.Count > 0)
                FreeMap();

            _layerCollection = CreateLayers();
            mapBox.Map.Layers.AddCollection(_layerCollection);

            ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctFact = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();

            foreach(VectorLayer layer in _layerCollection)
            {
                layer.CoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84, ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);
                layer.ReverseCoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator, ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);
            }
            
            mapBox.Map.ZoomToExtents();

            mapBox.Refresh();

            mapBox.Map.BackgroundLayer.Add(CreateBackgroundLayer());

            //pan is selected by default
            mapBox.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
        }

        private ILayer CreateBackgroundLayer()
        {
            return new TileAsyncLayer(new BruTile.Web.OsmTileSource(), "OSM");
        }

        private LayerCollection CreateLayers()
        {
            LayerCollection tmpLayerCollection = new LayerCollection();

            VectorLayer zone = new VectorLayer("Zone");
            VectorLayer linije = new VectorLayer("Linije");
            VectorLayer stanice = new VectorLayer("Stanice");

            zone.DataSource = new SharpMap.Data.Providers.PostGIS(
                _connectionStrings["PostgreSQL"].ConnectionString, "zone", "gid");
            
            linije.DataSource = new SharpMap.Data.Providers.PostGIS(
                _connectionStrings["PostgreSQL"].ConnectionString, "linije", "gid");

            stanice.DataSource = new SharpMap.Data.Providers.PostGIS(
                _connectionStrings["PostgreSQL"].ConnectionString, "stanice", "gid");

            tmpLayerCollection.Add(zone);
            tmpLayerCollection.Add(linije);
            tmpLayerCollection.Add(stanice);

            return tmpLayerCollection;
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

        ~MainForm()
        {
            FreeMap();
        }
    }
}
