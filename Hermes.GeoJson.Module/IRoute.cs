using System.Collections.Generic;

namespace Hermes.GeoJson.Module
{
    /// <summary>
    ///     Маршрут траспортного средства
    /// </summary>
    public interface IRoute
    {
        /// <summary>
        ///     Заголовок маршрута траспортного средства
        /// </summary>
        IRouteElementHeader Header { get; set; }

        /// <summary>
        ///     Сегменты
        /// </summary>
        IEnumerable<IRouteSegment> Segments { get; set; }
    }
}
