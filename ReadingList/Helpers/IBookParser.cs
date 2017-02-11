using System;
namespace ReadingList
{
	public interface IBookParser
	{
		Book ParseBook(string bookString); 
	}
}
