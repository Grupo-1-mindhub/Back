using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using backend.Services;
using backend.DTOs;

namespace backend.Controllers
{
    [Route("api/auth")]
    [ApiController]

    public class AuthController : Controller
    {
            private IClientRepository _clientRepository;
            private readonly ITokenServices _tokenServices;

            public AuthController(IClientRepository clientRepository, ITokenServices tokenServices)
            {
                _clientRepository = clientRepository;
                _tokenServices = tokenServices;
                
            }

            [HttpPost("login")]
            public IActionResult Login([FromBody] LoginDTO client)
            {
                try
                {
                    Client user = _clientRepository.FindByEmail(client.Email);
                    if (user == null || !String.Equals(user.Password, client.Password))
                        return Unauthorized();

                    string jwtToken = _tokenServices.GenerateToken(client.Email);
                    return Ok(new { token = jwtToken });

                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }

            [HttpPost("logout")]
            public async Task<IActionResult> Logout()
            {
                try
                {
                    await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            }
    }
}
