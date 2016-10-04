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
using SharpBPP.Entities;
using SharpBPP.DataAccess;

namespace SharpBPP.Forms
{
    public partial class MainForm : Form
    {
        private ConnectionStringSettingsCollection _connectionStrings;
        private NameValueCollection _appSettings;
        private DataProcessor dataProcessor;

        public MainForm()
        {
            InitializeComponent();

            _connectionStrings = ConfigurationManager.ConnectionStrings;
            _appSettings = ConfigurationManager.AppSettings;

            dataProcessor = new DataProcessor(_connectionStrings);
            
            AddLayers();
        }        

        private void AddLayers()
        {
            SharpMap.Layers.VectorLayer zone = new SharpMap.Layers.VectorLayer("Zone");
            SharpMap.Layers.VectorLayer linije = new SharpMap.Layers.VectorLayer("Linije");
            SharpMap.Layers.VectorLayer stanice = new SharpMap.Layers.VectorLayer("Stanice");

            zone.DataSource = new SharpMap.Data.Providers.PostGIS(
                _connectionStrings["PostgreSQL"].ConnectionString, "zone", "gid");

            linije.DataSource = new SharpMap.Data.Providers.PostGIS(
                _connectionStrings["PostgreSQL"].ConnectionString, "linije", "gid");

            stanice.DataSource = new SharpMap.Data.Providers.PostGIS(
                _connectionStrings["PostgreSQL"].ConnectionString, "stanice", "gid");
            
            
            mapBox.Map.Layers.Add(zone);
            mapBox.Map.Layers.Add(linije);
            mapBox.Map.Layers.Add(stanice);

            ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory ctFact = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
            zone.CoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84, ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);
            zone.ReverseCoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator, ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);
            linije.CoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84, ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);
            linije.ReverseCoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator, ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);
            stanice.CoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84, ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);
            stanice.ReverseCoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator, ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);

            mapBox.Map.ZoomToExtents();
            mapBox.Refresh();

            mapBox.Map.BackgroundLayer.Add(new SharpMap.Layers.TileAsyncLayer(new BruTile.Web.OsmTileSource(), "OSM"));
            
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
    }
}
