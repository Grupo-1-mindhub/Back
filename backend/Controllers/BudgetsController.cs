using backend.DTOs;
using backend.Enumerates;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BudgetsController : Controller
    {
        private IBudgetRepository _budgetRepository;
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;

        public BudgetsController(IBudgetRepository budgetRepository, IClientRepository clientRepository, IAccountRepository accountRepository)
        {
            _budgetRepository = budgetRepository;
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
        }

        [HttpGet("clients/accounts/{id}/budgets")]
        public IActionResult GetBudgetsByAccount(long id)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                    return StatusCode(403, "Cliente no autorizado");

                Client cl = _clientRepository.FindByEmail(email);
                if (cl == null)
                    return StatusCode(403, "Cliente no encontrado");

                //var acc = _accountRepository.FindById(id);

                var acc = cl.Accounts.FirstOrDefault(account => account.Id == id);
                if (acc == null)
                    return StatusCode(403, "Cuenta invalida");

                var budDTO = new List<BudgetDTO>(); //variable DTO xq no queremos mostrar todos los datos

                foreach (Budget bud in acc.Budgets)
                {
                    BudgetDTO budgetDTO = new BudgetDTO()
                    {
                        Id = bud.Id,
                        Amount = bud.Amount,
                        Transactions = bud.Transactions.Select(tr => new TransactionDTO
                        {
                            Id = tr.Id,
                            Amount = tr.Amount,
                            Description = tr.Description,
                            CreationDate = tr.CreationDate,
                        }).ToList()
                    };
                    budDTO.Add(new BudgetDTO());
                }
                return Ok(budDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("budgets/{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var bud = _budgetRepository.FindById(id);
                if (bud == null)
                {
                    return NotFound();
                }
                var budDTO = new BudgetDTO
                {
                    Id = bud.Id,
                    Amount = bud.Amount,
                    Transactions = bud.Transactions.Select(tr => new TransactionDTO
                    {
                        Id = tr.Id,
                        Amount = tr.Amount,
                        Description = tr.Description,
                        CreationDate = tr.CreationDate,
                    }).ToList()
                };
                return Ok(budDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("budgets")]
        public IActionResult Post([FromBody] Budget budget)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "Cliente no autorizado");
                }

                Client cl = _clientRepository.FindByEmail(email);
                if (cl == null)
                {
                    return StatusCode(403, "Cliente no encontrado");
                }

                Budget newBudget = new Budget
                {
                    Id = budget.Id,
                    Amount = budget.Amount,
                    AccountId = budget.AccountId,
                    CategoryId = budget.CategoryId,
                    Transactions = new List<Transaction>()
                };

                _budgetRepository.Save(newBudget);

                BudgetDTO newBudgetDTO = new BudgetDTO
                {
                    Id = newBudget.Id,
                    Amount = newBudget.Amount,
                    Transactions = new List<TransactionDTO>()
                };

                return Created("", newBudgetDTO);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
