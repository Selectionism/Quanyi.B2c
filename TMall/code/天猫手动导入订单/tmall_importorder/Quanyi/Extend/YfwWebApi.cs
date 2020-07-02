using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quanyi.Extend
{
    public class YfwWebApi
    {
        /// <summary>
        /// 调用药房网接口
        /// </summary>
        /// <param name="apiName"></param>
        /// <param name="appParamList"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        public JObject YfwApiRequest(string apiName, Dictionary<string, string> appParamList, string method = "")
        {
            using (var _webApi = new WebApi())
            {
                try
                {
                    var responseData = _webApi.CreateHttpRequest(apiName, appParamList, method);
                    //返回的是一个json字符串
                    var jo = (JObject)JsonConvert.DeserializeObject(responseData);
                    //包含code节点，获取数据的
                    if (jo.Property("code") != null && jo.Property("code").ToString() != "")
                    {
                        if ("1".Equals(jo["code"]?.ToString()))
                        {
                            //包含code节点，获取数据的
                            return new JObject
                            {
                                {"result","1" },
                                {"message","成功" },
                                {"body",jo }
                            };
                        }
                    }
                    //包含error_response节点
                    if (jo.Property("error_response") != null && jo.Property("error_response").ToString() != "")
                    {
                        var errorJo = JObject.Parse(jo["error_response"].ToString());
                        if (errorJo.Property("code") != null && errorJo.Property("code").ToString() != "")
                        {
                            //捕捉签名等错误信息
                            //var code = errorJo["code"].ToString();
                            //if (code.Equals("-999"))
                            //{
                            //    var msg = errorJo["msg"].ToString();
                            //    return new JObject
                            //    {
                            //        {"result","0" },
                            //        {"message",msg },
                            //        {"body","" }
                            //    };
                            //}
                            var msg = errorJo["msg"].ToString();
                            return new JObject
                            {
                                {"result","0" },
                                {"message",msg },
                                {"body","" }
                            };
                        }
                    }
                    //包含success_response节点，更新信息的
                    if (jo.Property("success_response") != null && jo.Property("success_response").ToString() != "")
                    {
                        var successJo = JObject.Parse(jo["success_response"].ToString());
                        if (successJo.Property("is_success") != null && successJo.Property("is_success").ToString() != "")
                        {
                            //捕捉签名等错误信息
                            var is_success = Convert.ToBoolean(successJo["is_success"]);
                            if (is_success)
                            {
                                //是否要写入日志表？是否要前台界面展示？
                                return new JObject
                                {
                                    {"result","1" },
                                    {"message","成功" },
                                    {"body","" }
                                };
                            }
                        }
                    }
                    return new JObject
                    {
                        {"result","0" },
                        {"message", "失败" },
                        {"body","" }
                    };
                }
                catch (Exception ex)
                {
                    return new JObject
                    {
                        {"result","0" },
                        {"message", ex.InnerException!=null?ex.InnerException.ToString():ex.Message },
                        {"body","" }
                    };
                }
            }
        }

        //public JObject YfwMedicineDetailApiRequest(string apiName, Dictionary<string, string> appParamList, string method)
        //{
        //    using (var _webApi = new WebApi())
        //    {
        //        var responseData = _webApi.GetMedicineDetail(apiName, appParamList, method);
        //        //以error开头，表示异常，则也返回失败
        //        if (responseData.StartsWith("error:"))
        //        {
        //            return new JObject
        //            {
        //                {"result","0" },
        //                {"message", responseData.Substring(6) },
        //                {"body","" }
        //            };
        //        }
        //        //返回的是一个json字符串
        //        var jo = (JObject)JsonConvert.DeserializeObject(responseData);
        //        //包含code节点，获取数据的
        //        if (jo.Property("code") != null && jo.Property("code").ToString() != "")
        //        {
        //            if ("1".Equals(jo["code"]?.ToString()))
        //            {
        //                //包含code节点，获取数据的
        //                return new JObject
        //                {
        //                    {"result","1" },
        //                    {"message","成功" },
        //                    {"body",jo }
        //                };
        //            }
        //        }
        //        //包含error_response节点
        //        if (jo.Property("error_response") != null && jo.Property("error_response").ToString() != "")
        //        {
        //            var errorJo = JObject.Parse(jo["error_response"].ToString());
        //            if (errorJo.Property("code") != null && errorJo.Property("code").ToString() != "")
        //            {
        //                //捕捉签名等错误信息
        //                //var code = errorJo["code"].ToString();
        //                //if (code.Equals("-999"))
        //                //{
        //                //    var msg = errorJo["msg"].ToString();
        //                //    return new JObject
        //                //    {
        //                //        {"result","0" },
        //                //        {"message",msg },
        //                //        {"body","" }
        //                //    };
        //                //}
        //                var msg = errorJo["msg"].ToString();
        //                return new JObject
        //                {
        //                    {"result","0" },
        //                    {"message",msg },
        //                    {"body","" }
        //                };
        //            }
        //        }
        //        //包含success_response节点，更新信息的
        //        if (jo.Property("success_response") != null && jo.Property("success_response").ToString() != "")
        //        {
        //            var successJo = JObject.Parse(jo["success_response"].ToString());
        //            if (successJo.Property("is_success") != null && successJo.Property("is_success").ToString() != "")
        //            {
        //                //捕捉签名等错误信息
        //                var is_success = Convert.ToBoolean(successJo["is_success"]);
        //                if (is_success)
        //                {
        //                    //是否要写入日志表？是否要前台界面展示？
        //                    return new JObject
        //                    {
        //                        {"result","1" },
        //                        {"message","成功" },
        //                        {"body","" }
        //                    };
        //                }
        //            }
        //        }
        //        //都不包含，说明catch异常了，接口调用直接报错了，直接返回报错文本吧
        //        return new JObject
        //        {
        //            {"result","0" },
        //            {"message","接口返回数据解析错误" },
        //            {"body","" }
        //        };
        //    }
        //}

        //public void YfwTestApi()
        //{
        //    using (var _webApi = new WebApi())
        //    {
        //        _webApi.Test();
        //    }
        //}
    }
}
