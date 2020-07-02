using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quanyi.B2c.Yaofangwang.mock;
using Quanyi.Entity.APIModel.Medicine;
using Quanyi.Entity.APIModel.Stock;
using Quanyi.Entity.DBEntity.Medicine;
using Quanyi.Entity.HttpModel;
using Quanyi.Extend;
using Quanyi.Sql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Quanyi.B2c.Yaofangwang
{
    public class B2cMedicine
    {
        private WebApiMock _webApiMock;
        private YfwWebApi _yfwWebApi;
        private MedicineInfoSql _medicineInfoSql;

        public B2cMedicine()
        {
            _webApiMock = new WebApiMock();
            _yfwWebApi = new YfwWebApi();
            _medicineInfoSql = new MedicineInfoSql();
        }

        public bool SetSwitchInfo(int type, int cbVal,out string msg)
        {
            var paramList = new List<DbParameter>();
            var sql = _medicineInfoSql.SetSwitchInfo(type, cbVal, paramList);
            return MedicineCommit(sql, out msg, paramList);
        }

        /// <summary>
        /// 待上传的商品在数据库中已存在的记录(已存在即已上传，上传过记录数据库；没传过，数据库不存在)
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public List<MedicineInfo> GetMyMedicineList(System.Data.DataTable dt)
        {
            using (var mysql = new MysqlHelper())
            {
                var list = new DbTableConvertor<MedicineInfo>().ConvertToList(dt, 0);
                var primaryKeyList = string.Join(",", list.Select(n => n.PrimaryKey).Distinct().ToArray()).Replace(",", "','");
                var sqlMedicineInfo = _medicineInfoSql.GetMedicineList(primaryKeyList);
                var dtMedicine = mysql.FillTable(sqlMedicineInfo.ToString());
                var listExists = new DbTableConvertor<MedicineInfo>().ConvertToList(dtMedicine, 0);
                return listExists;
            }
        }

        /// <summary>
        /// 只为药房网批量更新库存用
        /// </summary>
        /// <returns></returns>
        public List<MedicineInfo> GetMedicineListForStockUpdate()
        {
            using (var mysql = new MysqlHelper())
            {
                var sqlMedicine = _medicineInfoSql.GetMedicineListForStockUpdate();
                var dtMedicine = mysql.FillTable(sqlMedicine.ToString());
                var listExists = new DbTableConvertor<MedicineInfo>().ConvertToList(dtMedicine, 0);
                return listExists;
            }
        }

        public HttpResponseModal<ApiStockInfo> GetPosAvailStockInfo(List<MedicineInfo> medicineList)
        {
            using (var mysql = new MysqlHelper())
            {
                string primaryKeyList = string.Join(",", medicineList.Select(n => n.PrimaryKey).Distinct().ToList()).Replace(",", "','");
                var sqlMedicine = _medicineInfoSql.GetPosAvailStockInfo(primaryKeyList);
                var dtMedicine = mysql.FillTable(sqlMedicine.ToString());
                var listExists = new DbTableConvertor<ApiStockInfo>().ConvertToList(dtMedicine, 0);
                HttpResponseModal<ApiStockInfo> result = new HttpResponseModal<ApiStockInfo>();
                result.Items = listExists;
                result.Code = "S";
                result.Message = "成功";
                return result;
            }
        }

        //public List<MedicineInfo> GetPosAvailMedicineList()
        //{
        //    using (var mysql = new MysqlHelper())
        //    {
        //        var sqlMedicine = _medicineInfoSql.GetPosAvailMedicineList();
        //        var dtMedicine = mysql.FillTable(sqlMedicine.ToString());
        //        var listExists = new DbTableConvertor<MedicineInfo>().ConvertToList(dtMedicine, 0);
        //        return listExists;
        //    }
        //}

        public List<MedicineInfo> GetMedicineListForPeriodUpdate()
        {
            using (var mysql = new MysqlHelper())
            {
                var sqlMedicine = _medicineInfoSql.GetMedicineListForPeriodUpdate();
                var dtMedicine = mysql.FillTable(sqlMedicine.ToString());
                var listExists = new DbTableConvertor<MedicineInfo>().ConvertToList(dtMedicine, 0);
                return listExists;
            }
        }

        /// <summary>
        /// 获取商品资料表全部记录，即已上传药房网的商品记录
        /// </summary>
        /// <returns></returns>
        public List<MedicineInfo> GetAllMedicineList()
        {
            using (var mysql = new MysqlHelper())
            {
                var sqlMedicine = _medicineInfoSql.GetMedicineList();
                var dtMedicine = mysql.FillTable(sqlMedicine.ToString());
                var listExists = new DbTableConvertor<MedicineInfo>().ConvertToList(dtMedicine, 0);
                return listExists;
            }
        }

        public List<MedicineInfo> GetMedicineList(string primaryKey="")
        {
            using (var mysql = new MysqlHelper())
            {
                var sqlMedicine = _medicineInfoSql.GetMedicineList(primaryKey);
                var dtMedicine = mysql.FillTable(sqlMedicine.ToString());
                var listExists = new DbTableConvertor<MedicineInfo>().ConvertToList(dtMedicine, 0);
                return listExists;
            }
        }

        /// <summary>
        /// 商品资料导入。逐个遍历更新或插入数据库，并上传药房网
        /// </summary>
        /// <param name="dr">导入dgv当前循环行</param>
        /// <param name="medicineList">导入数据对应在数据库中存在的记录</param>
        /// <param name="dt">剩余待上传的数据集合</param>
        /// <param name="dt">当前要上传的品种对应SAP库存记录</param>
        public bool MedicineImport(System.Data.DataRow dr, List<MedicineInfo> medicineList, 
            HttpResponseModal<ApiStockInfo> newStockList)
        {
            //是否在数据库已存在
            var model = medicineList.Where(n => n.PrimaryKey.Equals(dr["SAP商品编码"].ToString())).FirstOrDefault();
            var newModel = newStockList.Items.Where(n => n.PrimaryKey.Equals(dr["SAP商品编码"].ToString())).FirstOrDefault();
            if(newModel!=null)
            {
                //已存在，更新数据库，并调用更新接口
                if (model != null)
                {
                    //并判断各项信息和数据库是否有变化，无变化则不需要更新数据库，也不需要上传药房网
                    if (model.Namecn.Equals(dr["通用名"].ToString())
                        && model.Aliascn.Equals(dr["商品名/品牌"].ToString())
                        && model.Standard.Equals(dr["规格/型号"].ToString())
                        && model.TrocheType.Equals(dr["剂型"].ToString())
                        && model.AuthorizedCode.Equals(dr["批准文号"].ToString())
                        && model.ProductBarcode.Equals(dr["条形码"].ToString())
                        && model.Milltitle.Equals(dr["生产厂家"].ToString())
                        && model.Weight.Equals(dr["重量(单位克)"].ToString())
                        && model.Price.Equals(dr["线上价"].ToString())
                        && model.MaxShelfStock.Equals(dr["最大上架库存"].ToString())
                        && model.AvailableStock.Equals(newModel.AvailableStock)
                        && model.ProduceDate.Equals(newModel.ProduceDate)
                        && model.PeriodTo.Equals(newModel.PeriodTo)
                        && model.Unit.Equals(newModel.Unit)
                        && model.ProductBatchNo.Equals(newModel.ProductBatchNo))
                    {
                        dr["操作结果"] = "成功";
                        return true;
                    }
                    model.AuthorizedCode = dr["批准文号"].ToString();
                    model.Namecn = dr["通用名"].ToString();
                    model.Aliascn = dr["商品名/品牌"].ToString();
                    model.TrocheType = dr["剂型"].ToString();
                    model.Standard = dr["规格/型号"].ToString();
                    model.Milltitle = dr["生产厂家"].ToString();
                    model.ProductNumber = dr["产品编号"].ToString();
                    model.Weight = dr["重量(单位克)"].ToString();
                    //model.Discount = dr["Discount"].ToString();
                    model.ProductBarcode = dr["条形码"].ToString();
                    //价格必须是用户导入的
                    model.Price = dr["线上价"].ToString();
                    //希望上架最大库存数量必须是用户导入的
                    model.MaxShelfStock = dr["最大上架库存"].ToString();
                    //可用库存必须是SAP接口读来的
                    model.AvailableStock = newModel.AvailableStock;
                    //库存必须是比较出来的
                    var availableStock = Convert.ToDecimal(newModel.AvailableStock);
                    //如果上传文件里该值没有填，为空，直接下面这样转化就将报错，所以，导入操作那边是否要加过滤筛选？
                    var maxShelfStock = Convert.ToDecimal(dr["最大上架库存"].ToString());
                    model.Stock = availableStock > maxShelfStock ? maxShelfStock.ToString() : availableStock.ToString();
                    //生产日期必须是SAP接口读来的
                    model.ProduceDate = newModel.ProduceDate;
                    model.MaxBuyQuantity= dr["单次购买最大数量"].ToString();
                    model.SendDay = dr["发货周期"].ToString();
                    model.StatusId = dr["商品状态"].ToString();
                    model.PeriodTo = newModel.PeriodTo;
                    model.ProduceDate = newModel.ProduceDate;
                    model.ProductBatchNo = newModel.ProductBatchNo;
                    //单位必须是SAP接口读来的
                    model.Unit = newModel.Unit;
                    //换算比率必须是用户导入的
                    //还有一些字段需要补充上去吧
                    model.ModifyTime = DateTime.Now;
                    string msg;
                    bool result = MedicineUpload(0, model, out msg);
                    if (result)
                    {
                        dr["操作结果"] = "成功";
                    }
                    else
                    {
                        dr["操作结果"] = msg;
                    }
                    return result;
                    //dr["操作结果"] = "失败";
                    //return false;
                }
                else
                {
                    var availableStock = Convert.ToDecimal(newModel.AvailableStock);
                    var maxShelfStock = Convert.ToDecimal(dr["最大上架库存"].ToString());
                    //插入数据库，并调用新增接口
                    MedicineInfo entity = new MedicineInfo()
                    {
                        PrimaryKey = dr["SAP商品编码"].ToString(),
                        AuthorizedCode = dr["批准文号"].ToString(),
                        Namecn = dr["通用名"].ToString(),
                        Aliascn = dr["商品名/品牌"].ToString(),
                        Standard = dr["规格/型号"].ToString(),
                        TrocheType = dr["剂型"].ToString(),
                        Milltitle = dr["生产厂家"].ToString(),
                        ProductNumber= dr["产品编号"].ToString(),
                        Weight = dr["重量(单位克)"].ToString(),
                        //Discount = dr["Discount"].ToString(),
                        ProductBarcode = dr["条形码"].ToString(),
                        //Category = dr["Category"].ToString(),
                        //ReceivePrice = dr["ReceivePrice"].ToString(),
                        Price = dr["线上价"].ToString(),
                        MaxShelfStock = dr["最大上架库存"].ToString(),
                        AvailableStock = newModel.AvailableStock,
                        Stock = availableStock > maxShelfStock ? maxShelfStock.ToString() : availableStock.ToString(),
                        MaxBuyQuantity = dr["单次购买最大数量"].ToString(),
                        SendDay = dr["发货周期"].ToString(),
                        StatusId = dr["商品状态"].ToString(),
                        PeriodTo = newModel.PeriodTo,
                        ProduceDate = newModel.ProduceDate,
                        ProductBatchNo = newModel.ProductBatchNo,
                        Unit = newModel.Unit,
                        //ConversionRatio = dr["ConversionRatio"].ToString(),
                        //还有一些字段也要补充上去吧
                        CreateTime = DateTime.Now
                    };
                    string msg;
                    bool result = MedicineUpload(1, entity, out msg);
                    if (result)
                    {
                        dr["操作结果"] = "成功";
                    }
                    else
                    {
                        dr["操作结果"] = msg;
                    }
                    return result;
                    //dr["操作结果"] = "失败";
                    //return false;
                }
            }
            return false;
        }

        public bool MedicinePeriodBatchUpdate(List<MedicineInfo> list, HttpResponseModal<ApiStockInfo> stockInfo, out string msg)
        {
            msg = "";
            //只是为了批量更新SAP效期等信息用。若SAP效期等信息和数据库一样，则跳过，不更新，不上传
            var medicineListUpdate = new List<MedicineInfo>();
            foreach (var item in list)
            {
                var newStockModel = stockInfo.Items.Where(n => n.PrimaryKey.Equals(item.PrimaryKey)).FirstOrDefault();
                if(newStockModel == null)
                {
                    continue;
                }
                if(newStockModel.PeriodTo == item.PeriodTo
                    && newStockModel.ProduceDate == item.ProduceDate
                    && newStockModel.ProductBatchNo == item.ProductBatchNo)
                {
                    continue;
                }
                medicineListUpdate.Add(new MedicineInfo()
                {
                    PrimaryKey = item.PrimaryKey,
                    AuthorizedCode = item.AuthorizedCode,
                    Namecn = item.Namecn,
                    Aliascn = item.Aliascn,
                    TrocheType = item.TrocheType,
                    Standard = item.Standard,
                    Milltitle = item.Milltitle,
                    ProductNumber = item.ProductNumber,
                    Weight = item.Weight,
                    ProductBarcode = item.ProductBarcode,
                    Price = item.Price,
                    Stock = item.Stock,
                    MaxBuyQuantity = item.MaxBuyQuantity,
                    SendDay = item.SendDay,
                    StatusId = item.StatusId,
                    PeriodTo = newStockModel.PeriodTo,
                    ProduceDate = newStockModel.ProduceDate,
                    ProductBatchNo = newStockModel.ProductBatchNo
                });
            }
            if(medicineListUpdate.Count==0)
            {
                return true;
            }
            var apiMedicineList = new List<ApiMedicineInfo>();
            medicineListUpdate.ForEach(item =>
            {
                //apiMedicineList.Add(new ApiMedicineInfo()
                //{
                //    //+这种特殊字符在webapi传输的时候不被识别，因此需要转换成对应的16进制值(md5)
                //    primary_key = item.PrimaryKey.Replace("+", "%2B"),
                //    authorizedcode = item.AuthorizedCode.Replace("+", "%2B"),
                //    namecn = item.Namecn.Replace("+", "%2B"),
                //    aliascn = item.Aliascn.Replace("+", "%2B"),
                //    trochetype = item.TrocheType.Replace("+", "%2B"),
                //    standard = item.Standard.Replace("+", "%2B"),
                //    milltitle = item.Milltitle.Replace("+", "%2B"),
                //    product_number = item.ProductNumber.Replace("+", "%2B"),
                //    weight = item.Weight.Replace("+", "%2B"),
                //    product_barcode = item.ProductBarcode.Replace("+", "%2B"),
                //    price = item.Price.Replace("+", "%2B"),
                //    reserve = item.Stock.Replace("+", "%2B"),
                //    max_buy_quantity = item.MaxBuyQuantity.Replace("+", "%2B"),
                //    send_day = item.SendDay.Replace("+", "%2B"),
                //    status_id = item.StatusId.Replace("+", "%2B"),
                //    //有效期和生产日期从SAP传来，有可能为空
                //    period_to = item.PeriodTo?.Replace("+", "%2B"),
                //    produce_date = item.ProduceDate?.Replace("+", "%2B"),
                //    product_batchno = item.ProductBatchNo.Replace("+", "%2B")
                //});
                apiMedicineList.Add(new ApiMedicineInfo()
                {
                    //+这种特殊字符在webapi传输的时候不被识别，因此需要转换成对应的16进制值(md5)
                    primary_key = item.PrimaryKey,
                    authorizedcode = item.AuthorizedCode,
                    namecn = item.Namecn,
                    aliascn = item.Aliascn,
                    trochetype = item.TrocheType,
                    standard = item.Standard,
                    milltitle = item.Milltitle,
                    product_number = item.ProductNumber,
                    weight = item.Weight,
                    product_barcode = item.ProductBarcode,
                    price = item.Price,
                    reserve = item.Stock,
                    max_buy_quantity = item.MaxBuyQuantity,
                    send_day = item.SendDay,
                    status_id = item.StatusId,
                    //有效期和生产日期从SAP传来，有可能为空
                    period_to = item.PeriodTo,
                    produce_date = item.ProduceDate,
                    product_batchno = item.ProductBatchNo
                });
            });
            var model = new ApiMedicineBatchInfo()
            {
                items = apiMedicineList
            };
            //实体转json字符串
            var json = JsonConvert.SerializeObject(model);
            //定义应用参数
            var appParamList = new Dictionary<string, string>();
            appParamList.Add("product_info", json);
            var responseData = _yfwWebApi.YfwApiRequest("api.set.medicine.update.batch", appParamList, "POST");
            //错误
            if (responseData["result"].ToString().Equals("0"))
            {
                //调用药房网接口就失败来了，记录原因
                msg = responseData["message"].ToString();
                return false;
            }
            //调用药房网接口失败的消息是否要记录数据库？
            var sqlMedicine = _medicineInfoSql.UpdateMedicineForSap(medicineListUpdate);
            return MedicineCommit(sqlMedicine, out msg);
        }

        public bool MedicineStockBatchUpdate(List<MedicineInfo> list, HttpResponseModal<ApiStockInfo> stockInfo, out string msg)
        {
            list.ForEach(item => 
            {
                var newStockModel = stockInfo.Items.Where(n => n.PrimaryKey.Equals(item.PrimaryKey)).FirstOrDefault();
                if(newStockModel!=null)
                {
                    item.AvailableStock = newStockModel.AvailableStock;
                    var availableStock = Convert.ToDecimal(newStockModel.AvailableStock);
                    //如果上传文件里该值没有填，为空，直接下面这样转化就将报错，所以，导入操作那边是否要加过滤筛选？
                    var maxShelfStock = Convert.ToDecimal(item.MaxShelfStock);
                    item.Stock = availableStock > maxShelfStock ? maxShelfStock.ToString() : availableStock.ToString();
                }
            });
            var apiStockList = new List<ApiStockBatchInfo>();
            list.ForEach(item => 
            {
                apiStockList.Add(new ApiStockBatchInfo()
                {
                    primary_key = item.PrimaryKey,
                    reserve = item.Stock
                });
            });
            ApiStockModel model = new ApiStockModel()
            {
                items = apiStockList
            };
            //实体转json字符串
            var json = JsonConvert.SerializeObject(model);
            //定义应用参数
            var appParamList = new Dictionary<string, string>();
            appParamList.Add("product_info", json);
            var responseData = _yfwWebApi.YfwApiRequest("api.set.medicine.update.reserve.batch", appParamList, "POST");
            //错误
            if (responseData["result"].ToString().Equals("0"))
            {
                //调用药房网接口就失败来了，记录原因
                msg = responseData["message"].ToString();
                return false;
            }
            //调用药房网接口失败的消息是否要记录数据库？
            var sqlMedicine = _medicineInfoSql.UpdateMedicineForStock(list);
            return MedicineCommit(sqlMedicine, out msg);
        }

        /// <summary>
        /// 商品资料表记录定时更新库存等信息
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="stockInfo"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool MedicinePeriodUpdate(MedicineInfo entity, HttpResponseModal<ApiStockInfo> stockInfo, out string msg)
        {
            msg = "";
            var sapModel = stockInfo.Items.Where(n => n.PrimaryKey.Equals(entity.PrimaryKey)).FirstOrDefault();
            if (sapModel != null)
            {
                ////判断新的可用库存、效期、生产日期、批次和数据库上一次记录是否相等，相等则不同步药房网
                //if(sapModel.AvailableStock == entity.AvailableStock
                //    && sapModel.PeriodTo == entity.PeriodTo
                //    && sapModel.ProduceDate == entity.ProduceDate
                //    && sapModel.ProductBatchNo == entity.ProductBatchNo)
                //{
                //    return true;
                //}
                //库存必须是比较出来的
                var availableStock = Convert.ToDecimal(sapModel.AvailableStock);
                //如果上传文件里该值没有填，为空，直接下面这样转化就将报错，所以，导入操作那边是否要加过滤筛选？
                var maxShelfStock = Convert.ToDecimal(entity.MaxShelfStock);
                entity.Stock = availableStock > maxShelfStock ? maxShelfStock.ToString() : availableStock.ToString();
                entity.AvailableStock = sapModel.AvailableStock;
                entity.PeriodTo = sapModel.PeriodTo;
                entity.ProduceDate = sapModel.ProduceDate;
                entity.ProductBatchNo = sapModel.ProductBatchNo;
                return MedicineUpload(0, entity, out msg);
            }
            return false;
        }

        /// <summary>
        /// 商品上架、下架、删除
        /// </summary>
        /// <param name="type">0-上架 1-下架 2-删除</param>
        /// <param name="entity"></param>
        /// <param name="msg"></param>
        public void MedicineUpdate(int type, MedicineInfo entity, out string msg)
        {
            Dictionary<string, string> appParamList = new Dictionary<string, string>();
            if(type!=2)
            {
                //var apiMedicineInfo = new ApiMedicineInfo()
                //{
                //    //+这种特殊字符在webapi传输的时候不被识别，因此需要转换成对应的16进制值(md5)
                //    primary_key = entity.PrimaryKey.Replace("+", "%2B"),
                //    authorizedcode = entity.AuthorizedCode.Replace("+", "%2B"),
                //    namecn = entity.Namecn.Replace("+", "%2B"),
                //    aliascn = entity.Aliascn.Replace("+", "%2B"),
                //    trochetype = entity.TrocheType.Replace("+", "%2B"),
                //    standard = entity.Standard.Replace("+", "%2B"),
                //    milltitle = entity.Milltitle.Replace("+", "%2B"),
                //    product_number = entity.ProductNumber.Replace("+", "%2B"),
                //    weight = entity.Weight.Replace("+", "%2B"),
                //    product_barcode = entity.ProductBarcode.Replace("+", "%2B"),
                //    price = entity.Price.Replace("+", "%2B"),
                //    reserve = entity.Stock.Replace("+", "%2B"),
                //    max_buy_quantity = entity.MaxBuyQuantity.Replace("+", "%2B"),
                //    send_day = entity.SendDay.Replace("+", "%2B"),
                //    status_id = entity.StatusId.Replace("+", "%2B"),
                //    //有效期和生产日期从SAP传来，有可能为空
                //    period_to = entity.PeriodTo?.Replace("+", "%2B"),
                //    produce_date = entity.ProduceDate?.Replace("+", "%2B"),
                //    product_batchno = entity.ProductBatchNo.Replace("+", "%2B")
                //};
                var apiMedicineInfo = new ApiMedicineInfo()
                {
                    //+这种特殊字符在webapi传输的时候不被识别，因此需要转换成对应的16进制值(md5)
                    primary_key = entity.PrimaryKey,
                    authorizedcode = entity.AuthorizedCode,
                    namecn = entity.Namecn,
                    aliascn = entity.Aliascn,
                    trochetype = entity.TrocheType,
                    standard = entity.Standard,
                    milltitle = entity.Milltitle,
                    product_number = entity.ProductNumber,
                    weight = entity.Weight,
                    product_barcode = entity.ProductBarcode,
                    price = entity.Price,
                    reserve = entity.Stock,
                    max_buy_quantity = entity.MaxBuyQuantity,
                    send_day = entity.SendDay,
                    status_id = entity.StatusId,
                    //有效期和生产日期从SAP传来，有可能为空
                    period_to = entity.PeriodTo,
                    produce_date = entity.ProduceDate,
                    product_batchno = entity.ProductBatchNo
                };
                var json = JsonConvert.SerializeObject(apiMedicineInfo);
                appParamList.Add("product_info", json);
            }
            else
            {
                appParamList.Add("primary_key", entity.PrimaryKey);
            }
            var responseData = _yfwWebApi.YfwApiRequest(type == 2 ? "api.set.medicine.delete" : "api.set.medicine.update", appParamList, "POST");
            if (responseData["result"].ToString().Equals("0"))
            {
                msg = responseData["message"].ToString();
                return;
            }
            var sqlMedicine = _medicineInfoSql.MedicineUpdate(type, entity);
            MedicineCommit(sqlMedicine, out msg);
        }

        /// <summary>
        /// 商品信息的上传，包括新增数据库和上传药房网
        /// </summary>
        /// <param name="type">0-更新、1-新增</param>
        /// <param name="entity"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool MedicineUpload(int type, MedicineInfo entity, out string msg)
        {
            #region 调用药房网webapi
            msg = "";
            //TO DO：调用药房网webapi
            var apiMedicineInfo = new ApiMedicineInfo()
            {
                //+这种特殊字符在webapi传输的时候不被识别，因此需要转换成对应的16进制值(md5)
                primary_key = entity.PrimaryKey,
                authorizedcode = entity.AuthorizedCode,
                namecn = entity.Namecn,
                aliascn = entity.Aliascn,
                trochetype = entity.TrocheType,
                standard = entity.Standard,
                milltitle = entity.Milltitle,
                product_number = entity.ProductNumber,
                weight = entity.Weight,
                product_barcode = entity.ProductBarcode,
                price = entity.Price,
                reserve = entity.Stock,
                max_buy_quantity = entity.MaxBuyQuantity,
                send_day = entity.SendDay,
                status_id = entity.StatusId,
                //有效期和生产日期从SAP传来，有可能为空
                period_to = entity.PeriodTo,
                produce_date = entity.ProduceDate,
                product_batchno = entity.ProductBatchNo
            };
            //var apiMedicineInfo = new ApiMedicineInfo()
            //{
            //    //+这种特殊字符在webapi传输的时候不被识别，因此需要转换成对应的16进制值(md5)
            //    primary_key = HttpUtility.UrlEncode(entity.PrimaryKey),
            //    authorizedcode = HttpUtility.UrlEncode(entity.AuthorizedCode),
            //    namecn = HttpUtility.UrlEncode(entity.Namecn),
            //    aliascn = HttpUtility.UrlEncode(entity.Aliascn),
            //    trochetype = HttpUtility.UrlEncode(entity.TrocheType),
            //    standard = HttpUtility.UrlEncode(entity.Standard),
            //    milltitle = HttpUtility.UrlEncode(entity.Milltitle),
            //    product_number = HttpUtility.UrlEncode(entity.ProductNumber),
            //    weight = HttpUtility.UrlEncode(entity.Weight),
            //    product_barcode = HttpUtility.UrlEncode(entity.ProductBarcode),
            //    price = HttpUtility.UrlEncode(entity.Price),
            //    reserve = HttpUtility.UrlEncode(entity.Stock),
            //    max_buy_quantity = HttpUtility.UrlEncode(entity.MaxBuyQuantity),
            //    send_day = HttpUtility.UrlEncode(entity.SendDay),
            //    status_id = HttpUtility.UrlEncode(entity.StatusId),
            //    //有效期和生产日期从SAP传来，有可能为空
            //    period_to = HttpUtility.UrlEncode(entity.PeriodTo),
            //    produce_date = HttpUtility.UrlEncode(entity.ProduceDate),
            //    product_batchno = HttpUtility.UrlEncode(entity.ProductBatchNo)
            //};
            //var apiMedicineInfo = new ApiMedicineInfo()
            //{
            //    //+这种特殊字符在webapi传输的时候不被识别，因此需要转换成对应的16进制值(md5)
            //    primary_key = StringFilter(entity.PrimaryKey),
            //    authorizedcode = StringFilter(entity.AuthorizedCode),
            //    namecn = StringFilter(entity.Namecn),
            //    aliascn = StringFilter(entity.Aliascn),
            //    trochetype = StringFilter(entity.TrocheType),
            //    //standard = entity.Standard.Replace("+", "%2B"),
            //    //standard = StringFilter(entity.Standard),
            //    standard = entity.Standard,
            //    milltitle = StringFilter(entity.Milltitle),
            //    product_number = StringFilter(entity.ProductNumber),
            //    weight = StringFilter(entity.Weight),
            //    product_barcode = StringFilter(entity.ProductBarcode),
            //    price = StringFilter(entity.Price),
            //    reserve = StringFilter(entity.Stock),
            //    max_buy_quantity = StringFilter(entity.MaxBuyQuantity),
            //    send_day = StringFilter(entity.SendDay),
            //    status_id = StringFilter(entity.StatusId),
            //    //有效期和生产日期从SAP传来，有可能为空
            //    period_to = StringFilter(entity.PeriodTo),
            //    produce_date = StringFilter(entity.ProduceDate),
            //    product_batchno = StringFilter(entity.ProductBatchNo)
            //};
            //实体转json字符串
            var json = JsonConvert.SerializeObject(apiMedicineInfo);
            //定义应用参数
            var appParamList = new Dictionary<string, string>();
            appParamList.Add("product_info", json);
            var responseData = _yfwWebApi.YfwApiRequest(type == 0 ? "api.set.medicine.update" : "api.set.medicine.add", appParamList, "POST");
            //错误
            if (responseData["result"].ToString().Equals("0"))
            {
                //调用药房网接口就失败来了，记录原因
                msg = responseData["message"].ToString();
                return false;
            }
            //调用药房网接口失败的消息是否要记录数据库？
            var sqlMedicine = type == 0 ? _medicineInfoSql.UpdateMedicine(entity) : _medicineInfoSql.InsertMedicine(entity);
            return MedicineCommit(sqlMedicine, out msg);
            #endregion
        }

        /// <summary>
        /// 商品资料表的增删改操作
        /// </summary>
        /// <param name="sqlMedicine"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool MedicineCommit(StringBuilder sqlMedicine, out string msg, List<DbParameter> paramList = null)
        {
            //成功则记录数据库
            using (var mysql = new MysqlHelper())
            {
                //开启事务
                var dbTran = mysql.BeginTransaction();
                try
                {
                    if(paramList == null)
                        mysql.ExecuteNonQuery(sqlMedicine.ToString());
                    else
                        mysql.ExecuteNonQuery(sqlMedicine.ToString(), paramList.ToArray());
                    //增删改一律是ExecuteNonQuery
                    mysql.Commit(dbTran);
                    dbTran.Commit();
                    msg = "ok";
                    return true;
                }
                catch (Exception ex)
                {
                    mysql.RollBack(dbTran);
                    dbTran.Rollback();
                    msg = "error:" + (ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
                    return false;
                }
                finally
                {
                    dbTran.Dispose();
                }
            }
        }

        /// <summary>
        /// 单个商品库存更新，包括更新数据库和上传药房网
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        private bool StockUpdate(MedicineInfo entity)
        {
            return true;
        }

        /// <summary>
        /// 单个商品价格更新，包括更新数据库和上传药房网
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public bool PriceUpdate(MedicineInfo entity,out string msg)
        {
            //1、调用药房网接口，上传商品价格
            //定义应用参数
            var appParamList = new Dictionary<string, string>();
            appParamList.Add("primary_key", entity.PrimaryKey);
            appParamList.Add("price", entity.Price);
            appParamList.Add("type", "1");
            var responseData = _yfwWebApi.YfwApiRequest("api.set.medicine.update.price", appParamList, "GET");
            //错误
            if (responseData["result"].ToString().Equals("0"))
            {
                //调用药房网接口就失败来了，记录原因
                msg = responseData["message"].ToString();
                return false;
            }
            var sql = _medicineInfoSql.UpdateMedicinePrice(entity);
            return MedicineCommit(sql, out msg);
        }
    }
}
