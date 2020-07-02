using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanyi.B2c.Yaofangwang.ViewModel.Order
{
    public class DataGridViewBillInfo
    {
        /// <summary>
        /// 选择
        /// </summary>
        [DisplayName("选择")]
        public bool IsChecked { get; set; }
        /// <summary>
        /// 订购时间
        /// </summary>
        [DisplayName("订购时间")]
        public string OrderTime { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        [DisplayName("订单号")]
        public string OrderNo { get; set; }
        /// <summary>
        /// 订购人姓名
        /// </summary>
        [DisplayName("订购人")]
        public string BuyerName { get; set; }
        /// <summary>
        /// 订购人手机
        /// </summary>
        [DisplayName("订购人手机")]
        public string BuyerMobile { get; set; }
        /// <summary>
        /// 收货人姓名
        /// </summary>
        [DisplayName("收货人")]
        public string ReceiverName { get; set; }
        /// <summary>
        /// 收货人手机
        /// </summary>
        [DisplayName("收货人手机")]
        public string ReceiverMobile { get; set; }
        /// <summary>
        /// 订单状态id
        /// </summary>
        [DisplayName("订单状态id")]
        public string StatusId { get; set; }
        /// <summary>
        /// 订单状态名称
        /// </summary>
        [DisplayName("订单状态")]
        public string StatusName { get; set; }
    }
}
