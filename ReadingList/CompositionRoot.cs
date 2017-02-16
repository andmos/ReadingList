using System;
using System.Configuration;
using LightInject;
namespace ReadingList
{
	public class CompositionRoot : ICompositionRoot
	{

		public void Compose(IServiceRegistry serviceRegistry)
		{
			serviceRegistry.Register<ILogFactory, Log4NetLogFactory>(new PerContainerLifetime());
			serviceRegistry.Register<Type, ILog>((factory, type) => factory.GetInstance<ILogFactory>().GetLogger(type));
			serviceRegistry.RegisterConstructorDependency(
			(factory, info) => factory.GetInstance<Type, ILog>(info.Member.DeclaringType));

			serviceRegistry.Register<ITrelloAuthorizationWrapper>(factory => new TrelloAuthorizationWrapper(ConfigurationManager.AppSettings["TrelloAPIKey"], ConfigurationManager.AppSettings["TrelloUserToken"]), new PerContainerLifetime());
			serviceRegistry.Register<ITrelloWebHookSources, TrelloWebHookSourcesConfigFileReader>();
			serviceRegistry.Register<IBookParser, TrelloBookParser>();
			serviceRegistry.Register<IReadingListService>(factory => new ReadingListService(TrelloBoardConstans.BoardId, TrelloBoardConstans.ReadingListId, factory.GetInstance<IBookParser>()),new PerContainerLifetime());
		}
	}
}
