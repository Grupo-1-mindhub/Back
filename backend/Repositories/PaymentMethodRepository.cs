using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repositories
{
    public class PaymentMethodRepository:RepositoryBase<PaymentMethod>, IPaymentMethodRepository
    {
        public PaymentMethodRepository(MyContext repositoryContext) : base(repositoryContext) { }
        public PaymentMethod FindById(long id)
        {
            return FindByCondition(pm=>pm.Id== id).FirstOrDefault();
        }
        public IEnumerable<PaymentMethod> GetAllPaymentMethods()
        {
            return FindAll().Include(pm=>pm.Transactions).ToList();
        }
        public void Save(PaymentMethod paymentMethod)
        {
            Create(paymentMethod);
            SaveChanges();
        }
    }
}
