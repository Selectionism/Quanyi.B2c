using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

namespace Quanyi
{
    public static class DbExtends
    {
        public static string GetSqlString(this DbDataReader rdr, int ordinal)
        {
            var value = rdr.GetValue(ordinal);
            if (value == null || DBNull.Value.Equals(value))
            {
                return null;
            }
            return (string)value;
        }

        /// <summary>
        /// DbCommand生成SQL语句,任何数据类型的值前后都加上单引号,仅支持@参数占位符
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public static string GetSql(this DbCommand cmd)
        {
            var cmdText = cmd.CommandText;
            if (string.IsNullOrEmpty(cmdText))
            {
                return string.Empty;
            }
            foreach (DbParameter item in cmd.Parameters)
            {
                var value = string.Format("'{0}'", item.Value);
                cmdText = cmdText.Replace("@" + item.ParameterName, value);
            }
            //cmdText = cmdText.Replace("CURRENT_TIMESTAMP()", string.Format("'{0}'", DateTime.Now));
            return cmdText;
        }
    }
}
