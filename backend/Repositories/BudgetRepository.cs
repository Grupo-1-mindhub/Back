using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class BudgetRepository : RepositoryBase<Budget>, IBudgetRepository
    {
        public BudgetRepository(MyContext repositoryContext) : base(repositoryContext)
        {
        }

        public Budget FindById(long id)
        {
            return FindByCondition(budget => budget.Id == id)
               .FirstOrDefault();
        }

        public IEnumerable<Budget> GetAllBudgets()
        {
            return FindAll()
              .Include(budget => budget.Transactions)
              .ToList();
        }

        public void Save(Budget budget)
        {
            Create(budget);
            SaveChanges();
        }
    }
}
