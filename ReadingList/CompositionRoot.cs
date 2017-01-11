using System;
using System.Configuration;
using LightInject; 
namespace ReadingList
{
	public class CompositionRoot : ICompositionRoot
	{

		public void Compose(IServiceRegistry serviceRegistry)
		{
			serviceRegistry.Register<ITrelloAuthorizationWrapper>(factory => new TrelloAuthorizationWrapper(ConfigurationManager.AppSettings["TrelloAPIKey"], ConfigurationManager.AppSettings["TrelloUserToken"]), new PerContainerLifetime());
			serviceRegistry.Register<IReadingListService>(factory => new ReadingListService("hWsZ9uhl", "51c1bff352ec1db00f003e96"),new PerContainerLifetime());
		}
	}
}
