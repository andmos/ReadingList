using LightInject.Nancy;
using Nancy;
using System.Configuration;

namespace ReadingList
{
	public class BootStrapper : LightInjectNancyBootstrapper 
	{
		protected override IRootPathProvider RootPathProvider => new Nancy.Hosting.Self.FileSystemRootPathProvider();
		protected override void ApplicationStartup(LightInject.IServiceContainer container, Nancy.Bootstrapper.IPipelines pipelines)
		{
			SetupWebHook(container.GetInstance<IWebHookCaller>(), container.GetInstance<ILogFactory>().GetLogger(GetType()));
			base.ApplicationStartup(container, pipelines);
		}

		private void SetupWebHook(IWebHookCaller caller, ILog logger) 
		{
			var callBackUrl = ConfigurationManager.AppSettings["HostUrl"];
			if (string.IsNullOrEmpty(callBackUrl)) 
			{
				logger.Info("HostUrl not configured, skipping webhook-setup.");
				return; 
			}
			var webhookObject = new TrelloWebhook { callbackURL = $"{ConfigurationManager.AppSettings["HostUrl"]}/api/callBack", description = "ReadingListWebhook", idModel = TrelloBoardConstans.BoardWebhookId };
			caller.SetUpWebHook(webhookObject);
		}
	}
}
