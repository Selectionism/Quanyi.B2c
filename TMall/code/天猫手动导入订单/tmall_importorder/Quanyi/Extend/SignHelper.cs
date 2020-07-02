using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Quanyi.Extend
{
    /// <summary>
    /// 签名计算类
    /// </summary>
    public class SignHelper
    {
        private readonly MD5 _md5 = new MD5CryptoServiceProvider();

        /// <summary>
        /// 计算签名
        /// </summary>
        /// <param name="sysParamList">全局请求参数</param>
        /// <param name="appParamList">应用参数</param>
        /// <param name="api_secret">密钥</param>
        /// <param name="method">方法</param>
        /// <returns></returns>
        public string ComputeSign(IDictionary<string, string> sysParamList, IDictionary<string,string> appParamList, string api_secret, string method = "")
        {
            var sb = UnionParamExSecret(sysParamList, appParamList, method);
            sb.Append($"&api_secret={api_secret}");
            return GetMd5(sb.ToString());
        }

        /// <summary>
        /// 连接参数，不包含API secret
        /// </summary>
        /// <param name="sysParamList"></param>
        /// <param name="appParamList"></param>
        /// <param name="apiMethod"></param>
        /// <returns></returns>
        public StringBuilder UnionParamExSecret(IDictionary<string, string> sysParamList, IDictionary<string, string> appParamList, string apiMethod = "")
        {
            var sb = new StringBuilder();
            string api_key = string.Empty;
            string method = string.Empty;
            string version = string.Empty;
            string format = string.Empty;
            string sign_method = string.Empty;
            string timestamp = string.Empty;
            foreach (var item in sysParamList)
            {
                switch (item.Key)
                {
                    case "api_key":
                        api_key = item.Value;
                        break;
                    case "method":
                        method = item.Value;
                        break;
                    case "version":
                        version = item.Value;
                        break;
                    case "format":
                        format = item.Value;
                        break;
                    case "sign_method":
                        sign_method = item.Value;
                        break;
                    case "timestamp":
                        timestamp = item.Value;
                        break;
                }
            }
            var sbApp = new StringBuilder();
            foreach (var item in appParamList)
            {
                //sbApp.Append($"&{HttpUtility.UrlEncode(item.Key, Encoding.UTF8)}={HttpUtility.UrlEncode(item.Value, Encoding.UTF8)}");
                //sbApp.Append($"&{HttpUtility.UrlEncode(item.Key, Encoding.GetEncoding("gb2312"))}={HttpUtility.UrlEncode(item.Value, Encoding.GetEncoding("gb2312"))}");
                //if (apiMethod.Equals("POST"))
                //{
                //    sbApp.Append($"&{item.Key}={HttpUtility.UrlEncode(item.Value, Encoding.UTF8)}");
                //    continue;
                //}
                sbApp.Append($"&{item.Key}={item.Value}");
            }
            sb.Append($@"api_key={api_key}&method={method}&format={format}&version={version}&sign_method={sign_method}&timestamp={timestamp}{sbApp}");
            return sb;
        }

        public StringBuilder UnionSysParam(IDictionary<string, string> sysParamList)
        {
            var sb = new StringBuilder();
            string api_key = string.Empty;
            string method = string.Empty;
            string version = string.Empty;
            string format = string.Empty;
            string sign_method = string.Empty;
            string timestamp = string.Empty;
            foreach (var item in sysParamList)
            {
                switch (item.Key)
                {
                    case "api_key":
                        api_key = item.Value;
                        break;
                    case "method":
                        method = item.Value;
                        break;
                    case "version":
                        version = item.Value;
                        break;
                    case "format":
                        format = item.Value;
                        break;
                    case "sign_method":
                        sign_method = item.Value;
                        break;
                    case "timestamp":
                        timestamp = item.Value;
                        break;
                }
            }
            //sb.Append($@"api_key={api_key}&method={method}&format={format}&version={version}&sign_method={sign_method}&timestamp={HttpUtility.UrlEncode(timestamp, Encoding.UTF8)}");
            //sb.Append($@"api_key={api_key}&method={method}&format={format}&version={version}&sign_method={sign_method}&timestamp={HttpUtility.UrlEncode(timestamp)}");
            sb.Append($@"api_key={api_key}&method={method}&format={format}&version={version}&sign_method={sign_method}&timestamp={Uri.EscapeUriString(timestamp)}");
            return sb;
        }

        /// <summary>
        /// 发送请求前设置签名所需全局参数
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="method"></param>
        /// <param name="version"></param>
        /// <param name="format"></param>
        /// <param name="signMethod"></param>
        /// <returns></returns>
        public IDictionary<string, string> SetSignParam(string apiKey, string method, string version, string format, string signMethod)
        {
            IDictionary<string, string> sysParamList = new Dictionary<string, string>();
            sysParamList.Add("api_key", apiKey);
            sysParamList.Add("method", method);
            sysParamList.Add("format", format);
            sysParamList.Add("version", version);
            sysParamList.Add("sign_method", signMethod);
            //sysParamList.Add("timestamp", GetCurrTimeStamp());
            sysParamList.Add("timestamp", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            return sysParamList;
        }

        /// <summary>
        /// 获取时间戳(这边要求的时间戳格式为yyyy-MM-dd HH:mm:ss)
        /// </summary>
        /// <returns></returns>
        private string GetCurrTimeStamp()
        {
            var ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalMilliseconds).ToString();
        }

        public string GetMd5(string encypStr)
        {
            using (var m5 = new MD5CryptoServiceProvider())
            {
                var bytes = m5.ComputeHash(Encoding.UTF8.GetBytes(encypStr));
                //var bytes = m5.ComputeHash(Encoding.GetEncoding("gb2312").GetBytes(encypStr));
                return BytesToHex(bytes);
            }
        }

        public string BytesToHex(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder(32);
            foreach (var item in bytes)
            {
                sb.Append(item.ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="originalString"></param>
        /// <returns></returns>
        private string ComputeHash(string originalString)
        {
            var fromData = Encoding.Default.GetBytes(originalString);
            var targetData = _md5.ComputeHash(fromData);
            var sb = new StringBuilder();
            foreach (var item in targetData)
            {
                sb.Append(item.ToString("x"));
            }
            return sb.ToString();
        }
    }
}
