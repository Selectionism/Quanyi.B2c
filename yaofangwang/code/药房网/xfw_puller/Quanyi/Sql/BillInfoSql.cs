using MySql.Data.MySqlClient;
using Quanyi.Entity.DBEntity.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Quanyi.Sql
{
    public class BillInfoSql
    {
        /// <summary>
        /// 待确认订单
        /// </summary>
        /// <param name="orderNoList"></param>
        /// <returns></returns>
        public StringBuilder GetCommitingBillList(string storage)
        {
            var sqlBillInfo = new StringBuilder();
            //否则就只请求那些还未确认的订单
            sqlBillInfo.Append($@"SELECT * FROM yfw_billinfo
WHERE IsConfirm=0
AND IsDelete=0");
            if(storage.Equals("TMall"))
            {
                sqlBillInfo.Append(@" AND Platform='TMall'");
            }
            else
            {
                sqlBillInfo.Append(@" AND Platform<>'TMall'");
            }
            return sqlBillInfo;
        }

        /// <summary>
        /// 待确认订单明细
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetCommitingBillProList()
        {
            var sqlBillProInfo = new StringBuilder();
            //否则就只请求那些还未确认的订单
            sqlBillProInfo.Append($@"SELECT t2.* FROM yfw_billinfo t1
INNER JOIN yfw_billproinfo t2 ON t2.OrderNo=t1.OrderNo
WHERE t1.IsConfirm=0;");
            return sqlBillProInfo;
        }

        /// <summary>
        /// 获取订单表已有记录
        /// </summary>
        /// <param name="orderNoList"></param>
        /// <returns></returns>
        public StringBuilder GetBillList(string orderNoList="")
        {
            var sqlBillInfo = new StringBuilder();
            sqlBillInfo.Append($@"SELECT * FROM yfw_billinfo
WHERE OrderNo IN ('{orderNoList}');");
            return sqlBillInfo;
        }

        /// <summary>
        /// 获取订单明细表已有记录
        /// </summary>
        /// <param name="orderNoList"></param>
        /// <returns></returns>
        public StringBuilder GetBillProList(string orderNoList="")
        {
            var sqlBillProInfo = new StringBuilder();
            sqlBillProInfo.Append($@"SELECT * FROM yfw_billproinfo
WHERE OrderNo IN ('{orderNoList}');");
            return sqlBillProInfo;
        }

        public void InsertBill(DataTable dt, StringBuilder sql, List<DbParameter> dbParameters)
        {
            sql.Append(@"INSERT INTO yfw_billinfo (
    OrderNo,
    #StatusId,
    StatusName,
    OrderTotal,
    #OrderType,
    #NeedAuditRx,
    #PayType,
    BuyerName,
    #BuyerPhone,
    #BuyerMobile,
    ReceiverName,
    ReceiverPhone,
    ReceiverMobile,
    #ReceiverEmail,
    ReceiverAddress,
    #ReceiverRegion,
    #IsPrescription,
    CreateTime,
    OrderTime,
    Platform
)
VALUES");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sql.Append($@"(@OrderNo{i},@StatusName{i},@OrderTotal{i},@BuyerName{i},@ReceiverName{i},@ReceiverPhone{i},
                    @ReceiverMobile{i},@ReceiverAddress{i},@CreateTime{i},@OrderTime{i},'TMall'),");
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    MySqlParameter parameterCol = new MySqlParameter();
                    switch (dt.Columns[j].ColumnName.Trim())
                    {
                        case "订单编号":
                            parameterCol.ParameterName = $"@OrderNo{i}";
                            parameterCol.DbType = DbType.String;
                            parameterCol.Direction = ParameterDirection.Input;
                            parameterCol.Value = dt.Rows[i][j];
                            dbParameters.Add(parameterCol);
                            break;
                        //case "订单状态ID":
                        //    break;
                        case "订单状态":
                            parameterCol.ParameterName = $"@StatusName{i}";
                            parameterCol.DbType = DbType.String;
                            parameterCol.Direction = ParameterDirection.Input;
                            parameterCol.Value = dt.Rows[i][j];
                            dbParameters.Add(parameterCol);
                            break;
                        case "买家实际支付金额":
                            parameterCol.ParameterName = $"@OrderTotal{i}";
                            parameterCol.DbType = DbType.String;
                            parameterCol.Direction = ParameterDirection.Input;
                            parameterCol.Value = dt.Rows[i][j];
                            dbParameters.Add(parameterCol);
                            break;
                        //case "订单类型":
                        //    break;
                        //case "当前订单是否需要审核处方":
                        //    break;
                        //case "支付方式":
                        //    break;
                        case "买家会员名":
                            parameterCol.ParameterName = $"@BuyerName{i}";
                            parameterCol.DbType = DbType.String;
                            parameterCol.Direction = ParameterDirection.Input;
                            parameterCol.Value = dt.Rows[i][j];
                            dbParameters.Add(parameterCol);
                            break;
                        //case "订购人电话":
                        //    break;
                        //case "订购人手机":
                        //    break;
                        case "收货人姓名":
                            parameterCol.ParameterName = $"@ReceiverName{i}";
                            parameterCol.DbType = DbType.String;
                            parameterCol.Direction = ParameterDirection.Input;
                            parameterCol.Value = dt.Rows[i][j];
                            dbParameters.Add(parameterCol);
                            break;
                        case "联系电话":
                            parameterCol.ParameterName = $"@ReceiverPhone{i}";
                            parameterCol.DbType = DbType.String;
                            parameterCol.Direction = ParameterDirection.Input;
                            parameterCol.Value = dt.Rows[i][j];
                            dbParameters.Add(parameterCol);
                            break;
                        case "联系手机":
                            parameterCol.ParameterName = $"@ReceiverMobile{i}";
                            parameterCol.DbType = DbType.String;
                            parameterCol.Direction = ParameterDirection.Input;
                            parameterCol.Value = dt.Rows[i][j];
                            dbParameters.Add(parameterCol);
                            break;
                        //case "收货人邮箱":
                        //    break;
                        case "收货地址":
                            parameterCol.ParameterName = $"@ReceiverAddress{i}";
                            parameterCol.DbType = DbType.String;
                            parameterCol.Direction = ParameterDirection.Input;
                            parameterCol.Value = dt.Rows[i][j];
                            dbParameters.Add(parameterCol);
                            break;
                        //case "收货地区":
                        //    break;
                        //case "是否处方订单":
                        //    break;
                        case "订单创建时间":
                            parameterCol.ParameterName = $"@OrderTime{i}";
                            parameterCol.DbType = DbType.String;
                            parameterCol.Direction = ParameterDirection.Input;
                            parameterCol.Value = dt.Rows[i][j];
                            dbParameters.Add(parameterCol);
                            break;
                    }
                }
                MySqlParameter parameter = new MySqlParameter();
                parameter.ParameterName = $"@CreateTime{i}";
                parameter.DbType = DbType.DateTime;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = DateTime.Now;
                dbParameters.Add(parameter);
            }
            sql.Remove(sql.ToString().LastIndexOf(','), 1);
            sql.Append(";");
        }

        public void InsertBillPro(DataTable dt, StringBuilder sql, List<DbParameter> dbParameters)
        {
            sql.Append(@"INSERT INTO yfw_billproinfo (
	OrderNo,
	PrimaryKey,
	ProductNumber,
	Aliascn,
	#Standard,
	#TrocheType,
	#MillTitle,
	#ProduceNo,
	#UnitPrice,
	Quantity,
	Total
	#ReturnQuantity
)
VALUES");
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                sql.Append($@"(@OrderNo{i},@PrimaryKey{i},@ProductNumber{i},@Aliascn{i},
                    @Quantity{i},@Total{i}),");
                int goodsQuantity = 1;
                int packageQuantity = 1;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    MySqlParameter parameterCol = null;
                    switch (dt.Columns[j].ColumnName.Trim())
                    {
                        case "订单编号":
                            parameterCol = new MySqlParameter();
                            parameterCol.ParameterName = $"@OrderNo{i}";
                            parameterCol.DbType = DbType.String;
                            parameterCol.Direction = ParameterDirection.Input;
                            parameterCol.Value = dt.Rows[i][j];
                            dbParameters.Add(parameterCol);
                            break;
                        case "商家编码":
                            parameterCol = new MySqlParameter();
                            parameterCol.ParameterName = $"@PrimaryKey{i}";
                            parameterCol.DbType = DbType.String;
                            parameterCol.Direction = ParameterDirection.Input;
                            if(dt.Rows[i][j]!=null && !string.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                            {
                                var val = dt.Rows[i][j].ToString();
                                if(val.IndexOf("*") > -1)
                                {
                                    parameterCol.Value = val.Substring(0, val.IndexOf("*"));
                                    goodsQuantity = Convert.ToInt32(val.Substring(val.IndexOf("*") + 1));
                                }
                                else
                                {
                                    parameterCol.Value = val;
                                }
                            }
                            dbParameters.Add(parameterCol);
                            break;
                        case "外部系统编号":
                            parameterCol = new MySqlParameter();
                            parameterCol.ParameterName = $"@ProductNumber{i}";
                            parameterCol.DbType = DbType.String;
                            parameterCol.Direction = ParameterDirection.Input;
                            if (dt.Rows[i][j] != null && !string.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                            {
                                var val = dt.Rows[i][j].ToString();
                                if (val.IndexOf("*") > -1)
                                {
                                    parameterCol.Value = val.Substring(0, val.IndexOf("*"));
                                }
                                else
                                {
                                    parameterCol.Value = val;
                                }
                            }
                            dbParameters.Add(parameterCol);
                            break;
                        case "标题":
                            parameterCol = new MySqlParameter();
                            parameterCol.ParameterName = $"@Aliascn{i}";
                            parameterCol.DbType = DbType.String;
                            parameterCol.Direction = ParameterDirection.Input;
                            parameterCol.Value = dt.Rows[i][j];
                            dbParameters.Add(parameterCol);
                            break;
                        case "价格":
                            parameterCol = new MySqlParameter();
                            parameterCol.ParameterName = $"@Total{i}";
                            parameterCol.DbType = DbType.String;
                            parameterCol.Direction = ParameterDirection.Input;
                            parameterCol.Value = dt.Rows[i][j];
                            dbParameters.Add(parameterCol);
                            break;
                        case "购买数量":
                            packageQuantity = Convert.ToInt32(dt.Rows[i][j]);
                            break;
                    }
                }
                var parameter = new MySqlParameter();
                parameter.ParameterName = $"@Quantity{i}";
                parameter.DbType = DbType.String;
                parameter.Direction = ParameterDirection.Input;
                parameter.Value = goodsQuantity * packageQuantity;
                dbParameters.Add(parameter);
            }
            sql.Remove(sql.ToString().LastIndexOf(','), 1);
            sql.Append(";");
        }

        /// <summary>
        /// 插入订单表和订单明细表
        /// </summary>
        /// <param name="orderList"></param>
        /// <returns></returns>
        public List<StringBuilder> InsertBillAndPro(BindingList<BillInfo> orderList)
        {
            var sqlInsertOrderInfo = new StringBuilder();
            sqlInsertOrderInfo.Append(@"INSERT INTO yfw_billinfo (
	OrderNo,
	StatusId,
	StatusName,
	OrderTotal,
	OrderType,
	NeedAuditRx,
	PayType,
	BuyerName,
	BuyerPhone,
	BuyerMobile,
	ReceiverName,
	ReceiverPhone,
	ReceiverMobile,
	ReceiverEmail,
	ReceiverAddress,
	ReceiverRegion,
	IsPrescription,
    CreateTime,
    OrderTime
)
VALUES");
            var sqlInsertOrderProInfo = new StringBuilder();
            sqlInsertOrderProInfo.Append(@"INSERT INTO yfw_billproinfo (
	OrderNo,
	PrimaryKey,
	ProductNumber,
	Aliascn,
	Standard,
	TrocheType,
	MillTitle,
	ProduceNo,
	UnitPrice,
	Quantity,
	Total,
	ReturnQuantity
)
VALUES");
            foreach (var item in orderList)
            {
                //订单表插入
                sqlInsertOrderInfo.Append($@"('{item.OrderNo}','{item.StatusId}','{item.StatusName}',
                    '{item.OrderTotal}','{item.OrderType}','{item.NeedAuditRx}','{item.PayType}','{item.BuyerName}',
                    '{item.BuyerPhone}','{item.BuyerMobile}','{item.ReceiverName}','{item.ReceiverPhone}','{item.ReceiverMobile}',
                    '{item.ReceiverEmail}','{item.ReceiverAddress}','{item.ReceiverRegion}','{item.IsPrescription}','{DateTime.Now}','{item.OrderTime}'),");
                foreach (var itemDetail in item.Details)
                {
                    sqlInsertOrderProInfo.Append($@"('{item.OrderNo}','{itemDetail.PrimaryKey}','{itemDetail.ProductNumber}',
                        '{itemDetail.Aliascn}','{itemDetail.Standard}','{itemDetail.TrocheType}','{itemDetail.MillTitle}',
                        '{itemDetail.ProduceNo}','{itemDetail.UnitPrice}','{itemDetail.Quantity}','{itemDetail.Total}',
                        '{itemDetail.ReturnQuantity}'),");
                }
            }
            //去掉结尾逗号
            sqlInsertOrderInfo.Remove(sqlInsertOrderInfo.Length - 1, 1);
            sqlInsertOrderProInfo.Remove(sqlInsertOrderProInfo.Length - 1, 1);
            List<StringBuilder> list = new List<StringBuilder>();
            list.Add(sqlInsertOrderInfo);
            list.Add(sqlInsertOrderProInfo);
            return list;
        }

        /// <summary>
        /// 更新订单表
        /// </summary>
        /// <param name="orderNoList"></param>
        /// <returns></returns>
        public StringBuilder UpdateBill(string orderNoList="")
        {
            var sqlUpdateOrderInfo = new StringBuilder();
            sqlUpdateOrderInfo.Append($@"UPDATE yfw_billinfo
SET IsConfirm=1,ConfirmTime='{DateTime.Now}'
WHERE OrderNo IN ('{orderNoList}');");
            return sqlUpdateOrderInfo;
        }

        public StringBuilder DeleteBill(string orderNoList)
        {
            var sqlDeleteOrderInfo = new StringBuilder();
            sqlDeleteOrderInfo.Append($@"UPDATE yfw_billinfo
SET IsDelete=1,ModifyTime='{DateTime.Now}'
WHERE OrderNo IN ('{orderNoList}');");
            return sqlDeleteOrderInfo;

        }

        /// <summary>
        /// 获取请货ID
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetStoreNeedId()
        {
            var sql = new StringBuilder();
            sql.Append(@"SELECT sv_br_id FROM system_var;");
            return sql;
        }

        /// <summary>
        /// 获取请货最大日期
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetReceiveDate()
        {
            var sql = new StringBuilder();
            sql.Append(@"SELECT MAX(snp_receive_date) FROM store_need_product;");
            return sql;
        }

        /// <summary>
        /// 所有的请货品种
        /// </summary>
        /// <param name="billProList"></param>
        /// <returns></returns>
        public List<StringBuilder> InsertPleaseOrderInfo(List<BillProInfo> billProList,string snp_id, DateTime new_receive_date)
        {
            var sqlStoreNeedInsert = new StringBuilder();
            sqlStoreNeedInsert.Append(@"INSERT INTO store_need_product (
	snp_br_id,
	snp_receive_date,
	snp_pro_id,
	snp_need_qty,
	snp_status
)
VALUES");
            foreach (var item in billProList)
            {
                sqlStoreNeedInsert.Append($@"('{snp_id}','{new_receive_date}','{item.PrimaryKey}',{item.Quantity},'1'),");
            }
            //去掉结尾逗号
            sqlStoreNeedInsert.Remove(sqlStoreNeedInsert.Length - 1, 1);
            var sqlUploadLogInsert = new StringBuilder();
            sqlUploadLogInsert.Append(@"INSERT INTO snp_upload_log (
	snl_br_id,
	snl_receive_date,
	snl_update_flag,
	snl_update_date,
	snl_upload_flag,
	snl_upload_date,
	snl_memo,
	snl_process_flag,
	snl_process_date
)
VALUES");
            sqlUploadLogInsert.Append($@"('{snp_id}','{new_receive_date.ToString("yyyy/MM/dd")}','1',
                '{new_receive_date.ToString("yyyy/MM/dd")}','0',null,null,'0',null);");
            var list = new List<StringBuilder>();
            list.Add(sqlStoreNeedInsert);
            list.Add(sqlUploadLogInsert);
            return list;
        }

        /// <summary>
        /// 获取订单表最大下单时间，即药房网下单时间
        /// </summary>
        /// <returns></returns>
        public StringBuilder GetBillMaxOrderTime()
        {
            var sql = new StringBuilder();
            sql.Append(@"SELECT MAX(OrderTime) FROM yfw_billinfo;");
            return sql;
        }

        /// <summary>
        /// 更新订单状态
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="statusId"></param>
        /// <param name="statusName"></param>
        /// <returns></returns>
        public StringBuilder UpdateBillStatus(string orderNo, string statusId, string statusName)
        {
            var sql = new StringBuilder();
            sql.Append($@"UPDATE yfw_billinfo
SET StatusId='{statusId}',
StatusName='{statusName}'
WHERE OrderNo='{orderNo}';");
            return sql;
        }
    }
}
