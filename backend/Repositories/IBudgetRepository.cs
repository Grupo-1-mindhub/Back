using backend.Models;

namespace backend.Repositories
{
    public interface IBudgetRepository
    {
        IEnumerable<Budget> GetAllBudgets();
        void Save(Budget budget);
        Budget FindById(long id);

        
    }
}
