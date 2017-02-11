using System;
using Nancy.Hosting.Self;

namespace ReadingList
{
	public class ReadingListSelfHost
	{

		public bool Start()
		{
			return true;
		}

		public bool Stop()
		{
			return false;
		}	
	}
}
