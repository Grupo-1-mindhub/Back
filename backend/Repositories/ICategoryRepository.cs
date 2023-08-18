using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Linq;
using System;
using backend.Models;

namespace backend.Repositories
{
    public interface ICategoryRepository
    {
        IEnumerable<Category> GetAllCategorys();
        void Save(Category category);
        Category FindById(long id);
    }
}

