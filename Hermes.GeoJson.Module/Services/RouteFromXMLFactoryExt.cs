using System;
using System.Collections.Generic;
using System.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;

namespace Hermes.GeoJson.Module.Services
{


    /// <summary>
    /// Методы расширения для обработки данных в формате geojson
    /// </summary>
    public static class RouteFromXMLFactoryExt
	{

		/// <summary>
		/// Tos the feature collection.
		/// </summary>
		/// <returns>The feature collection.</returns>
		/// <param name="segment">Segment.</param>
		private static Feature SegmentToFeature(this IRouteSegment segment)
		{
			Feature result = null;
            var list = new List<IPosition>();
			segment.With(x => x.Points.Do(pl =>
		{
			var routePoints = pl as IRoutePoint[] ?? pl.ToArray();
                list.AddRange(routePoints.Select(p => new GeographicPosition(p.Position.Lat, p.Position.Lon, p.Position.Ele)));
            var speedList = routePoints.Select(rp => new { rp.Time, rp.Speed }).ToList();

			var line = new LineString(list);
			var properties = new Dictionary<string, object>
				{
					{"Distance",routePoints.ToSegmentDistance()},
					{"Duration",$"{routePoints.ToSegmentDuration()}"},
					{"Start",$"{segment.Header.StarTime}"},
					{"Stop",$"{segment.Header.StopTime}"},
                    {"MovePoints",speedList }
				};
			result = new Feature(line, properties);
		}));
			return result;
		}

		/// <summary>
		/// Tos the feature collection.
		/// </summary>
		/// <returns>The feature collection.</returns>
		/// <param name="routes">Routes.</param>
		public static FeatureCollection RouteToFeatureCollection(this IRoute routes)
		{
			if (routes == null)
				throw new ArgumentNullException(nameof(routes));
			FeatureCollection result = null;
			routes.With(x => x.Segments.Do(segments =>
			{
				result = new FeatureCollection(segments.Select(segment => segment.SegmentToFeature()).ToList());
			}));
			return result;
		}

		/// <summary>
		///     Получение коллекции геометрий для точек доставки GeoJson
		/// </summary>
		/// <param name="collection">Коллекция точек доставки</param>
		/// <returns></returns>
		public static FeatureCollection ToFeatureCollection(this IEnumerable<Feature> collection)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection));
			return new FeatureCollection(collection.ToList());
		}
	}
}
