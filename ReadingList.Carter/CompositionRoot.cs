using System;
using LightInject;
using ReadingList.Logging;
using ReadingList.Carter.Logging;
using ReadingList.Trello.Models;
using ReadingList.Carter.Trello;
using Readinglist.Notes.Logic.Services;
using ReadingList.Trello.Services;

namespace ReadingList.Carter
{
    public class CompositionRoot : ICompositionRoot
    {
        public void Compose(IServiceRegistry serviceRegistry)
        {
            serviceRegistry.Register<ILogFactory, SerilogFactory>(new PerContainerLifetime());
            serviceRegistry.Register<Type, ILog>((factory, type) => factory.GetInstance<ILogFactory>().GetLogger(type));
            serviceRegistry.RegisterConstructorDependency(
            (factory, info) => factory.GetInstance<Type, ILog>(info.Member.DeclaringType));

            serviceRegistry.Register<ITrelloWebHookSources, TrelloWebHookSourcesConfigFileReader>();

            serviceRegistry.RegisterFrom<ReadingList.Trello.CompositionRoot>();

            serviceRegistry.RegisterFrom<ReadingList.Notes.Github.CompositionRoot>();
        }
    }
}
