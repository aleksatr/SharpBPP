using Npgsql;
using SharpBPP.Entities;
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
        public DataProcessor() { }

        public DataProcessor(ConnectionStringSettingsCollection connectionStrings)
        {
            this.connectionStrings = connectionStrings;
        }

        public List<LayerRecord> GetAllLayers()
        {
            NpgsqlConnection conn = new NpgsqlConnection(connectionStrings["PostgreSQL"].ConnectionString);
            conn.Open();

            NpgsqlCommand command = new NpgsqlCommand("select * from geometry_columns where f_table_schema = 'public';", conn);

            NpgsqlDataReader reader = command.ExecuteReader();
            List<LayerRecord> layers = new List<LayerRecord>();
            while (reader.Read())
            {
                layers.Add(new LayerRecord(reader[1].ToString(), reader[2].ToString(),
                    reader[3].ToString(), (int)reader[4], (int)reader[5], reader[6].ToString()));
            }

            conn.Dispose();

            return layers;
        }
    }
}
