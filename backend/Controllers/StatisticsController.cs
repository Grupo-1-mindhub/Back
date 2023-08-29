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

                Client cl = _clientRepository.FindById(id);
                if (cl == null)
                {
                    return StatusCode(403, "Cliente no encontrado");
                }

                var accs = cl.Accounts;
                if (accs == null)
                    return StatusCode(403, "El cliente no tiene cuentas");



                var trans = new List<Transaction>();
                foreach (Account acc in accs)
                {
                    foreach (Transaction trs in acc.Transactions)
                    {
                        trans.Add(trs);
                    }
                    
                }

        
                return Ok(trans);
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
