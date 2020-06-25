using System;
using System.Configuration;
using LightInject;
using ReadingList.Logging;
using ReadingList.Web.Logging;
using ReadingList.Trello.Models;
using ReadingList.Web.Trello;

namespace ReadingList.Web
{
	public class CompositionRoot : ICompositionRoot
	{
		public void Compose(IServiceRegistry serviceRegistry)
		{
			serviceRegistry.Register<ILogFactory, Log4NetLogFactory>(new PerContainerLifetime());
			serviceRegistry.Register<Type, ILog>((factory, type) => factory.GetInstance<ILogFactory>().GetLogger(type));
			serviceRegistry.RegisterConstructorDependency(
			(factory, info) => factory.GetInstance<Type, ILog>(info.Member.DeclaringType));

			serviceRegistry.Register<ITrelloAuthModel>(factory => new TrelloAuthSettings { TrelloAPIKey = ConfigurationManager.AppSettings["TrelloAPIKey"], TrelloUserToken = ConfigurationManager.AppSettings["TrelloUserToken"] }, new PerContainerLifetime());

			serviceRegistry.Register<ITrelloWebHookSources, TrelloWebHookSourcesConfigFileReader>();

			serviceRegistry.RegisterFrom<ReadingList.Trello.CompostionRoot>();

        }
	}
}
