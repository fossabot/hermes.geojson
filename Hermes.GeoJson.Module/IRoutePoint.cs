using System;

namespace Hermes.GeoJson.Module
{
    /// <summary>
    ///     Точка маршрута траспорного средства
    /// </summary>
    public interface IRoutePoint
    {
        /// <summary>
        ///     позиция
        /// </summary>
        IGPosition Position { get; set; }

        /// <summary>
        ///     Скорость
        /// </summary>
        double Speed { get; set; }

        /// <summary>
        ///     Время фиксации измерений
        /// </summary>
        DateTime Time { get; set; }
    }
    }
