using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AesRsaCryptography.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _requestDelegate;

        public AuthenticationMiddleware(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate ?? throw new ArgumentNullException(nameof(requestDelegate));
        }

        public async Task Invoke(HttpContext context)
        {
            var authorizationToken = context.Request.Headers.Authorization;

            if (!string.IsNullOrEmpty(authorizationToken))
            {
                var token = authorizationToken.ToString().Replace("Bearer ", string.Empty);
                var readJwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var identity = new ClaimsIdentity(readJwt.Claims, "Bearer");
                context.User = new ClaimsPrincipal(identity);
            }

            await _requestDelegate(context).ConfigureAwait(false);
        }
    }
}
