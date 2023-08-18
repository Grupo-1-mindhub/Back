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
            throw new NotImplementedException();
        }

        public Account FindByNumber(string number)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Account> GetAccountsByClient(long clientId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            throw new NotImplementedException();
        }

        public void Save(Account account)
        {
            throw new NotImplementedException();
        }
    }

}
