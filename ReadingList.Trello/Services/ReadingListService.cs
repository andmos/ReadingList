using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manatee.Trello;
using ReadingList.Logging;
using ReadingList.Logic.Models;
using ReadingList.Logic.Services;
using ILog = ReadingList.Logging.ILog;

namespace ReadingList.Trello.Services
{
    public class ReadingListService : IReadingListService
    {
        private readonly IBoard _board;
        private readonly IBookFactory _bookFactory;
        private readonly ILog _logger;

        public ReadingListService(
            ITrelloFactory factory,
            string boardId,
            IBookFactory bookFactory,
            ILogFactory logFactory)
        {
            _board = factory.Board(boardId);

            _bookFactory = bookFactory;
            _logger = logFactory.GetLogger(this.GetType());
        }

        public async Task<IEnumerable<Book>> GetReadingList(string listName, string label = null)
        {
            await _board.Lists.Refresh();

            var cardList = string.IsNullOrEmpty(label) ?
                _board.Lists.FirstOrDefault(l => l.Name.Equals(listName))?.Cards :
                _board.Lists.FirstOrDefault(l => l.Name.Equals(listName))?.Cards.Where(c => c.Labels.All(l => l.Name.ToLower().Equals(label.ToLower())));

            return cardList?.Select(card => _bookFactory.Create(card.Name, card.Labels.FirstOrDefault()?.Name.ToLower() ?? ReadingListConstants.UnspecifiedLabel)).ToList();
        }

        public async Task<bool> AddBookToBacklog(string book, string authors, string label)
        {
            await _board.Lists.Refresh();

            bool addSuccessfull;
            var backlogCardListId = _board.Lists.FirstOrDefault(l => l.Name.Equals(ReadingListConstants.Backlog))?.Id;
            var backlogCardList = new List(backlogCardListId);
            try
            {
                var bookLabel = _board.Labels.FirstOrDefault(l => l.Name.ToLower().Equals(label.ToLower()));
                await backlogCardList.Cards.Add(name: FormatCardName(book, authors), labels: new[] { bookLabel });
                _logger.Info($"Adding {book}, {authors}, {label} to {ReadingListConstants.Backlog}");
                addSuccessfull = true;
            }
            catch (Exception ex)
            {
                _logger.Error($"Error when trying to add {book}, {authors}, {label} to {ReadingListConstants.Backlog}: ", ex);
                addSuccessfull = false;
            }
            return addSuccessfull;
        }

        public async Task<bool> UpdateDoneListFromReadingList(string book)
        {
            await _board.Lists.Refresh();

            bool updateSuccessful;
            string doneCardListId = _board.Lists.FirstOrDefault(l => l.Name.Equals(ReadingListConstants.DoneReading))?.Id;
            var doneCardList = new List(doneCardListId);
            try
            {
                var card = _board.Lists.FirstOrDefault(l => l.Name.Equals(ReadingListConstants.CurrentlyReading))?.Cards.SingleOrDefault(c => c.Name.ToLower().Contains(book.ToLower()));
                if (card == null)
                {
                    _logger.Info($"Could not find {book} in {ReadingListConstants.CurrentlyReading}, so can't move to {ReadingListConstants.DoneReading}.");
                    return updateSuccessful = false;
                }
                card.List = doneCardList;
                card.Position = new Position(1);
                updateSuccessful = true;
                _logger.Info($"Moving {book} to {ReadingListConstants.DoneReading} from {ReadingListConstants.CurrentlyReading}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Error when trying to move {book} to {ReadingListConstants.DoneReading}: ", ex);
                updateSuccessful = false;
            }
            return updateSuccessful;
        }
        
        private string FormatCardName(string book, string author) => $"{book} - {author}";
    }
}
