using System;
using ReadingList.Logging;

namespace ReadingList.Carter.Logging
{
    public class ConsoleLogger : ILog
    {

        public void Debug(string message)
        {
            Console.WriteLine($"Debug: {message}");
        }

        public void Error(string message, Exception exception = null)
        {
            Console.WriteLine($"Error: {message}, {exception.Message}");
        }

        public void Info(string message)
        {
            Console.WriteLine($"Info: {message}");
        }
    }
}
