using backend.DTOs;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class TransactionController : Controller
    {
        private ITransactionRepository _transactionRepository;
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ICategoryRepository _categoryRepository;
      

        public TransactionController(ITransactionRepository transactionRepository, IClientRepository clientRepository, IAccountRepository accountRepository, ICategoryRepository categoryRepository)
        {
            _transactionRepository = transactionRepository;
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _categoryRepository = categoryRepository;
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
                    var categoryid = transaction.CategoryId;
                    Category category = _categoryRepository.FindById(categoryid);
                    var newTransactionDTO = new TransactionDTO
                    {
                        Id = transaction.Id,
                        Amount = transaction.Amount,
                        Description = transaction.Description,
                        CreationDate = transaction.CreationDate,
                        Category=category.Description
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

        [HttpPost("clients/current/{accountId}/transactions")]
        public IActionResult Post([FromBody] Transaction transaction, long accountId)
        {
            try
            {
                string email = User.FindFirst("Client") != null ? User.FindFirst("Client").Value : string.Empty;
                if (email == string.Empty)
                    return StatusCode(403, "Cliente no autorizado");

                Client cl = _clientRepository.FindByEmail(email);
                if (cl == null)
                    return StatusCode(403, "Cliente no encontrado");

                Account account = cl.Accounts.FirstOrDefault(account => account.Id == accountId);
                if (account == null)
                    return StatusCode(403, "Cuenta inexistente");

 
                if (transaction.Amount <= 0 || transaction.Description == string.Empty) 
                    return StatusCode(403, "Datos invalidos");

                if (account.Balance < transaction.Amount )
                    return StatusCode(403, "Fondos insuficientes para realizar la operacion");

                _transactionRepository.Save(new Transaction
                {  
                    Amount = transaction.Amount * -1,
                    Description = transaction.Description,
                    CreationDate = DateTime.Now,
                    AccountId = account.Id,
                    PaymentMethodId = transaction.PaymentMethodId,
                    CategoryId= transaction.CategoryId
                });


                //Probar si account se actualiza cuando se guarda una transaccion que apunta a su ID
                //si no lo actualiza hay que agregar la transaccion que se acaba de guardar en el transactiones
                //del updatedaccount



                Account updatedAccount = new Account
                {
                    Id = account.Id,
                    Number = account.Number,
                    Description = account.Description,
                    Balance = account.Balance + transaction.Amount * -1,
                    CreationDate = account.CreationDate,
                    ClientId = account.ClientId,
                    Transactions = account.Transactions.Select(tr => new Transaction
                    {
                        Id = tr.Id,
                        Amount = tr.Amount,
                        Description = tr.Description,
                        CreationDate = tr.CreationDate,
                    }).ToList()
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
