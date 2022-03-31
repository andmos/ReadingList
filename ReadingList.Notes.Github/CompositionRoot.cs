﻿using LightInject;
using Readinglist.Notes.Logic.Services;
using ReadingList.Notes.Github.Repositories;
using ReadingList.Notes.Github.Services;

namespace ReadingList.Notes.Github
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.RegisterSingleton<IBookRecordCache, BookRecordCache>();
            serviceRegistry.RegisterSingleton<IGithubBookRecordService, GithubBookRecordService>();
            serviceRegistry.Register<IBookRecordRepository, ReadingListDataRepository>();
            serviceRegistry.Register<IBookNotesService, BookNotesService>();
        }
    }
}
