using System;
namespace Hermes.GeoJson.Module.Model
{
    /// <summary>
    /// Route point.
    /// </summary>
    public class RoutePoint:IRoutePoint
    {
        /// <summary>
        /// Gets or sets the position.
        /// </summary>
        /// <value>The position.</value>
        public IGPosition Position { get; set; }
        /// <summary>
        /// Gets or sets the speed.
        /// </summary>
        /// <value>The speed.</value>
        public double Speed { get; set; }
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>The time.</value>
        public DateTime Time { get; set; }
    }
}
