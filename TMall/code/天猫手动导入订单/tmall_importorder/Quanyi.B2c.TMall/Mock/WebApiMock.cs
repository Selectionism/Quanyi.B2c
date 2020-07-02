using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quanyi.B2c.Yaofangwang.mock
{
    internal class WebApiMock
    {
        /// <summary>
        /// 模拟http get方法
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public string HttpGet(string json, string sign)
        {
            //TO DO：get请求在发送之前需要经过什么处理？
            return GetMockData(json, sign);
        }

        /// <summary>
        /// 模拟http post方法
        /// </summary>
        /// <param name="json"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public string HttpPost(string json, string sign)
        {
            //TO DO：post不同于get，需要经过特殊处理
            return GetMockData(json, sign);
        }

        /// <summary>
        /// 获取mock数据
        /// </summary>
        /// <param name="json"></param>
        /// <param name="sign"></param>
        /// <returns></returns>
        public string GetMockData(string json, string sign)
        {
            var rand = new Random().Next(1, 10);
            Thread.Sleep(rand * 1000);
            //if (rand == 4)
            //{
            //    throw new InvalidOperationException("随机错误");
            //}
            var resp = mockFunc[GetMethod(json)]();
            return resp.ToString();
        }

        /// <summary>
        /// 创建http请求
        /// </summary>
        /// <param name="apiName">接口名称</param>
        /// <param name="appParamList">应用参数</param>
        /// <param name="method">请求方式</param>
        /// <returns></returns>
        public JObject CreateHttpRequest(string apiName, Dictionary<string, string> appParamList, string method)
        {
            //系统参数计算...
            //签名计算...
            //除api_secret、sign之外所有参数的连接...
            //post请求方式参数的传递...
            //发送http请求
            try
            {
                //获取http请求的返回结果response
                JObject result;
                switch(apiName)
                {
                    case "api.get.order.list":
                        result=GetOrderListMockData();
                        break;
                    case "api.get.order.detail":
                        result=GetOrderDetailMockData(appParamList["order_no"]);
                        break;
                    default:
                        result=SetInfo();
                        break;
                }
                //预处理，省得上一层再解析
                if (result.Property("code") != null && result.Property("code").ToString() != "")
                {
                    if("1".Equals(result["code"]?.ToString()))
                    {
                        //包含code节点，获取数据的
                        return new JObject
                        {
                            {"result","1" },
                            {"message","成功" },
                            {"body",result }
                        };
                    }
                }
                //包含error_response节点
                if (result.Property("error_response") != null && result.Property("error_response").ToString() != "")
                {
                    var errorJo = JObject.Parse(result["error_response"]?.ToString());
                    if (errorJo.Property("code") != null && errorJo.Property("code").ToString() != "")
                    {
                        //捕捉签名等错误信息
                        var code = errorJo["code"]?.ToString(); ;
                        if (code.Equals("-999"))
                        {
                            var msg = errorJo["msg"].ToString(); ;
                            return new JObject
                            {
                                {"result","0" },
                                {"message",msg },
                                {"body","" }
                            };
                        }
                    }
                }
                //包含success_response节点，更新信息的
                if (result.Property("success_response") != null && result.Property("success_response").ToString() != "")
                {
                    var successJo = JObject.Parse(result["success_response"]?.ToString());
                    if (successJo.Property("is_success") != null && successJo.Property("is_success").ToString() != "")
                    {
                        //捕捉签名等错误信息
                        var is_success = Convert.ToBoolean(successJo["is_success"]?.ToString());
                        if (is_success)
                        {
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
                    {"message","接口返回数据解析错误" },
                    {"body","" }
                };
            }
            catch (Exception ex)
            {
                //result：0-失败 1-成功
                return new JObject
                {
                    {"result","0" },
                    {"message",ex.InnerException != null ? ex.InnerException.ToString() : ex.Message },
                    {"body","" }
                };
            }
            finally
            {
            }
        }

        /// <summary>
        /// 调用订单列表接口返回数据
        /// </summary>
        /// <returns></returns>
        private JObject GetOrderListMockData()
        {
            var rand = new Random().Next(0, 10);
            //随着取数的不同，需要时长也不同，因此这边模拟等待不同时间
            Thread.Sleep(rand * 1000);
            //也有可能一个值都没有
            if(rand==0)
            {
                return new JObject
                {
                    {
                        "error_response",new JObject
                        {
                            {"code","-999" },
                            {"msg","该时间段没有订单" }
                        }
                    }
                };
            }
            //随机数取到2，表示订单列表接口返回error
            if (rand==2)
            {
                return new JObject
                {
                    {
                        "error_response",new JObject
                        {
                            {"code","-999" },
                            {"msg","接口计算错误" }
                        }
                    }
                };
            }
            //随机数取到4，表示调用接口异常报错
            if (rand==4)
            {
                throw new InvalidOperationException("接口调用异常");
            }
            //当前订单的状态
            var statusDic = new Dictionary<int, string>
            {
                { 10, "暂未付款" },
                { 11, "等待发货" },
                { 111, "申请退款" },
                { 112, "取消申请退款" },
                { 113, "拒绝退款" },
                { 13, "暂未收货" },
                { 14, "交易完成" },
                { 15, "交易失败" },
                { 16, "正在退款" },
                { 17, "交易取消" }
            };
            var statusIdList = new List<int>() { 10, 11, 111, 112, 113, 13, 14, 15, 16, 17 };
            //其余随机数，表示订单列表返回的结果集个数
            var jArr = new JArray();
            //JArray[] jArr = new JArray[rand];
            for (int i = 0; i < rand; i++)
            {
                var jo = new JObject
                {
                    {"order_no","C70228155621300"+i },
                    {"status_id",statusIdList[i].ToString() },
                    {"status_name",statusDic[statusIdList[i]] },
                    {"order_total",(268.00+i).ToString() },
                    {"order_type","online" },
                    {"need_audit_rx","" }
                };
                jArr.Add(jo);
            }
            return new JObject
            {
                {"code",1 },
                {"total",rand },
                {"items",jArr}
            };
        }

        /// <summary>
        /// 调用订单详情接口返回数据
        /// </summary>
        /// <param name="order_no"></param>
        /// <returns></returns>
        private JObject GetOrderDetailMockData(string order_no)
        {
            var rand = new Random().Next(1, 10);
            //随着取数的不同，需要时长也不同，因此这边模拟等待不同时间
            Thread.Sleep(rand * 1000);
            //随机数取到2，表示订单列表接口返回error
            if (rand == 2)
            {
                return new JObject
                {
                    {
                        "error_response",new JObject
                        {
                            {"code","-999" },
                            {"msg","接口计算错误" }
                        }
                    }
                };
            }
            //随机数取到4，表示调用接口异常报错
            if (rand == 4)
            {
                throw new InvalidOperationException("接口调用异常");
            }
            //单个订单所有的状态变化
            var statusDic = new Dictionary<int, string>();
            statusDic.Add(10, "暂未付款");
            statusDic.Add(11, "等待发货");
            statusDic.Add(111, "申请退款");
            statusDic.Add(112, "取消申请退款");
            statusDic.Add(113, "拒绝退款");
            statusDic.Add(13, "暂未收货");
            statusDic.Add(14, "交易完成");
            statusDic.Add(15, "交易失败");
            statusDic.Add(16, "正在退款");
            statusDic.Add(17, "交易取消");
            var primaryKeyDic = new Dictionary<int, string>();
            primaryKeyDic.Add(0, "1000412");
            primaryKeyDic.Add(1, "1000429");
            primaryKeyDic.Add(2, "1000443");
            primaryKeyDic.Add(3, "1000983");
            primaryKeyDic.Add(4, "1000413");
            primaryKeyDic.Add(5, "1000428");
            primaryKeyDic.Add(6, "1000448");
            primaryKeyDic.Add(7, "1000987");
            primaryKeyDic.Add(8, "1000416");
            primaryKeyDic.Add(9, "1000425");
            var statusIdList = new List<int>() { 10, 11, 111, 112, 113, 13, 14, 15, 16, 17 };
            //药品集合
            var medicineJArr = new JArray();
            //状态变化集合
            var statusJArr = new JArray();
            for (int i = 0; i < rand; i++)
            {
                var joMedicine = new JObject
                {
                    {"primary_key",primaryKeyDic[i] },
                    {"order_medicine_id","C702071552418655-"+i },
                    {"smid","3809697"+i },
                    {"product_number","2009"+i },
                    {"authorizedcode","H61022890"+i },
                    {"namecn","" },
                    {"aliascn",$"中药阿嘎嘎嘎嘎嘎嘎啊啊啊啊哦哦哦哦噢噢噢哦哦{i}号" },
                    {"standard","20x2/" },
                    {"trochetype","" },
                    {"milltitle","" },
                    {"package_name","" },
                    {"produce_no","20160266,1"+","+i },
                    {"unit_price",(2.00+i).ToString() },
                    {"quantity","1"+i },
                    {"total",(2.00+i).ToString() },
                    {"discount",(1.00+i).ToString() },
                    {"return_quantity",(0+i).ToString() }
                };
                medicineJArr.Add(joMedicine);
            }
            var orderJo= new JObject
            {
                 {
                      "code", 1
                 },
                 {
                      "order", new JObject
                      {
                            { "order_no", order_no},
                            { "createtime", "2017-02-07 03:52:41"},
                            { "order_type", "online"},
                            { "status_id", statusIdList[rand]},
                            { "status_name", statusDic[statusIdList[rand]]},
                            { "buyer_name", "test"},
                            { "buyer_phone", "021-88888888"},
                            { "buyer_mobile", "1381765889"+rand},
                            { "receiver_name", ""},
                            { "receiver_phone", ""},
                            { "receiver_mobile", "1388888888"+rand},
                            { "receiver_email", "66666666@qq.com"},
                            { "receiver_address", "29"+rand},
                            { "receiver_region", "||"},
                            {
                                "medicines", medicineJArr
                            }
                            //订单状态变化集合？是否要添加？
                      }
                 }
            };
            return orderJo;
        }

        /// <summary>
        /// 其余更新、新增、删除等操作返回数据
        /// </summary>
        /// <returns></returns>
        private JObject SetInfo()
        {
            var rand = new Random().Next(1, 3);
            //随着取数的不同，需要时长也不同，因此这边模拟等待不同时间
            Thread.Sleep(rand * 1000);
            if (rand == 1)
            {
                return new JObject
                {
                    {
                        "error_response",new JObject
                        {
                            {"code","-999" },
                            {"msg","接口计算错误" }
                        }
                    }
                };
            }
            //随机数取到4，表示调用接口异常报错
            if (rand == 2)
            {
                throw new InvalidOperationException("接口调用异常");
            }
            return new JObject
            {
                {
                    "success_response",new JObject
                    {
                        {"is_success",true }
                    }
                }
            };
        }

        public JObject GetMockData(string apiName)
        {
            if(apiName.Equals("api.get.order.list"))
            {
                return new JObject
                {
                    {
                        "items", new JArray
                        {
                            new JObject
                            {
                                { "order_no", "C702281656215501"},
                                { "status_id", "11"},
                                { "status_name", ""},
                                { "order_total", "128.50"},
                                { "order_type", "online"},
                            }
                        }
                    },
                    {
                        "code", 1
                    },
                    {
                        "total", 2
                    }
                };
            }
            if(apiName.Equals("api.get.order.detail"))
            {
                return new JObject
                {
                    {
                        "code", 1
                    },
                    {
                        "order", new JObject
                        {
                            { "order_no", "C702281656215501"},
                            { "createtime", "2017-02-07 03:52:41"},
                            { "order_type", "online"},
                            { "status_id", "11"},
                            { "status_name", ""},
                            { "buyer_name", "test"},
                            { "buyer_phone", "021-88888888"},
                            { "buyer_mobile", "13817658893"},
                            { "receiver_name", ""},
                            { "receiver_phone", ""},
                            { "receiver_mobile", "13888888888"},
                            { "receiver_email", "66666666@qq.com"},
                            { "receiver_address", "298"},
                            { "receiver_region", "||"},
                            {
                                "medicines", new JArray
                                {
                                    new JObject
                                    {
                                        { "order_medicine_id", "C702071552418655-1"},
                                        { "smid", "3809697"},
                                        { "product_number", "2009"},
                                        { "authorizedcode", "H61022890"},
                                        { "namecn", ""},
                                        { "aliascn", ""},
                                        { "standard", "20x2/"},
                                        { "trochetype", ""},
                                        { "milltitle", ""},
                                        { "package_name", ""},
                                        { "produce_no", "20160266,1"},
                                        { "unit_price", "2.00"},
                                        { "quantity", "1"},
                                        { "total", "2.00"},
                                        { "discount", "1.00"},
                                        { "return_quantity", "0"}
                                    }
                                }
                            }
                            //订单状态变化集合？是否要添加？
                        }
                    }
                };
            }
            if(apiName.Equals("api.set.medicine.update"))
            {
                return new JObject
                {
                    {
                        "success_response", new JObject
                        {
                            { "is_success", true }
                        }
                    }
                };
            }
            if (apiName.Equals("api.set.medicine.add"))
            {
                return new JObject
                {
                    { 
                        "success_response", new JObject 
                        {                
                            { "is_success", true }                 
                        } 
                    }
                };
            }
            if (apiName.Equals("api.set.medicine.update.reserve"))
            {
                return new JObject
                {
                    {
                        "success_response", new JObject
                        {
                            { "is_success", true }
                        }
                    }
                };
            }
            return null;
        }

        /// <summary>
        /// 调用药房网接口
        /// </summary>
        /// <param name="apiName"></param>
        /// <returns></returns>
        public string GetMockJsonString(string apiName)
        {
            //返回的是一个json字符串
            var jo = GetMockData(apiName);
            //包含code节点，获取数据的
            if (jo.Property("code") != null && jo.Property("code").ToString() != "")
            {
                //返回结果集
                return "result:" + jo.ToString();
            }
            //包含error_response节点
            if (jo.Property("error_response") != null && jo.Property("error_response").ToString() != "")
            {
                var errorJo = JObject.Parse(jo["error_response"].ToString());
                if (errorJo.Property("code") != null && errorJo.Property("code").ToString() != "")
                {
                    //捕捉签名等错误信息
                    var code = errorJo["code"].ToString(); ;
                    if (code.Equals("-999"))
                    {
                        var msg = errorJo["msg"].ToString(); ;
                        return "error:" + msg;
                    }
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
                        return "ok";
                    }
                }
            }
            return "";
        }

        /// <summary>
        /// mock func映射
        /// </summary>
        private static readonly Dictionary<string, Func<JObject>> mockFunc = new Dictionary<string, Func<JObject>>()
        {
            {
                //key-value
                "api.get.order.list", () => new JObject
                {
                    {
                        "items", new JArray
                        {
                            new JObject
                            {
                                { "order_no", "C702281656215502"},
                                { "status_id", "11"},
                                { "status_name", ""},
                                { "order_total", "128.50"},
                                { "order_type", "online"},
                            }
                        }
                    },
                    {
                        "code", 1
                    },
                    {
                        "total", 2
                    }
                }
            },
            {
                "api.get.order.detail", () => new JObject
                {
                    {
                        "code", 1
                    },
                    {
                        "order", new JObject
                        {
                            { "order_no", "C702281656215502"},
                            { "createtime", "2017-02-07 03:52:41"},
                            { "order_type", "online"},
                            { "status_id", "11"},
                            { "status_name", ""},
                            { "buyer_name", "test"},
                            { "buyer_phone", "021-88888888"},
                            { "buyer_mobile", "13817658893"},
                            { "receiver_name", ""},
                            { "receiver_phone", ""},
                            { "receiver_mobile", "13888888888"},
                            { "receiver_email", "66666666@qq.com"},
                            { "receiver_address", "298"},
                            { "receiver_region", "||"},
                            {
                                "medicines", new JArray
                                {
                                    new JObject
                                    {
                                        { "order_medicine_id", "C702071552418655-1"},
                                        { "smid", "3809697"},
                                        { "product_number", "2009"},
                                        { "authorizedcode", "H61022890"},
                                        { "namecn", ""},
                                        { "aliascn", ""},
                                        { "standard", "20x2/"},
                                        { "trochetype", ""},
                                        { "milltitle", ""},
                                        { "package_name", ""},
                                        { "produce_no", "20160266,1"},
                                        { "unit_price", "2.00"},
                                        { "quantity", "1"},
                                        { "total", "2.00"},
                                        { "discount", "1.00"},
                                        { "return_quantity", "0"}
                                    }
                                }
                            }
                            //订单状态变化集合？是否要添加？
                        }
                    }
                }
            }
        };

        /// <summary>
        /// 截取字符串，获取API method
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private string GetMethod(string json)
        {
            //获取药房网接口地址中，method参数对应第一个地址
            int methodIndex = json.IndexOf("method=");
            int formatIndex = json.IndexOf("&format=");
            return json.Substring(methodIndex + 7, formatIndex - methodIndex - 7);
        }
    }
}
