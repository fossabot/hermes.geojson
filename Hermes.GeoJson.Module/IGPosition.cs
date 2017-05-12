namespace Hermes.GeoJson.Module
{
    /// <summary>
    ///  Географическая позиция точки
    /// </summary>
    public interface IGPosition
    {
        /// <summary>
        ///     Высота
        /// </summary>
        /// <value>The ele.</value>
        double Ele
        {
            get;
            set;
        }

        /// <summary>
        ///     Долгота
        /// </summary>
        /// <value>The lat.</value>
        double Lat
        {
            get;
            set;
        }

        /// <summary>
        ///     Широта
        /// </summary>
        /// <value>The lon.</value>
        double Lon
        {
            get;
            set;
        }
    }
}