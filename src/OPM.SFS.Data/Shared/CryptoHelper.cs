using OPM.SFS.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OPM.SFS.Core.Shared
{
    public interface ICryptoHelper
    {
        string ComputeSha256Hash(string input);
        string ComputeSha512Hash(string input);
        string Decrypt(string input, EncryptionKeys cryptoKeys);
        string Decrypt(string input, List<GlobalConfiguration> GlobalConfigSettings);
        string Encrypt(string input, EncryptionKeys cryptoKeys);
        string Encrypt(string input, List<GlobalConfiguration> GlobalConfigSettings);
    }
    public class EncryptionKeys
    {
        public string PassPhrase { get; set; }
        public string Salt { get; set; }
        public int PasswordIterations { get; set; }
        public string InitVector { get; set; }
        public int KeySize { get; set; }
    }

    public class CryptoHelper : ICryptoHelper
    {
       public string ComputeSha256Hash(string input)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder(input.Length * 2);
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string ComputeSha512Hash(string input)
        {
            using (SHA512 sha256Hash = SHA512.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder builder = new StringBuilder(input.Length * 2);
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string Encrypt(string input, EncryptionKeys cryptoKeys)
        {
            string encrypted = "";
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(cryptoKeys.InitVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(cryptoKeys.Salt);
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);

            var password = new Rfc2898DeriveBytes(cryptoKeys.PassPhrase, saltValueBytes, cryptoKeys.PasswordIterations);
            byte[] keyBytes = password.GetBytes(cryptoKeys.KeySize / 8);

            using (Aes aesAlg = Aes.Create())
            {
                var encryptor = aesAlg.CreateEncryptor(keyBytes, initVectorBytes);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(input);
                        }
                    }
                    byte[] cipherBytes = msEncrypt.ToArray();
                    encrypted = Convert.ToBase64String(cipherBytes);
                }
            }
            return encrypted;
        }

        public string Decrypt(string input, EncryptionKeys cryptoKeys)
        {
           
            string decrypted = "";

            byte[] initVectorBytes = Encoding.ASCII.GetBytes(cryptoKeys.InitVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(cryptoKeys.Salt );           
            byte[] inputBytes = Convert.FromBase64String(input);

            var password = new Rfc2898DeriveBytes(cryptoKeys.PassPhrase, saltValueBytes, cryptoKeys.PasswordIterations);
            byte[] keyBytes = password.GetBytes(cryptoKeys.KeySize / 8);

            using (Aes aesAlg = Aes.Create())
            {                
                var decryptor = aesAlg.CreateDecryptor(keyBytes, initVectorBytes);
                using (MemoryStream msDecrypt = new MemoryStream(inputBytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            decrypted = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return decrypted;
        }		

		public string Decrypt(string input, List<GlobalConfiguration> GlobalConfigSettings)
        {
            EncryptionKeys _keys = new EncryptionKeys();
            _keys.Salt = GlobalConfigSettings.Where(m => m.Key == "Salt" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault();
            _keys.PassPhrase = GlobalConfigSettings.Where(m => m.Key == "Passphrase" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault();
            _keys.KeySize = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "Keysize" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault());
            _keys.InitVector = GlobalConfigSettings.Where(m => m.Key == "InitVect" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault();
            _keys.PasswordIterations = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "PasswordIterations" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault());
            return Decrypt(input, _keys);
        }

        public string Encrypt(string input, List<GlobalConfiguration> GlobalConfigSettings)
        {
            EncryptionKeys _keys = new EncryptionKeys();
            _keys.Salt = GlobalConfigSettings.Where(m => m.Key == "Salt" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault();
            _keys.PassPhrase = GlobalConfigSettings.Where(m => m.Key == "Passphrase" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault();
            _keys.KeySize = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "Keysize" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault());
            _keys.InitVector = GlobalConfigSettings.Where(m => m.Key == "InitVect" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault();
            _keys.PasswordIterations = Convert.ToInt32(GlobalConfigSettings.Where(m => m.Key == "PasswordIterations" && m.Type == "Encryption").Select(m => m.Value).FirstOrDefault());
            return Encrypt(input, _keys);
        }
    }
}
