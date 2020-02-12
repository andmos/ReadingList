using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Manatee.Trello;

namespace ReadingList
{
    public class ReadingListService : IReadingListService
    {
        private readonly IBoard m_board;
        private IBookFactory m_bookFactory;
        private ILog m_logger;


        public ReadingListService(ITrelloFactory factory, string boardId, IBookFactory bookFactory, ILogFactory logFactory)
        {
            m_board = factory.Board(boardId);

            m_bookFactory = bookFactory;
            m_logger = logFactory.GetLogger(this.GetType());
        }

        public async Task<IEnumerable<Book>> GetReadingList(string listName, string label = null)
        {
            IEnumerable<ICard> cardList;

            await m_board.Lists.Refresh();

            if (string.IsNullOrEmpty(label))
            {
                cardList = m_board.Lists.FirstOrDefault(l => l.Name.Equals(listName)).Cards;
            }
            else
            {
                cardList = m_board.Lists.FirstOrDefault(l => l.Name.Equals(listName)).Cards.Where(c => c.Labels.All(l => l.Name.ToLower().Equals(label.ToLower())));
            }

            var readingList = new List<Book>();

            foreach (var card in cardList)
            {
                readingList.Add(
                m_bookFactory.Create(
                    card.Name,
                    card.Labels.FirstOrDefault()?.Name.ToLower() ?? TrelloBoardConstans.UnspecifiedLabel));
            }

            return readingList;
        }

        public bool AddBookToBacklog(string book, string authors, string label)
        {
            bool addSuccessfull;
            string backlogCardListId = m_board.Lists.FirstOrDefault(l => l.Name.Equals(TrelloBoardConstans.Backlog)).Id;
            var backlogCardList = new List(backlogCardListId);
            try
            {
                var bookLabel = m_board.Labels.FirstOrDefault(l => l.Name.ToLower().Equals(label.ToLower()));
                backlogCardList.Cards.Add(name: FormatCardName(book, authors), labels: new[] { bookLabel });
                m_logger.Info($"Adding {book}, {authors}, {label} to {TrelloBoardConstans.Backlog}");
                addSuccessfull = true;
            }
            catch (Exception ex)
            {
                m_logger.Error($"Error when trying to add {book}, {authors}, {label} to {TrelloBoardConstans.Backlog}: ", ex);
                addSuccessfull = false;
            }
            return addSuccessfull;
        }

        public bool UpdateDoneListFromReadingList(string book)
        {
            bool updateSuccessful;
            string doneCardListId = m_board.Lists.FirstOrDefault(l => l.Name.Equals(TrelloBoardConstans.DoneReading)).Id;
            var doneCardList = new List(doneCardListId);
            try
            {
                var card = m_board.Lists.FirstOrDefault(l => l.Name.Equals(TrelloBoardConstans.CurrentlyReading)).Cards.SingleOrDefault(c => c.Name.ToLower().Contains(book.ToLower()));
                if (card == null)
                {
                    m_logger.Info($"Could not find {book} in {TrelloBoardConstans.CurrentlyReading}, so can't move to {TrelloBoardConstans.DoneReading}.");
                    return updateSuccessful = false;
                }
                card.List = doneCardList;
                card.Position = new Position(1);
                updateSuccessful = true;
                m_logger.Info($"Moving {book} to {TrelloBoardConstans.DoneReading} from {TrelloBoardConstans.CurrentlyReading}");
            }
            catch (Exception ex)
            {
                m_logger.Error($"Error when trying to move {book} to {TrelloBoardConstans.DoneReading}: ", ex);
                updateSuccessful = false;
            }
            return updateSuccessful;

        }

        private string FormatCardName(string book, string author)
        {
            return $"{book} - {author}";
        }

    }
}
