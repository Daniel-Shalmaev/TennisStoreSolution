using Microsoft.AspNetCore.Mvc;
using TennisStoreServer.Repositories;
using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct productService;

        public ProductController(IProduct productService) => this.productService = productService;

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts(bool featured)
        {
            var products = await productService.GetAllProducts(featured); return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await productService.GetProductById(id);
            if (product == null) return NotFound($"Product with ID {id} not found.");
            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> AddProduct(Product model)
        {
            if (model is null) return BadRequest("Model is null");
            var response = await productService.AddProduct(model);   
            return Ok(response);
        }
    }
}
