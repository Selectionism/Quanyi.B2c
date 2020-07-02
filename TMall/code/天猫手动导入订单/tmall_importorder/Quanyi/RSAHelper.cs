using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Quanyi
{
    public static class RSAHelper
    {
        /// <summary>
        /// 创建RSA公钥私钥
        /// </summary>
        public static void CreateRSAKey(string privateKeyPath, string publicKeyPath)
        {
            //创建RSA对象
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            //生成RSA[公钥私钥]
            string privateKey = rsa.ToXmlString(true);
            string publicKey = rsa.ToXmlString(false);
            //将密钥写入指定路径
            File.WriteAllText(privateKeyPath, privateKey);//文件内包含公钥和私钥
            File.WriteAllText(publicKeyPath, publicKey);//文件内只包含公钥
        }

        /// <summary>
        /// 创建RSA密钥文件
        /// </summary>
        /// <param name="path"></param>
        public static void CreateRSAKeyFile(string path)
        {
            using (var rsa = new RSACryptoServiceProvider())
            using (var sw = new StreamWriter(path))
            {
                sw.Write(rsa.ToXmlString(true));
            }
        }

        public static RSACryptoServiceProvider GetProvider(string key)
        {
            var rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(key);
            return rsa;
        }

        public static string Encrypt(string key, string value)
        {
            using (var rsa = GetProvider(key))
            {
                return Encrypt(rsa, value);
            }
        }

        public static string Encrypt(RSACryptoServiceProvider rsa, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            return Convert.ToBase64String(EncryptCore(rsa, Encoding.UTF8.GetBytes(value)));
        }

        private static byte[] EncryptCore(RSACryptoServiceProvider rsa, byte[] data)
        {
            var maxSize = rsa.KeySize / 8 - 11;//加密块最大长度限制
            //if (data.Length <= maxSize)
            //{
            //    return rsa.Encrypt(data, true);//小于maxSize的数据使用fOAEP==true来加密
            //}
            using (var ps = new MemoryStream(data))
            using (var cs = new MemoryStream())
            {
                var buffer = new byte[maxSize];
                int blockSize = ps.Read(buffer, 0, maxSize);
                while (blockSize > 0)
                {
                    byte[] toEncrypt = new byte[blockSize];//TODO:待优化，暂时申明在循环体内
                    Array.Copy(buffer, 0, toEncrypt, 0, blockSize);
                    var cryptograph = rsa.Encrypt(toEncrypt, false);
                    cs.Write(cryptograph, 0, cryptograph.Length);
                    blockSize = ps.Read(buffer, 0, maxSize);
                }
                return cs.ToArray();
            }
        }

        public static string Decrypt(string key, string value)
        {
            using (var rsa = GetProvider(key))
            {
                return Decrypt(rsa, value);
            }
        }

        public static string Decrypt(RSACryptoServiceProvider rsa, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }
            return Encoding.UTF8.GetString(DecryptCore(rsa, Convert.FromBase64String(value)));
        }

        private static byte[] DecryptCore(RSACryptoServiceProvider rsa, byte[] value)
        {
            int maxSize = rsa.KeySize / 8;//解密块最大长度限制
            //if (value.Length <= maxSize)
            //{
            //    return rsa.Decrypt(value, true);//小于maxSize的数据使用fOAEP==true来解密
            //}
            using (var cs = new MemoryStream(value))
            using (var ps = new MemoryStream())
            {
                byte[] buffer = new byte[maxSize];
                int blockSize = cs.Read(buffer, 0, maxSize);
                while (blockSize > 0)
                {
                    byte[] toDecrypt = new byte[blockSize];//TODO:待优化，暂时申明在循环体内
                    Array.Copy(buffer, 0, toDecrypt, 0, blockSize);
                    byte[] plaintext = rsa.Decrypt(toDecrypt, false);
                    ps.Write(plaintext, 0, plaintext.Length);
                    blockSize = cs.Read(buffer, 0, maxSize);
                }
                return ps.ToArray();
            }
        }
    }
}
