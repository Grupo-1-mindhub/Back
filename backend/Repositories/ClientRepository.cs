using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace backend.Repositories
{
    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public ClientRepository(MyContext repositoryContext) : base(repositoryContext)
        {
        }

        public Client FindByEmail(string email)
        {
            return FindByCondition(client => client.Email.ToUpper() == email.ToUpper()) 
            .Include(client => client.Accounts)
                .ThenInclude(account => account.Budgets)
                    .ThenInclude(budget => budget.Transactions)
            .Include(client => client.Cards)
                .FirstOrDefault();
        }

        public Client FindById(long id)
        {
            return FindByCondition(client => client.Id == id)
               .FirstOrDefault();
        }

        public IEnumerable<Client> GetAllClients()
        {
            return FindAll()
               .Include(client => client.Accounts)
               .ToList();
        }

        public void Save(Client client)
        {
            Create(client); 
            SaveChanges(); 
        }
    }
}

    
    

