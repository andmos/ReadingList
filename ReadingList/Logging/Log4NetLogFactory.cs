using System;
using log4net;
using log4net.Config;
using ReadingList.Trello.Logging;

namespace ReadingList.Web.Logging
{
	public class Log4NetLogFactory : ILogFactory
	{
		public Log4NetLogFactory()
		{
			XmlConfigurator.Configure();
		}

		public ReadingList.Trello.Logging.ILog GetLogger(Type type)
		{
			var logger = LogManager.GetLogger(type);
			return new Log(logger.Info, logger.Debug, logger.Error);
		}
	}
}
