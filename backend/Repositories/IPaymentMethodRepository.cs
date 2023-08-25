using backend.Models;

namespace backend.Repositories
{
    public interface IPaymentMethodRepository
    {
        IEnumerable<PaymentMethod> GetAllPaymentMethods();
        void Save(PaymentMethod paymentMethod);
        PaymentMethod FindById(long id);
    }
}
