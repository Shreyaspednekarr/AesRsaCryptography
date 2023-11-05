namespace AesRsaCryptography.Business.Interface
{
    /// <summary>
    /// Interface to declare AES related methods
    /// </summary>
    public interface IAesService
    {
        /// <summary>
        /// Generate random AES key to encrypt the data
        /// </summary>
        /// <param name="keyLengthInBits">Key length in bits (should be multiples of 8)</param>
        /// <returns>Return randomly generated AES key</returns>
        Task<string> GenerateRandomAesKey(int keyLengthInBits);

        /// <summary>
        /// Encrypt data using AES key
        /// </summary>
        /// <param name="dataAsJsonString">Data to be encrypted in a json string</param>
        /// <param name="aesKey">AES key to encrypt the data</param>
        /// <returns>Returns AES encrypted string</returns>
        Task<string> Encrypt(string dataAsJsonString, string aesKey);

        /// <summary>
        /// Decrypt data using AES key
        /// </summary>
        /// <param name="encryptedString">Encrypted string to be decrypted</param>
        /// <param name="aesKey">AES key to decrypt the data</param>
        /// <returns>Returns AES decrypted string</returns>
        Task<string> Decrypt(string encryptedString, string aesKey);
    }
}
