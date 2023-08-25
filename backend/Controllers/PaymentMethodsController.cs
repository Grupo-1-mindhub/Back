using backend.DTOs;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        private IPaymentMethodRepository _paymentMethodRepository;
        public PaymentMethodsController(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }
        [HttpGet("paymentmethod")]
        public IActionResult Get()
        {
            try
            {
                var pm = _paymentMethodRepository.GetAllPaymentMethods();
                var pmDTO = new List<PaymentMethodDTO>();
                foreach(PaymentMethod paymentMethod in pm)
                {
                    PaymentMethodDTO payMethDTO = new PaymentMethodDTO()
                    {
                        Id = paymentMethod.Id,
                        Description = paymentMethod.Description,
                    };
                    pmDTO.Add(new PaymentMethodDTO ());
                }
                return Ok(pmDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet("paymentmethod/{id}")]
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
