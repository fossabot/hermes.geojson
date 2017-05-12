using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using Hermes.GeoJson.Module.Model;

namespace Hermes.GeoJson.Module.Services
{
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
}
