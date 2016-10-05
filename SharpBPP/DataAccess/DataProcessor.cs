using Npgsql;
using SharpBPP.Entities;
using SharpMap.Layers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBPP.DataAccess
{
    public class DataProcessor
    {
        private ConnectionStringSettingsCollection connectionStrings;
        public Dictionary<VectorLayer, LayerRecord> Layers { get; set; }
        
        public DataProcessor()
        {
            Layers = new Dictionary<VectorLayer, LayerRecord>();
            this.connectionStrings = ConfigurationManager.ConnectionStrings;
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

        public ILayer CreateBackgroundLayer()
        {
            return new TileAsyncLayer(new BruTile.Web.OsmTileSource(), "OSM");
        }

        public LayerCollection CreateLayers()
        {
            LayerCollection tmpLayerCollection = new LayerCollection();
            Layers.Clear();
            foreach(LayerRecord record in GetAllLayers())
            {
                VectorLayer layer = new VectorLayer(record.TableName);
                Layers.Add(layer, record);
                layer.DataSource = new SharpMap.Data.Providers.PostGIS(
                connectionStrings["PostgreSQL"].ConnectionString, record.TableName, "gid");
                tmpLayerCollection.Add(layer);
            }
            
            return tmpLayerCollection;
        }

        public LayerCollection CreateLayers(List<LayerRecord> records)
        {
            LayerCollection tmpLayerCollection = new LayerCollection();

            foreach (LayerRecord record in records)
            {
                VectorLayer layer = new VectorLayer(record.TableName);
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
    }
}
