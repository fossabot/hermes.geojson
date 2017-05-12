namespace Hermes.GeoJson.Module
{
    /// <summary>
    /// Defines the <see cref = PointLatLng / >
    /// </summary>
    internal sealed class PointLatLng : IPointLatLng
    {
        /// <summary>
        /// Gets or sets a value indicating whether IsZero
        /// </summary>
        public bool IsZero
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
        /// Gets or sets the Lng
        /// </summary>
        public double Lng
        {
            get;
            set;
        }
    }
}