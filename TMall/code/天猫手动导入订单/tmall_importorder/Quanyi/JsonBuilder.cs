using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Quanyi
{
    public class JsonBuilder
    {
        public static readonly long unixEpochTicks = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).Ticks;

        private JObject _obj;

        public JsonBuilder()
        {
            _obj = new JObject();
        }

        public void Add(string propertyName, JToken value)
        {
            _obj.Add(propertyName, value);
        }

        public override string ToString()
        {
            return _obj.ToString();
        }

        #region DataSet相关类到Json的转换方法
        /// <summary>
        /// DataSet转换成Json格式字符串
        /// </summary>
        /// <param name="dsConvert">转换的DataSet</param>
        /// <returns></returns>
        public static string DataSetToJson(DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
            {
                return "";
            }
            //TODO 要加入表名供解析时判断属于哪个表
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < ds.Tables.Count; i++)
            {
                DataViewToJson(sb, ds.Tables[i].DefaultView, true);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将一个数据表转换成一个JSON字符串，在客户端可以直接转换成二维数组。
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="dt">需要转换的表。</param>
        /// <param name="arrayPrefix">是否加Json数组前缀[]</param>
        public static void DataTableToJson(StringBuilder sb, DataTable dt, bool arrayPrefix = true)
        {
            DataViewToJson(sb, dt.DefaultView, arrayPrefix);
        }

        public static void DataViewToJson(StringBuilder sb, DataView dv, bool arrayPrefix = true)
        {
            if (dv == null || dv.Count == 0)
            {
                if (arrayPrefix)
                {
                    sb.Append("[]");
                }
                else
                {
                    sb.Append("{}");
                }
                return;
            }
            DataColumnCollection dcc = dv.Table.Columns;
            if (arrayPrefix)
            {
                sb.Append('[');
            }
            foreach (DataRowView drv in dv)
            {
                sb.Append("{");
                for (int i = 0; i < dcc.Count; i++)
                {
                    var value = drv[i];
                    if (value != null && value != DBNull.Value)
                    {
                        sb.AppendFormat("\"{0}\":", dcc[i].ColumnName);
                        AppenDbData(sb, value);
                    }
                }
                //去除最后1个字符,,并加入一个},
                sb.RemoveLastChar(',');
                sb.Append("},");
            }
            //去除最后1个字符：,
            sb.RemoveLastChar(',');
            if (arrayPrefix)
            {
                sb.Append(']');
            }
        }

        public static void ReadToJson(DataColumnCollection columns, DataRow row, StringBuilder sb, int skipColumnIndex = -1, int[] jsonColumns = null)
        {
            sb.Append('{');
            for (int i = 0; i < columns.Count; i++)
            {
                if (skipColumnIndex != i)
                {
                    object value = row[i];
                    if (value != null && value != DBNull.Value)
                    {
                        sb.AppendFormat("\"{0}\":", columns[i].ColumnName);
                        if (jsonColumns != null && Array.IndexOf(jsonColumns, i) >= 0)
                        {//是json字符串列则不加前后的""
                            sb.AppendFormat("{0},", value);
                        }
                        else
                        {
                            AppenDbData(sb, value);
                        }
                    }
                }
            }
            //去掉最后一个,号
            sb.RemoveLastChar(',');
            sb.Append('}');
        }

        /// <summary>
        /// 将一个值附加到json字符串StringBuilder
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        public static void AppenDbData(StringBuilder sb, object value)
        {
            if (value is bool)
            {
                sb.Append(GetBoolJson(value));
                sb.Append(',');
            }
            else if (value is string)
            {
                sb.AppendFormat("\"{0}\",", JsonEncode((string)value));
            }
            else if (value is DateTime)
            {
                AppendDateTimeJson(sb, (DateTime)value);
                sb.Append(',');
            }
            else
            {
                sb.Append(value);
                sb.Append(',');
            }
        }

        /// <summary>
        /// 在StringBuilder中附加一个日期格式的Json字符串
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="valueDt"></param>
        /// <param name="encodePlus">是否将+号编码为%2b</param>
        /// <returns></returns>
        public static void AppendDateTimeJson(StringBuilder sb, DateTime valueDt, bool encodePlus = false)
        {
            //参考：System.Runtime.Serialization.Json.JsonWriterDelegator.WriteDateTime(DateTime)
            sb.Append("\"\\/Date(");
            sb.Append((long)((valueDt.ToUniversalTime().Ticks - unixEpochTicks) / 0x2710L));
            TimeSpan utcOffset;
            switch (valueDt.Kind)
            {
                case DateTimeKind.Unspecified:
                case DateTimeKind.Local:
                    utcOffset = TimeZone.CurrentTimeZone.GetUtcOffset(valueDt.ToLocalTime());
                    if (utcOffset.Ticks >= 0L)
                    {
                        if (encodePlus)
                        {
                            sb.Append("%2b");
                        }
                        else
                        {
                            sb.Append('+');
                        }
                        break;
                    }
                    sb.Append('-');
                    break;
                default:
                    goto Label_017B;
            }
            int num2 = Math.Abs(utcOffset.Hours);
            sb.Append((num2 < 10) ? ("0" + num2) : num2.ToString(CultureInfo.InvariantCulture));
            int num3 = Math.Abs(utcOffset.Minutes);
            sb.Append((num3 < 10) ? ("0" + num3) : num3.ToString(CultureInfo.InvariantCulture));
            Label_017B:
            sb.Append(")\\/\"");
        }

        /// <summary>
        /// 返回json中的bool值字符串
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string GetBoolJson(bool b)
        {
            return b ? "true" : "false";
        }

        /// <summary>
        /// 返回json中的bool值字符串
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static string GetBoolJson(object b)
        {
            return true.Equals(b) ? "true" : "false";
        }

        /// <summary>
        /// 将字符串中的""和\等替换为转义符
        /// </summary>
        /// <param name="str">需要序列化的字符串</param>
        /// <param name="repeat">是否将一个\替换为\\\\</param>
        /// <returns></returns>
        public static string JsonEncode(string str, bool repeat = false)
        {
            if (!string.IsNullOrWhiteSpace(str))
            {//TODO:更多编码可以参考System.Web.HttpUtility.JavaScriptStringEncode(String)方法
                if (repeat)
                {
                    str = str.Replace("\\", "\\\\\\\\");//替换\为\\"
                    str = str.Replace("\"", "\\\\\\\"");//替换"为\"，注意这两行Replace的先后顺序
                }
                else
                {
                    str = str.Replace("\\", "\\\\");//替换\为\\"
                    str = str.Replace("\"", "\\\"");//替换"为\"，注意这两行Replace的先后顺序
                }
            }
            return str;
        }
        #endregion

        public static JObject DataRowToJson(DataColumnCollection cols, DataRow row)
        {
            var jo = new JObject();
            foreach (DataColumn item in cols)
            {
                JValue jv;
                var val = row[item];
                if (val == null || DBNull.Value.Equals(row[item]))
                {
                    jv = JValue.CreateNull(); ;
                }
                else
                {
                    jv = new JValue(val);
                }
                jo.Add(item.ColumnName, jv);
            }
            return jo;
        }

        public static JArray DataTableToJson(DataTable dt)
        {
            var ja = new JArray();
            foreach (DataRow item in dt.Rows)
            {
                ja.Add(DataRowToJson(dt.Columns, item));
            }
            return ja;
        }
    }
}
