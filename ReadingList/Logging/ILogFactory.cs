using System;
namespace ReadingList
{
	public interface ILogFactory
	{
		ILog GetLogger(Type type);
	}
}
