using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;

namespace Hermes.GeoJson.Module.Services
{
    public static partial class RouteExtention{

        /// <summary>
        /// Tos the feature.
        /// </summary>
        /// <returns>The feature.</returns>
        /// <param name="segment">Segment.</param>
        public static Feature ToFeature(this IRouteSegment segment)
        {
            var geopositions = segment.Points.Select(x => new GeographicPosition(x.Position.Lat, x.Position.Lon, x.Position.Ele));
            var speedList = segment.Points.Select(x => new {x.Speed,x.Time}).ToList();
            var properties = new Dictionary<string, object>
            {
                { "speed", speedList }
            };
            var lineString = new LineString(geopositions);
            return new Feature(lineString,properties);
        }

        /// <summary>
        /// Tos the feature collection.
        /// </summary>
        /// <returns>The feature collection.</returns>
        /// <param name="segments">Segments.</param>
        public static FeatureCollection ToFeatureCollection(this IEnumerable<IRouteSegment> segments){

            var list = new List<Feature>();
            foreach (var item in segments)
            {
                var feature = item.ToFeature();
                list.Add(feature);
            }
            return new FeatureCollection(list);
        }
    }
}
