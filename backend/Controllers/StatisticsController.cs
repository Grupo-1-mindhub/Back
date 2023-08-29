using backend.DTOs;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private IAccountRepository _accountRepository;
        private IClientRepository _clientRepository;
        public StatisticsController(IAccountRepository accountRepository, IClientRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
        }
        [HttpGet("clients/accounts/{id}/budgets")]
        [Authorize] // Requiere autenticación
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
                var incomeMonths =new List<MonthDTO>();
                var expenseMonths = new List<MonthDTO>();
                foreach (Budget bud in budgets)
                {
                    var transactions = bud.Transactions;
                    var incomeTotal = transactions.Where(tr => tr.Amount > 0).Sum(tr => tr.Amount);
                    var expenseTotal = transactions.Where(tr => tr.Amount < 0).Sum(tr => tr.Amount);
                    var monthsWithTransactions = transactions
                        .GroupBy(tr => new { tr.CreationDate.Month, tr.CreationDate.Year })
                        .Select(group => new MonthDTO
                        {
                            Amount = group.Sum(tr => tr.Amount),
                            Month = (MonthsType)group.Key.Month,
                            Year = group.Key.Year
                        })
                        .ToList();
                    foreach (var month in monthsWithTransactions)
                    {
                        if (month.Amount > 0)
                        {
                            incomeMonths.Add(month);
                        }
                        else if (month.Amount < 0)
                        {
                            expenseMonths.Add(month);
                        }
                    }
                }

             }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
