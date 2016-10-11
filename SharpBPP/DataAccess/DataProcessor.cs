using NetTopologySuite.Geometries;
using Npgsql;
using SharpBPP.Entities;
using SharpMap.Layers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoAPI.Geometries;
using ProjNet.CoordinateSystems.Transformations;

namespace SharpBPP.DataAccess
{
    public class DataProcessor
    {
        private ConnectionStringSettingsCollection connectionStrings;
        private NameValueCollection _appSettings;
        public Dictionary<VectorLayer, LayerRecord> Layers { get; set; }
        public Point[] _routeStartEnd = new Point[2]; 
        
        public DataProcessor()
        {
            Layers = new Dictionary<VectorLayer, LayerRecord>();
            this.connectionStrings = ConfigurationManager.ConnectionStrings;
            _appSettings = ConfigurationManager.AppSettings;
        }

        public List<object> GetAllDistinctValues(string tableName, string columnName)
        {
            List<object> values;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionStrings["PostgreSQL"].ConnectionString))
            {
                conn.Open();

                using (NpgsqlCommand command = new NpgsqlCommand("select distinct " + columnName + " from " + tableName + ";", conn))
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    values = new List<object>();
                    while (reader.Read())
                    {
                        values.Add(reader[0]);
                    }
                }
            }

            return values;
        }

        public List<string> GetAllLayerAttributes(string layerName)
        {
            List<string> columnNames;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionStrings["PostgreSQL"].ConnectionString))
            {
                conn.Open();

                using (NpgsqlCommand command = new NpgsqlCommand("select column_name from information_schema.columns where table_name='" + layerName + "';", conn))
                {

                    NpgsqlDataReader reader = command.ExecuteReader();
                    columnNames = new List<string>();
                    while (reader.Read())
                    {
                        columnNames.Add(reader[0].ToString());
                    }
                }
            }

            return columnNames;
        }

        public List<LayerRecord> GetAllLayers()
        {
            List<LayerRecord> layers;
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionStrings["PostgreSQL"].ConnectionString))
            {
                conn.Open();

                using (NpgsqlCommand command = new NpgsqlCommand("select * from geometry_columns where f_table_schema = 'public';", conn))
                {

                    NpgsqlDataReader reader = command.ExecuteReader();
                    layers = new List<LayerRecord>();
                    while (reader.Read())
                    {
                        layers.Add(new LayerRecord(reader[1].ToString(), reader[2].ToString(),
                            reader[3].ToString(), (int)reader[4], (int)reader[5], reader[6].ToString()));
                    }
                }
            }

            return layers;
        }

        public List<LayerRecord> GetDefaultLayers()
        {
            List<LayerRecord> layers;
            StringBuilder sb = new StringBuilder("select * from geometry_columns where f_table_schema = 'public' and f_table_name in ('");
            string[] defaultLayers = _appSettings["defaultLayers"].Split(new char[] { ';' });

            if(defaultLayers.Length == 1)
                sb.Append(defaultLayers[0]);
            else if(defaultLayers.Length > 1)
                sb.Append(string.Join("', '", defaultLayers));
            sb.Append("');");

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionStrings["PostgreSQL"].ConnectionString))
            {
                conn.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(sb.ToString(), conn))
                {

                    NpgsqlDataReader reader = command.ExecuteReader();
                    layers = new List<LayerRecord>();
                    while (reader.Read())
                    {
                        layers.Add(new LayerRecord(reader[1].ToString(), reader[2].ToString(),
                            reader[3].ToString(), (int)reader[4], (int)reader[5], reader[6].ToString()));
                    }
                }
            }

            return layers;
        }

        public ILayer CreateBackgroundLayer()
        {
            return new TileLayer(new BruTile.Web.OsmTileSource(), "OSM");
        }

        public LayerCollection CreateLayers(List<LayerRecord> records = null)
        {
            LayerCollection tmpLayerCollection = new LayerCollection();
            Layers.Clear();
            if (records == null)
                records = GetDefaultLayers();
            foreach (LayerRecord record in records)
            {
                VectorLayer layer = new VectorLayer(record.TableName);
                Layers.Add(layer, record);
                layer.DataSource = new SharpMap.Data.Providers.PostGIS(
                connectionStrings["PostgreSQL"].ConnectionString, record.TableName, "gid");
                tmpLayerCollection.Add(layer);
            }

            return tmpLayerCollection;
        }

        public LabelLayer CreateLabelLayer(VectorLayer vLayer, string labelColumn)
        {
            LabelLayer labelLayer = new LabelLayer(vLayer.LayerName + "_label");
            labelLayer.DataSource = vLayer.DataSource;
            labelLayer.LabelColumn = labelColumn;
            labelLayer.Style.IsTextOnPath = false;
            labelLayer.Style.Halo = new System.Drawing.Pen(System.Drawing.Color.White, 2.0f);
            labelLayer.Style.CollisionDetection = true;
            labelLayer.Style.CollisionBuffer = new System.Drawing.SizeF(30.0f, 30.0f);
            labelLayer.Style.Font = new System.Drawing.Font(System.Drawing.FontFamily.GenericSansSerif, 11);
            labelLayer.Style.HorizontalAlignment = SharpMap.Styles.LabelStyle.HorizontalAlignmentEnum.Center;
            labelLayer.Style.VerticalAlignment = SharpMap.Styles.LabelStyle.VerticalAlignmentEnum.Top;
            labelLayer.MultipartGeometryBehaviour = LabelLayer.MultipartGeometryBehaviourEnum.Largest;

            return labelLayer;
        }
        
        public VectorLayer CreateFilteredLayer(VectorLayer baseLayer, string column, string filter, bool useLikeOperation)
        {
            VectorLayer layer = new VectorLayer(baseLayer.LayerName + "_filtered");
            var postGisProvider = new SharpMap.Data.Providers.PostGIS(connectionStrings["PostgreSQL"].ConnectionString, baseLayer.LayerName, "gid");

            if (useLikeOperation)
                postGisProvider.DefinitionQuery = "cast(" + column + " as text) like '%" + filter + "%'";
            else
                postGisProvider.DefinitionQuery = "cast(" + column + " as text) = '" + filter + "'";

            layer.DataSource = postGisProvider;
            
            layer.Style.Outline = new System.Drawing.Pen(System.Drawing.Color.DarkRed, 5);
            layer.Style.EnableOutline = true;
            layer.Style.Fill = new System.Drawing.SolidBrush(System.Drawing.Color.IndianRed);
            layer.Style.Line = new System.Drawing.Pen(System.Drawing.Color.IndianRed, 5);
            layer.Style.PointColor = new System.Drawing.SolidBrush(System.Drawing.Color.DarkRed);
            layer.Style.PointSize = baseLayer.Style.PointSize + 5.0f;
            return layer;
        }

        internal VectorLayer CreateRouteLayer()
        {
            CoordinateTransformationFactory ctFact = new CoordinateTransformationFactory();
            var reversTrans = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator, ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);
            Coordinate A = reversTrans.MathTransform.Transform(_routeStartEnd[0].Coordinate);
            Coordinate B = reversTrans.MathTransform.Transform(_routeStartEnd[1].Coordinate);

            //prepare temp table
            using (NpgsqlConnection conn = new NpgsqlConnection(connectionStrings["PostgreSQL"].ConnectionString))
            {
                conn.Open();

                using (NpgsqlCommand command = new NpgsqlCommand("drop table if exists temp_route;", conn))
                {
                    command.ExecuteNonQuery();
                }

                using (NpgsqlCommand command = new NpgsqlCommand("CREATE TABLE "+ _appSettings["tmpRouteTableName"] + 
                    " AS select * from pgr_fromAtoB('ways'," + A.X + "," + A.Y + "," + B.X + "," + B.Y + ");", conn))
                {
                    command.ExecuteNonQuery();
                }
            }

            //create layer
            VectorLayer layer = new VectorLayer("RouteAtoB");
            var postGisProvider = new SharpMap.Data.Providers.PostGIS(
                connectionStrings["PostgreSQL"].ConnectionString, _appSettings["tmpRouteTableName"], "geom", "seq");

            postGisProvider.SRID = 4326;
            layer.DataSource = postGisProvider;

            layer.CoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84, ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator);
            layer.ReverseCoordinateTransformation = ctFact.CreateFromCoordinateSystems(ProjNet.CoordinateSystems.ProjectedCoordinateSystem.WebMercator, ProjNet.CoordinateSystems.GeographicCoordinateSystem.WGS84);

            layer.Style.Line = new System.Drawing.Pen(System.Drawing.Color.IndianRed, 5);

            return layer;
        }
    }
}
