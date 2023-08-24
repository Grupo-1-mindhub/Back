using backend.DTOs;
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

        public BudgetsController(IBudgetRepository budgetRepository, IClientRepository clientRepository)
        {
            _budgetRepository = budgetRepository;
            _clientRepository = clientRepository;
        }

        [HttpGet("accounts/{id}/budgets")]
        public IActionResult Get()
        {
            try
            {
                var bud = _budgetRepository.GetAllBudgets(); //trae todos los budgets
                var budDTO = new List<BudgetDTO>(); //variable DTO xq no queremos mostrar todos los datos

                foreach (Budget budget in bud)
                {
                    BudgetDTO budgetDTO = new BudgetDTO()
                    {
                        Id = budget.Id,
                        Amount = budget.Amount,
                        Transactions = budget.Transactions.Select(tr => new TransactionDTO
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

        [HttpGet("budget/{id}")]
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
                    Transactions = budget.Transactions.Select(tr => new TransactionDTO
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

        [HttpPost]
        public IActionResult Post([FromBody] Budget budget)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                {
                    return StatusCode(403, "cliente no autorizado");
                }

                Client cl = _clientRepository.FindByEmail(email);
                if (cl == null)
                {
                    return StatusCode(403, "cliente no encontrado");
                }

                Budget newBudget = new Budget
                {
                    Id = budget.Id,
                    Balance = budget.Balance,
                };
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
