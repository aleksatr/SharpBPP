using GeoAPI.CoordinateSystems;
using GeoAPI.Geometries;
using ProjNet.CoordinateSystems;
using SharpMap.Layers;
using SharpMap.Rendering.Symbolizer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBPP.Helpers
{
    public static class TransformHelper
    {
        public static IProjectedCoordinateSystem CreateUtmProjection(int utmZone)
        {
            CoordinateSystemFactory cFac = new CoordinateSystemFactory();
            IEllipsoid elipsoid = cFac.CreateFlattenedSphere("WGS 84", 6378137, 298.257, LinearUnit.Metre);
            IHorizontalDatum datum = cFac.CreateHorizontalDatum("WGS_1984", DatumType.HD_Geocentric, elipsoid, null);
            IGeographicCoordinateSystem gcs = cFac.CreateGeographicCoordinateSystem("WGS 84", AngularUnit.Degrees, datum,
                PrimeMeridian.Greenwich, new AxisInfo("Lon", AxisOrientationEnum.East), new AxisInfo("Lat", AxisOrientationEnum.North));

            List<ProjectionParameter> parameters = new List<ProjectionParameter>();

            parameters.Add(new ProjectionParameter("latitude_of_origin", 0.0));
            parameters.Add(new ProjectionParameter("central_meridian", -183 + 6 * utmZone));
            parameters.Add(new ProjectionParameter("scale_factor", 0.9996));
            parameters.Add(new ProjectionParameter("false_easting", 500000));
            parameters.Add(new ProjectionParameter("false_northing", 0.0));
            IProjection projection = cFac.CreateProjection("Transverse Mercator", "Transverse Mercator", parameters);


            return cFac.CreateProjectedCoordinateSystem("WGS 84 / UTM zone " + utmZone + "N", gcs, projection, LinearUnit.Metre,
                new AxisInfo("East", AxisOrientationEnum.East), new AxisInfo("North", AxisOrientationEnum.North));
        }       
    }
}
