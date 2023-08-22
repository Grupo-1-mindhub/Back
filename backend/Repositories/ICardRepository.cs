using backend.Models;
using System.Collections.Generic;

namespace backend.Repositories
{

    public interface ICardRepository
    {
        void Save(Card card);
        Card FindById(long id);
    }
}