using LightInject.Nancy;
using Nancy;
using System.Configuration;
using System;

namespace ReadingList
{
	public class BootStrapper : LightInjectNancyBootstrapper 
	{
		protected override IRootPathProvider RootPathProvider => new Nancy.Hosting.Self.FileSystemRootPathProvider();
		protected override void ApplicationStartup(LightInject.IServiceContainer container, Nancy.Bootstrapper.IPipelines pipelines)
		{
			SetupWebHook(container.GetInstance<Lazy<IWebHookCaller>>(), container.GetInstance<ILogFactory>().GetLogger(GetType()));
			base.ApplicationStartup(container, pipelines);
		}

		private void SetupWebHook(Lazy<IWebHookCaller> caller, ILog logger) 
		{
			var callBackUrl = ConfigurationManager.AppSettings["HostUrl"];
			if (string.IsNullOrEmpty(callBackUrl)) 
			{
				logger.Info("HostUrl not configured, skipping webhook-setup.");
				return; 
			}
			var webhookObject = new TrelloWebhook { callbackURL = $"{ConfigurationManager.AppSettings["HostUrl"]}/api/callBack", description = "ReadingListWebhook", idModel = TrelloBoardConstans.BoardWebhookId };
			caller.Value.SetUpWebHook(webhookObject);
            caller.Value.Configured = true; 

        }
	}
}
