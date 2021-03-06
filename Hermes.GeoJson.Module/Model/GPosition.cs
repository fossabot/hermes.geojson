﻿namespace Hermes.GeoJson.Module.Model
{
    /// <summary>
    /// Defines the <see cref=GPosition />
    /// </summary>
    public class GPosition : IGPosition
    {
        /// <summary>
        /// Gets or sets the Ele
        /// </summary>
        public double Ele
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Lat
        /// </summary>
        public double Lat
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Lon
        /// </summary>
        public double Lon
        {
            get;
            set;
        }
    }
}