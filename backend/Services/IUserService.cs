using backend.Models;

namespace backend.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<Client> GetAll();
        Client GetById(int id);
    }
}
