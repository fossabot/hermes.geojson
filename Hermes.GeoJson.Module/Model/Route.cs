using System;
using System.Collections.Generic;
using GeoJSON.Net.Feature;

namespace Hermes.GeoJson.Module.Model
{
    /// <summary>
    /// Route.
    /// </summary>
    public sealed class Route:IHermesGeoJsonObject<IRoute>
    {
        /// <summary>
        /// Gets or sets the header.
        /// </summary>
        /// <value>The header.</value>
        public IRouteElementHeader Header { get; set; }
        /// <summary>
        /// Gets or sets the segments.
        /// </summary>
        /// <value>The segments.</value>
        public IEnumerable<IRouteSegment> Segments { get; set; }
        /// <summary>
        /// Gets or sets the feature collection.
        /// </summary>
        /// <value>The feature collection.</value>
        public FeatureCollection FeatureCollection { get; set; }
    }
}
