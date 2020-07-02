using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanyi.Entity.DBEntity.Order
{
    /// <summary>
    /// 订单主体信息
    /// </summary>
    public class BillInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [DisplayName("主键ID")]
        public UInt64 Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        [DisplayName("订单号")]
        public string OrderNo { get; set; }
        /// <summary>
        /// 订单状态ID
        /// 10：暂未付款
        /// 11：等待发货
        /// 111：申请退款
        /// 112：取消申请退款
        /// 113：拒绝退款
        /// 13：暂未收货
        /// 14：交易完成
        /// 15：交易失败
        /// 16：正在退款
        /// 17：交易取消
        /// </summary>
        public string StatusId { get; set; }
        /// <summary>
        /// 订单状态名称
        /// </summary>
        public string StatusName { get; set; }
        /// <summary>
        /// 订单总额（元）
        /// </summary>
        public string OrderTotal { get; set; }
        /// <summary>
        /// 订单类型（目前只有online：在线订单）
        /// </summary>
        public string OrderType { get; set; }
        /// <summary>
        /// 当前订单是否需要审核处方
        /// 1 订单需要审核处方
        /// 0 不需要审核处方
        /// </summary>
        public string NeedAuditRx { get; set; }
        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayType { get; set; }

        //金额先不管...

        /// <summary>
        /// 订购人姓名
        /// </summary>
        public string BuyerName { get; set; }
        /// <summary>
        /// 订购人电话（付款后显示）
        /// </summary>
        public string BuyerPhone { get; set; }
        /// <summary>
        /// 订购人手机（付款后显示）
        /// </summary>
        public string BuyerMobile { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string ReceiverName { get; set; }
        /// <summary>
        /// 收货人电话（付款后显示）
        /// </summary>
        public string ReceiverPhone { get; set; }
        /// <summary>
        /// 是否处方订单,0:否 1：是
        /// </summary>
        public string IsPrescription { get; set; }
        /// <summary>
        /// 收货人手机（付款后显示）
        /// </summary>
        public string ReceiverMobile { get; set; }
        /// <summary>
        /// 收货人邮箱（付款后显示）
        /// </summary>
        public string ReceiverEmail { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string ReceiverAddress { get; set; }
        /// <summary>
        /// 收货地区，格式：湖南省益阳市|安化县
        /// </summary>
        public string ReceiverRegion { get; set; }
        /// <summary>
        /// 是否确认，通过该字段区分请货没请货(0-未确认 1-已确认)
        /// </summary>
        public int IsConfirm { get; set; }
        /// <summary>
        /// 订购时间
        /// </summary>
        public string OrderTime { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime ModifyTime { get; set; }
        /// <summary>
        /// 请货时间
        /// </summary>
        public DateTime ConfirmTime { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public int IsDelete { get; set; }

        //物流信息不管...

        //发票信息不管...

        //退货信息不管...

        /// <summary>
        /// 订单明细
        /// </summary>
        public List<BillProInfo> Details { get; set; }
    }
}
