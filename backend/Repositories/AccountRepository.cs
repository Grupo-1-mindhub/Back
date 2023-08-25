using backend.Controllers;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace backend.Repositories
{
    public class AccountRepository : RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(MyContext repositoryContext) : base(repositoryContext)
        {
        }

        public Account FindById(long id)
        {
            return FindByCondition(account => account.Id == id)
                .Include(account => account.Budgets)
                    .ThenInclude(budget => budget.Transactions)
                .FirstOrDefault();
        }
        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll()
                .Include(account => account.Budgets)
                .ToList();
        }

        public void Save(Account account)
        {
            if (account.Id == 0)
            {
                Create(account);
            }
            else
            {
                Update(account);
            }

            SaveChanges();
        }
        public IEnumerable<Account> GetAccountsByClient(long clientId)
        {
            return FindByCondition(account => account.ClientId == clientId)
            .Include(account => account.Budgets)
            .ToList();
        }
        public Account FindByNumber(string number)
        {
            return FindByCondition(account => account.Number.ToUpper() == number.ToUpper())
            .Include(account => account.Budgets)
            .FirstOrDefault();
        }
    }

}
