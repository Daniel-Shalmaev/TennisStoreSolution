using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TennisStoreServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelProcessingController(IExcelProcessingService excelProcessingService) : ControllerBase
    {
        private readonly IExcelProcessingService _excelProcessingService = excelProcessingService;

        [HttpPost("upload")]
        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File not provided.");

            var result = await _excelProcessingService.ProcessExcelFileAsync(file);

            if (!result.Flag)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
    }
}
