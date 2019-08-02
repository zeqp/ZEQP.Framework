using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZEQP.Framework
{
    /// <summary>
    /// 加密类
    /// </summary>
    public static class EncryptProvider
    {

        #region AES

        /// <summary>
        /// Create ase key
        /// </summary>
        /// <returns></returns>
        public static (string Key, string IV) CreateAesKey()
        {
            return (StringHelper.GetRandomStr(32), StringHelper.GetRandomStr(16));
        }

        /// <summary>  
        /// AES encrypt
        /// </summary>  
        /// <param name="data">Raw data</param>  
        /// <param name="key">Key, requires 32 bits</param>  
        /// <param name="vector">IV,requires 16 bits</param>  
        /// <returns>Encrypted string</returns>  
        public static string AESEncrypt([Required]string data,
            [Required, StringLength(maximumLength: 32, MinimumLength = 32)]string key,
            [Required, StringLength(maximumLength: 16, MinimumLength = 16)]string vector)
        {
            Byte[] plainBytes = Encoding.UTF8.GetBytes(data);
            var encryptBytes = AESEncrypt(plainBytes, key, vector);
            if (encryptBytes == null) return null;
            return Convert.ToBase64String(encryptBytes);
        }

        /// <summary>
        /// AES encrypt
        /// </summary>
        /// <param name="data">Raw data</param>  
        /// <param name="key">Key, requires 32 bits</param>  
        /// <param name="vector">IV,requires 16 bits</param>  
        /// <returns>Encrypted byte array</returns>  
        public static byte[] AESEncrypt([Required]byte[] data,
            [Required, StringLength(maximumLength: 32, MinimumLength = 32)]string key,
            [Required, StringLength(maximumLength: 16, MinimumLength = 16)]string vector)
        {
            Byte[] plainBytes = data;
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            Byte[] bVector = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);
            Byte[] encryptData = null; // encrypted data
            using (Aes Aes = Aes.Create())
            {
                using (MemoryStream Memory = new MemoryStream())
                {
                    using (CryptoStream Encryptor = new CryptoStream(Memory, Aes.CreateEncryptor(bKey, bVector), CryptoStreamMode.Write))
                    {
                        Encryptor.Write(plainBytes, 0, plainBytes.Length);
                        Encryptor.FlushFinalBlock();
                        encryptData = Memory.ToArray();
                    }
                }
                return encryptData;
            }
        }

        /// <summary>  
        ///  AES decrypt
        /// </summary>  
        /// <param name="data">Encrypted data</param>  
        /// <param name="key">Key, requires 32 bits</param>  
        /// <param name="vector">IV,requires 16 bits</param>  
        /// <returns>Decrypted string</returns>  
        public static string AESDecrypt([Required]string data,
            [Required, StringLength(maximumLength: 32, MinimumLength = 32)]string key,
            [Required, StringLength(maximumLength: 16, MinimumLength = 16)]string vector)
        {
            Byte[] encryptedBytes = Convert.FromBase64String(data);
            Byte[] decryptBytes = AESDecrypt(encryptedBytes, key, vector);
            if (decryptBytes == null) return null;
            return Encoding.UTF8.GetString(decryptBytes);
        }

        /// <summary>  
        ///  AES decrypt
        /// </summary>  
        /// <param name="data">Encrypted data</param>  
        /// <param name="key">Key, requires 32 bits</param>  
        /// <param name="vector">IV,requires 16 bits</param>  
        /// <returns>Decrypted byte array</returns>  

        public static byte[] AESDecrypt([Required]byte[] data,
            [Required, StringLength(maximumLength: 32, MinimumLength = 32)]string key,
            [Required, StringLength(maximumLength: 16, MinimumLength = 16)]string vector)
        {
            Byte[] encryptedBytes = data;
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);
            Byte[] bVector = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(vector.PadRight(bVector.Length)), bVector, bVector.Length);
            Byte[] decryptedData = null; // decrypted data
            using (Aes Aes = Aes.Create())
            {
                using (MemoryStream Memory = new MemoryStream(encryptedBytes))
                {
                    using (CryptoStream Decryptor = new CryptoStream(Memory, Aes.CreateDecryptor(bKey, bVector), CryptoStreamMode.Read))
                    {
                        using (MemoryStream tempMemory = new MemoryStream())
                        {
                            Byte[] Buffer = new Byte[1024];
                            Int32 readBytes = 0;
                            while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                            {
                                tempMemory.Write(Buffer, 0, readBytes);
                            }
                            decryptedData = tempMemory.ToArray();
                        }
                    }
                }
                return decryptedData;
            }
        }

        /// <summary>  
        /// AES encrypt ( no IV)  
        /// </summary>  
        /// <param name="data">Raw data</param>  
        /// <param name="key">Key, requires 32 bits</param>  
        /// <returns>Encrypted string</returns>  
        public static string AESEncrypt([Required]string data,
            [Required, StringLength(maximumLength: 32, MinimumLength = 32)]string key)
        {
            using (MemoryStream Memory = new MemoryStream())
            {
                using (Aes aes = Aes.Create())
                {
                    byte[] plainBytes = Encoding.UTF8.GetBytes(data);
                    Byte[] bKey = new Byte[32];
                    Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.KeySize = 128;
                    //aes.Key = _key;  
                    aes.Key = bKey;
                    //aes.IV = _iV; 
                    using (CryptoStream cryptoStream = new CryptoStream(Memory, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Convert.ToBase64String(Memory.ToArray());
                    }
                }
            }
        }

        /// <summary>  
        /// AES decrypt( no IV)  
        /// </summary>  
        /// <param name="data">Encrypted data</param>  
        /// <param name="key">Key, requires 32 bits</param>  
        /// <returns>Decrypted string</returns>  
        public static string AESDecrypt([Required]string data,
            [Required, StringLength(maximumLength: 32, MinimumLength = 32)]string key)
        {
            Byte[] encryptedBytes = Convert.FromBase64String(data);
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

            using (MemoryStream Memory = new MemoryStream(encryptedBytes))
            {
                //mStream.Write( encryptedBytes, 0, encryptedBytes.Length );  
                //mStream.Seek( 0, SeekOrigin.Begin );  
                using (Aes aes = Aes.Create())
                {
                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.KeySize = 128;
                    aes.Key = bKey;
                    //aes.IV = _iV;  
                    using (CryptoStream cryptoStream = new CryptoStream(Memory, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        byte[] tmp = new byte[encryptedBytes.Length];
                        int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length);
                        byte[] ret = new byte[len];
                        Array.Copy(tmp, 0, ret, 0, len);
                        return Encoding.UTF8.GetString(ret);
                    }
                }
            }
        }
        #endregion

        #region DES

        /// <summary>
        /// Create des key
        /// </summary>
        /// <returns></returns>
        public static string CreateDesKey()
        {
            return StringHelper.GetRandomStr(24);
        }

        /// <summary>  
        /// DES encrypt
        /// </summary>  
        /// <param name="data">Raw data</param>  
        /// <param name="key">Key, requires 24 bits</param>  
        /// <returns>Encrypted string</returns>  
        public static string DESEncrypt([Required]string data,
            [Required, StringLength(maximumLength: 24, MinimumLength = 24)]string key)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(data);
            var encryptBytes = DESEncrypt(plainBytes, key);

            if (encryptBytes == null)
            {
                return null;
            }
            return Convert.ToBase64String(encryptBytes);
        }

        /// <summary>  
        /// DES encrypt
        /// </summary>  
        /// <param name="data">Raw data</param>  
        /// <param name="key">Key, requires 24 bits</param>  
        /// <returns>Encrypted byte array</returns>  
        public static byte[] DESEncrypt([Required]byte[] data, [Required, StringLength(maximumLength: 24, MinimumLength = 24)]string key)
        {
            using (MemoryStream Memory = new MemoryStream())
            {
                using (TripleDES des = TripleDES.Create())
                {
                    Byte[] plainBytes = data;
                    Byte[] bKey = new Byte[24];
                    Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

                    des.Mode = CipherMode.ECB;
                    des.Padding = PaddingMode.PKCS7;
                    des.Key = bKey;
                    using (CryptoStream cryptoStream = new CryptoStream(Memory, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        return Memory.ToArray();
                    }
                }
            }
        }

        /// <summary>  
        /// DES decrypt
        /// </summary>  
        /// <param name="data">Encrypted data</param>  
        /// <param name="key">Key, requires 24 bits</param>  
        /// <returns>Decrypted string</returns>  
        public static string DESDecrypt([Required]string data, [Required, StringLength(maximumLength: 24, MinimumLength = 24)]string key)
        {
            Byte[] encryptedBytes = Convert.FromBase64String(data);
            Byte[] bytes = DESDecrypt(encryptedBytes, key);

            if (bytes == null)
            {
                return null;
            }
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>  
        /// DES decrypt
        /// </summary>  
        /// <param name="data">Encrypted data</param>  
        /// <param name="key">Key, requires 24 bits</param>  
        /// <returns>Decrypted byte array</returns>  
        public static byte[] DESDecrypt([Required]byte[] data, [Required, StringLength(maximumLength: 24, MinimumLength = 24)]string key)
        {
            Byte[] encryptedBytes = data;
            Byte[] bKey = new Byte[24];
            Array.Copy(Encoding.UTF8.GetBytes(key.PadRight(bKey.Length)), bKey, bKey.Length);

            using (MemoryStream Memory = new MemoryStream(encryptedBytes))
            {
                using (TripleDES des = TripleDES.Create())
                {
                    des.Mode = CipherMode.ECB;
                    des.Padding = PaddingMode.PKCS7;
                    des.Key = bKey;
                    using (CryptoStream cryptoStream = new CryptoStream(Memory, des.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        try
                        {
                            byte[] tmp = new byte[encryptedBytes.Length];
                            int len = cryptoStream.Read(tmp, 0, encryptedBytes.Length);
                            byte[] ret = new byte[len];
                            Array.Copy(tmp, 0, ret, 0, len);
                            return ret;
                        }
                        catch
                        {
                            return null;
                        }
                    }
                }
            }
        }

        #endregion


        #region MD5
        /// <summary>
        /// MD5 hash
        /// </summary>
        /// <param name="srcString">The string to be encrypted.</param>
        /// <param name="length">The length of hash result , default value is <see cref="MD5Length.L32"/>.</param>
        /// <returns></returns>
        public static string Md5([Required]string srcString)
        {
            string str_md5_out = string.Empty;
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes_md5_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_md5_out = md5.ComputeHash(bytes_md5_in);

                str_md5_out = BitConverter.ToString(bytes_md5_out);

                str_md5_out = str_md5_out.Replace("-", "");
                return str_md5_out;
            }
        }
        #endregion

        #region HMACMD5
        /// <summary>
        /// HMACMD5 hash
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <param name="key">encrypte key</param>
        /// <returns></returns>
        public static string HMACMD5([Required]string srcString, [Required]string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACMD5 md5 = new HMACMD5(secrectKey))
            {
                byte[] bytes_md5_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_md5_out = md5.ComputeHash(bytes_md5_in);
                string str_md5_out = BitConverter.ToString(bytes_md5_out);
                str_md5_out = str_md5_out.Replace("-", "");
                return str_md5_out;
            }
        }

        #endregion

        #region SHA1
        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="str">The string to be encrypted</param>
        /// <returns></returns>
        public static string Sha1([Required]string str)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] bytes_sha1_in = Encoding.UTF8.GetBytes(str);
                byte[] bytes_sha1_out = sha1.ComputeHash(bytes_sha1_in);
                string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
                str_sha1_out = str_sha1_out.Replace("-", "");
                return str_sha1_out;
            }
        }
        #endregion

        #region SHA256

        /// <summary>
        /// SHA256 encrypt
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <returns></returns>
        public static string Sha256([Required]string srcString)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes_sha256_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_sha256_out = sha256.ComputeHash(bytes_sha256_in);
                string str_sha256_out = BitConverter.ToString(bytes_sha256_out);
                str_sha256_out = str_sha256_out.Replace("-", "");
                return str_sha256_out;
            }
        }

        #endregion

        #region SHA384

        /// <summary>
        /// SHA384 encrypt
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <returns></returns>
        public static string Sha384([Required]string srcString)
        {
            using (SHA384 sha384 = SHA384.Create())
            {
                byte[] bytes_sha384_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_sha384_out = sha384.ComputeHash(bytes_sha384_in);
                string str_sha384_out = BitConverter.ToString(bytes_sha384_out);
                str_sha384_out = str_sha384_out.Replace("-", "");
                return str_sha384_out;
            }

        }
        #endregion

        #region SHA512
        /// <summary>
        /// SHA512 encrypt
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <returns></returns>
        public static string Sha512([Required]string srcString)
        {
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] bytes_sha512_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_sha512_out = sha512.ComputeHash(bytes_sha512_in);
                string str_sha512_out = BitConverter.ToString(bytes_sha512_out);
                str_sha512_out = str_sha512_out.Replace("-", "");
                return str_sha512_out;
            }
        }

        #endregion

        #region HMACSHA1

        /// <summary>
        /// HMAC_SHA1
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <param name="key">encrypte key</param>
        /// <returns></returns>
        public static string HMACSHA1([Required]string srcString, [Required]string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACSHA1 hmac = new HMACSHA1(secrectKey))
            {
                hmac.Initialize();

                byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);

                string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
                str_hamc_out = str_hamc_out.Replace("-", "");

                return str_hamc_out;
            }
        }

        #endregion

        #region HMACSHA256

        /// <summary>
        /// HMAC_SHA256 
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <param name="key">encrypte key</param>
        /// <returns></returns>
        public static string HMACSHA256([Required]string srcString, [Required]string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACSHA256 hmac = new HMACSHA256(secrectKey))
            {
                hmac.Initialize();

                byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);

                string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
                str_hamc_out = str_hamc_out.Replace("-", "");

                return str_hamc_out;
            }
        }

        #endregion

        #region HMACSHA384

        /// <summary>
        /// HMAC_SHA384
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <param name="key">encrypte key</param>
        /// <returns></returns>
        public static string HMACSHA384([Required]string srcString, [Required]string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACSHA384 hmac = new HMACSHA384(secrectKey))
            {
                hmac.Initialize();

                byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);
                string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
                str_hamc_out = str_hamc_out.Replace("-", "");

                return str_hamc_out;
            }
        }

        #endregion

        #region HMACSHA512

        /// <summary>
        /// HMAC_SHA512
        /// </summary>
        /// <param name="srcString">The string to be encrypted</param>
        /// <param name="key">encrypte key</param>
        /// <returns></returns>
        public static string HMACSHA512([Required]string srcString, [Required]string key)
        {
            byte[] secrectKey = Encoding.UTF8.GetBytes(key);
            using (HMACSHA512 hmac = new HMACSHA512(secrectKey))
            {
                hmac.Initialize();

                byte[] bytes_hmac_in = Encoding.UTF8.GetBytes(srcString);
                byte[] bytes_hamc_out = hmac.ComputeHash(bytes_hmac_in);

                string str_hamc_out = BitConverter.ToString(bytes_hamc_out);
                str_hamc_out = str_hamc_out.Replace("-", "");

                return str_hamc_out;
            }
        }

        #endregion

        #region Machine Key

        /// <summary>
        /// Create decryptionKey
        /// </summary>
        /// <param name="length">decryption key length range is 16 -48</param>
        /// <returns>DecryptionKey</returns>
        public static string CreateDecryptionKey([Range(16, 48)]int length)
        {
            return CreateMachineKey(length);
        }

        /// <summary>
        /// Create validationKey
        /// </summary>
        /// <param name="length"></param>
        /// <returns>ValidationKey</returns>
        public static string CreateValidationKey([Range(48, 128)]int length)
        {
            return CreateMachineKey(length);
        }

        /// <summary>
        /// 使用加密服务提供程序实现加密生成随机数
        /// 
        /// 说明：
        /// validationKey 的值可以是48到128个字符长，强烈建议使用可用的最长密钥
        /// decryptionKey 的值可以是16到48字符长，建议使用48字符长
        /// 
        /// 使用方式：
        /// string decryptionKey = EncryptManager.CreateMachineKey(48);
        /// string validationKey = EncryptManager.CreateMachineKey(128);
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns></returns>
        private static string CreateMachineKey(int length)
        {
            byte[] random = new byte[length / 2];
            RandomNumberGenerator rng = RandomNumberGenerator.Create();
            rng.GetBytes(random);
            StringBuilder machineKey = new StringBuilder(length);
            for (int i = 0; i < random.Length; i++)
            {
                machineKey.Append(string.Format("{0:X2}", random[i]));
            }
            return machineKey.ToString();
        }

        #endregion

        #region Base64

        #region Base64加密解密
        /// <summary>
        /// Base64 encrypt
        /// </summary>
        /// <param name="input">input value</param>
        /// <returns></returns>
        public static string Base64Encrypt(string input)
        {
            return Base64Encrypt(input, Encoding.UTF8);
        }

        /// <summary>
        /// Base64 encrypt
        /// </summary>
        /// <param name="input">input value</param>
        /// <param name="encoding">text encoding</param>
        /// <returns></returns>
        public static string Base64Encrypt([Required]string input, Encoding encoding)
        {
            return Convert.ToBase64String(encoding.GetBytes(input));
        }

        /// <summary>
        /// Base64 decrypt
        /// </summary>
        /// <param name="input">input value</param>
        /// <returns></returns>
        public static string Base64Decrypt(string input)
        {
            return Base64Decrypt(input, Encoding.UTF8);
        }

        /// <summary>
        /// Base64 decrypt
        /// </summary>
        /// <param name="input">input value</param>
        /// <param name="encoding">text encoding</param>
        /// <returns></returns>
        public static string Base64Decrypt([Required]string input, Encoding encoding)
        {
            return encoding.GetString(Convert.FromBase64String(input));
        }
        #endregion

        #endregion
    }
}
