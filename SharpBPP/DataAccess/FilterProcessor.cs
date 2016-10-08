using GeoAPI.CoordinateSystems.Transformations;
using GeoAPI.Geometries;
using NetTopologySuite.Geometries;
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

        public FilterProcessor(MainForm parent)
        {
            this.parent = parent;

            coordTrans = parent.Box.Map.Layers.Cast<VectorLayer>().First().CoordinateTransformation;
            reversTrans = parent.Box.Map.Layers.Cast<VectorLayer>().First().ReverseCoordinateTransformation;
        }
        public FilterProcessor(MainForm parent, int circleSize)
            :this(parent)
        {            
            this.circleSize = circleSize;
        }

        private void CreateGeometry(System.Drawing.Point location, bool withLayer = true)
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


            IPolygon circle = gf.CreateCircle();


            geometry = CreateGeometryToCompare(circle.Coordinates);
            if(withLayer)
                layer = CreateLayerFromDrawing(circle, "CircleLayer", Color.FromArgb(50, 51, 181, 229), Color.Blue);
        }

        private IPolygon CreateGeometryToCompare(Coordinate[] coordinates)
        {
            Coordinate[] transformedCoordinates = new Coordinate[coordinates.Length];

            for (int i = 0; i < coordinates.Length - 1; i++)
            {
                Coordinate transformed = reversTrans.MathTransform.Transform(coordinates[i]);
                transformedCoordinates[i] = transformed;
            }
            transformedCoordinates[coordinates.Length - 1] = transformedCoordinates.First();

            LinearRing lr = new LinearRing(transformedCoordinates);
            return new Polygon(lr); ;
        }

        private VectorLayer CreateLayerFromDrawing(IGeometry geom, string name, Color fillColor, Color outlineColor)
        {
            GeometryProvider geoProvider = new GeometryProvider(geom);

            VectorLayer resultLayer= new VectorLayer(name);
            resultLayer.DataSource = geoProvider;

            resultLayer.Style.Fill = new SolidBrush(fillColor);

            resultLayer.Style.EnableOutline = true;
            resultLayer.Style.Outline.Width = 4;

            resultLayer.Style.Outline.Color = outlineColor;

            return resultLayer;
        }

        private FeatureDataSet GetFeatureDataSet(LayerCollection layers, IGeometry geometry)
        {
            FeatureDataSet ds = new FeatureDataSet();
            foreach (VectorLayer layer in layers)
            {
                layer.DataSource.ExecuteIntersectionQuery(geometry.EnvelopeInternal, ds);
            }
            return ds;
        }

        public LayerCollection CrateResultLayerCollection(LayerCollection layers, System.Drawing.Point location)
        {
            CreateGeometry(location);
            layer.CoordinateTransformation = reversTrans;
            layer.ReverseCoordinateTransformation = coordTrans;

            FeatureDataSet ds = GetFeatureDataSet(layers, geometry);

            LayerCollection resultLayerCollection = new LayerCollection();
            foreach (FeatureDataTable table in ds.Tables)
            {
                List<GeoAPI.Geometries.IGeometry> collection = new List<GeoAPI.Geometries.IGeometry>();
                foreach (FeatureDataRow row in table.Rows)
                {
                    if (row.Geometry.Intersects(geometry))
                    {
                        collection.Add(row.Geometry);
                    }
                }
                VectorLayer layerOnMap = layers.Where(y => y.LayerName == table.TableName).First() as VectorLayer;
                VectorLayer resultLayer = CreateResultLayer(layerOnMap, collection);
                resultLayerCollection.Add(resultLayer);
            }
            layer.CoordinateTransformation = null;
            layer.ReverseCoordinateTransformation = null;
            resultLayerCollection.Add(layer);

            return resultLayerCollection;
        }

        private VectorLayer CreateResultLayer(VectorLayer layerOnMap, IEnumerable<IGeometry> collection)
        {
            VectorLayer resultLayer = new VectorLayer(layerOnMap.LayerName);
            GeometryProvider geoProvider = new GeometryProvider(collection);
            resultLayer.DataSource = geoProvider;
            resultLayer.Style = layerOnMap.Style;
            resultLayer.CoordinateTransformation = layerOnMap.CoordinateTransformation;
            resultLayer.ReverseCoordinateTransformation = layerOnMap.ReverseCoordinateTransformation;

            return resultLayer;
        }

        public LayerCollection PolygonFiltering(LayerCollection layers, IGeometry geom)
        {
            IGeometry polygon = (IGeometry)geom.Clone();
            LayerCollection resultLayerCollection = new LayerCollection();
            foreach (VectorLayer layerOnMap in layers)
            {
                List<IGeometry> geometries = layerOnMap.DataSource.GetGeometriesInView(layerOnMap.DataSource.GetExtents()).ToList();

                IPolygon p = CreateGeometryToCompare(polygon.Coordinates);
                
                VectorLayer resultLayer = CreateResultLayer(layerOnMap, geometries.Where(g => g.Within(p)));
                resultLayerCollection.Add(resultLayer);
            }
            layer = CreateLayerFromDrawing(geom, "PolygonLayer", Color.FromArgb(50, 51, 181, 229), Color.Blue);
            resultLayerCollection.Add(layer);
            return resultLayerCollection;
        }

        public string GetFeatureInfo(System.Drawing.Point location, LayerCollection collection)
        {
            string featureInfo = "";
            CreateGeometry(location, false);

            FeatureDataSet ds = GetFeatureDataSet(collection, geometry);

            LayerCollection resultLayerCollection = new LayerCollection();
            foreach (FeatureDataTable table in ds.Tables)
            {
                if (table.Rows.Count > 0)
                {
                    featureInfo += table.TableName + ":" + Environment.NewLine;
                    foreach (FeatureDataRow row in table.Rows)
                    {
                        for (int i = 0; i < row.ItemArray.Length; i++)
                        {
                            featureInfo += table.Columns[i] + " : " + row.ItemArray[i] + Environment.NewLine;
                        }
                    }
                }
            }

            return featureInfo;
        }

        public void Dispose()
        {
            layer.Dispose();            
        }
    }
}
