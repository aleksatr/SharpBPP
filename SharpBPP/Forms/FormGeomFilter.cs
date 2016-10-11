using GeoAPI.Geometries;
using Npgsql;
using SharpMap.Data.Providers;
using SharpMap.Layers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpBPP.Forms
{
    public partial class FormGeomFilter : Form
    {
        public IList<IGeometry> ResultGeometries { get; private set; }
        private LayerCollection layers;

        public FormGeomFilter()
        {
            InitializeComponent();
        }

        public FormGeomFilter(LayerCollection layers)
        {
            InitializeComponent();
            this.layers = layers;
            comboSource.DataSource = layers.Select(x => x.LayerName).ToList();
            comboTarget.DataSource = layers.Skip(1).Select(x => x.LayerName).ToList();
            comboOperation.SelectedIndex = 0;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            CalculateResultGeometry();
            this.Dispose();
        }

        private void CalculateResultGeometry()
        {
            VectorLayer sourceLayer = layers.Where(x => x.LayerName == comboSource.SelectedItem.ToString()).First() as VectorLayer;
            PostGIS postgis = sourceLayer.DataSource as PostGIS;
            postgis.DefinitionQuery = GetAllLayers();

        }

        public string GetAllLayers()
        {
            string where = string.Empty;
            using (NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString))
            {
                conn.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(@"select ll.gid from zone ll where ll.gid in 
                            (select l.gid from zone l, stanice s where l.gid = ll.gid and (ST_Contains(l.geom, s.geom)));", conn))
                {

                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (where == string.Empty)
                        {
                            where = "gid in ( ";
                        }

                        where += reader[0] + ",";
                    }

                    if (where != string.Empty)
                    {
                        where = where.Remove(where.Length - 1);
                        where += ")";
                    }
                }
            }
            return where;
        }


        private bool FilterOperation(IGeometry source, IGeometry target)
        {
            switch (comboOperation.SelectedIndex)
            {
                case 0:
                    return source.Contains(target);
                case 1:
                    return source.Within(target);
                case 2:
                    return source.Intersects(target);
                case 3:
                    return source.IsWithinDistance(target, (double)numericDistance.Value);
            }
            return false;
        }

        private void comboOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            numericDistance.Visible = comboOperation.SelectedIndex == comboOperation.Items.Count - 1;
            lblDistance.Visible = numericDistance.Visible;
        }

        private void comboSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboTarget.DataSource = layers.Where(y => y.LayerName != comboSource.SelectedItem.ToString()).Select(x => x.LayerName).ToList();
        }

        private IList<IGeometry> GetGeometriesFromLayer(VectorLayer layer)
        {
            List<IGeometry> geometries = layer.DataSource.GetGeometriesInView(layer.DataSource.GetExtents()).ToList();

            return geometries;
        }
    }
}
