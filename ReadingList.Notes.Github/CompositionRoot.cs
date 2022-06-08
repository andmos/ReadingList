using LightInject;
using Readinglist.Notes.Logic.Services;
using ReadingList.Notes.Github.Repositories;
using ReadingList.Notes.Github.Services;

namespace ReadingList.Notes.Github
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.RegisterSingleton<IBookRecordCache, GitBookRecordCache>();
            serviceRegistry.RegisterSingleton<IGithubBookRecordService, GithubBookRecordService>();
            serviceRegistry.Decorate<IGithubBookRecordService, GithubBookRecordServiceProfiler>();
            serviceRegistry.Register<IBookRecordRepository, ReadingListDataRepository>();
            serviceRegistry.Register<IBookNotesService, BookNotesService>();
        }
    }
}

