namespace Hermes.GeoJson.Module
{
    public interface IPointLatLng
    {
        /// <summary>
        /// Gets or sets a value indicating whether IsZero
        /// </summary>
        bool IsZero
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Lat
        /// </summary>
        double Lat
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Lng
        /// </summary>
        double Lng
        {
            get;
            set;
        }
    }
}