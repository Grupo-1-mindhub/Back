using backend.DTOs;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace backend.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
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



                var trans = new List<TransactionDTO>();
                foreach (Account acc in accs)
                {
                    foreach (Transaction trs in acc.Transactions)
                    {
                        var transactionDTO = new TransactionDTO
                        {
                            Id = trs.Id,
                            Amount = trs.Amount,
                            Description = trs.Description,
                            CreationDate = trs.CreationDate 
                        };
                        trans.Add(transactionDTO);
                    }
                    
                }
                var positiveTransactions = trans.Where(tr => tr.Amount > 0).ToList();
                var negativeTransactions = trans.Where(tr => tr.Amount < 0).ToList();

                var positiveTransactionsByMonth = positiveTransactions
                         .GroupBy(tr => new { tr.CreationDate.Year, tr.CreationDate.Month })
                         .Select(group => new MonthDTO
                         {
                             Year = group.Key.Year,
                             Month = (MonthsType)group.Key.Month,
                             Amount = group.Sum(tr => tr.Amount)
                         })
                         .ToList();

                var negativeTransactionsByMonth = negativeTransactions
                        .GroupBy(tr => new { tr.CreationDate.Year, tr.CreationDate.Month })
                        .Select(group => new MonthDTO
                        {
                            Year = group.Key.Year,
                            Month = (MonthsType)group.Key.Month,
                            Amount = group.Sum(tr => tr.Amount)
                        })
                        .ToList();




                return Ok(new
                {
                    positiveTransactionsByMonth,
                    negativeTransactionsByMonth
                });
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
