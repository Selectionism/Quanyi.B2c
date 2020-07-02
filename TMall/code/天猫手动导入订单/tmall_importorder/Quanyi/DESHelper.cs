using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Quanyi
{
    public static class DESHelper
    {
        public static string Encrypt(byte[] rgbKey, byte[] rgbIV, string data)
        {
            using (var cryptoProvider = new DESCryptoServiceProvider())
            using (var ms = new MemoryStream())
            using (var cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cst))
            {
                sw.Write(data);
                sw.Flush();
                cst.FlushFinalBlock();
                sw.Flush();
                return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
            }
        }

        public static string Decrypt(byte[] rgbKey, byte[] rgbIV, string data)
        {
            var bytes = Convert.FromBase64String(data);
            using (var cryptoProvider = new DESCryptoServiceProvider())
            using (var ms = new MemoryStream(bytes))
            using (var cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Read))
            using (var sr = new StreamReader(cst))
            {
                return sr.ReadToEnd();
            }
        }
    }
}
