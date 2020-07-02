using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Quanyi.Extend
{
    /// <summary>
    /// 数据表转换类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbTableConvertor<T> where T : new()
    {
        /// <summary>
        /// 将DataTable转换成实体列表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="type">type=0，正常转换；type=1，导入Excel等</param>
        /// <returns></returns>
        public List<T> ConvertToList(DataTable dt, int type)
        {
            //定义集合
            var list = new List<T>();
            if(dt.Rows.Count == 0)
            {
                return list;
            }
            //获得此模型的可写公共属性
            IEnumerable<PropertyInfo> propertyList = new T().GetType().GetProperties().Where(n => n.CanWrite);
            list = ConvertToList(dt, propertyList, type);
            return list;
        }

        ///// <summary>
        ///// 将DataTable的首行转换为实体
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <returns></returns>
        //public T ConvertToEntity(DataTable dt)
        //{
        //    var entity = new T();
        //    if (dt.Rows.Count == 0)
        //    {
        //        return entity;
        //    }
        //    DataTable dtTable = dt.Clone();
        //    dtTable.Rows.Add(dt.Rows[0].ItemArray);
        //    entity = ConvertToList(dtTable)[0];
        //    return entity;
        //}

        /// <summary>
        /// 将DataTable，结合属性，转换成实体列表
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="propertyList"></param>
        /// <param name="type">type=0，正常转换；type=1，导入Excel等</param>
        /// <returns></returns>
        private List<T> ConvertToList(DataTable dt,IEnumerable<PropertyInfo> propertyList, int type)
        {
            var list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                var entity = new T();
                //遍历该对象的所有属性
                foreach (PropertyInfo p in propertyList)
                {
                    //将属性名称赋值给临时变量
                    string tmpName = p.Name;
                    if(type==0)
                    {
                        //检查DataTable是否包含此列(列名=对象的属性名)
                        if (!dt.Columns.Contains(tmpName))
                            continue;
                    }
                    else
                    {
                        if(!importNameDic.ContainsKey(tmpName))
                            continue;
                        if (!dt.Columns.Contains(importNameDic[tmpName]))
                            continue;
                    }
                    //取值
                    object val = dr[type == 0 ? tmpName : importNameDic[tmpName]];
                    //如果非空，则赋给对象的属性
                    if(val != DBNull.Value)
                    {
                        p.SetValue(entity, val, null);
                    }
                }
                //对象添加到泛型集合中
                list.Add(entity);
            }
            return list;
        }
        
        private string GetDisplayName(Type modelType,string propertyDisplayName)
        {
            return (TypeDescriptor.GetProperties(modelType)[propertyDisplayName].Attributes[typeof(DisplayNameAttribute)] as DisplayNameAttribute).DisplayName;
        }

        public static string GetDisplay(Type modelType, string propertyDisplayName)
        {
            return (TypeDescriptor.GetProperties(modelType)[propertyDisplayName].Attributes[typeof(DisplayAttribute)] as DisplayAttribute).Name;
        }

        public readonly Dictionary<string, string> importNameDic = new Dictionary<string, string>()
        {
            {  "PrimaryKey","SAP商品编码" },
            {  "AuthorizedCode","批准文号" },
            {  "Namecn","通用名" },
            {  "Aliascn","商品名/品牌" },
            {  "TrocheType","剂型" },
            {  "Standard","规格/型号" },
            {  "Milltitle","生产厂家" },
            {  "ProductNumber","产品编号" },
            {  "Weight","重量(单位克)" },
            {  "ProductBarcode","条形码" },
            {  "Price","线下价" },
            {  "MaxShelfStock","最大上架库存" },
            {  "MaxBuyQuantity","单次购买最大数量" },
            {  "SendDay","发货周期" },
            {  "StatusId","商品状态" }
        };
    }
}
