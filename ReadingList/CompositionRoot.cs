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

			serviceRegistry.Register<ITrelloAuthModel>(factory => new TrelloAuthModel(ConfigurationManager.AppSettings["TrelloAPIKey"], ConfigurationManager.AppSettings["TrelloUserToken"]), new PerContainerLifetime());
			serviceRegistry.Register<ITrelloAuthorizationWrapper, TrelloAuthorizationWrapper>(new PerContainerLifetime());
			serviceRegistry.Register<ITrelloWebHookSources, TrelloWebHookSourcesConfigFileReader>();
			serviceRegistry.Register<IBookFactory, BookFactory>();
			serviceRegistry.Register<IWebHookCaller, WebHookCaller>(); 
			serviceRegistry.Register<IReadingListService>(factory => new ReadingListService(TrelloBoardConstans.BoardId, factory.GetInstance<IBookFactory>(), factory.GetInstance<ILogFactory>()), new PerContainerLifetime());
			serviceRegistry.Register<IReadingBoardService>(factory => new ReadingBoardService(factory.GetInstance<IReadingListService>(), TrelloBoardConstans.BoardId), new PerContainerLifetime());
            serviceRegistry.Decorate<IReadingListService, ReadingListCache>();
            serviceRegistry.Decorate<IReadingListService, ReadingListServiceProfiler>();
			
		}
	}
}
