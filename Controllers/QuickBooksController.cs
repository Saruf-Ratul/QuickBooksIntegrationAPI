using Microsoft.AspNetCore.Mvc;
using QuickBooksIntegrationAPI.Services;
using System.Threading.Tasks;

namespace QuickBooksIntegrationAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class QuickBooksController : ControllerBase
    {
        private readonly QuickBooksService _quickBooksService;

        public QuickBooksController(QuickBooksService quickBooksService)
        {
            _quickBooksService = quickBooksService;
        }

        [HttpGet("auth-url")]
        public IActionResult GetAuthUrl()
        {
            var authUrl = _quickBooksService.GetAuthUrl();
            return Ok(new { authUrl });
        }

        [HttpPost("exchange-code")]
        public async Task<IActionResult> ExchangeCodeForToken([FromBody] string code)
        {
            try
            {
                var response = await _quickBooksService.ExchangeCodeForTokenAsync(code);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}