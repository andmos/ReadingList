﻿using System;
using LightInject;
using ReadingList.Logging;
using ReadingList.Carter.Logging;
using ReadingList.Carter.Helpers;

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

            serviceRegistry.RegisterFrom<ReadingList.Trello.CompositionRoot>();

            serviceRegistry.RegisterFrom<ReadingList.Notes.Github.CompositionRoot>();

            serviceRegistry.Register<CacheWarmUpService>();
        }
    }
}
