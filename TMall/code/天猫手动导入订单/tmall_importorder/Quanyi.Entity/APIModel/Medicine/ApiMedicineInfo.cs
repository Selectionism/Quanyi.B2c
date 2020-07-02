using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanyi.Entity.APIModel.Medicine
{
    public class ApiMedicineInfo
    {
        /// <summary>
        /// ERP系统商品主键
        /// </summary>
        public string primary_key { get; set; }
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
        /// 剂型
        /// </summary>
        public string trochetype { get; set; }
        /// <summary>
        /// 规格/型号
        /// </summary>
        public string standard { get; set; }
        /// <summary>
        /// 生产厂家
        /// </summary>
        public string milltitle { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        public string product_number { get; set; }
        /// <summary>
        /// 重量（单位克）
        /// </summary>
        public string weight { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        public string product_barcode { get; set; }
        /// <summary>
        /// 线下价（元）【未独立设置商城价格的前提下，商城价等于线下价】
        /// </summary>
        public string price { get; set; }
        /// <summary>
        /// 库存
        /// </summary>
        public string reserve { get; set; }
        /// <summary>
        /// 单次购买最大数量（不得超过500），0为不限购
        /// </summary>
        public string max_buy_quantity { get; set; }
        /// <summary>
        /// 发货周期，0:1天两发 1:24小时内发货批发商家支持1，2,3
        /// </summary>
        public string send_day { get; set; }
        /// <summary>
        /// 商品状态ID，1：发布 2：热销 3：促销4：新品 5：推荐 -999：主动下架
        /// </summary>
        public string status_id { get; set; }
        /// <summary>
        /// 有效期至 批发、连锁专用
        /// </summary>
        public string period_to { get; set; }
        /// <summary>
        /// 生产日期 批发、连锁专用
        /// </summary>
        public string produce_date { get; set; }
        /// <summary>
        /// 批次 批发、连锁专用
        /// </summary>
        public string product_batchno { get; set; }
        ///// <summary>
        ///// 【批发】采购倍数，值：1~1000
        ///// </summary>
        //public string purchase_times { get; set; }
        ///// <summary>
        ///// 【批发】采购起订量 值：2~1000
        ///// </summary>
        //public string purchase_minsum { get; set; }
        ///// <summary>
        ///// 进货价
        ///// </summary>
        //public string receive_price { get; set; }
    }
}
