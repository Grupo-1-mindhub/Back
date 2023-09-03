using backend.DTOs;
using backend.Models;
using backend.Repositories;
using backend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ClientsController : Controller
    {
        private IClientRepository _clientRepository;
        private IAccountRepository _accountRepository;
        private ITokenServices _tokenServices;

        public ClientsController(IClientRepository clientRepository, IAccountRepository accountRepository, ITokenServices tokenServices)
        { 
            _clientRepository = clientRepository;
            _accountRepository = accountRepository;
            _tokenServices = tokenServices;
        }

        [HttpGet] //cuando hagamos un peticion de tipo get al controlador va a responder con el sgte metodo
        public IActionResult Get()
        {
            try
            {
                var clients = _clientRepository.GetAllClients(); //el GEtallClients incluye las cuentas
                //con var no especificamos el tipo de dato
                var clientsDTO = new List<ClientDTO>(); //variable DTO xq no queremos mostrar todos los datos

                foreach (Client client in clients) //recorremos
                {
                    var newClientDTO = new ClientDTO //creamos nuevos cliente DTO
                    {
                        Id = client.Id,
                        Email = client.Email,
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Accounts = client.Accounts.Select(ac => new AccountDTO //SELECT metodo de linq para modificar datos
                        {
                            Id = ac.Id,
                            Balance = ac.Balance,
                            Description = ac.Description,
                            CreationDate = ac.CreationDate,
                            Number = ac.Number
                        }).ToList(),
                    };
                    clientsDTO.Add(newClientDTO);
                }
                return Ok(clientsDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var client = _clientRepository.FindById(id);
                if (client == null)
                {
                    return NotFound(); //no encontro el cliente
                }
                var clientDTO = new ClientDTO
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        Description = ac.Description,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number
                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        DeadLine = c.Deadline,
                        Number = c.Number,
                        Type = c.Type
                    }).ToList(),

                };
                return Ok(clientDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("current")]
        [Authorize]
        public IActionResult GetCurrent()
        {
            try
            {
                var email= HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                if (email == string.Empty)
                {
                    return StatusCode(401, "Unauthorized");
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null) //lo buscamos como cliente en nuestra base de datos
                {
                    return Forbid();
                }

                var clientDTO = new ClientDTO //si lo encontramos en la base de datos, traemos todos los datos
                {
                    Id = client.Id,
                    Email = client.Email,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Accounts = client.Accounts.Select(ac => new AccountDTO
                    {
                        Id = ac.Id,
                        Balance = ac.Balance,
                        Description = ac.Description,
                        CreationDate = ac.CreationDate,
                        Number = ac.Number,
                        Transactions = ac.Transactions.Select(tr => new TransactionDTO
                        {
                            Id = tr.Id,
                            Amount = tr.Amount,
                            Description = tr.Description,
                            //CategoryId = tr.CategoryId,
                            CreationDate = tr.CreationDate, 
                            }).ToList(),
                    }).ToList(),
                    Cards = client.Cards.Select(c => new CardDTO
                    {
                        Id = c.Id,
                        DeadLine = c.Deadline,
                        Number = c.Number,
                        Type = c.Type
                    }).ToList()

                };

                return Ok(clientDTO); //muestra el cliente en el Front
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost] //CREAR UN NUEVO CLIENTE
        public IActionResult Post([FromBody] RegisterDTO client)
        {
            try
            {
                if (String.IsNullOrEmpty(client.Email) || String.IsNullOrEmpty(client.Password) || String.IsNullOrEmpty(client.FirstName) || String.IsNullOrEmpty(client.LastName))
                    return StatusCode(403, "Invalid Data");
                //buscamos si ya existe el usuario
                Client user = _clientRepository.FindByEmail(client.Email); //buscamos en el repositorio
                if (user != null)
                {
                    return StatusCode(403, "Email está en uso");
                }

                Client newClient = new Client //creamos nuevo objeto de tipo cliente
                {
                    Email = client.Email,
                    Password = client.Password,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                };
                _clientRepository.Save(newClient); //usamos el repo para utilizar el metodo SAVE 
                ClientDTO newCDTO = new ClientDTO
                {
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Email = client.Email,
                };
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut("update")]
        [Authorize]
        public IActionResult Put( [FromBody] UpdateClientDTO updateClient)
        {
            try
            {
                var email = HttpContext.User.FindFirst(ClaimTypes.Email)?.Value;
                if (email == string.Empty)
                {
                    return StatusCode(401, "Unauthorized");
                }

                Client client = _clientRepository.FindByEmail(email);
                if (client == null) //lo buscamos como cliente en nuestra base de datos
                {
                    return Forbid();
                }

                // Actualiza los campos que desees
                client.FirstName = updateClient.FirstName;
                client.LastName = updateClient.LastName;
                client.Email= updateClient.Email;
                // Actualiza otros campos según sea necesario

                // Guarda los cambios en el repositorio
                _clientRepository.Save(client);

                return Ok(); // Devuelve el cliente actualizado
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}
