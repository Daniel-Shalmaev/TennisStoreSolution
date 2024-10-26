using Microsoft.AspNetCore.Mvc;
using TennisStoreServer.Repositories;
using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategory categoryService) : ControllerBase
    {
        private readonly ICategory categoryService = categoryService;

        [HttpGet]
        public async Task<ActionResult<List<Category>>> GetAllCategories()
        {
            var products = await categoryService.GetAllCategories(); return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> AddCategory(Category model)
        {
            if (model is null) return BadRequest("Model is null");
            var response = await categoryService.AddCategory(model);   
            return Ok(response);
        }
    }
}
