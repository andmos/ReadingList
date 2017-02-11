using Topshelf;
using Topshelf.Nancy;

namespace ReadingList
{
	public class MainClass
	{
		public static void Main(string[] args)
		{
			var host = HostFactory.New(x =>
			{
				x.UseLinuxIfAvailable();
				x.Service<ReadingListSelfHost>(s =>
				{
					s.ConstructUsing(settings => new ReadingListSelfHost());
					s.WhenStarted(service => service.Start());
					s.WhenStopped(service => service.Stop());
					s.WithNancyEndpoint(x, c =>
					{
						c.AddHost(port: 1337);
						c.CreateUrlReservationsOnInstall();
						c.OpenFirewallPortsOnInstall(firewallRuleName: "ReadingListService");
					});
				});

				x.StartAutomatically();
				x.SetServiceName("ReadingListService");
				x.SetDisplayName("ReadingListService");
				x.SetDescription("ReadingListService");
				x.RunAsNetworkService();

			});
			host.Run();
		}
	}
}
