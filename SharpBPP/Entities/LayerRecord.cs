using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBPP.Entities
{
    public class LayerRecord
    {
        public string Schema { get; set; }
        public string TableName { get; set; }
        public string GeometryColumnName { get; set; }
        public int CoordDimension { get; set; }
        public int SRID { get; set; }
        public string Type { get; set; }

        public LayerRecord() { }

        public LayerRecord(string Schema, string TableName, string geom, int coord, int srid, string Type)
        {
            this.Schema = Schema;
            this.TableName = TableName;
            this.GeometryColumnName = geom;
            this.CoordDimension = coord;
            this.SRID = srid;
            this.Type = Type;
        }
    }
}
