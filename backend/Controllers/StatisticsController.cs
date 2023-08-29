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
       // [HttpGet("clients/accounts/{id}/statistics")]
        //public IActionResult GetBudgetsByAccount(long id)
        //{
        //    try
        //    {

        //        //Client cl = _clientRepository.FindById(id);
        //        //if (cl == null)
        //        //{
        //        //    return StatusCode(403, "Cliente no encontrado");
        //        //}

        //        //var accs = cl.Accounts;
        //        //if (accs== null)
        //        //    return StatusCode(403, "El cliente no tiene cuentas");

        //        //var buds = new List<Budget>();
                
        //        //foreach (Account acc in accs)
        //        //{

        //        //    foreach (Budget budget in acc.Budgets) 
        //        //    { 
        //        //        buds.Add(budget);
        //        //    }
        //        //}

        //        //    var allTransactions = new List<TransactionDTO>();

        //        //foreach (Budget bud in budgets)
        //        //{
        //        //    allTransactions.AddRange((IEnumerable<TransactionDTO>)bud.Transactions);
        //        //}
        //        ////var transactionsByMonthAndYear = allTransactions
        //        //            .GroupBy(tr => new { tr.CreationDate.Year, tr.CreationDate.Month })
        //        //            .ToDictionary(
        //        //                group => $"{group.Key.Year}-{group.Key.Month}",
        //        //                group => group.Select(tr => new TransactionDTO
        //        //                {
        //        //                    Id = tr.Id,
        //        //                    Amount = tr.Amount,
        //        //                    Description = tr.Description,
        //        //                    CreationDate = tr.CreationDate
        //        //                }).ToList()
        //        //            );

        //        //var monthsWithTransactions = transactionsByMonthAndYear.Keys.Select(key =>
        //        //{
        //        //    var parts = key.Split('-');
        //        //    return new MonthDTO
        //        //    {
        //        //        Year = int.Parse(parts[0]),
        //        //        Month = (MonthsType)int.Parse(parts[1]),
        //        //        Amount = transactionsByMonthAndYear[key].Sum(tr => tr.Amount)
        //        //    };
        //        //}).ToList();



        //        //return Ok(monthsWithTransactions);



        //        //  return Ok(allTransactions);
        //    //    return Ok(buds);
        //    }

        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}
    }
}
