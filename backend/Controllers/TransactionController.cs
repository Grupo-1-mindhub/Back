﻿using backend.DTOs;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : Controller
    {
        //private IClientRepository _clientRepository;
        //private IAccountRepository _accountRepository;
        private ITransactionRepository _transactionRepository;


        public TransactionController(ITransactionRepository transactionRepository)
        {
            //_clientRepository = clientRepository;
            //_accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
        }

        [HttpGet]
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

        [HttpPost]
        public IActionResult Post([FromBody] TransactionDTO transactionDTO)
        {
            try
            {

                if (transactionDTO.Amount <= 0) 
                {
                    return Forbid("El monto debe ser superior a 0");
                }
                if (transactionDTO.Description == string.Empty) 
                {
                    return Forbid("Debe ingresar una descripcion");
                }

                _transactionRepository.Save(new Transaction
                {  
                    Amount = transactionDTO.Amount * -1,
                    Description = transactionDTO.Description,
                    CreationDate = DateTime.Now,
                });

                return Ok();
            
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
