using AesRsaCryptography.Business.Interface;
using System.Security.Cryptography;
using System.Text;

namespace AesRsaCryptography.Business.Implementation
{
    /// <inheritdoc/>
    public class AesService : IAesService
    {
        /// <inheritdoc/>
        public async Task<string> GenerateRandomAesKey(int keyLengthInBits)
        {
            if (keyLengthInBits % 8 != 0 || keyLengthInBits < 128 || keyLengthInBits > 256)
            {
                throw new ArgumentException("Invalid key length. Key length must be 128, 192 or 256 bits.");
            }

            //using Aes aes = Aes.Create();
            //aes.KeySize = keyLengthInBits;
            //aes.GenerateKey();
            //byte[] aesKeyBytes = aes.Key;
            //var aesKey = BitConverter.ToString(aesKeyBytes).Replace("-", string.Empty);

            byte[] aesKeyBytes = new byte[keyLengthInBits / 8];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(aesKeyBytes);
            }
            var aesKey = BitConverter.ToString(aesKeyBytes).Replace("-", string.Empty);
            return aesKey;
        }

        /// <inheritdoc/>
        public async Task<string> Encrypt(string dataAsJsonString, string aesKey)
        {
            byte[] array;
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(aesKey);
                aes.IV = Encoding.UTF8.GetBytes(aesKey.Substring(0, 16));
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(dataAsJsonString);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }

        /// <inheritdoc/>
        public async Task<string> Decrypt(string encryptedString, string aesKey)
        {
            byte[] buffer = Convert.FromBase64String(encryptedString);
            using (Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(aesKey);
                aes.IV = Encoding.UTF8.GetBytes(aesKey.Substring(0, 16));
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var memoryStream = new MemoryStream(buffer))
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (var streamReader = new StreamReader(cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
