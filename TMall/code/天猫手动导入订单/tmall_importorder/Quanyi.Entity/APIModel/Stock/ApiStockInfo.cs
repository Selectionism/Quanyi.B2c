using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanyi.Entity.APIModel.Stock
{
    /// <summary>
    /// 库存、批次、效期等信息映射类(SAP接口访问)
    /// </summary>
    public class ApiStockInfo
    {
        /// <summary>
        /// 仓库代码
        /// </summary>
        public string Storage { get; set; }
        /// <summary>
        /// ERP系统商品主键，即SAP商品编码
        /// </summary>
        public string PrimaryKey { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string ProductBatchNo { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public string PeriodTo { get; set; }
        /// <summary>
        /// 生产日期
        /// </summary>
        public string ProduceDate { get; set; }
        /// <summary>
        /// 可用库存
        /// </summary>
        public string AvailableStock { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }
    }
}
