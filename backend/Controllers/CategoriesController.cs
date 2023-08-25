using backend.DTOs;
using backend.Models;
using backend.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private ICategoryRepository _categoryRepository;
       
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("categories")]
        public IActionResult Get()
        {
            try
            {
                var cat = _categoryRepository.GetAllCategorys(); //trae todas las categorias
                var catDTO = new List<CategoryDTO>(); //variable DTO xq no queremos mostrar todos los datos

                foreach (Category category in cat)
                {
                    CategoryDTO categoryDTO = new CategoryDTO()
                    {
                        Id = category.Id,
                        Description = category.Description,
                    };
                    catDTO.Add(new CategoryDTO());
                }
                return Ok(catDTO);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("categories/{id}")]
        public IActionResult Get(long id) 
        { 
            try
            {
                var cat = _categoryRepository.FindById(id);
                if (cat == null)
                {
                    return NotFound();
                }
                var catDTO = new CategoryDTO
                {
                    Id = cat.Id,
                    Description = cat.Description,
                };
                return Ok(catDTO);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
