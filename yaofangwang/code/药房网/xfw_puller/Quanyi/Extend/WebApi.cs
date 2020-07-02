using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Quanyi.Extend
{
    public class WebApi : IDisposable
    {
        //到时候一起写到配置文件，从配置文件读取信息，加载，得到
        private string _baseAddress = ConfigurationManager.AppSettings["YfwApiBaseAddress"];
        private string _apiKey = ConfigurationManager.AppSettings["YfwApiKey"];
        private string _apiSecret = ConfigurationManager.AppSettings["YfwApiSecret"];
        private string _version = ConfigurationManager.AppSettings["YfwVersion"];
        private string _format = ConfigurationManager.AppSettings["YfwFormat"];
        private string _signMethod = ConfigurationManager.AppSettings["YfwSignMethod"];
        private Encoding ContentEncoding { get; set; } = Encoding.UTF8;
        private string ContentType { get; set; } = "application/x-www-form-urlencoded";
        private SignHelper _signHelper;
        /// <summary>
        /// 解决在某些电脑上HTTPS证书验证无效的回调
        /// </summary>
        private RemoteCertificateValidationCallback certificateValidationCallback;

        public WebApi()
        {
            //string YfwApiBaseAddress = ConfigurationManager.AppSettings["YfwApiBaseAddress"];
            _signHelper = new SignHelper();
        }

        /// <summary>
        /// 创建http请求
        /// </summary>
        /// <param name="apiName">接口名称</param>
        /// <param name="appParamList">应用参数</param>
        /// <param name="method">应用参数</param>
        /// <returns></returns>
        public string CreateHttpRequest(string apiName, Dictionary<string, string> appParamList, string method = "")
        {
            string responseContent = string.Empty;
            //系统参数
            var sysParamList = _signHelper.SetSignParam(_apiKey, apiName, _version, _format, _signMethod);
            var sign = _signHelper.ComputeSign(sysParamList, appParamList, _apiSecret, method);
            ////连接除api_secret之外所有参数
            //var jsonData = _signHelper.UnionParamExSecret(sysParamList, appParamList);
            //var json = jsonData.Append($"&sign={sign}");
            //HttpWebRequest http = null;
            //var url = Uri.EscapeUriString(_baseAddress + (json.ToString() == "" ? "" : "?") + json);
            //http = (HttpWebRequest)WebRequest.Create(url);
            //http.ContentType = ContentType;
            //if (certificateValidationCallback == null
            //    && _baseAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            //{
            //    certificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
            //    ServicePointManager.ServerCertificateValidationCallback = certificateValidationCallback;
            //    ServicePointManager.DefaultConnectionLimit = 100;
            //}
            //using (var resp = http.GetResponse())
            //{
            //    using (var sr = new StreamReader(resp.GetResponseStream(), ContentEncoding))
            //    {
            //        responseContent = sr.ReadToEnd();
            //    }
            //}
            //return responseContent;
            if (certificateValidationCallback == null
                && _baseAddress.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                certificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                ServicePointManager.ServerCertificateValidationCallback = certificateValidationCallback;
                ServicePointManager.DefaultConnectionLimit = 100;
            }
            switch (method)
            {
                case "GET":
                    //连接除api_secret之外所有参数
                    var jsonData = _signHelper.UnionParamExSecret(sysParamList, appParamList);
                    var json = jsonData.Append($"&sign={sign}");
                    var url = Uri.EscapeUriString(_baseAddress + (json.ToString() == "" ? "" : "?") + json);
                    responseContent = GetWebRequest(url);
                    break;
                case "POST":
                    responseContent = PostWebRequest(_baseAddress, sysParamList, appParamList, sign);
                    break;
            }
            return responseContent;
        }

        /// <summary>
        /// GET数据接口
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string GetWebRequest(string url)
        {
            string responseContent = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.ContentType = ContentType;
            request.Method = "GET";
            using (var response = (HttpWebResponse)request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream, ContentEncoding))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            return responseContent;
        }

        /// <summary>
        /// POST数据接口
        /// </summary>
        /// <param name="url"></param>
        /// <param name="appParamList"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public string PostWebRequest(string url, IDictionary<string, string> sysParamList, Dictionary<string, string> appParamList, string sign)
        {
            string responseContent = string.Empty;
            var sysParam = _signHelper.UnionSysParam(sysParamList);
            var appParam = new StringBuilder();
            foreach (var item in appParamList)
            {

                //appParam.Append($"&{HttpUtility.UrlEncode(item.Key, ContentEncoding)}={HttpUtility.UrlEncode(item.Value, ContentEncoding)}");
                //appParam.Append($"&{item.Key}={HttpUtility.UrlEncode(item.Value, ContentEncoding)}");
                appParam.Append($"&{item.Key}={HttpUtility.UrlEncode(item.Value.Replace("+", "%2B"))}");
                //appParam.Append($"&{item.Key}={Uri.EscapeUriString(item.Value)}");
                //appParam.Append($"&{item.Key}={item.Value}");
                //appParam.Append($"&{item.Key}={System.Text.RegularExpressions.Regex.Escape(item.Value)}");
            }
            string param = sysParam.Append(appParam).Append($"&sign={sign}").ToString();
            //byte[] byteArray = ContentEncoding.GetBytes(param);
            byte[] byteArray = ContentEncoding.GetBytes(param);
            //byte[] byteArray = Encoding.GetEncoding("gb2312").GetBytes(param);
            HttpWebRequest webReq = (HttpWebRequest)WebRequest.Create(url);
            webReq.Method = "POST";
            webReq.ContentType = ContentType;
            webReq.ContentLength = byteArray.Length;
            using (var reqStream = webReq.GetRequestStream())
            {
                //写入参数
                reqStream.Write(byteArray, 0, byteArray.Length);
            }
            using (var response = (HttpWebResponse)webReq.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream, ContentEncoding))
                    {
                        responseContent = reader.ReadToEnd();
                    }
                }
            }
            return responseContent;
        }

        /// <summary>
        /// 特殊字符串转义十六进制对应值，防止XSS攻击，用于特殊字符正常显示并验证签名
        /// </summary>
        /// <param name="enStr"></param>
        private string StringFilter(string enStr)
        {
            var s = "";
            if (string.IsNullOrEmpty(enStr))
            {
                return "";
            }
            s = enStr.Replace("+", "%2B");
            //实际上/不能转换，目前只转换+
            //s = s.Replace("/", "%2F");
            s = s.Replace("?", "%3F");
            s = s.Replace("%", "%25");
            s = s.Replace("#", "%25");
            s = s.Replace("&", "%26");
            return s;
        }

        public void Test()
        {
            var info = "product_info={\"primary_key\":\"666666\",\"authorizedcode\":\"Z20025430\",\"namecn\":\"namecn123\",\"aliascn\":\"aliascn123\",\"trochetype\":\"\",\"standard\":\"40gx6/\",\"milltitle\":\" \",\"product_number\":\"\",\"weight\":30,\"product_barcode\":\"6920980000000\",\"price\":6.50,\"reserve \":0,\"max_buy_quantity\":null,\"send_day\":0,\"status_id\":1}";
            var parms =
                "api_key=af46652d4d005c052d1fca3f4362a06a&method=api.set.medicine.update" +
                "&format=json&version=1.0&sign_method=md5&timestamp={0:yyyy-MM-dd HH:mm:ss}";
            parms = string.Format(parms, DateTime.Now);
            parms = parms + "&" + info; //业务参数可在url参数中任意位置
                                        //var sign = Utils.GetMd5(parms + '&' + info + "&api_secret=208cb86c3a9d20c454e26eefa4ce9314").ToLower();
            var sign = Utils.GetMd5(parms + "&api_secret=208cb86c3a9d20c454e26eefa4ce9314").ToLower();
            var url = "https://api.yaofangwang.com/api_gateway.ashx?" + parms + "&sign=" + sign;
            url = Uri.EscapeUriString(url);
            HttpWebRequest http = null;
            http = (HttpWebRequest)WebRequest.Create(url);
            WebResponse resp = null;
            StreamReader sr = null;
            resp = http.GetResponse();
            sr = new StreamReader(resp.GetResponseStream(), ContentEncoding);
            var result = sr.ReadToEnd();
            sr?.Dispose();
            resp?.Close();
        }

        /// <summary>
        /// 创建get方式的http请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public string GetJson(string url, string json)
        {
            var http = (HttpWebRequest)WebRequest.Create(url + (json == "" ? "" : "?") + json);
            http.Method = "GET";
            http.ContentType = ContentType;
            if (certificateValidationCallback == null && url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                certificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                ServicePointManager.ServerCertificateValidationCallback = certificateValidationCallback;
            }
            var response = (HttpWebResponse)http.GetResponse();
            Stream req = null;
            StreamReader sr = null;
            try
            {
                req = response.GetResponseStream();
                sr = new StreamReader(req, ContentEncoding);
                return sr.ReadToEnd();
            }
            finally
            {
                req?.Dispose();
                sr?.Dispose();
            }
        }

        /// <summary>
        /// 创建post方式的http请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public string PostJson(string url, string json)
        {
            var buffer = ContentEncoding.GetBytes(json);
            var http = (HttpWebRequest)WebRequest.Create(url);
            http.Method = "POST";
            http.ContentType = ContentType;
            http.ContentLength = buffer.Length;
            if (certificateValidationCallback == null && url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            {
                certificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                ServicePointManager.ServerCertificateValidationCallback = certificateValidationCallback;
            }
            Stream req = null;
            WebResponse resp = null;
            StreamReader sr = null;
            try
            {
                req = http.GetRequestStream();
                req.Write(buffer, 0, buffer.Length);
                resp = http.GetResponse();
                sr = new StreamReader(resp.GetResponseStream(), ContentEncoding);
                return sr.ReadToEnd();
            }
            finally
            {
                req?.Dispose();
                sr?.Dispose();
                resp?.Close();
            }
        }

        private bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //验证服务器证书回调自动验证
            return true;
        }

        public void Dispose()
        {

        }
    }
}
