using AesRsaCryptography.Business.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AesRsaCryptography.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RsaController : ControllerBase
    {
        private readonly IRsaService _rsaService;

        public RsaController(IRsaService rsaService)
        {
            _rsaService = rsaService ?? throw new ArgumentNullException(nameof(rsaService));
        }

        [HttpGet("EncryptUsingPublicKey")]
        public async Task<IActionResult> EncryptUsingPublicKey([FromQuery] string dataToEncrypt)
        {
            var result = await _rsaService.EncryptUsingPublicKey(dataToEncrypt).ConfigureAwait(false);
            return Ok(result);
        }

        [HttpGet("DecryptUsingPrivateKey")]
        public async Task<IActionResult> DecryptUsingPrivateKey([FromQuery] string encryptedText)
        {
            var result = await _rsaService.DecryptUsingPrivateKey(encryptedText).ConfigureAwait(false);
            return Ok(result);
        }
    }
}
