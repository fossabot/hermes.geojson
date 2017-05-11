using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Hermes.GeoJson.Module.Model;

namespace Hermes.GeoJson.Module.Services
{
    public static class RouteExtention{

        /// <summary>
        /// Tos the feature.
        /// </summary>
        /// <returns>The feature.</returns>
        /// <param name="segment">Segment.</param>
        public static Feature ToFeature(this IRouteSegment segment){

            var geopositions = segment.Points.Select(x=> new GeographicPosition(x.Position.Lat, x.Position.Lon, x.Position.Ele));
            var lineString = new LineString(geopositions);
            return new Feature(lineString);
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

    public interface IRouteFactory:IHermesGeoJsonFactory<IRoute,XDocument>
    {
    }

    /// <summary>
    /// Фабрика создания элементов из документа XML
    /// </summary>
    public sealed class RouteFromXMLFactory : IRouteFactory
    {
        /// <summary>
        /// Асинхронное создание географического объекта из документа в формате gpx
        /// </summary>
        /// <returns>The object async.</returns>
        /// <param name="item">Item.</param>
        public async Task<IEnumerable<IHermesGeoJsonObject<IRoute>>> CreateObjectAsync(XDocument item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
            var task = Task<IEnumerable<IHermesGeoJsonObject<IRoute>>>.Factory.StartNew(() => {

                try
                {
                    //Контент документа
                    var trackContent = item.ToTrackRouteSegment().ToSegmentsDictionary();
                    var result = new List<IHermesGeoJsonObject<IRoute>>();
                    foreach (var key in trackContent)
                    {
                        var date = key.Key;
                        var conetent = key.Value;
                        //TODO Дополнительное преобразование структуры данных для получения представления о элементах
                        var segments = new List<IRouteSegment>();
                        foreach (var sgmt in conetent)
                        {
                            var header = sgmt.ToSegmentHeader();
                            var segment = new RouteSegment()
                            {
                                Header = header,
                                Points = sgmt
                            };
                            segments.Add(segment);
                        }
                        var segmentHeader = segments.ToRouteHeader();
                        var resultItem = new Route
                        {
                            Header = segmentHeader,
                            Segments = segments,
                            FeatureCollection = segments.ToFeatureCollection()

                        };
                        result.Add(resultItem);
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
            return await task;
        }
    }


	/// <summary>
	/// Методы расширения для обработки данных в формате geojson
	/// </summary>
	public static class GeoJsonExtentionMethods
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
                list.AddRange((IEnumerable<IPosition>)routePoints.Select(p => new GeographicPosition(p.Position.Lat, p.Position.Lon, p.Position.Ele)));
			var line = new LineString(list);
			var properties = new Dictionary<string, object>
				{
					{"Distance",routePoints.ToSegmentDistance()},
					{"Duration",$"{routePoints.ToSegmentDuration()}"},
					{"Start",$"{segment.Header.StarTime}"},
					{"Stop",$"{segment.Header.StopTime}"},
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
