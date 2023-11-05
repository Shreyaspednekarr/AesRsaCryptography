using AesRsaCryptography.Business.Interface;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace AesRsaCryptography.Business.Implementation
{
    /// <inheritdoc/>
    public class RsaService : IRsaService
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor to be initialized
        /// </summary>
        /// <param name="configuration">To get the configuration values</param>
        /// <exception cref="ArgumentNullException"></exception>
        public RsaService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <inheritdoc/>
        public async Task<string> EncryptUsingPublicKey(string dataToEncrypt)
        {
            var publicKey = await GetPrivateOrPublicKey("PublicKey").ConfigureAwait(false);
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportFromPem(publicKey.ToCharArray());
                var cipherText = rsa.Encrypt(Encoding.UTF8.GetBytes(dataToEncrypt), true);
                var encryptedText = Convert.ToBase64String(cipherText);
                return encryptedText;
            }
        }

        /// <inheritdoc/>
        public async Task<string> DecryptUsingPrivateKey(string encryptedText)
        {
            var privateKey = await GetPrivateOrPublicKey("PrivateKey").ConfigureAwait(false);
            using (var rsa = new RSACryptoServiceProvider())
            {
                var cipherBytes = Convert.FromBase64String(encryptedText);
                rsa.ImportFromPem(privateKey.ToCharArray());
                var decryptedBytes = rsa.Decrypt(cipherBytes, true);
                var decryptedText = Encoding.UTF8.GetString(decryptedBytes);
                return decryptedText;
            }
        }

        /// <summary>
        /// Method to get RSA private or public key based on key type = 'PRIVATE' or 'PUBLIC'
        /// </summary>
        /// <param name="keyType">Key Type = 'PRIVATE' or 'PUBLIC'</param>
        /// <returns>Returns RSA private or public key based on key type</returns>
        private async Task<string> GetPrivateOrPublicKey(string keyType)
        {
            var key = _configuration["RSA:" + keyType]?.ToString();
            return key;
        }
    }
}
