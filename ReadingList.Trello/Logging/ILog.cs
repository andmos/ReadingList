using System;

namespace ReadingList.Trello.Logging
{
	/// <summary>
	/// Logger interface
	/// </summary>
	public interface ILog
	{
		/// <summary>
		///  Log the specified message as Info
		/// </summary>
		/// <param name="message">Message.</param>
		void Info(string message);

		/// <summary>
		/// Log the specified message as Debug
		/// </summary>
		/// <param name="message">Message.</param>
		void Debug(string message);

		/// <summary>
		/// Log the specified error message and exception.
		/// </summary>
		/// <param name="message">Message.</param>
		/// <param name="exception">Exception.</param>
		void Error(string message, Exception exception = null);
	}
}
