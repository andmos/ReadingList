using LightInject;
using Readinglist.Notes.Logic.Services;
using ReadingList.Notes.Github.Repositories;

namespace ReadingList.Notes.Github
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.RegisterSingleton<IGithubTextFileService, GithubTextFileService>();
            serviceRegistry.Decorate<IGithubTextFileService, CachedGithubTextFileServiceDecorator>();
            serviceRegistry.RegisterSingleton<IBookRecordRepository, GithubReadingListDataService>();
            serviceRegistry.Register<IBookNotesService, BookNotesService>();
        }
    }
}

