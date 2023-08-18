using backend.Models;
using System.Collections.Generic;

namespace backend.Repositories
{
    public interface ITransactionRepository
    {
        void Save(Transaction transaction);
        Transaction FindByNumber(long id);
    }
}