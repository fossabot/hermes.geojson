using System;
namespace Hermes.GeoJson.Module.Model
{
    /// <summary>
    /// Route element header.
    /// </summary>
    public class RouteElementHeader : IRouteElementHeader
    {
        /// <summary>
        /// Gets or sets the distance.
        /// </summary>
        /// <value>The distance.</value>
        public double Distance { get; set; }
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>The duration.</value>
        public TimeSpan Duration { get; set; }
        /// <summary>
        /// Gets or sets the star time.
        /// </summary>
        /// <value>The star time.</value>
        public DateTime StarTime { get; set; }
        /// <summary>
        /// Gets or sets the stop time.
        /// </summary>
        /// <value>The stop time.</value>
        public DateTime StopTime { get; set; }
    }
}
