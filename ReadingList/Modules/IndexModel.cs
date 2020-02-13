using System.Collections.Generic;
using Nancy.Routing;

namespace ReadingList.Modules
{
	internal class IndexModel
	{
		public IEnumerable<RouteDescription> Routes { get; set; }
	}

	internal class RouteMetadata
	{
		public string Path { get; set; }
		public string Method { get; set; }
	}
}
