using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hermes.GeoJson.Module
{
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
}
