namespace backend.Services
{
    public interface ITokenServices
    {
        string GenerateToken(string email);

    }
}
