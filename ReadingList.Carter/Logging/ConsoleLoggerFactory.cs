using System;
using ReadingList.Logging;

namespace ReadingList.Carter.Logging
{
    public class ConsoleLoggerFactory : ILogFactory
    {
        private ILog m_consoleLogger;
        public ConsoleLoggerFactory()
        {
            m_consoleLogger = new ConsoleLogger();
        }

        public ILog GetLogger(Type type)
        {
            return m_consoleLogger;
        }
    }
}
