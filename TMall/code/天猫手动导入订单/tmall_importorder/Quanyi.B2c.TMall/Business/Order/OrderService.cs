using Quanyi.B2c.Yaofangwang.ViewModel.Order;
using Quanyi.Entity.DBEntity.Order;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanyi.B2c.Yaofangwang.Business.Order
{
    public class OrderService
    {
        private B2cPuller _puller;

        public OrderService()
        {
            _puller = new B2cPuller();
        }

        /// <summary>
        /// 获取所有新订单(包括待确认的)
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="IsFirstOpened">是否第一次打开</param>
        /// <param name="billBindingList">订单dgv数据集合</param>
        /// <param name="billProInfoList">订单明细数据集合</param>
        /// <param name="isNew">默认没有新的订单</param>
        public void GetNewOrder(out string msg, bool IsFirstOpened, BindingList<DataGridViewBillInfo> billBindingList, 
            List<BillProInfo> billProInfoList, out bool isNew, string storage)
        {
            isNew = false;
            msg = "";
            //待确认订单。肯定不为空，顶多个数为0
            var commitingBills = _puller.GetCommitingList(storage);
            //新增订单
            var bills = storage.Equals("TMall") ? null : _puller.PullBills(out msg, IsFirstOpened);
            List<BillInfo> myPleaseOrderList;
            if (bills != null)
            {
                myPleaseOrderList = commitingBills.Union(bills).ToList();
            }
            else
            {
                myPleaseOrderList = commitingBills;
            }
            var newMyPleaseOrderList = new BindingList<BillInfo>();
            foreach (var item in myPleaseOrderList)
            {
                newMyPleaseOrderList.Add(item);
            }
            //默认没有新的订单
            foreach (var item in newMyPleaseOrderList)
            {
                if (billBindingList.Where(n => n.OrderNo.Equals(item.OrderNo)).Count() == 0)
                {
                    //有新的订单进来
                    isNew = true;
                    billBindingList.Add(new DataGridViewBillInfo()
                    {
                        //IsChecked = false,
                        OrderTime = item.OrderTime,
                        OrderNo = item.OrderNo,
                        BuyerName = item.BuyerName,
                        BuyerMobile = item.BuyerMobile,
                        ReceiverName = item.ReceiverName,
                        ReceiverMobile = item.ReceiverMobile,
                        StatusId = item.StatusId,
                        StatusName = item.StatusName
                    });
                    billProInfoList.AddRange(item.Details);
                }
            }
        }
    }
}
