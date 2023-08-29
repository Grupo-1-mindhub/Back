using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace backend.Repositories
{
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(MyContext repositoryContext) : base(repositoryContext)
        {
        }

        public Category FindById(long id)
        {
            return FindByCondition(category => category.Id == id)
               .FirstOrDefault();
        }

        public IEnumerable<Category> GetAllCategorys()
        {
            return FindAll()
              .Include(category => category.Transactions)
              .ToList();
        }

        public void Save(Category category)
        {
            Create(category);
            SaveChanges();
        }
    }
}
