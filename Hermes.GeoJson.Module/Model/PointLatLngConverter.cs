namespace Hermes.GeoJson.Module
{
    using System;

    /// <summary>
    /// Defines the <see cref=PointLatLngConverter />
    /// </summary>
    public sealed class PointLatLngConverter : Newtonsoft.Json.Converters.CustomCreationConverter<IPointLatLng>
    {
        public override IPointLatLng Create(Type objectType)
        {
            return new PointLatLng();
        }
    }
}