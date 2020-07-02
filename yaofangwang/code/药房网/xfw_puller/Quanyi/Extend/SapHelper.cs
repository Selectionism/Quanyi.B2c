using Quanyi.Entity.APIModel.Stock;
using Quanyi.Entity.HttpModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;

namespace Quanyi.Extend
{
    /// <summary>
    /// SAP接口访问类(访问SAP接口webservice，实际上也是进行API请求，因此也是继承IDisposable)
    /// </summary>
    public class SapHelper : IDisposable
    {
        ////到时候一起写到配置文件，从配置文件读取信息(我们的原则，配置信息尽量不写在程序中，否则还要取改代码，后期)
        //private string username = "JT_SOAP";
        //private string password = "123456";

        public SapHelper()
        {

        }

        /// <summary>
        /// 获取大仓品种可用库存
        /// </summary>
        /// <returns></returns>
        public HttpResponseModal<ApiStockInfo> GetAvailableStockInfo(ApiStockParam param)
        {
            HttpResponseModal<ApiStockInfo> result = new HttpResponseModal<ApiStockInfo>();
            try
            {
                Zwm_Rfc_Kykc_Stock.ZWM_RFC_KYKC zWM_RFC_KYKC = new Zwm_Rfc_Kykc_Stock.ZWM_RFC_KYKC();
                zWM_RFC_KYKC.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["SapUsername"], ConfigurationManager.AppSettings["SapPassword"]);
                Zwm_Rfc_Kykc_Stock.ZwmRfcKykc zwmRfcKykc = new Zwm_Rfc_Kykc_Stock.ZwmRfcKykc();
                zwmRfcKykc.IWerks = ConfigurationManager.AppSettings["Storage"];
                zwmRfcKykc.IMatnr=new Zwm_Rfc_Kykc_Stock.ZsmatnrCx[param.PrimaryKeyList.Count];
                for (int i = 0; i < param.PrimaryKeyList.Count; i++)
                {
                    Zwm_Rfc_Kykc_Stock.ZsmatnrCx zsmatnrCx = new Zwm_Rfc_Kykc_Stock.ZsmatnrCx()
                    {
                        Matnr = param.PrimaryKeyList[i]
                    };
                    zwmRfcKykc.IMatnr[i] = zsmatnrCx;
                }
                zwmRfcKykc.EKc = new Zwm_Rfc_Kykc_Stock.ZswmKc[] { new Zwm_Rfc_Kykc_Stock.ZswmKc() };
                Zwm_Rfc_Kykc_Stock.ZwmRfcKykcResponse response = zWM_RFC_KYKC.ZwmRfcKykc(zwmRfcKykc);
                //成功
                if("S".Equals(response?.ERetcode))
                {
                    List<ApiStockInfo> items = new List<ApiStockInfo>();
                    foreach (var item in response.EKc)
                    {
                        //其中有可能多返回给我，过滤掉为空的
                        if(!string.IsNullOrEmpty(item.Matnr))
                        {
                            string PeriodTo = null;
                            string ProduceDate = null;
                            if (!string.IsNullOrEmpty(item.Vfdat) && !"0000-00-00".Equals(item.Vfdat))
                            {
                                PeriodTo = DateTime.Parse(item.Vfdat).ToString("yyyy/MM/dd");
                            }
                            if (!string.IsNullOrEmpty(item.Hsdat) && !"0000-00-00".Equals(item.Hsdat))
                            {
                                ProduceDate = DateTime.Parse(item.Hsdat).ToString("yyyy/MM/dd");
                            }
                            ApiStockInfo entity = new ApiStockInfo()
                            {
                                Storage = item.Werks,
                                PrimaryKey = item.Matnr?.TrimStart(new char[] { '0' }),
                                ProductBatchNo = item.Charg,
                                PeriodTo = PeriodTo,
                                ProduceDate = ProduceDate,
                                //库存最后要上传药房网是正整数
                                AvailableStock = ((int)item.Labst).ToString(),
                                Unit = item.Meins
                            };
                            items.Add(entity);
                        }
                    }
                    result.Items = items;
                }
                result.Code = response.ERetcode;
                result.Message = response.ERetmsg;      
            }
            catch (Exception ex)
            {
                result.Code = "E";
                result.Message = ex.InnerException != null ? ex.InnerException.ToString() : ex.Message;
            }
            finally
            {
            }
            return result;
        }

        public void Dispose()
        {

        }
    }
}
