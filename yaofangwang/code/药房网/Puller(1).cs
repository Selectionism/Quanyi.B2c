using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Quanyi.XbPuller
{
    public class Puller : IDisposable
    {
        private const int INTERVAL_TIME = 30 * 1000;
        private const int MAX_SLEEP_TIME = 5 * 60 * 1000;
        private const string PULLER_ID_FILE = @"c:\mainpos\XbPuller.id";

        #region json字段和列名对照
        public static readonly Dictionary<string, object> MasterFieldsMap = new Dictionary<string, object>
        {
            { "platformsOrderId", "eh_orderno"},
            { "orderId", "eh_orderid" },
            { "platforms", "eh_ordtype" },
            { "shopNo", "eh_brid" },
            { "customerPay", "eh_order_payment" },
            { "totalPrice", "eh_order_total_price" },
            { "shippingFee", "eh_freight_price" },
            { "recipientName", "eh_fullname" },
            { "recipientAddress", "eh_full_address" },
            { "recipientPhone", "eh_mobile" },
            { "caution", "eh_remark" },
            { "invoicedFlag", "eh_fp_type" },
            { "invoiceTitle", "eh_fp_title" },
            { "taxpayerId", "eh_fp_taxid" },
            { "daySeq", "eh_cyz" },
            { "deliveryTime", "eh_booking_time" },
            { "?1", "eh_pay_type" },
            { "?2", "eh_pay_state" },
            { "createTime", new Convert("eh_order_start_time", (val) => Convert.FormatTime(val)) },
        };

        public static readonly Dictionary<string, object> DetailFieldsMap = new Dictionary<string, object>
        {
            { "orderId", "ed_orderid" },
            { "medicineCode", "ed_pro_id" },
            { "quantity", "ed_qty" },
            { "?1", "ed_price" },
        };
        #endregion

        private readonly string _branchId;

        private readonly Action<Puller> _notify;

        private string _lastOrderId;

        private DataTable _dtMaster;
        private DbDataAdapter _daMaster;

        private DataTable _dtDetail;
        private DbDataAdapter _daDetail;

        private DataTable _dtLog;
        private DbDataAdapter _daLog;

        public int PullCount { get; private set; }

        public int InsertCount { get; private set; }

        public int ErrorCount { get; private set; }

        public Puller(string branchId, Action<Puller> notify)
        {
            _branchId = branchId;
            _notify = notify;
        }

        private void InitDataTables(MysqlHelper db)
        {
            var dtMaster = new DataTable { TableName = "pos_epo_head" };
            _daMaster = CreateDataAdapter(db, dtMaster);

            _dtDetail = new DataTable { TableName = "pos_epo_detail" };
            _daDetail = CreateDataAdapter(db, _dtDetail);

            _dtLog = new DataTable { TableName = "epo_push_log" };
            _daLog = CreateDataAdapter(db, _dtLog);

            _dtMaster = dtMaster;
        }

        private void LogError(Exception e)
        {
            LogInfo(e.ToString());
        }

        private void LogInfo(string info)
        {
            var path = string.Format(@"c:\mainpos\{0:yyyyMMdd}puller.log", DateTime.Today);
            File.AppendAllText(path, Environment.NewLine + DateTime.Now.ToString() + Environment.NewLine + info);
        }

        private void Test()
        {
            var file = "test.json";
            if (File.Exists(file))
            {
                _dtMaster = new DataTable();
                _dtMaster.Columns.Add("eh_orderid");
                _dtMaster.Columns.Add("eh_order_start_time");
                _dtDetail = new DataTable();
                _dtDetail.Columns.Add("ed_orderid");
                _dtDetail.Columns.Add("ed_pro_id");
                _dtDetail.Columns.Add("ed_xh");
                Parse(JArray.Parse(File.ReadAllText(file)), new MysqlHelper());
            }
        }

        public void Run()
        {
            var retryCount = 0;
            var pos = GetPosProcessName();
            while (true)
            {
                try
                {
                    Pull();
                    retryCount = 0;
                    Notify();
                    Thread.Sleep(INTERVAL_TIME);
                }
                catch (Exception e)
                {
                    retryCount++;
                    var ms = retryCount * INTERVAL_TIME;
                    if (ms > MAX_SLEEP_TIME)
                    {
                        ms = MAX_SLEEP_TIME;
                    }
                    else if (ms <= 0)
                    {
                        ms = INTERVAL_TIME;
                    }
                    ErrorCount++;
                    Notify();
                    LogError(e);
                    Thread.Sleep(ms);
                }
                if (!string.IsNullOrEmpty(pos) && !IsProcessRunning(pos))
                {
                    return;
                }
            }
        }

        private string GetPosProcessName()
        {
            var pcs = Process.GetProcesses();
            foreach (var item in pcs)
            {
                try
                {
                    if (item.MainModule.FileName.EndsWith("pos.exe", StringComparison.OrdinalIgnoreCase))
                    {
                        return item.ProcessName;
                    }
                }
                catch (Exception)
                {
                }
            }
            return null;
        }

        private bool IsProcessRunning(string processName)
        {
            try
            {
                var procs = Process.GetProcessesByName(processName);
                return procs != null && procs.Length > 0;
            }
            catch (Exception e)
            {
                LogError(e);
            }
            return false;
        }

        private void Notify()
        {
            _notify?.Invoke(this);
        }

        private void Pull()
        {
            using (var db = new MysqlHelper())
            {
                if (_dtMaster == null)
                {
                    InitDataTables(db);
                }
                if (File.Exists(PULLER_ID_FILE))
                {
                    _lastOrderId = File.ReadAllText(PULLER_ID_FILE);
                }
                if (string.IsNullOrEmpty(_lastOrderId))
                {
                    _lastOrderId = db.ExecuteScalar("SELECT eh_orderid FROM pos_epo_head ORDER BY eh_order_start_time DESC LIMIT 1").ToString();
                }
                //if (string.IsNullOrEmpty(_lastOrderId))
                //{
                //    throw new Exception("没有有效的历史单号");
                //}
                var url = GetOrdersUrl(_lastOrderId);
                LogInfo("开始获取: " + url);
                var result = DownloadOrders(url);
                var jo = JObject.Parse(result);
                if (jo.TryGetValue("page", out var page) &&
                    page is JObject pg &&
                    pg.TryGetValue("list", out var list))
                {
                    Parse(list as JArray, db);
                    Notify();
                    File.WriteAllText(PULLER_ID_FILE, _lastOrderId);
                }
            }
        }

        private string DownloadOrders(string url)
        {
            var http = (HttpWebRequest)WebRequest.Create(url);
            using (var resp = http.GetResponse())
            using (var sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
            {
                return sr.ReadToEnd();
            }

        }

        private void Parse(JArray orders, MysqlHelper db)
        {
            if (orders == null)
            {
                return;
            }
            foreach (JObject ord in orders)
            {
                PullCount++;
                _dtMaster.Rows.Clear();
                _dtMaster.AcceptChanges();
                _dtDetail.Rows.Clear();
                _dtDetail.AcceptChanges();
                _dtLog.Rows.Clear();
                _dtLog.AcceptChanges();
                var row = CreateRow(MasterFieldsMap, ord, _dtMaster);
                _lastOrderId = row["eh_orderid"].ToString();
                SetRow(row, "eh_order_state", "1");
                SetRow(row, "eh_notice_msg", "new");
                _dtMaster.Rows.Add(row);
                var sn = 0;
                foreach (JObject item in (JArray)ord.GetValue("items"))
                {
                    var dr = CreateRow(DetailFieldsMap, item, _dtDetail);
                    dr["ed_xh"] = ++sn;
                    //var mc = dr["ed_pro_id"];
                    //if (mc == null || DBNull.Value.Equals(mc))
                    //{
                    //    dr["ed_pro_id"] = "unknow";
                    //}
                    try
                    {
                        _dtDetail.Rows.Add(dr);
                    }
                    catch (Exception e)
                    {
                        LogError(e);
                    }
                }
                if (_dtDetail.Rows.Count > 0)
                {
                    AddToLogTable();
                    Commit(db);
                }
            }
        }

        private void AddToLogTable()
        {
            var log = _dtLog.NewRow();
            log["epo_orderid"] = GetOrderId();
            log["epo_msg_id"] = "1";
            log["epo_push_date"] = DateTime.Today.ToString("yyyy/MM/dd");
            log["epo_notify_flag"] = "0";
            _dtLog.Rows.Add(log);
        }

        private object GetOrderId()
        {
            if (_dtMaster.Rows.Count > 0)
            {
                return _dtMaster.Rows[0]["eh_orderid"];
            }
            return DBNull.Value;
        }

        private void Commit(MysqlHelper db)
        {
            DbTransaction trans = null;
            try
            {
                trans = db.BeginTransaction();
                _daMaster.Update(_dtMaster);
                _daDetail.Update(_dtDetail);
                _daLog.Update(_dtLog);
                trans.Commit();
                InsertCount++;
            }
            catch (Exception e)
            {
                trans.Rollback();
                ErrorCount++;
                LogError(e);
            }
        }

        private void SetRow(DataRow row, string column, object value)
        {
            if (row.Table.Columns.Contains(column))
            {
                row[column] = value;
            }
        }

        public DbDataAdapter CreateDataAdapter(MysqlHelper db, DataTable dt)
        {
            DbDataAdapter da = db.FillTableSchema(dt);
            var cb = db.Factor.CreateCommandBuilder();
            cb.DataAdapter = da;
            da.InsertCommand = cb.GetInsertCommand();
            da.InsertCommand.UpdatedRowSource = UpdateRowSource.None;
            return da;
        }

        private DataRow CreateRow(Dictionary<string, object> map, JObject jo, DataTable dt)
        {
            var dr = dt.NewRow();
            foreach (var kv in jo)
            {
                if (map.TryGetValue(kv.Key, out var mapItem))
                {
                    if (mapItem == null)
                    {
                        continue;
                    }
                    SetValue(dr, kv.Value.ToObject(typeof(object)), mapItem, dt.Columns);
                }
            }
            return dr;
        }

        private void SetValue(DataRow dr, object value, object mapItem, DataColumnCollection columns)
        {
            string field = null;
            if (mapItem is string str)
            {
                field = str;
            }
            else if (mapItem is Convert cv)
            {
                field = cv.FieldName;
                value = cv.Function(value);
            }
            if (string.IsNullOrEmpty(field) || !columns.Contains(field))
            {
                return;
            }
            if (value == null)
            {
                value = DBNull.Value;
            }
            dr[field] = value;
        }

        private string GetOrdersUrl(string lastOrderId)
        {
            return string.Format(
                "http://xbprd.myquanyi.com:8080/wm-bridge/xb/pos/getLastOrders?shopNo={0}&orderId={1}",
                _branchId, lastOrderId);
        }

        public void Dispose()
        {
        }
    }
}
