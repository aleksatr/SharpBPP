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
        private const string transform = "ST_Transform";
        private const string serbianProjection = ", 31277";
        private const string queryTemplate = @"select source.{1} from {0} source where source.{1} in 
                            (select s.{1} from {0} s, {4} t where {2} s.{1} = source.{1} and ({6}( {8}(s.{3} {9}), ( {8}(t.{5} {9}))) {7}));";

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
            VectorLayer targetLayer = layers.Where(x => x.LayerName == comboTarget.SelectedItem.ToString()).First() as VectorLayer;
            PostGIS postgisSource = sourceLayer.DataSource as PostGIS;
            PostGIS postgisTarget = targetLayer.DataSource as PostGIS;
            //  postgis.DefinitionQuery = GetAllLayers();
            string operation = FilterOperation();
            string trans = "", proj = "";
            double? distance = null;
            if (numericDistance.Visible)
            {
                distance = (double)numericDistance.Value;
                trans = transform;
                proj = serbianProjection;
            }
            string query = QueryBuilder(postgisSource.Table, postgisSource.ObjectIdColumn, postgisSource.DefinitionQuery,
                postgisSource.GeometryColumn, postgisTarget.Table, postgisTarget.DefinitionQuery,
                postgisTarget.GeometryColumn, operation, distance, trans, proj);
            string whereArgs = GetWhereArgs(query, postgisSource.ObjectIdColumn);

            postgisSource.DefinitionQuery = whereArgs;
            
        }

        private void CheckIfAlreadyFiltered(VectorLayer layer)
        {
            PostGIS postgis = layer.DataSource as PostGIS;

            //if (!string.IsNullOrEmpty(postgis.DefinitionQuery))
            //{
            //    postgis.DefinitionQuery
            //}
        }

        private string QueryBuilder(string sourceTable, string sourceIdColumn, string sourceWhereArgs, string sourceGeomColumn,
            string targetTable, string targetWhereArgs, string targetGeomColumn,
            string operation, double? distance, string trans, string proj)
        {
            string whereArgs = "";
            string distanceString = "";
            if (!string.IsNullOrEmpty(sourceWhereArgs) || !string.IsNullOrEmpty(targetWhereArgs))
            {
                if (!string.IsNullOrEmpty(sourceWhereArgs))
                {
                    sourceWhereArgs = "s." + sourceWhereArgs.Trim() + " and ";
                }
                if (!string.IsNullOrEmpty(targetWhereArgs))
                {
                    targetWhereArgs = "t." + targetWhereArgs.Trim() + " and ";
                }
                whereArgs += sourceWhereArgs  + targetWhereArgs;
            }
            if (distance.HasValue)
            {
                distanceString = " < " + distance.Value;
            }
            string s = string.Format(queryTemplate, sourceTable, sourceIdColumn, whereArgs,
                sourceGeomColumn, targetTable, targetGeomColumn, operation, distanceString, trans, proj);
            return s;
        }

        public string GetWhereArgs(string query, string idColumn)
        {
            string where = string.Empty;
            using (NpgsqlConnection conn = new NpgsqlConnection(ConfigurationManager.ConnectionStrings["PostgreSQL"].ConnectionString))
            {
                conn.Open();

                using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                {

                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        if (where == string.Empty)
                        {
                            where = idColumn + " in ( ";
                        }

                        where += reader[0] + ",";
                    }

                    if (where != string.Empty)
                    {
                        where = where.Remove(where.Length - 1);
                        where += ")";
                    }
                    else
                    {
                        where = "true = false";
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

        private string FilterOperation()
        {
            switch (comboOperation.SelectedIndex)
            {
                case 0:
                    return "ST_Contains";
                case 1:
                    return "ST_Within";
                case 2:
                    return "ST_Intersects";
                case 3:
                    return "ST_Distance";
            }
            return string.Empty;
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
