using System;

namespace ReadingList.Trello.Logging
{
	/// <summary>
	/// Log factory.
	/// </summary>
	public interface ILogFactory
	{
		/// <summary>
		/// Gets the logger.
		/// </summary>
		/// <returns>The logger.</returns>
		/// <param name="type">Type.</param>
		ILog GetLogger(Type type);
	}
}
