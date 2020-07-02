using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quanyi.Entity.APIModel.Stock
{
    /// <summary>
    /// 库存、批次、效期等信息接口参数类(SAP接口访问)
    /// </summary>
    public class ApiStockParam
    {
        ///// <summary>
        ///// 仓库代码
        ///// </summary>
        //public string Storage { get; set; }
        /// <summary>
        /// ERP系统商品主键，即SAP商品编码
        /// </summary>
        public List<string> PrimaryKeyList { get; set; }
    }
}
