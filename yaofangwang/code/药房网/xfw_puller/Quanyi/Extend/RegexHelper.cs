using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Quanyi.Extend
{
    public class RegexHelper
    {
        /// <summary>
        /// 判断字符串是否为浮点数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsFloat(string str)
        {
            string regextext = @"^\d+\.\d+$";
            Regex regex = new Regex(regextext, RegexOptions.None);
            return regex.IsMatch(str);
        }

        /// <summary>
        /// 判断字符串是否为整数
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool IsInteger(string str)
        {
            try
            {
                int i = Convert.ToInt32(str);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
