using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GeoJSON.Net.Feature;

namespace Hermes.GeoJson.Module
{
    /// <summary>
    ///  Географическая позиция точки
    /// </summary>
    public interface IGPosition
    {
        /// <summary>
        ///     Долгота
        /// </summary>
        /// <value>The lat.</value>
        double Lat { get; set; }

        /// <summary>
        ///     Широта
        /// </summary>
        /// <value>The lon.</value>
        double Lon { get; set; }

        /// <summary>
        ///     Высота
        /// </summary>
        /// <value>The ele.</value>
        double Ele { get; set; }
    }

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
    /// <summary>
    /// Базовый интерфейс для геообъектов
    /// </summary>
    public interface IHermesGeoJsonObject<T> where T : class
    {
        /// <summary>
        /// Gets or sets the feature collection.
        /// </summary>
        /// <value>The feature collection.</value>
        FeatureCollection FeatureCollection { get; set; }
    }

    /// <summary>
    /// Hermes geo json factory.
    /// </summary>
    public interface IHermesGeoJsonFactory<T, TItem>
        where T : class
        where TItem : class
    {
        /// <summary>
        /// А
        /// </summary>
        /// <returns>The object async.</returns>
        /// <param name="item">Item.</param>
        Task<IEnumerable<IHermesGeoJsonObject<T>>> CreateObjectAsync(TItem item);
    }

    static class MaybeMonadic
    {
        public static TResult With<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator) where TResult : class where TInput : class
        {
            return o == null ? null : evaluator(o);
        }

        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult failureValue) where TInput : class
        {
            return o == null ? failureValue : evaluator(o);
        }

        public static TInput If<TInput>(this TInput o, Func<TInput, bool> evaluator) where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? o : null;
        }

        public static TInput Unless<TInput>(this TInput o, Func<TInput, bool> evaluator)
               where TInput : class
        {
            if (o == null) return null;
            return evaluator(o) ? null : o;
        }

        public static TResult WithValue<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator) where TInput : struct
        {
            return evaluator(o);
        }

        public static TInput Do<TInput>(this TInput o, Action<TInput> action) where TInput : class
        {
            if (o == null) return null;
            action(o);
            return o;
        }
    }
}
