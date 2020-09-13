using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Quanyi.B2c.Yaofangwang.mock;
using Quanyi.B2c.Yaofangwang.ViewModel.Order;
using Quanyi.Entity.APIModel.Medicine;
using Quanyi.Entity.APIModel.Order;
using Quanyi.Entity.DBEntity.Medicine;
using Quanyi.Entity.DBEntity.Order;
using Quanyi.Entity.HttpModel;
using Quanyi.Extend;
using Quanyi.Sql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quanyi.B2c.Yaofangwang
{
    public class B2cPuller
    {
        private WebApiMock _webApiMock;
        private YfwWebApi _yfwWebApi;
        private BillInfoSql _billInfoSql;

        public B2cPuller()
        {
            _webApiMock = new WebApiMock();
            _yfwWebApi = new YfwWebApi();
            _billInfoSql = new BillInfoSql();
        }

        /// <summary>
        /// 上传订单Excel数据，写入数据库
        /// </summary>
        /// <param name="dtOrder"></param>
        /// <param name="dtOrderDetail"></param>
        public void UploadBillExcelData(DataTable dtOrder, DataTable dtOrderDetail)
        {
            if (null == dtOrder || dtOrder.Rows.Count <= 0)
            {
                return;
            }
            if (null == dtOrderDetail || dtOrderDetail.Rows.Count <= 0)
            {
                return;
            }
            //注：DataTable抬头都是中文列，需确认，固定抬头列，固定名称！
            var sqlOrder = new StringBuilder();
            var paramOrderList = new List<DbParameter>();
            var sqlOrderDetail = new StringBuilder();
            var paramOrderDetailList = new List<DbParameter>();
            _billInfoSql.InsertBill(dtOrder, sqlOrder, paramOrderList);
            _billInfoSql.InsertBillPro(dtOrderDetail, sqlOrderDetail, paramOrderDetailList);
            //成功则记录数据库
            using (var mysql = new MysqlHelper())
            {
                //开启事务
                var dbTran = mysql.BeginTransaction();
                try
                {
                    mysql.ExecuteNonQuery(sqlOrder.ToString(), paramOrderList.ToArray());
                    mysql.ExecuteNonQuery(sqlOrderDetail.ToString(), paramOrderDetailList.ToArray());
                    //增删改一律是ExecuteNonQuery
                    mysql.Commit(dbTran);
                    dbTran.Commit();
                }
                catch (Exception ex)
                {
                    mysql.RollBack(dbTran);
                    dbTran.Rollback();
                    throw new Exception(ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
                }
                finally
                {
                    //先关闭transaction，外面using再关闭connection
                    dbTran.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取订单列表(包含根据订单号获取订单详情信息)
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="IsFirstOpened">
        /// true-第一次打开，同步最后一次日期到当前时间为止期间的所有订单；
        /// false-非第一次打开，则同步定时job设置的间隔时间之内的。
        /// </param>
        /// <returns></returns>
        public BindingList<BillInfo> PullBills(out string msg, bool IsFirstOpened)
        {
            msg = "";
            DateTime? beginDate = GetBillMaxOrderTime();
            //if(IsFirstOpened)
            //{
            //    beginDate = GetBillMaxCreateTime();
            //}
            //每隔120秒订单同步程序运行一次
            var orderStartDate = DateTime.Now.AddSeconds(-120).ToString("yyyy-MM-dd HH:mm:ss");
            var orderEndDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //第一次打开，同时数据库有记录，同步期间所有订单
            //疑问：门店电脑会关，但是有的关，有的不关，因此第一次打开逻辑，就不适用了，
            //特别是数据库那台电脑关的话，肯定会影响到单据同步的，因此，我的建议是，改造现有系统吧，
            //至于门店电脑关不关，左右不了，让他们保持现状吧
            //if(IsFirstOpened && beginDate!=null)
            //{
            //    orderStartDate = beginDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            //}
            if (beginDate != null)
            {
                orderStartDate = beginDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }

            #region 测试淮安广济20号丢订单问题

            //string hgOrderStartDate = "2020/7/25 00:00:00";
            //string hgOrderEndDate = "2020/7/27 00:00:00";

            #endregion
            //定义应用参数
            var appParamList = new Dictionary<string, string>();
            appParamList.Add("order_date_start", orderStartDate);
            appParamList.Add("order_date_end", orderEndDate);
            //appParamList.Add("order_date_start", hgOrderStartDate);
            //appParamList.Add("order_date_end", hgOrderEndDate);
            #region 药房网webapi调用
            //TO DO：调用webapi，调用药房网订单列表接口
            var responseData = _yfwWebApi.YfwApiRequest("api.get.order.list", appParamList, "GET");
            //错误
            if (responseData["result"].ToString().Equals("0"))
            {
                msg = responseData["message"].ToString();
                return null;
            }
            //截取
            var mainData = JObject.Parse(responseData["body"].ToString());
            //json对象转实体对象
            var orderList = JsonConvert.DeserializeObject<List<ApiBillInfo>>(mainData["items"]?.ToString());
            //每次都获取数据库订单表已有订单
            var myOrderList = GetMyOrderList(orderList);
            //遍历已有集合，剔除已有的，保留新增的，即差集
            var newOrderList = new BindingList<BillInfo>();
            foreach (var item in orderList)
            {
                //若数据库中不存在该单号，则新增
                if (!myOrderList.Exists(n => n.OrderNo.Equals(item.order_no)))
                {
                    //新增订单，需调用订单详情接口
                    var appDetailParamList = new Dictionary<string, string>();
                    appDetailParamList.Add("order_no", item.order_no);
                    #region 药房网webapi调用
                    //调用订单详情接口(TO DO：调用webapi，调用药房网订单详情接口)
                    var responseDetailData = _yfwWebApi.YfwApiRequest("api.get.order.detail", appDetailParamList, "GET");
                    //错误
                    if (responseDetailData["result"].ToString().Equals("0"))
                    {
                        msg = responseDetailData["message"].ToString();
                        //当前该订单号在订单详情接口中没有获取到信息，跳过，继续下一个
                        continue;
                    }
                    //截取
                    var detailData = JObject.Parse(responseDetailData["body"].ToString());
                    //json对象转实体对象
                    var orderJo = JObject.Parse(detailData["order"].ToString());
                    var orderEntity = JsonConvert.DeserializeObject<ApiBillInfo>(detailData["order"].ToString());
                    var medicineList = JsonConvert.DeserializeObject<List<ApiBillIProInfo>>(orderJo["medicines"].ToString());
                    var newOrderDetailList = new List<BillProInfo>();
                    medicineList.ForEach(n =>
                    {
                        newOrderDetailList.Add(new BillProInfo()
                        {
                            OrderNo = item.order_no,
                            PrimaryKey = n.primary_key,
                            ProductNumber = n.product_number,
                            Aliascn = n.aliascn,
                            Standard = n.standard,
                            TrocheType = n.trochetype,
                            MillTitle = n.milltitle,
                            ProduceNo = n.produce_no,
                            UnitPrice = n.unit_price,
                            Quantity = n.quantity,
                            Total = n.total
                        });
                    });
                    newOrderList.Add(new BillInfo()
                    {
                        OrderNo = item.order_no,
                        StatusId = item.status_id,
                        StatusName = item.status_name,
                        OrderTotal = item.order_total,
                        OrderType = item.order_type,
                        NeedAuditRx = item.need_audit_rx,
                        Details = newOrderDetailList,
                        BuyerMobile = orderEntity.buyer_mobile,
                        BuyerName = orderEntity.buyer_name,
                        BuyerPhone = orderEntity.buyer_phone,
                        ReceiverAddress = orderEntity.receiver_address,
                        ReceiverEmail = orderEntity.receiver_email,
                        ReceiverMobile = orderEntity.receiver_mobile,
                        ReceiverName = orderEntity.receiver_name,
                        ReceiverPhone = orderEntity.receiver_phone,
                        ReceiverRegion = orderEntity.receiver_region,
                        OrderTime = orderEntity.createtime,
                        IsDelete=0//默认没有删除
                    });
                    #endregion
                }
            }
            //新增订单写入数据库，错误就抛出异常，前台能够捕捉
            if (newOrderList.Count > 0)
            {
                InsertOrderInfo(newOrderList);
            }
            return newOrderList;
            #endregion
        }

        /// <summary>
        /// 定时更新订单状态
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="orderNo"></param>
        public ApiBillInfo RegularUpdateBillStatus(out string msg,string orderNo)
        {
            msg = "";
            var appDetailParamList = new Dictionary<string, string>();
            appDetailParamList.Add("order_no", orderNo);
            var responseDetailData = _yfwWebApi.YfwApiRequest("api.get.order.detail", appDetailParamList, "GET");
            if (responseDetailData["result"].ToString().Equals("0"))
            {
                msg = responseDetailData["message"].ToString();
                return null;
            }
            //截取
            var mockDetailData = JObject.Parse(responseDetailData["body"].ToString());
            var orderEntity = JsonConvert.DeserializeObject<ApiBillInfo>(mockDetailData["order"].ToString());
            //获取到新的订单状态之后，应该立马更新数据库
            var sql = _billInfoSql.UpdateBillStatus(orderNo, orderEntity.status_id, orderEntity.status_name);
            var sqlArr = new List<StringBuilder>();
            sqlArr.Add(sql);
            BillCommit(sqlArr.ToArray());
            return orderEntity;
        }

        /// <summary>
        /// 待确认的
        /// </summary>
        /// <returns></returns>
        public List<BillInfo> GetCommitingList(string storage)
        {
            using (var mysql = new MysqlHelper())
            {
                //获取订单表已有订单集合
                var sqlBillInfo = _billInfoSql.GetCommitingBillList(storage);
                var dt = mysql.FillTable(sqlBillInfo.ToString());
                var list = new DbTableConvertor<BillInfo>().ConvertToList(dt, 0);
                //获取订单明细表已有订单集合
                var sqlBillProInfo = _billInfoSql.GetCommitingBillProList();
                var dtPro = mysql.FillTable(sqlBillProInfo.ToString());
                var listPro = new DbTableConvertor<BillProInfo>().ConvertToList(dtPro, 0);
                foreach (var item in list)
                {
                    item.Details = listPro.Where(n => n.OrderNo.Equals(item.OrderNo)).ToList();
                }
                return list;
            }
        }

        public List<BillInfo> GetOrderTimeList()
        {
            using(var mysql=new MysqlHelper())
            {
                var sql = @"SELECT * FROM yfw_billinfo
WHERE OrderTime='';";
                var dt = mysql.FillTable(sql);
                var list = new DbTableConvertor<BillInfo>().ConvertToList(dt, 0);
                return list;
            }
        }

        /// <summary>
        /// 根据接口获取的订单集合，遍历订单表对应的订单集合
        /// 请货表和订单表是一张表，通过字段“是否确认”来区分二者
        /// </summary>
        /// <param name="apiBillInfoList"></param>
        /// <returns></returns>
        private List<BillInfo> GetMyOrderList(List<ApiBillInfo> apiBillInfoList)
        {
            using(var mysql=new MysqlHelper())
            {
                //获取订单表已有订单集合
                var orderNoList = string.Join(",", apiBillInfoList.Select(n => n.order_no).Distinct().ToArray()).Replace(",", "','");
                var sqlBillInfo = _billInfoSql.GetBillList(orderNoList);
                var dt = mysql.FillTable(sqlBillInfo.ToString());
                var list = new DbTableConvertor<BillInfo>().ConvertToList(dt, 0);
                //获取订单明细表已有订单集合
                var sqlBillProInfo = _billInfoSql.GetBillProList(orderNoList);
                var dtPro = mysql.FillTable(sqlBillProInfo.ToString());
                var listPro = new DbTableConvertor<BillProInfo>().ConvertToList(dtPro, 0);
                foreach (var item in list)
                {
                    item.Details = listPro.Where(n => n.OrderNo.Equals(item.OrderNo)).ToList();
                }
                return list;
            }
        }

        /// <summary>
        /// 新订单插入订单表和订单明细表
        /// </summary>
        /// <param name="orderList"></param>
        private void InsertOrderInfo(BindingList<BillInfo> orderList)
        {
            var sqlInsertList = _billInfoSql.InsertBillAndPro(orderList);
            BillCommit(sqlInsertList.ToArray());
        }

        /// <summary>
        /// 订单确认
        /// 1、更新选中订单的确认标识
        /// 2、写入请货表
        /// </summary>
        /// <param name="selectedBillList"></param>
        public void Commit(IList<DataGridViewBillInfo> selectedBillList, List<BillProInfo> billProList)
        {
            var orderNoList = string.Join(",", selectedBillList.Select(n => n.OrderNo).Distinct().ToArray()).Replace(",", "','");
            var sqlUpdateOrderInfo = _billInfoSql.UpdateBill(orderNoList);
            BillCommit(sqlUpdateOrderInfo);
        }

        /// <summary>
        /// 订单信息的增删改操作
        /// </summary>
        /// <param name="sqlList"></param>
        private void BillCommit(params StringBuilder[] sqlList)
        {
            //成功则记录数据库
            using (var mysql = new MysqlHelper())
            {
                //开启事务
                var dbTran = mysql.BeginTransaction();
                try
                {
                    foreach (var item in sqlList)
                    {
                        mysql.ExecuteNonQuery(item.ToString());
                    }
                    //增删改一律是ExecuteNonQuery
                    mysql.Commit(dbTran);
                    dbTran.Commit();
                }
                catch (Exception ex)
                {
                    mysql.RollBack(dbTran);
                    dbTran.Rollback();
                    throw new Exception(ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
                }
                finally
                {
                    //先关闭transaction，外面using再关闭connection
                    dbTran.Dispose();
                }
            }
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="selectedBillList"></param>
        public void DeleteOrder(IList<DataGridViewBillInfo> selectedBillList)
        {
            using (var mysql = new MysqlHelper())
            {
                //开启事务
                var dbTran = mysql.BeginTransaction();
                try
                {
                    var orderNoList = string.Join(",", selectedBillList.Select(n => n.OrderNo).Distinct().ToArray()).Replace(",", "','");
                    var sqlDeleteOrderInfo = _billInfoSql.DeleteBill(orderNoList);
                    mysql.ExecuteNonQuery(sqlDeleteOrderInfo.ToString());
                    //增删改一律是ExecuteNonQuery
                    mysql.Commit(dbTran);
                    dbTran.Commit();
                }
                catch (Exception ex)
                {
                    mysql.RollBack(dbTran);
                    dbTran.Rollback();
                    throw new Exception(ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
                }
                finally
                {
                    //先关闭transaction，外面using再关闭connection
                    dbTran.Dispose();
                }
            }
        }

        /// <summary>
        /// 请货信息统一写到数据库
        /// </summary>
        /// <param name="selectedBillList"></param>
        /// <param name="billProList"></param>
        public void StoreNeed(IList<DataGridViewBillInfo> selectedBillList, List<BillProInfo> billProList)
        {
            //一个事务，完成以下全部工作
            using(var mysql = new MysqlHelper())
            {
                //开启事务
                var dbTran = mysql.BeginTransaction();
                try
                {
                    var orderNoList = string.Join(",", selectedBillList.Select(n => n.OrderNo).Distinct().ToArray()).Replace(",", "','");
                    //订单表确认标识更新
                    var sqlUpdateOrderInfo = _billInfoSql.UpdateBill(orderNoList);
                    //查询system_var表
                    var sqlStoreNeedId = _billInfoSql.GetStoreNeedId();
                    //查询最大请货日期
                    var sqlMaxReceiveDate = _billInfoSql.GetReceiveDate();
                    var snp_id = mysql.ExecuteScalar(sqlStoreNeedId.ToString());
                    string snp_id_str = "";
                    DateTime new_receive_dat = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
                    var max_receive_date= mysql.ExecuteScalar(sqlMaxReceiveDate.ToString());
                    if(snp_id!= DBNull.Value && snp_id.ToString()!="")
                    {
                        snp_id_str = snp_id.ToString();
                    }
                    if(max_receive_date!=DBNull.Value && max_receive_date.ToString()!="")
                    {
                        new_receive_dat = Convert.ToDateTime(max_receive_date).AddDays(1);
                    }
                    //对订单明细按照商品编码进行请货数量汇总，同样品种合并成一项，数量求和，只保留一条记录
                    string quantity = "0";
                    var groupBillProList = billProList.GroupBy(n => n.PrimaryKey).Select(m =>
                        new BillProInfo
                        {
                            PrimaryKey = m.Key,
                            Quantity = m.Sum(x => Convert.ToDecimal(string.IsNullOrEmpty(x.Quantity) ? quantity : x.Quantity)).ToString()
                        }).ToList();
                    //请货表和日志表插入
                    var sqlStoreNeedList = _billInfoSql.InsertPleaseOrderInfo(groupBillProList, snp_id_str, new_receive_dat);
                    mysql.ExecuteNonQuery(sqlUpdateOrderInfo.ToString());
                    foreach (var item in sqlStoreNeedList)
                    {
                        mysql.ExecuteNonQuery(item.ToString());
                    }
                    //增删改一律是ExecuteNonQuery
                    mysql.Commit(dbTran);
                    dbTran.Commit();
                }
                catch (Exception ex)
                {
                    mysql.RollBack(dbTran);
                    dbTran.Rollback();
                    throw new Exception(ex.InnerException != null ? ex.InnerException.ToString() : ex.Message);
                }
                finally
                {
                    //先关闭transaction，外面using再关闭connection
                    dbTran.Dispose();
                }
            }
        }

        /// <summary>
        /// 获取订单表最大下单时间
        /// </summary>
        /// <returns></returns>
        private DateTime? GetBillMaxOrderTime()
        {
            using(var mysql=new MysqlHelper())
            {
                DateTime? beginDate = null;
                var sql = _billInfoSql.GetBillMaxOrderTime();
                var maxDate = mysql.ExecuteScalar(sql.ToString());
                if(maxDate != DBNull.Value)
                {
                    beginDate = Convert.ToDateTime(maxDate);
                }
                return beginDate;
            }
        }
    }
}
