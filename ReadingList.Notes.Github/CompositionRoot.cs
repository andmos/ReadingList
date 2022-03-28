using System;
using LightInject;
using Readinglist.Notes.Logic.Services;
using ReadingList.Notes.Github.Repositories;

namespace ReadingList.Notes.Github
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<IBookRecordRepository, GithubReadingListDataRepository>();
            serviceRegistry.Register<IBookNotesService, BookNotesService>();
        }
    }
}

