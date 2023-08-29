namespace backend.Models;



public class AuthenticateResponse
    {
        public long Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string Token { get; set; }
        public AuthenticateResponse(Client cl, string token)
        {
            Id = cl.Id;
            FirstName = cl.FirstName;
            LastName = cl.LastName;
            Email = cl.Email;
            Token = token;
        }

    }

