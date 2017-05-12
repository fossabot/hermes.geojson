namespace Hermes.GeoJson.Module.Model
{
    using System.Collections.Generic;

    /// <summary>
    /// Route segment.
    /// </summary>
    public class RouteSegment : IRouteSegment
    {
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
        /// Gets or sets the points.
        /// </summary>
        /// <value>The points.</value>
        public IEnumerable<IRoutePoint> Points
        {
            get;
            set;
        }
    }
}