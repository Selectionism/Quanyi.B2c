using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;

namespace Quanyi
{
    public static class Utils
    {
        /// <summary>
        /// 获得系统当前路径，路径最后包含\
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentDirectory()
        {
            return AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
        }

        /// <summary>
        /// 删除StringBuilder中的最后一个字符
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        public static StringBuilder RemoveLastChar(this StringBuilder sb, char c)
        {
            var last = sb.Length - 1;
            if (last >= 0 && sb[last] == c)
            {
                sb.Remove(last, 1);
            }
            return sb;
        }

        private static readonly char[] char_constant =
            {'0','1','2','3','4','5','6','7','8','9',
             'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
             'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z'};

        /// <summary>
        /// 生成一个随机字符串
        /// </summary>
        /// <param name="size">字符串长度</param>
        /// <returns></returns>
        public static string GetRandomString(int size)
        {
            StringBuilder sb = new StringBuilder(size);
            Random rd = new Random();
            for (int i = 0; i < size; i++)
            {
                sb.Append(char_constant[rd.Next(62)]);
            }
            return sb.ToString();
        }

        public static string GetMd5(string encypStr)
        {
            using (var m5 = new MD5CryptoServiceProvider())
            {
                var bytes = m5.ComputeHash(Encoding.UTF8.GetBytes(encypStr));
                return BytesToHex(bytes);
            }
        }

        public static string BytesToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(32);
            foreach (var item in bytes)
            {
                sb.Append(item.ToString("X2"));
            }
            return sb.ToString();
        }

        public static byte[] HexToBytes(string str)
        {
            var bytes = new byte[str.Length / 2];
            for (int i = 0; i < str.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
            }
            return bytes;
        }

        public static bool Explorer(string dir, string arguments = "/e,/root,")
        {
            var pi = new ProcessStartInfo("explorer", arguments + dir);
            return Process.Start(pi).Handle != IntPtr.Zero;
        }

        public static Process RunAs(string fileName, string argument, string workingDirectory)
        {
            var pricipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            var hasAdministratorRight = pricipal.IsInRole(WindowsBuiltInRole.Administrator);
            var processInfo = new ProcessStartInfo();
            if (!hasAdministratorRight)
            {//此属性值指示以管理员权限运行
                processInfo.Verb = "runas";
            }
            processInfo.FileName = fileName;
            processInfo.Arguments = argument;
            processInfo.WorkingDirectory = workingDirectory;
            return Process.Start(processInfo);
        }

        public static string AesEncrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }
            var toEncryptArray = Encoding.UTF8.GetBytes(str);
            using (var rm = CreateRijndaelManaged(key))
            using (var cTransform = rm.CreateEncryptor())
            {
                var resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
        }

        private static RijndaelManaged CreateRijndaelManaged(string key)
        {
            return new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
        }
    }
}
