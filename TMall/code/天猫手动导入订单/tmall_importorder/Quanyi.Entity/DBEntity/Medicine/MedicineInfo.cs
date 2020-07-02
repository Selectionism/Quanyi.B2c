using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanyi.Entity.DBEntity.Medicine
{
    public class MedicineInfo
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        public UInt64 Id { get; set; }
        /// <summary>
        /// ERP系统商品主键
        /// </summary>
        [DisplayName("SAP商品编码")]
        public string PrimaryKey { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [DisplayName("修改时间")]
        public DateTime ModifyTime { get; set; }
        /// <summary>
        /// 通用名
        /// </summary>
        [DisplayName("通用名")]
        public string Namecn { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        [DisplayName("商品名/品牌")]
        public string Aliascn { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        [DisplayName("规格/型号")]
        public string Standard { get; set; }
        /// <summary>
        /// 剂型
        /// </summary>
        [DisplayName("剂型")]
        public string TrocheType { get; set; }
        /// <summary>
        /// 批准文号
        /// </summary>
        [DisplayName("批准文号")]
        public string AuthorizedCode { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        [DisplayName("折扣")]
        public string Discount { get; set; }
        /// <summary>
        /// 条形码
        /// </summary>
        [DisplayName("条形码")]
        public string ProductBarcode { get; set; }
        ///// <summary>
        ///// 分类
        ///// </summary>
        //public string Category { get; set; }
        /// <summary>
        /// 药厂名称
        /// </summary>
        [DisplayName("生产厂家")]
        public string Milltitle { get; set; }
        /// <summary>
        /// 重量
        /// </summary>
        [DisplayName("重量(单位克)")]
        public string Weight { get; set; }
        ///// <summary>
        ///// 进价
        ///// </summary>
        //[DisplayName("进价")]
        //public string ReceivePrice { get; set; }
        /// <summary>
        /// 线下价
        /// </summary>
        [DisplayName("线下价")]
        public string Price { get; set; }
        /// <summary>
        /// 希望上架最大库存数量
        /// </summary>
        [DisplayName("最大上架库存")]
        public string MaxShelfStock { get; set; }
        /// <summary>
        /// 可用库存
        /// </summary>
        [DisplayName("可用库存")]
        public string AvailableStock { get; set; }
        /// <summary>
        /// 库存。上传给药房网的库存
        /// </summary>
        [DisplayName("库存")]
        public string Stock { get; set; }
        /// <summary>
        /// 生产日期
        /// </summary>
        [DisplayName("生产日期")]
        public string ProduceDate { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        [DisplayName("有效期")]
        public string PeriodTo { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [DisplayName("单位")]
        public string Unit { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        [DisplayName("批次")]
        public string ProductBatchNo { get; set; }
        /// <summary>
        /// 产品编号
        /// </summary>
        [DisplayName("产品编号")]
        public string ProductNumber { get; set; }
        /// <summary>
        /// 单次购买最大数量
        /// </summary>
        [DisplayName("单次购买最大数量")]
        public string MaxBuyQuantity { get; set; }
        /// <summary>
        /// 发货周期
        /// </summary>
        [DisplayName("发货周期")]
        public string SendDay { get; set; }
        /// <summary>
        /// 商品状态
        /// </summary>
        [DisplayName("商品状态")]
        public string StatusId { get; set; }
        ///// <summary>
        ///// 换算比率
        ///// </summary>
        //public string ConversionRatio { get; set; }
        /// <summary>
        /// 库存更新结果。0-未上传 1-成功上传 2-上传失败
        /// </summary>
        public int StockUpdateCode { get; set; }
        /// <summary>
        /// 库存更新上传失败日志消息
        /// </summary>
        public string StockUpdateMsg { get; set; }
        /// <summary>
        /// 价格更新结果。0-未上传 1-成功上传 2-上传失败
        /// </summary>
        public int PriceUpdateCode { get; set; }
        /// <summary>
        /// 价格更新上传失败日志消息
        /// </summary>
        public string PriceUpdateMsg { get; set; }
        /// <summary>
        /// 商品资料基本信息更新上传结果。0-未上传 1-成功上传 2-上传失败
        /// </summary>
        public int BasicUpdateCode { get; set; }
        /// <summary>
        /// 商品资料基本信息更新上传失败日志消息
        /// </summary>
        public string BasicUpdateMsg { get; set; }
        /// <summary>
        /// 商品资料基本信息新增上传结果。0-未上传 1-成功上传 2-上传失败
        /// </summary>
        public int BasicInsertCode { get; set; }
        /// <summary>
        /// 商品资料基本信息新增上传失败日志消息
        /// </summary>
        public string BasicInsertMsg { get; set; }
        /// <summary>
        /// SAP库存获取结果。0-未获取 1-获取成功 2-获取失败
        /// </summary>
        public int SapStockGetCode { get; set; }
        /// <summary>
        /// SAP库存获取失败日志消息
        /// </summary>
        public string SapStockGetMsg { get; set; }
    }
}
