using System.Collections.Generic;
using System.Xml.Linq;

namespace Hermes.GeoJson.Module
{
    /// <summary>
    /// Исходные данные сегментов маршрутов (неотсортированные в сыром виде)"
    /// </summary>
    public sealed class SegmentSource : Dictionary<int, IEnumerable<XElement>>
	{

	}
}