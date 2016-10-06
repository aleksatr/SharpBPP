using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.Geometries;
using NetTopologySuite.Utilities;
using SharpBPP.Forms;
using SharpMap.Data;
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
        private VectorLayer layer;
        private IGeometry geometry;
        private ICoordinateTransformation coordTrans;
        private ICoordinateTransformation reversTrans;
        public FilterProcessor(MainForm parent, int circleSize)
        {
            this.parent = parent;
            this.circleSize = circleSize;

            coordTrans = parent.Box.Map.Layers.Cast<VectorLayer>().First().CoordinateTransformation;
            reversTrans = parent.Box.Map.Layers.Cast<VectorLayer>().First().ReverseCoordinateTransformation;
        }

        private void CreateGeometry(Point location)
        {
            Coordinate coords = parent.Box.Map.ImageToWorld(location);           

            coords.X -= circleSize / 2;
            coords.Y -= circleSize / 2;
            Coordinate transformedCoordinate = reversTrans.MathTransform.Transform(coords);

            GeometricShapeFactory gf = new GeometricShapeFactory();
            gf.Base = coords;
            gf.Centre = coords;            
            gf.Size = circleSize;
            gf.Width = circleSize;
            gf.Height = circleSize;
            

            IPolygon circle  = gf.CreateCircle();

            Coordinate transformedBottomLeft =  reversTrans.MathTransform.Transform(gf.Envelope.BottomLeft());
            Coordinate transformedBottomRight = reversTrans.MathTransform.Transform(gf.Envelope.BottomRight());

            Coordinate transformedTopLeft = reversTrans.MathTransform.Transform(gf.Envelope.TopLeft());

            gf = new GeometricShapeFactory();
            gf.Base = transformedCoordinate;
            gf.Centre = transformedCoordinate;
            gf.Size = transformedBottomRight.Distance(transformedBottomLeft);
            gf.Width = transformedBottomRight.Distance(transformedBottomLeft);
            gf.Height = transformedTopLeft.Distance(transformedBottomLeft);


            geometry = gf.CreateCircle();

            GeometryProvider geoProvider = new GeometryProvider(circle);

            layer = new VectorLayer("CircleLayer");
            layer.DataSource = geoProvider;

            layer.Style.Fill = new SolidBrush(Color.FromArgb(50, 51, 181, 229));

            layer.Style.EnableOutline = true;
            layer.Style.Outline.Width = 4;

            layer.Style.Outline.Color = Color.Blue;
        }

        public LayerCollection CrateResultLayerCollection(LayerCollection layers, Point location)
        {
            CreateGeometry(location);
            layer.CoordinateTransformation = reversTrans;
            layer.ReverseCoordinateTransformation = coordTrans;

            FeatureDataSet ds = new FeatureDataSet();
            foreach (VectorLayer layer in layers)
            {
              //  try
              //  {
                    layer.DataSource.ExecuteIntersectionQuery(geometry.EnvelopeInternal, ds);
              //  }
              //  catch (Exception exception)
              //  {
              //      filterProcessor.Dispose();
              //      filterProcessor = null;
              //      return;
              //  }
            }

            LayerCollection resultLayerCollection = new LayerCollection();
            foreach (FeatureDataTable table in ds.Tables)
            {
                VectorLayer resultLayer = new VectorLayer(table.TableName);
                List<GeoAPI.Geometries.IGeometry> collection = new List<GeoAPI.Geometries.IGeometry>();
                foreach (FeatureDataRow row in table.Rows)
                {
                    if (row.Geometry.Intersects(geometry))
                    {
                        collection.Add(row.Geometry);
                    }
                    GeometryProvider geoProvider = new GeometryProvider(collection);
                    resultLayer.DataSource = geoProvider;
                    VectorLayer layerOnMap = layers.Where(y => y.LayerName == table.TableName).First() as VectorLayer;
                    resultLayer.Style = layerOnMap.Style;

                    resultLayer.CoordinateTransformation = layerOnMap.CoordinateTransformation;
                    resultLayer.ReverseCoordinateTransformation = layerOnMap.ReverseCoordinateTransformation;
                }
                resultLayerCollection.Add(resultLayer);

            }
            layer.CoordinateTransformation = null;
            layer.ReverseCoordinateTransformation = null;
            resultLayerCollection.Add(layer);

            return resultLayerCollection;
        }

        public void Dispose()
        {
            layer.Dispose();            
        }
    }
}
