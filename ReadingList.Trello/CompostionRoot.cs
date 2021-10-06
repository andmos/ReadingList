using LightInject;
using Manatee.Trello;
using ReadingList.Logging;
using ReadingList.Logic.Services;
using ReadingList.Trello.Helpers;
using ReadingList.Trello.Models;
using ReadingList.Trello.Services;

namespace ReadingList.Trello
{
    public class CompostionRoot : ICompositionRoot
    {

        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<ITrelloFactory, TrelloFactory>();
            serviceRegistry.Register<ITrelloAuthorizationWrapper, TrelloAuthorizationWrapper>(new PerContainerLifetime());
            serviceRegistry.Register<IBookFactory, BookMapper>();
            serviceRegistry.Register<IWebHookCaller, WebHookCaller>();
            serviceRegistry.Register<IReadingListCache, ReadingListCache>(new PerContainerLifetime());
            serviceRegistry.Register<IReadingListService>(factory => new ReadingListService(factory.GetInstance<ITrelloFactory>(), TrelloBoardConstans.BoardId, factory.GetInstance<IBookFactory>(), factory.GetInstance<ILogFactory>()), new PerContainerLifetime());
            serviceRegistry.Register<IReadingListCollectionService>(factory => new ReadingBoardService(factory.GetInstance<ITrelloFactory>(), factory.GetInstance<IReadingListService>(), TrelloBoardConstans.BoardId), new PerContainerLifetime());
            serviceRegistry.Register<ITrelloStatusService, TrelloStatusService>();

            serviceRegistry.Decorate<IReadingListService, CachedReadingListService>();
            serviceRegistry.Decorate<IReadingListService, ReadingListServiceProfiler>();

        }
    }
}
