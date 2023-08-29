using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;


namespace backend.Models
{
    public class AuthenticateRequest
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
