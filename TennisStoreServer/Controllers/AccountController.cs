using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TennisStoreServer.Repositories;
using TennisStoreSharedLibrary.DTOs;
using TennisStoreSharedLibrary.Responses;

namespace TennisStoreServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController(IUserAccount accountService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> CreateAccount(UserDTO model)
        {
            if (model is null) return BadRequest("Model is null");
            var response = await accountService.Register(model);
            return Ok(response);    
        }
    }
}
