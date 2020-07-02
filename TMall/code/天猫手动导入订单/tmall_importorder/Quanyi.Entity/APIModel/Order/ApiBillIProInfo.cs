using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanyi.Entity.APIModel.Order
{
    public class ApiBillIProInfo
    {
        /// <summary>
        /// 订单商品ID（商品订单号）
        /// </summary>
        public string order_medicine_id { get; set; }
        /// <summary>
        /// 商家商品ID（商城分配给商家商品的唯一ID）
        /// </summary>
        public string smid { get; set; }
        /// <summary>
        /// ERP主键
        /// </summary>
        public string primary_key { get; set; }
        /// <summary>
        /// 商品编号
        /// </summary>
        public string product_number { get; set; }
        /// <summary>
        /// 批准文号
        /// </summary>
        public string authorizedcode { get; set; }
        /// <summary>
        /// 通用名
        /// </summary>
        public string namecn { get; set; }
        /// <summary>
        /// 商品名/品牌
        /// </summary>
        public string aliascn { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string standard { get; set; }
        /// <summary>
        /// 剂型/型号
        /// </summary>
        public string trochetype { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string milltitle { get; set; }
        /// <summary>
        /// 商品所属套餐
        /// </summary>
        public string package_name { get; set; }
        /// <summary>
        /// 产品批次或生产日期
        /// </summary>
        public string produce_no { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public string unit_price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string quantity { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public string total { get; set; }
        /// <summary>
        /// 折扣，取值范围：0~1,1表示不打折
        /// </summary>
        public string discount { get; set; }
        /// <summary>
        /// 退货数量
        /// </summary>
        public string return_quantity { get; set; }
    }
}
