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
    }
}
