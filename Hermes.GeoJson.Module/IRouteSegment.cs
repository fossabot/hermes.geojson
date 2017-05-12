using System.Collections.Generic;

namespace Hermes.GeoJson.Module
{
    /// <summary>
    ///     Сегмент маршрута автотраспорта
    /// </summary>
    public interface IRouteSegment
    {
        /// <summary>
        ///     Заголовок
        /// </summary>
        IRouteElementHeader Header { get; set; }

        /// <summary>
        /// Точки маршрута
        /// </summary>
        IEnumerable<IRoutePoint> Points { get; set; }
    }
}
