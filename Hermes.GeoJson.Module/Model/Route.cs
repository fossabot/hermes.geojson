namespace Hermes.GeoJson.Module.Model
{
    using GeoJSON.Net.Feature;
    using System.Collections.Generic;

    /// <summary>
    /// Route.
    /// </summary>
    public sealed class Route : IHermesGeoJsonObject<IRoute>, IRoute
    {
        /// <summary>
        /// Gets or sets the feature collection.
        /// </summary>
        /// <value>The feature collection.</value>
        public FeatureCollection FeatureCollection
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public IRouteElementHeader Header
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the segments.
        /// </summary>
        /// <value>The segments.</value>
        public IEnumerable<IRouteSegment> Segments
        {
            get;
            set;
        }
    }
}