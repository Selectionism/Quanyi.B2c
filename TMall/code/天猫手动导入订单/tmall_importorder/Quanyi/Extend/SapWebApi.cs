using Quanyi.Entity.APIModel.Stock;
using Quanyi.Entity.DBEntity.Medicine;
using Quanyi.Entity.HttpModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Quanyi.Extend
{
    public class SapWebApi
    {
        /// <summary>
        /// 一次获取SAP大仓库存信息
        /// </summary>
        /// <param name="medicineList"></param>
        /// <returns></returns>
        public HttpResponseModal<ApiStockInfo> GetAvailableStockInfo(List<MedicineInfo> medicineList)
        {
            using (var sapHelper = new SapHelper())
            {
                //是一个个调用SAP库存接口，还是一次性怼过去，然后本地做筛选？(我初步认为应该一次性，这样也节省效率)
                List<string> primaryKeyList = medicineList.Select(n => n.PrimaryKey).Distinct().ToList();
                ApiStockParam param = new ApiStockParam();
                param.PrimaryKeyList = primaryKeyList;
                HttpResponseModal<ApiStockInfo> result = sapHelper.GetAvailableStockInfo(param);
                return result;
            }
        }
    }
}
