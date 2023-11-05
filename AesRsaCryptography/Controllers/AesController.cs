using AesRsaCryptography.Business.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AesRsaCryptography.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AesController : ControllerBase
    {
        private readonly IAesService _aesService;

        public AesController(IAesService aesService)
        {
            _aesService = aesService ?? throw new ArgumentNullException(nameof(aesService));
        }

        [HttpGet("GenerateRandomAesKey")]
        public async Task<IActionResult> GenerateRandomAesKey([FromQuery] int keyLengthInBits = 128)
        {
            var result = await _aesService.GenerateRandomAesKey(keyLengthInBits).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet("Encrypt")]
        public async Task<IActionResult> Encrypt([FromQuery] string dataAsJsonString, [FromQuery] string aesKey)
        {
            var result = await _aesService.Encrypt(dataAsJsonString, aesKey).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet("Decrypt")]
        public async Task<IActionResult> Decrypt([FromQuery] string encryptedString, [FromQuery] string aesKey)
        {
            var result = await _aesService.Decrypt(encryptedString, aesKey).ConfigureAwait(false);
            return Ok(result);
        }
    }
}
