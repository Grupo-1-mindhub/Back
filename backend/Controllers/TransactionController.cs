using backend.DTOs;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/")]
    [ApiController]
    public class TransactionController : Controller
    {
        private ITransactionRepository _transactionRepository;
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private IBudgetRepository _budgetRepository;

        public TransactionController(ITransactionRepository transactionRepository, IClientRepository clientRepository, IAccountRepository accountRepository, IBudgetRepository budgetRepository)
        {
            _transactionRepository = transactionRepository;
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _budgetRepository = budgetRepository;
        }

        [HttpGet("transactions")]
        public IActionResult Get() 
        {
            try
            {
                var tran = _transactionRepository.GetAllTransactions();
                var transactionDTO = new List<TransactionDTO>();
                foreach (var transaction in tran)
                {
                    var newTransactionDTO = new TransactionDTO
                    {
                        //Id = transaction.Id,
                        Amount = transaction.Amount,
                        Description = transaction.Description,
                        CreationDate = transaction.CreationDate
                    };
                    transactionDTO.Add(newTransactionDTO);
                }
                return Ok(transactionDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("clients/current/{accountId}/{budgetId}/transactions")]
        public IActionResult Post([FromBody] Transaction transaction, long accountId, long budgetId)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                    return StatusCode(403, "Cliente no autorizado");

                Client cl = _clientRepository.FindByEmail(email);
                if (cl == null)
                    return StatusCode(403, "Cliente no encontrado");

                //ESTO ES AL PEDO SALE DE CLIENTE (evitamos consultas a la base pero no se si esta bien sacarlo)
                //Account account = _accountRepository.FindById(accountId);
                //if (account == null)
                //    return StatusCode(403, "Cuenta inexistente");
                //Budget budget = _budgetRepository.FindById(budgetId);
                //if (budget == null)
                //    return StatusCode(403, "Presupuesto inexistente");

                Account account = cl.Accounts.FirstOrDefault(account => account.Id == accountId);
                if (account == null)
                    return StatusCode(403, "Cuenta inexistente");

                Budget budget = account.Budgets.FirstOrDefault(budget => budget.Id == budgetId);
                if (budget == null)
                    return StatusCode(403, "Presupuesto inexistente");


                if (transaction.Amount <= 0 || transaction.Description == string.Empty) 
                    return StatusCode(403, "Datos invalidos");

                if (account.Balance < transaction.Amount || budget.Amount < transaction.Amount)
                    return StatusCode(403, "Fondos insuficientes para realizar la operacion");

                _transactionRepository.Save(new Transaction
                {  
                    Amount = transaction.Amount * -1,
                    Description = transaction.Description,
                    CreationDate = DateTime.Now,
                    BudgetId = budget.Id,
                    PaymentMethodId = transaction.PaymentMethodId
                });

                
                //Probar si budget se actualiza cuando se guarda una transaccion que apunta a su ID
                //si no lo actualiza hay que agregar la transaccion que se acaba de guardar en el transactiones
                //del updatedBudget
                Budget updatedBudget = new Budget
                {
                    Id = budget.Id,
                    Amount = budget.Amount + transaction.Amount * -1,
                    AccountId = budget.AccountId,
                    CategoryId = budget.CategoryId,
                    Transactions = budget.Transactions.Select(tr => new Transaction
                    {
                        Id = tr.Id,
                        Amount = tr.Amount,
                        Description = tr.Description,
                        CreationDate = tr.CreationDate,
                    }).ToList()
                };

                _budgetRepository.Save(updatedBudget);

                Account updatedAccount = new Account
                {
                    Id = account.Id,
                    Number = account.Number,
                    Description = account.Description,
                    Balance = account.Balance + transaction.Amount * -1,
                    CreationDate = account.CreationDate,
                    ClientId = account.ClientId
                };

                _accountRepository.Save(updatedAccount);

                //devolver dto con la transaction creada
                return Ok();
            
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
