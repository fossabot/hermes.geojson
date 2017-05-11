using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using Hermes.GeoJson.Module.Model;

namespace Hermes.GeoJson.Module
{

    /// <summary>
    /// Routes dictionary base.
    /// </summary>
    public abstract class TrackContextBase : Dictionary<DateTime, List<List<IRoutePoint>>>
	{

	}
    /// <summary>
    /// Track context.
    /// </summary>
    public sealed class TrackContext:TrackContextBase{
        
    }

	/// <summary>
	/// Обработанные данные по сегментам маршрутов
	/// </summary>
	public sealed class Segments : Dictionary<int, IEnumerable<IRoutePoint>>
	{

	}

	/// <summary>
	/// Исходные данные сегментов маршрутов (неотсортированные в сыром виде)"
	/// </summary>
	public sealed class SegmentSource : Dictionary<int, IEnumerable<XElement>>
	{

	}
	/// <summary>
	/// Методы рсширения для конвертера маршрутов автотраспорта в формате gpx
	/// </summary>
	public static class GpxFileDataConverter
	{
		/// <summary>
		/// Радиус Земли
		/// </summary>
		private const double EarthRadius = 6378.7;

		/// <summary>
		/// Возвращает результат компаратора точки трека в дате
		/// </summary>
		private static readonly Func<DateTime, IRoutePoint, bool> CheckPointInDate = (date, point) =>
		{
			var date1 =
				date.ToBeginDayDateTime();
			var date2 =
				point.Time.ToBeginDayDateTime();
			return date1.Equals(date2);
		};

		/// <summary>
		/// Сегмент трека
		/// </summary>
		// ReSharper disable once ArrangeTypeMemberModifiers
		const string Trkseg = "trkseg";


		/// <summary>
		/// Возвращает набор дат
		/// </summary>
		/// <param name="segments">Сегменты треков</param>
		/// <returns>Список дат треков</returns>
		private static IEnumerable<DateTime> ToDateList(this IEnumerable<List<IRoutePoint>> segments)
		{
			var points = new List<IRoutePoint>();
			foreach (var segment in segments)
			{
				points.AddRange(segment);
			}
			var q = (from items in points select items.Time.ToBeginDayDateTime()).Distinct();
			return q.ToList();
		}

		/// <summary>
		/// Возвращает соловарь сегментов треков по датаи
		/// </summary>
		/// <param name="segments">Отфильтрованные сегменты трека</param>
		/// <returns>Словарь сегментов треков по датам</returns>
		/// <exception cref="Exception">A delegate callback throws an exception.</exception>
		public static TrackContext ToSegmentsDictionary(this IEnumerable<List<IRoutePoint>> segments)
		{
			if (segments == null)
				throw new ArgumentNullException(nameof(segments));
			var result = new TrackContext();
			var enumerable = segments as List<IRoutePoint>[] ?? segments.ToArray();
			var dates = enumerable.ToDateList();
			//Для каждой даты в полученных данных
			foreach (var dateTime in dates)
			{
				//новый трек
				var time1 = dateTime;
				var segmetpoint = (from segment in enumerable
								   let time = time1
								   select segment.Where(trackPoint => CheckPointInDate(time, trackPoint)).ToList()).ToList();
				//для каждого сегмента треков
				result.Add(dateTime, segmetpoint);
			}
			return result;
		}

		/// <summary>
		///  Возвращает дистанцию по сегменту трека
		/// </summary>
		/// <param name="points"></param>
		/// <returns></returns>
		public static double ToSegmentDistance(this IEnumerable<IRoutePoint> points)
		{
			if (points == null)
				throw new ArgumentNullException(nameof(points));
			var p = points.ToList();
			var result = 0.0;
			for (var i = 0; i < p.Count; i++)
			{
				var p1 = p[i];
				if (i == p.Count - 1) continue; //!+Что делать если точка одна
				var p2 = p[i + 1];
				result = result + GetDistanceFromPoints(p1, p2);
			}
			return result;
		}
		/// <summary>
		/// Длительность сегмента трека
		/// </summary>
		/// <param name="points">Набр точек, характеризующих маршрут трансопртного средства</param>
		/// <returns></returns>
		public static TimeSpan ToSegmentDuration(this IEnumerable<IRoutePoint> points)
		{
			if (points == null)
				throw new ArgumentNullException(nameof(points));
			var q = points.OrderBy(x => x.Time);
			var d1 = q.FirstOrDefault();
			var d2 = q.LastOrDefault();
			if ((d2 != null) && (d1 != null)) return d2.Time - d1.Time;
			return TimeSpan.Zero;
		}
		/// <summary>
		/// Возвращает растояние между двумя географическими точками
		/// </summary>
		/// <param name="point1">Первая точка(начало отсчета)</param>
		/// <param name="point2">Вторая точка(окончание отсчета)</param>
		/// <returns>Растояние между двумя географическими точками</returns>
		/// <remarks>
		/// Используется расчет предоставленный  http://www.meridianworlddata.com/Distance-Calculation.asp
		/// Формула расчета x = EarthRadius * arctan[sqrt(1-x^2)/x], где
		/// EarthRadius - радиус Земли
		/// х = x = [sin(lat1/57.2958) * sin(lat2/57.2958)] +
		/// +[cos(lat1/57.2958) * cos(lat2/57.2958) * cos(lon2/57.2958 - lon1/57.2958)]
		/// </remarks>
		private static double GetDistanceFromPoints(IRoutePoint point1, IRoutePoint point2)
		{
			var dLat1InRad = point1.Position.Lat * (Math.PI / 180.0);
			var dLong1InRad = point1.Position.Lon * (Math.PI / 180.0);
			var dLat2InRad = point2.Position.Lat * (Math.PI / 180.0);
			var dLong2InRad = point2.Position.Lon * (Math.PI / 180.0);
			var dLongitude = dLong2InRad - dLong1InRad;
			var dLatitude = dLat2InRad - dLat1InRad;
			var x = Math.Pow(Math.Sin(dLatitude / 2.0), 2.0) +
				Math.Cos(dLat1InRad) * Math.Cos(dLat2InRad) *
				Math.Pow(Math.Sin(dLongitude / 2.0), 2.0);
			var dist = 2.0 * Math.Atan2(Math.Sqrt(x), Math.Sqrt(1.0 - x));
			return EarthRadius * dist;
		}

		/// <summary>
		/// Возвращает коллекцию сегментов трека согласно полученному документу gpx
		/// </summary>
		/// <param name="document">Входной файл формата gpx</param>
		/// <returns>Сегменты полученного трека</returns>
		public static IEnumerable<List<IRoutePoint>> ToTrackRouteSegment(this XDocument document)
		{
			if (document == null)
				throw new ArgumentNullException(nameof(document));
			var result = new List<List<IRoutePoint>>();
			//все треки в текущем документе
			if (document.Root == null) return result.ToList().Distinct();
			// ReSharper disable once MaximumChainedReferences
			var tracks = document.Root.Elements().Where(x => x.Name.LocalName.Equals("trk"));
			foreach (var segments in tracks
					 .Select(xElement => xElement.Elements()
							 .Where(x => x.Name.LocalName.Equals("trkseg"))))
			{
				result.AddRange(from segment in segments
								let pointlist = new List<IRoutePoint>()
								select segment.ToPointsSegmentList());
			}
			return result.ToList().Distinct();
		}
		/// <summary>
		/// Время на начало дня
		/// </summary>
		/// <param name="date"></param>
		/// <returns></returns>
		private static DateTime ToBeginDayDateTime(this DateTime date)
		{
			return date.AddHours(-date.Hour)
					   .AddMinutes(-date.Minute)
					   .AddSeconds(-date.Second)
					   .AddMilliseconds(-date.Millisecond);
		}
		/// <summary>
		/// анализ треков в формате xml и получение набора точек, составляющих маршрут
		/// </summary>
		private static List<IRoutePoint> ToPointsSegmentList(this XContainer item)
		{
			if (item == null)
				throw new ArgumentNullException(nameof(item));
			var ci = new CultureInfo("en-US");
			return (from point in item.Elements()
					let attribute = point.Attribute("lat")
					where attribute != null
					let latitude = double.Parse(attribute.Value, ci)
					let xAttribute = point.Attribute("lon")
					where xAttribute != null
					let longtitude = double.Parse(xAttribute.Value, ci)
					let qTime = (from items in point.Elements()
								 where items.Name.LocalName == "time"
								 select items)
					let orDefault = qTime.FirstOrDefault()
					where orDefault != null
					let time = DateTime.Parse(orDefault.Value, ci, DateTimeStyles.AssumeLocal)
					let qSpeed = (from items in point.Elements()
								  where items.Name.LocalName == "speed"
								  select items)
					let firstOrDefault = qSpeed.FirstOrDefault()
					where firstOrDefault != null
					let speed = double.Parse(firstOrDefault.Value, ci)
					let qEle = (from items in point.Elements()
								where items.Name.LocalName == "ele"
								select items).FirstOrDefault()
					let ele = double.Parse(qEle.Value, ci)

					select new RoutePoint
					{
                        Position = new GPosition
						{
							Lat = latitude,
							Lon = longtitude,
							Ele = ele
						},
						Speed = speed * 3600 / 1000,
						Time = time
					}).Cast<IRoutePoint>().ToList();
		}

		public static Segments SegmentSourceToSegments(this SegmentSource source)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));
			var segments = new Segments();
			foreach (var item in source)
			{
				var key = item.Key;
				var val = item.Value;
				var l = new List<IRoutePoint>();
				foreach (var v in val)
				{
					var point = v.ToRoutePoint();
					l.Add(point);
				}
				segments.Add(key, l);
			}
			return segments;
		}

		/// <summary>
		/// Получение сегментов маршрутов транспорта
		/// </summary>
		/// <returns>The segments.</returns>
		/// <param name="trk">Trk.</param>
		public static SegmentSource ToSegmentSource(this IEnumerable<XElement> trk)
		{
			if (trk == null)
				throw new ArgumentNullException(nameof(trk));
			var result = new SegmentSource();
			var i = 0;
			foreach (var elements in trk.Select(item => item.Elements()
				.Where(x => x.Name.LocalName
					.ToLower()
					.Equals(Trkseg))))
			{
				result.Add(i, elements);
				i++;
			}
			return result;
		}

		/// <summary>
		/// Преобразование элемента XML в точку марщрута
		/// </summary>
		/// <returns>The route point.</returns>
		/// <param name="element">Element.</param>
		public static IRoutePoint ToRoutePoint(this XElement element)
		{
			var result = new RoutePoint();
			var timeValue = element.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("time"));
			var ci = CultureInfo.InvariantCulture;
			if (timeValue != null)
			{
                if (DateTime.TryParse(timeValue.Value, ci, DateTimeStyles.AssumeUniversal, out DateTime time))
                {
                    result.Time = time;
                }
            }
			var latValue = element.Attributes().FirstOrDefault(x => x.Name.LocalName.Equals("lat"));
			if (latValue != null)
			{
                if (double.TryParse(latValue.Value, NumberStyles.Number, ci, out double lat))
                {
                    result.Position.Lat = lat;
                }
            }
			var lonValue = element.Attributes().FirstOrDefault(x => x.Name.LocalName.Equals("lon"));
			if (lonValue != null)
			{
                if (double.TryParse(lonValue.Value, NumberStyles.Number, ci, out double lon))
                {
                    result.Position.Lon = lon;
                }
            }
			var speedValue = element.Elements().FirstOrDefault(x => x.Name.LocalName.Equals("speed"));
			if (speedValue != null)
			{
                if (double.TryParse(speedValue.Value, NumberStyles.Number, ci, out double speed))
                {
                    result.Speed = speed;
                }
                else
                {
                    result.Speed = -1;
                }
            }
			else
			{
				result.Speed = -1;
			}
			return result;

		}

		/// <summary>
		/// Время на начало дня(гавнокод!!!!)
		/// </summary>
		/// <returns>The begin day.</returns>
		/// <param name="time">Time.</param>
		public static DateTime ToBeginDay(this DateTime time)
		{
			return
				time.AddHours(-time.Hour)
					.AddMinutes(-time.Minute)
					.AddSeconds(-time.Second)
					.AddMilliseconds(-time.Millisecond);
		}

		/// <summary>
		///  Возвращает даты полученных треков на основании всех точек
		/// </summary>
		/// <returns>The route dates.</returns>
		/// <param name="dict">Dict.</param>
		public static IEnumerable<DateTime> ToRouteDates(this Dictionary<int, IEnumerable<IEnumerable<IRoutePoint>>> dict)
		{
			if (dict == null)
				throw new ArgumentNullException(nameof(dict));
			var result = new List<DateTime>();
			var temp = new List<IRoutePoint>();
			foreach (var item in dict)
			{
				var val = item.Value;
				foreach (var segment in val)
				{
					temp.AddRange(segment);
				}
			}
			var q = temp.Select(x => x.Time.ToBeginDay()).Distinct();
			result.AddRange(q);
			return result;
		}
		/// <summary>
		/// фильтрация данных в сегменте трека по дню
		/// </summary>
		/// <returns>The filtered by day.</returns>
		/// <param name="segment">Segment.</param>
		/// <param name="date">Date.</param>
		private static IEnumerable<IRoutePoint> ToFilteredByDay(this IEnumerable<IRoutePoint> segment, DateTime date)
		{
			var q = segment.Where(x => x.Time.ToBeginDay() == date);
			return q;
		}

		/// <summary>
		/// Tos the route filtered by day.
		/// </summary>
		/// <returns>The route filtered by day.</returns>
		/// <param name="track">Track.</param>
		/// <param name="date">Date.</param>
		private static IEnumerable<IEnumerable<IRoutePoint>> ToRouteFilteredByDay(this IEnumerable<IEnumerable<IRoutePoint>> track, DateTime date)
		{
			if (track == null)
				throw new ArgumentNullException(nameof(track));
			return track.Select(item => item.ToFilteredByDay(date)).Where(o => o != null).ToList();
		}

		/// <summary>
		/// Возвращает преобразованную до определенного вида
		/// </summary>
		/// <returns>The route by date.</returns>
		/// <param name="dictionary">Dictionary.</param>
		/// <param name="dates">Dates.</param>
		public static Dictionary<DateTime, IEnumerable<IEnumerable<IEnumerable<IRoutePoint>>>> ToRouteByDate(
				this Dictionary<int, IEnumerable<IEnumerable<IRoutePoint>>> dictionary,
			IEnumerable<DateTime> dates)
		{
			if (dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));
			if (dates == null)
				throw new ArgumentNullException(nameof(dates));
			var result = new Dictionary<DateTime, IEnumerable<IEnumerable<IEnumerable<IRoutePoint>>>>();
			foreach (var item in dates)
			{
				var key = item;
				var item1 = item;
				var r = dictionary.Select(route => route.Value)
								  .Select(track => track.ToRouteFilteredByDay(item1)).ToList();
				result.Add(key, r);
			}
			return result;
		}

		/// <summary>
		/// Возвращает сегменты по дням
		/// </summary>
		/// <returns>The segments by day.</returns>
		/// <param name="sourceDictionary">Source dictionary.</param>
		private static Dictionary<DateTime, IEnumerable<IEnumerable<IRoutePoint>>> ToSegmentsByDay(
			this Dictionary<int, IEnumerable<IEnumerable<IRoutePoint>>> sourceDictionary)
		{
			if (sourceDictionary == null)
				throw new ArgumentNullException(nameof(sourceDictionary));
			var result = new Dictionary<DateTime, IEnumerable<IEnumerable<IRoutePoint>>>();
			var dates = sourceDictionary.ToRouteDates();
			var t = sourceDictionary.ToRouteByDate(dates);
			foreach (var elements in t)
			{
				var l = new List<List<IRoutePoint>>();
				var key = elements.Key;
				var value = elements.Value; //все сегменты
				foreach (var item in value)
				{
					l.AddRange(from o in item
							   select o as IRoutePoint[] ?? o.ToArray()
						into points
							   where points.Count() > 1
							   select points.ToList());
				}
				result.Add(key, l);
			}
			return result;
		}

		/// <summary>
		///  Получение данных о сегменте маршрута транспорта
		/// </summary>
		/// <returns>The segment header.</returns>
		/// <param name="points">Points.</param>
		public static IRouteElementHeader ToSegmentHeader(this IEnumerable<IRoutePoint> points)
		{
			if (points == null)
				throw new ArgumentNullException(nameof(points));
			var routePoints = points as IRoutePoint[] ?? points.ToArray();
			var h = new RouteElementHeader
			{
				Distance = routePoints.ToSegmentDistance(),
				Duration = routePoints.ToSegmentDuration(),
				StarTime = routePoints.FirstOrDefault().Time,
				StopTime = routePoints.LastOrDefault().Time
			};
			return h;
		}

		/// <summary>
		/// Получение заголовочных данных по маршруту за день на основании данных сегмента
		/// </summary>
		/// <returns>The route header.</returns>
		public static IRouteElementHeader ToRouteHeader(this IEnumerable<IRouteSegment> segments)
		{
			if (segments == null)
				throw new ArgumentNullException(nameof(segments));
			var routeSegments = segments as IRouteSegment[] ?? segments.ToArray();
			var h = new RouteElementHeader { Distance = routeSegments.Sum(a => a.Header.Distance) };
			var duration = routeSegments.Select(item => item.Header.Duration).Aggregate(TimeSpan.Zero, (current, d) => current + d);
			h.Duration = duration;
			return h;
		}

		/// <summary>
		/// Получение заголовочных данных по треку на основании данных о маршрутах за день
		/// </summary>
		/// <returns>The track header.</returns>
		/// <param name="routes">Routes.</param>
		public static IRouteElementHeader ToTrackHeader(this IEnumerable<IRoute> routes)
		{
			if (routes == null)
				throw new ArgumentNullException(nameof(routes));
			var enumerable = routes as IRoute[] ?? routes.ToArray();
			var h = new RouteElementHeader { Distance = enumerable.Sum(a => a.Header.Distance) };
			var duration = enumerable.Select(item => item.Header.Duration).Aggregate(TimeSpan.Zero, (current, d) => current + d);
			h.Duration = duration;
			return h;
		}

	}
}