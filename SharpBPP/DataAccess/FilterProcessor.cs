using GeoAPI.Geometries;
using NetTopologySuite.Utilities;
using SharpBPP.Forms;
using SharpMap.Data.Providers;
using SharpMap.Layers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpBPP.DataAccess
{
    public class FilterProcessor : IDisposable
    {
        private MainForm parent;
        private int circleSize;
        public VectorLayer Layer { get; private set; }
        public IGeometry Geometry { get;private set; }
        public FilterProcessor(MainForm parent, int circleSize)
        {
            this.parent = parent;
            this.circleSize = circleSize;
            parent.BeforeDrawCircle += Parent_BeforeDrawCircle;
        }

        private void Parent_BeforeDrawCircle(object sender, EventArgs e)
        {
            MouseEventArgs args = e as MouseEventArgs;
            Coordinate coords = parent.Box.Map.ImageToWorld(args.Location);
            coords.X -= circleSize / 2;
            coords.Y -= circleSize / 2;

            GeometricShapeFactory gf = new GeometricShapeFactory();
            gf.Base = coords;
            gf.Centre = coords;
            gf.Size = circleSize;
            gf.Width = circleSize;
            IPolygon circle = gf.CreateCircle();            

            GeometryProvider geoProvider = new GeometryProvider(circle);
            Layer = new VectorLayer("CircleLayer");
            Layer.DataSource = geoProvider;

            Layer.Style.Fill = new SolidBrush(Color.FromArgb(50, 51, 181, 229));

            Layer.Style.EnableOutline = true;
            Layer.Style.Outline.Width = 4;

            Layer.Style.Outline.Color = Color.Blue;
        }

        public void Dispose()
        {
            parent.BeforeDrawCircle -= Parent_BeforeDrawCircle;
        }
    }
}
