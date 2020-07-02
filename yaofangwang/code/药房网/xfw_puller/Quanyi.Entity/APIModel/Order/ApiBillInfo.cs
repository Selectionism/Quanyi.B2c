using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanyi.Entity.APIModel.Order
{
    public class ApiBillInfo
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string order_no { get; set; }
        /// <summary>
        /// 订单状态ID
        /// </summary>
        public string status_id { get; set; }
        /// <summary>
        /// 订单状态名称
        /// </summary>
        public string status_name { get; set; }
        /// <summary>
        /// 订单总额（元）
        /// </summary>
        public string order_total { get; set; }
        /// <summary>
        /// 订单类型（目前只有online：在线订单）
        /// </summary>
        public string order_type { get; set; }
        /// <summary>
        /// 当前订单是否需要审核处方【1 订单需要审核处方0 不需要审核处方】
        /// </summary>
        public string need_audit_rx { get; set; }
        /// <summary>
        /// 订购人姓名
        /// </summary>
        public string buyer_name { get; set; }
        /// <summary>
        /// 订购人电话（付款后显示）
        /// </summary>
        public string buyer_phone { get; set; }
        /// <summary>
        /// 订购人手机（付款后显示）
        /// </summary>
        public string buyer_mobile { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        public string receiver_name { get; set; }
        /// <summary>
        /// 收货人电话（付款后显示）
        /// </summary>
        public string receiver_phone { get; set; }
        /// <summary>
        /// 收货人手机（付款后显示）
        /// </summary>
        public string receiver_mobile { get; set; }
        /// <summary>
        /// 收货人邮箱（付款后显示）
        /// </summary>
        public string receiver_email { get; set; }
        /// <summary>
        /// 收货地址
        /// </summary>
        public string receiver_address { get; set; }
        /// <summary>
        /// 收货地区，格式：湖南省益阳市|安化县
        /// </summary>
        public string receiver_region { get; set; }
        /// <summary>
        /// 订购时间
        /// </summary>
        public string createtime { get; set; }
        /// <summary>
        /// 订单商品集合
        /// </summary>
        public List<ApiBillIProInfo> medicines { get; set; }
    }
}
