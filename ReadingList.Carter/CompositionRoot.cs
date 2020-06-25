using System;
using System.Configuration;
using LightInject;
using ReadingList.Logging;
using ReadingList.Carter.Logging;
using ReadingList.Trello;
using ReadingList.Trello.Models;
using ReadingList.Carter.Trello;
using Microsoft.Extensions.Configuration;

namespace ReadingList.Carter
{
	public class CompositionRoot : ICompositionRoot
	{
		public void Compose(IServiceRegistry serviceRegistry)
		{
			
			serviceRegistry.Register<ILogFactory, ConsoleLoggerFactory>(new PerContainerLifetime());
			serviceRegistry.Register<Type, ILog>((factory, type) => factory.GetInstance<ILogFactory>().GetLogger(type));
			serviceRegistry.RegisterConstructorDependency(
			(factory, info) => factory.GetInstance<Type, ILog>(info.Member.DeclaringType));

			serviceRegistry.Register<ITrelloWebHookSources, TrelloWebHookSourcesConfigFileReader>();

			serviceRegistry.RegisterFrom<ReadingList.Trello.CompostionRoot>();

        }
	}
}
