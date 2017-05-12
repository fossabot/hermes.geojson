using System.Collections.Generic;

namespace Hermes.GeoJson.Module
{
    /// <summary>
    /// Обработанные данные по сегментам маршрутов
    /// </summary>
    public sealed class Segments : Dictionary<int, IEnumerable<IRoutePoint>>
	{

	}
}