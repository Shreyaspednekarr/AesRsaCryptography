using AesRsaCryptography.Business.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace AesRsaCryptography.Filters
{
    public class CryptoFilter : ActionFilterAttribute, IActionFilter
    {
        private string _key { get; set; }

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.HttpContext.Request.QueryString.HasValue)
            {
                var rsaService = context.HttpContext.RequestServices.GetRequiredService<IRsaService>();
                var aesService = context.HttpContext.RequestServices.GetRequiredService<IAesService>();

                var symmetricKey = context.HttpContext.Request.Query["key"].ToString();

                _key = await rsaService.DecryptUsingPrivateKey(symmetricKey).ConfigureAwait(false);

                var decryptedText = await aesService.Decrypt("", _key).ConfigureAwait(false);
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context != null)
            {
                var rsaService = context.HttpContext.RequestServices.GetRequiredService<IRsaService>();
                var aesService = context.HttpContext.RequestServices.GetRequiredService<IAesService>();

                var plainTextResponse = JsonConvert.SerializeObject(((ObjectResult)context.Result).Value);

                var encryptedResponse = aesService.Encrypt(plainTextResponse, _key);

                var symmetricKey = rsaService.EncryptUsingPublicKey(_key);

                var response = new
                {
                    Key = symmetricKey,
                    Data = encryptedResponse
                };

                context.Result = new ObjectResult(response);
            }
        }
    }
}
