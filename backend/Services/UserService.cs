using backend.Helpers;
using backend.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace backend.Services
{
    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<Client> _client = new List<Client>
        {
          new Client { Id = 1, FirstName = "Test", LastName = "User",  Email = "test@gmail.com", Password = "test" }
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var client = _client.SingleOrDefault(x => x.Email == model.Email && x.Password == model.Password);

            // return null if user not found
            if (client == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(client);

            return new AuthenticateResponse(client, token);
        }

        public IEnumerable<Client> GetAll()
        {
            return _client;
        }

        public Client GetById(int id)
        {
            return _client.FirstOrDefault(x => x.Id == id);
        }

        // helper methods

        private string generateJwtToken(Client client)
        {
   
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", client.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
