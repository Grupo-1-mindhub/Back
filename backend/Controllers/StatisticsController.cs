using backend.DTOs;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
  
        private IClientRepository _clientRepository;
        public StatisticsController( IClientRepository clientRepository)
        {
   
            _clientRepository = clientRepository;
        }
        [HttpGet("clients/accounts/{id}/statistics")]
        public IActionResult GetBudgetsByAccount(long id)
        {
            try
            {
                string email = User.FindFirst("Client")?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    return StatusCode(403, "Cliente no autorizado");
                }

                Client cl = _clientRepository.FindByEmail(email);
                if (cl == null)
                {
                    return StatusCode(403, "Cliente no encontrado");
                }

                var acc = cl.Accounts.FirstOrDefault(account => account.Id == id);
                if (acc == null)
                {
                    return StatusCode(403, "Cuenta inválida");
                }
                var budgets = acc.Budgets;
                

                return Ok();
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
