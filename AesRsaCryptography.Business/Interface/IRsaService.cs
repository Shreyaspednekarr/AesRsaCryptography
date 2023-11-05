namespace AesRsaCryptography.Business.Interface
{
    /// <summary>
    /// Interface to declare RSA related methods
    /// </summary>
    public interface IRsaService
    {
        /// <summary>
        /// Encrypt data using RSA public key
        /// </summary>
        /// <param name="dataToEncrypt">Data to encrypt</param>
        /// <returns>Returns RSA encrypted string</returns>
        Task<string> EncryptUsingPublicKey(string dataToEncrypt);

        /// <summary>
        /// Decrypt data using RSA private key
        /// </summary>
        /// <param name="encryptedText">Encrypted text</param>
        /// <returns>Returns RSA decrypted string</returns>
        Task<string> DecryptUsingPrivateKey(string encryptedText);
    }
}
