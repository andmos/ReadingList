using Nancy;

namespace ReadingList.Modules
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
