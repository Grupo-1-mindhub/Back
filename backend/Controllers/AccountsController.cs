using backend.DTOs;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class AccountsController : ControllerBase
    {
        private IAccountRepository _accountRepository;
        private IClientRepository _clientRepository;
        public AccountsController(IAccountRepository accountRepository, IClientRepository clientRepository)
        {
            _accountRepository = accountRepository;
            _clientRepository = clientRepository;
        }
        [HttpGet("accounts")]
        public IActionResult Get()
        {
            try
            {
                var acc = _accountRepository.GetAllAccounts();  //trae todas las cuentas
                var accDTO = new List<AccountDTO>();
                foreach (var account in acc)
                {
                    var newAccDTO = new AccountDTO
                    {
                        Id = account.Id,
                        Number = account.Number,
                        Description= account.Description,
                        CreationDate = account.CreationDate,
                        Balance = account.Balance,
                    };
                    accDTO.Add(newAccDTO);
                }
                return Ok(accDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("accounts/{id}")]
        public IActionResult Get(long id)   //ahora busca por id
        {
            try
            {
                var acc = _accountRepository.FindById(id);
                if (acc == null)
                {
                    return NotFound();      //no se encuentra el cliente
                }
                var accDTO = new AccountDTO
                {
                    Id = acc.Id,
                    Number = acc.Number,
                    Description= acc.Description,
                    CreationDate = acc.CreationDate,
                    Balance = acc.Balance,
                };
                return Ok(accDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("clients/current/accounts")]
        public IActionResult GetAcc()
        {
            try
            {
                var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                if (email == string.Empty)
                {
                    return StatusCode(403, "cliente no autorizado");
                }
                Client cl = _clientRepository.FindByEmail(email);
                if (cl == null)
                {
                    return StatusCode(403, "cliente no encontrado");
                }
                var accDTO = new List<AccountDTO>();    //si encuentra el cliente se trae los datos
                foreach (Account acc in cl.Accounts)
                {
                    var newAccDTO = new AccountDTO
                    {
                        Id = acc.Id,
                        Number = acc.Number,
                        Description = acc.Description,
                        CreationDate = acc.CreationDate,
                        Balance = acc.Balance,
                        Transactions = acc.Transactions.Select(y => new TransactionDTO
                        {
                            Id = y.Id,
                            Amount = y.Amount,
                            Description = y.Description,
                            CreationDate = y.CreationDate
                        }).ToList()
                    };
                    accDTO.Add(newAccDTO);
                }
                return Ok(accDTO);  //muestra la cuenta en el front
            }
            catch (Exception ex)
            {
                return StatusCode(403, ex.Message);
            }
        }
        [HttpPost("clients/current/accounts")]  //crear una nueva cuenta
        public IActionResult Post([FromBody]AccountCreationDTO account)
        {
            try
            {
                var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                if (email == string.Empty)
                {
                    return StatusCode(403, "cliente no autorizado");
                }
                Client cl = _clientRepository.FindByEmail(email);
                if (cl == null)
                {
                    return StatusCode(403, "cliente no encontrado");
                }

                //esto capaz se va a romper si no tiene cuentas por eso lo comento
                //int num = cl.Accounts.Count() + 1;

                var random = new Random();
                string num = "VIN-" + random.Next(0, 99999999).ToString().PadLeft(8, '0');
                Account newAcc = new Account
                {
                    Number = num,
                    Description=account.Description,
                    CreationDate = DateTime.Now,
                    Balance = account.Balance,
                    ClientId = cl.Id
                };
                _accountRepository.Save(newAcc);
                AccountDTO newAccDTO = new AccountDTO
                {
                    Number = newAcc.Number,
                    Description=newAcc.Description,
                    CreationDate = newAcc.CreationDate,
                    Balance = newAcc.Balance,
                };
                return Created("", newAccDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpDelete("accounts/{id}")]
        public IActionResult DeleteAccount(long id)
        {
            try
            {
                var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    return StatusCode(403, "Cliente no autorizado");
                }
                Client client = _clientRepository.FindByEmail(email);
                if (client == null)
                {
                    return StatusCode(403, "Cliente no encontrado");
                }
                Account accountToDelete = _accountRepository.FindById(id);
                if (accountToDelete == null || accountToDelete.ClientId != client.Id)
                {
                    return NotFound();
                }
                _accountRepository.DeleteAccount(accountToDelete);
                return Ok(new { message = "Cuenta eliminada exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
