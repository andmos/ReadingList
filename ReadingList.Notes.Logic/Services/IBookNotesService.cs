using System.Threading.Tasks;
using Readinglist.Notes.Logic.Models;

namespace Readinglist.Notes.Logic.Services
{
	public interface IBookNotesService
	{
		Task<BookNote> GetRandomBookNote();
	}
}

