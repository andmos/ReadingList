using System;
using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.Routing;

namespace ReadingList
{
	public class RouteModule : NancyModule
	{
        private readonly IRouteCacheProvider m_routeCacheProvider;

        public RouteModule(IRouteCacheProvider routeCacheProvider) : base("/api/")
        {
            m_routeCacheProvider = routeCacheProvider;
            this.EnableCors();
            Get["/"] = parameters =>
            {
                var responseObject = new IndexModel();
                var cache = m_routeCacheProvider.GetCache();

                responseObject.Routes = cache.Values.SelectMany(t => t.Select(t1 => t1.Item2));

                return Response.AsJson(responseObject.Routes.Select(p => new KeyValuePair<string, string>(p.Path, p.Method)));
            };

            Get["/ping"] = parameters =>
            {
                return "pong";
            };
        }
    }
}
