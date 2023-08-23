using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace backend.Repositories
{
    public class CardRepository : RepositoryBase<Card>, ICardRepository
    {
        public CardRepository(MyContext repositoryContext) : base(repositoryContext)
        {
        }

        public Card FindById(long id)
        {
            return FindByCondition(card => card.Id == id)
                .FirstOrDefault();
        }
        public IEnumerable<Card> GetAllCards()
        {
            return FindAll()
                .ToList();
        }

        public void Save(Card card)
        {
            if (card.Id == 0)
            {
                Create(card);
            }
            else
            {
                Update(card);
            }

            SaveChanges();
        }
    }

}
