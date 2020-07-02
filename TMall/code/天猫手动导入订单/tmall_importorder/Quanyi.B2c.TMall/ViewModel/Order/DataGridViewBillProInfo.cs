using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanyi.B2c.Yaofangwang.ViewModel.Order
{
    public class DataGridViewBillProInfo
    {
        /// <summary>
        /// ERP主键
        /// </summary>
        [DisplayName("ERP主键")]
        public string PrimaryKey { get; set; }
        ///// <summary>
        ///// 商品编号
        ///// </summary>
        //[DisplayName("商品编号")]
        //public string ProductNumber { get; set; }
        /// <summary>
        /// 商品名/品牌
        /// </summary>
        [DisplayName("商品名/品牌")]
        public string Aliascn { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        [DisplayName("规格")]
        public string Standard { get; set; }
        ///// <summary>
        ///// 剂型/型号
        ///// </summary>
        //[DisplayName("剂型/型号")]
        //public string TrocheType { get; set; }
        /// <summary>
        /// 产品批次或生产日期
        /// </summary>
        [DisplayName("产品批次或生产日期")]
        public string ProduceNo { get; set; }
        ///// <summary>
        ///// 单价
        ///// </summary>
        //[DisplayName("单价")]
        //public string UnitPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        [DisplayName("数量")]
        public string Quantity { get; set; }
        ///// <summary>
        ///// 总价
        ///// </summary>
        //[DisplayName("总价")]
        //public string Total { get; set; }
        ///// <summary>
        ///// 退货数量
        ///// </summary>
        //[DisplayName("退货数量")]
        //public string ReturnQuantity { get; set; }
    }
}
