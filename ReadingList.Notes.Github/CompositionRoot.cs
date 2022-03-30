using LightInject;
using Readinglist.Notes.Logic.Services;
using ReadingList.Notes.Github.Repositories;

namespace ReadingList.Notes.Github
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.RegisterSingleton<IGithubBookRecordService, GithubBookRecordService>();
            serviceRegistry.Register<IBookRecordRepository, ReadingListDataRepository>();
            serviceRegistry.Register<IBookNotesService, BookNotesService>();
        }
    }
}

