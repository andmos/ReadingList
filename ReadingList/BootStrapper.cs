using System;
using LightInject.Nancy;
using Nancy;

namespace ReadingList
{
	public class BootStrapper : LightInjectNancyBootstrapper 
	{
		protected override IRootPathProvider RootPathProvider => new Nancy.Hosting.Self.FileSystemRootPathProvider();
	}
}
