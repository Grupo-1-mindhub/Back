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
    public class PaymentMethodsController : ControllerBase
    {
        private IPaymentMethodRepository _paymentMethodRepository;
        public PaymentMethodsController(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }
        [HttpGet("paymentmethods")]
        public IActionResult Get()
        {
            try
            {
                var pm = _paymentMethodRepository.GetAllPaymentMethods();
                var pmDTO = new List<PaymentMethodDTO>();
                foreach(PaymentMethod paymentMethod in pm)
                {
                    PaymentMethodDTO paymentDTO = new PaymentMethodDTO()
                    {
                        Id = paymentMethod.Id,
                        Description = paymentMethod.Description,
                    };
                    pmDTO.Add(paymentDTO);
                }
                return Ok(pmDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("paymentmethods/{id}")]
        public IActionResult Get(long id)
        {
            try
            {
                var pm = _paymentMethodRepository.FindById(id);
                if(pm== null)
                {
                    return NotFound();
                }
                var pmDTO = new PaymentMethodDTO
                {
                    Id = pm.Id,
                    Description = pm.Description,
                };
                return Ok(pmDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
