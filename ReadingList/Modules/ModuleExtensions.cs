using System;
using Nancy;

namespace ReadingList
{
	public static class ModuleExtensions
	{
		public static void EnableCors(this NancyModule module)
		{
			module.After.AddItemToEndOfPipeline(x =>
			{
				x.Response.WithHeader("Access-Control-Allow-Origin", "*");
			});
		}
	}
}
