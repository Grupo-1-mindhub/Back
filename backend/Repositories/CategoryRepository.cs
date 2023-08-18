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
            throw new NotImplementedException();
        }

        public IEnumerable<Category> GetAllCategorys()
        {
            throw new NotImplementedException();
        }

        public void Save(Category category)
        {
            throw new NotImplementedException();
        }
    }

}
