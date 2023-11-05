namespace AesRsaCryptography.Business.Interface
{
    /// <summary>
    /// Interface to declare JWT token related methods
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Method to generate JWT token by passing claims in request
        /// </summary>
        /// <param name="claims">Claims</param>
        /// <returns>Returns JWT token string</returns>
        Task<string> GenerateJwtToken(IDictionary<string, string> claims);

        /// <summary>
        /// Method to extract claim details by passing JWT token in request
        /// </summary>
        /// <param name="jwtToken">JWT token</param>
        /// <returns>Returns claims</returns>
        Task<IDictionary<string, string>> ExtractJwtToken(string jwtToken);
    }
}
