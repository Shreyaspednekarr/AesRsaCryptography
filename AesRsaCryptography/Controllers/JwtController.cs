using AesRsaCryptography.Business.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AesRsaCryptography.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public JwtController(IJwtService jwtService)
        {
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        }

        [HttpPost("GenerateJwtToken")]
        public async Task<IActionResult> GenerateJwtToken([FromBody] IDictionary<string, string> claims)
        {
            var result = await _jwtService.GenerateJwtToken(claims).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet("ExtractJwtToken")]
        public async Task<IActionResult> ExtractJwtToken([FromQuery] string jwtToken)
        {
            var result = await _jwtService.ExtractJwtToken(jwtToken).ConfigureAwait(false);
            return Ok(result);
        }
    }
}
