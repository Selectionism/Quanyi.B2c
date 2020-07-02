using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanyi.Entity.DBEntity.Order
{
    /// <summary>
    /// 订单明细信息(即品种信息)
    /// </summary>
    public class BillProInfo
    {
        /// <summary>
        /// 订单号明细表主键ID
        /// </summary>
        public UInt64 Id { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// ERP主键
        /// </summary>
        public string PrimaryKey { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string ProductNumber { get; set; }
        /// <summary>
        /// 商品名/品牌
        /// </summary>
        public string Aliascn { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Standard { get; set; }
        /// <summary>
        /// 剂型/型号
        /// </summary>
        public string TrocheType { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string MillTitle { get; set; }
        /// <summary>
        /// 产品批次或生产日期
        /// </summary>
        public string ProduceNo { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public string UnitPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string Quantity { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public string Total { get; set; }

        //那些优惠、折扣信息先不管...

        /// <summary>
        /// 退货数量
        /// </summary>
        public string ReturnQuantity { get; set; }
    }
}
