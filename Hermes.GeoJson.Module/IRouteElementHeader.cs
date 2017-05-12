using System;

namespace Hermes.GeoJson.Module
{
    /// <summary>
    ///     Заголовок элемента маршрута траспортного средства
    /// </summary>
    public interface IRouteElementHeader
    {
        /// <summary>
        ///     Дистанция
        /// </summary>
        double Distance { get; set; }

        /// <summary>
        ///     Длительность
        /// </summary>
        TimeSpan Duration { get; set; }

        /// <summary>
        ///     Начало движения
        /// </summary>
        DateTime StarTime { get; set; }

        /// <summary>
        ///     Окончание движения
        /// </summary>
        DateTime StopTime { get; set; }
    }
}
