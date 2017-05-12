using GeoJSON.Net.Feature;

namespace Hermes.GeoJson.Module
{
    /// <summary>
    /// Базовый интерфейс для геообъектов
    /// </summary>
    public interface IHermesGeoJsonObject<T> where T : class
    {
        IRouteElementHeader Header { get; set; }

        /// <summary>
        /// Gets or sets the feature collection.
        /// </summary>
        /// <value>The feature collection.</value>
        FeatureCollection FeatureCollection { get; set; }
    }
}
