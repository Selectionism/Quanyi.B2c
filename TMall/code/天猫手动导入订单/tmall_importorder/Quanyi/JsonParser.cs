using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Quanyi
{
    public class JsonParser
    {
        public JObject Json { get; private set; }

        public JsonParser(JObject jo)
        {
            Json = jo;
        }

        public JsonParser(JToken jo)
        {
            Json = (JObject)jo;
        }

        public JsonParser(string json)
            : this(JObject.Parse(json))
        {
        }

        public void Set(string propertyName, JToken value)
        {
            Json[propertyName] = value;
        }

        public bool TryGetValue(string propertyName, out object value)
        {
            return TryGetValue(Json, propertyName, out value);
        }

        public static bool TryGetValue(JObject jo, string propertyName, out object value)
        {
            if (jo.TryGetValue(propertyName, out JToken jt))
            {
                switch (jt.Type)
                {
                    case JTokenType.String:
                        value = (string)jt;
                        break;
                    case JTokenType.Integer:
                        value = (int)jt;
                        break;
                    case JTokenType.Boolean:
                        value = (bool)jt;
                        break;
                    case JTokenType.Date:
                        value = (DateTime)jt;
                        break;
                    case JTokenType.Float:
                        value = (float)jt;
                        break;
                    case JTokenType.Bytes:
                        value = (byte)jt;
                        break;
                    case JTokenType.Guid:
                        value = (Guid)jt;
                        break;
                    case JTokenType.Uri:
                        value = (Uri)jt;
                        break;
                    case JTokenType.TimeSpan:
                        value = (TimeSpan)jt;
                        break;
                    case JTokenType.None:
                    case JTokenType.Null:
                    case JTokenType.Undefined:
                        value = null;
                        break;
                    case JTokenType.Object:
                    case JTokenType.Array:
                    case JTokenType.Constructor:
                    case JTokenType.Property:
                    case JTokenType.Comment:
                    case JTokenType.Raw:
                        value = jt;
                        break;
                    default:
                        value = jt;
                        break;
                }
                return true;
            }
            value = null;
            return false;
        }

        public string GetString(string propertyName)
        {
            if (GetString(propertyName, out string v))
            {
                return v;
            }
            return null;
        }

        public bool GetString(string propertyName, out string v)
        {
            if (Json != null && Json.TryGetValue(propertyName, out var jt) && !jt.HasValues)
            {
                v = (string)jt;
                return true;
            }
            v = null;
            return false;
        }

        public bool GetBoolean(string propertyName, out bool v)
        {
            if (Json != null && Json.TryGetValue(propertyName, out var jt) && !jt.HasValues)
            {
                v = (bool)jt;
                return true;
            }
            v = false;
            return false;
        }

        public bool GetInt32(string propertyName, out int v)
        {
            if (Json != null && Json.TryGetValue(propertyName, out var jt) && !jt.HasValues)
            {
                v = (int)jt;
                return true;
            }
            v = 0;
            return false;
        }

        public bool GetDouble(string propertyName, out double v)
        {
            if (Json != null && Json.TryGetValue(propertyName, out var jt) && !jt.HasValues)
            {
                v = (double)jt;
                return true;
            }
            v = 0.0;
            return false;
        }

        public JsonParser GetObject(string propertyName)
        {
            if (Json != null && Json.TryGetValue(propertyName, out var jt) && jt.HasValues)
            {
                return new JsonParser((JObject)jt);
            }
            return null;
        }

        public JArray GetArray(string propertyName)
        {
            if (Json != null && Json.TryGetValue(propertyName, out var jt))
            {
                return (JArray)jt;
            }
            return null;
        }

        public bool ValueEquals(string propertyName, string value)
        {
            if (GetString(propertyName, out string v))
            {
                return value.Equals(v);
            }
            return false;
        }

        public bool ValueEquals(string propertyName, bool value)
        {
            if (GetBoolean(propertyName, out bool v))
            {
                return value.Equals(v);
            }
            return false;
        }

        public bool ValueEquals(string propertyName, int value)
        {
            if (GetInt32(propertyName, out int v))
            {
                return value.Equals(v);
            }
            return false;
        }

        public bool ValueEquals(string propertyName, double value)
        {
            if (GetDouble(propertyName, out double v))
            {
                return value.Equals(v);
            }
            return false;
        }

        /// <summary>
        /// 使用JSON中的字段值来格式化字符串，占位符格式为:{字段名}
        /// </summary>
        /// <param name="format">包含占位符的格式化字符串</param>
        /// <returns></returns>
        public string Format(string format)
        {
            var reg = new Regex("{.*?}");//TODO:正则待完善处理{aaa{{}}这种复杂的标签
            var matchs = reg.Matches(format);
            if (matchs.Count == 0)
            {
                return format;
            }
            var sb = new StringBuilder(format.Length);
            int nextIdx = 0;
            foreach (Match mc in matchs)
            {
                if (nextIdx < mc.Index)
                {
                    var len = mc.Index - nextIdx;
                    sb.Append(format, nextIdx, len);
                    nextIdx += len;
                }
                var field = mc.Value;
                if (field.Length > 2 && TryGetValue(field.Substring(1, field.Length - 2), out object s))
                {//去除前后的{}
                    sb.Append(s);
                }
                else
                {
                    sb.Append(mc.Value);
                }
                nextIdx += mc.Length;
            }
            if (nextIdx < format.Length - 1)
            {//后面还有字符
                sb.Append(format, nextIdx, format.Length - nextIdx);
            }
            return sb.ToString();
        }

        public override string ToString()
        {
            return Json.ToString();
        }
    }
}