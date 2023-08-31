using backend.Models;
using System.Collections.Generic;

namespace backend.Repositories
{
    public interface ITransactionRepository
    {
        IEnumerable<Transaction> GetAllTransactions();
        void Save(Transaction transaction);
        Transaction FindByNumber(long id);
        void DeleteTransaction(Transaction transaction);
    }
}