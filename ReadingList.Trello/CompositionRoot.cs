using LightInject;
using Manatee.Trello;
using ReadingList.Logging;
using ReadingList.Logic.Services;
using ReadingList.Trello.Helpers;
using ReadingList.Trello.Models;
using ReadingList.Trello.Services;

namespace ReadingList.Trello
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<ITrelloFactory, TrelloFactory>();
            serviceRegistry.Register<ITrelloAuthorizationWrapper, TrelloAuthorizationWrapper>(new PerContainerLifetime());
            serviceRegistry.Register<IWebHookCaller, WebHookCaller>();
            serviceRegistry.Register<ITrelloWebHookSources, TrelloWebHookSourcesService>();
            serviceRegistry.Register<IReadingListCache, ReadingListCache>(new PerContainerLifetime());
            serviceRegistry.Register<IReadingListService, ReadingListService>(new PerContainerLifetime());
            serviceRegistry.Register<IReadingListCollectionService, ReadingBoardService>(new PerContainerLifetime());
            serviceRegistry.Register<ITrelloStatusService, TrelloStatusService>();

            serviceRegistry.Decorate<IReadingListService, CachedReadingListService>();
            serviceRegistry.Decorate<IReadingListService, ReadingListServiceProfiler>();
        }
    }
}
