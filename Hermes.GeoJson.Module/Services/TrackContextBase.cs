using System;
using System.Collections.Generic;

namespace Hermes.GeoJson.Module
{
    /// <summary>
    /// Routes dictionary base.
    /// </summary>
    public abstract class TrackContextBase : Dictionary<DateTime, List<List<IRoutePoint>>>
	{

	}
}