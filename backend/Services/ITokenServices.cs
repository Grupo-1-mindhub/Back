using backend.Models;

namespace backend.Services
{
    public interface ITokenServices
    {
        string GenerateToken(string username);
    }
}
