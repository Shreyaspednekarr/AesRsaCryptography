using AesRsaCryptography.Business.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AesRsaCryptography.Business.Implementation
{
    /// <inheritdoc/>
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor to be initialized
        /// </summary>
        /// <param name="configuration">To get the configuration values</param>
        /// <exception cref="ArgumentNullException"></exception>
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc/>
        public async Task<string> GenerateJwtToken(IDictionary<string, string> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claimList = new();
            foreach (var item in claims)
            {
                var claim = new Claim(item.Key, item.Value);
                claimList.Add(claim);
            }

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                                            _configuration["Jwt:Audience"],
                                            claims: claimList,
                                            expires: DateTime.UtcNow.AddHours(1),
                                            signingCredentials: credentials);

            var writeToken = new JwtSecurityTokenHandler().WriteToken(token);
            return writeToken;
        }

        /// <inheritdoc/>
        public async Task<IDictionary<string, string>> ExtractJwtToken(string jwtToken)
        {
            var readJwt = new JwtSecurityTokenHandler().ReadJwtToken(jwtToken);
            var claims = readJwt.Claims.ToDictionary(x => x.Type, x => x.Value);
            return claims;
        }
    }
}
