using backend.Repositories;
using backend.Models;
using System.Linq;

namespace HomeBanking.Repositories
{
    public class TransactionRepository : RepositoryBase<Transaction>, ITransactionRepository
    {
        public TransactionRepository(MyContext repositoryContext) : base(repositoryContext) { }

        public IEnumerable<Transaction> GetAllTransactions()
        {
            return FindAll().
                ToList();
        }
        public Transaction FindByNumber(long id)
        {
            return FindByCondition(transaction => transaction.Id == id).FirstOrDefault();
        }

        public void Save(Transaction transaction)
        {
            Create(transaction);
            SaveChanges();
        }
        public void DeleteTransaction(Transaction transaction)
        {
            Delete(transaction);
            SaveChanges();
        }
    }
}