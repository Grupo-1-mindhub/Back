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
        public IActionResult Post([FromBody]Account account)
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
                int num = cl.Accounts.Count() + 1;
                Account newAcc = new Account
                {
                    Number = num.ToString("D3"),
                    Description=account.Description,
                    CreationDate = DateTime.Now,
                    Balance = 0,
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
    }
}
