using Microsoft.AspNetCore.Mvc;
using TennisStoreServer.Repositories;
using TennisStoreSharedLibrary.Models;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController(IBrand brandService) : ControllerBase
    {
        private readonly IBrand brandService = brandService;

        [HttpGet]
        public async Task<ActionResult<List<Brand>>> GetAllBrands()
        {
            var brands = await brandService.GetAllBrands();
            return Ok(brands);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> AddBrand(Brand model)
        {
            if (model is null) return BadRequest("Model is null");
            var response = await brandService.AddBrand(model);
            return Ok(response);
        }
    }
}
