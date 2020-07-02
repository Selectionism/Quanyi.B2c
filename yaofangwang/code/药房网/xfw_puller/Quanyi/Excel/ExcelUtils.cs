using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
//using Microsoft.Office.Interop.Excel;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace Quanyi.Excel
{
    public class ExcelUtils
    {
        //private Workbook workbook;
        //private object oMissiong;
        //private Application app;
        //private Worksheet worksheet;

        /// <summary>
        /// 读取CSV文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public DataTable ReadCsv(string fileName)
        {
            DataTable dt = new DataTable();
            using(var fs=new FileStream(fileName,FileMode.Open,FileAccess.Read))
            {
                using(var sr=new StreamReader(fs, Encoding.GetEncoding("gb2312")))
                {
                    //记录每次读取的一行记录
                    string strLine = null;
                    //记录每行记录中的各字段内容
                    string[] arrayLine = null;
                    //分隔符
                    string[] separators = { "," };
                    //判断，若是第一次，建立表头
                    bool isFirst = true;
                    //逐行读取CSV文件
                    while((strLine=sr.ReadLine())!=null)
                    {
                        strLine = strLine.Trim();//去除头尾空格
                        arrayLine = strLine.Split(separators, StringSplitOptions.RemoveEmptyEntries);//分隔字符串，返回数组
                        int dtColumns = arrayLine.Length;//列的个数
                        if(isFirst)//建立表头
                        {
                            for (int i = 0; i < dtColumns; i++)
                            {
                                dt.Columns.Add(arrayLine[i]);//每一列名称
                            }
                        }
                        else//表内容
                        {
                            DataRow dataRow = dt.NewRow();//新建一行
                            for (int i = 0; i < dtColumns; i++)
                            {
                                dataRow[i] = arrayLine[i];
                            }
                            dt.Rows.Add(dataRow);//添加一行
                        }
                        isFirst = false;
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Excel导入成Datable
        /// </summary>
        /// <param name="file">导入路径(包含文件名与扩展名)</param>
        /// <returns></returns>
        public DataTable ExcelToTable(string file)
        {
            DataTable dt = new DataTable();
            IWorkbook workbook;
            string fileExt = Path.GetExtension(file).ToLower();
            using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                //XSSFWorkbook 适用XLSX格式，HSSFWorkbook 适用XLS格式
                if (fileExt == ".xlsx") 
                { 
                    workbook = new XSSFWorkbook(fs); 
                } 
                else if (fileExt == ".xls")
                { 
                    workbook = new HSSFWorkbook(fs); 
                } 
                else 
                { 
                    workbook = null; 
                }
                if (workbook == null) 
                {
                    return null; 
                }
                ISheet sheet = workbook.GetSheetAt(0);
                //表头  
                IRow header = sheet.GetRow(sheet.FirstRowNum);
                List<int> columns = new List<int>();
                for (int i = 0; i < header.LastCellNum; i++)
                {
                    object obj = GetValueType(header.GetCell(i));
                    if (obj == null || obj.ToString() == string.Empty)
                    {
                        dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                    }
                    else
                        dt.Columns.Add(new DataColumn(obj.ToString()));
                    columns.Add(i);
                }
                //数据  
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    DataRow dr = dt.NewRow();
                    bool hasValue = false;
                    foreach (int j in columns)
                    {
                        dr[j] = GetValueType(sheet.GetRow(i).GetCell(j));
                        if (dr[j] != null && dr[j].ToString() != string.Empty)
                        {
                            hasValue = true;
                        }
                    }
                    if (hasValue)
                    {
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }

        /// <summary>
        /// Datable导出成Excel
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="file"></param>
        public void TableToExcel(DataTable dt, string file)
        {
            IWorkbook workbook;
            string fileExt = Path.GetExtension(file).ToLower();
            if(fileExt== ".xlsx")
            {
                workbook = new XSSFWorkbook();
            }
            else if(fileExt== ".xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                workbook = null;
            }
            if(workbook==null)
            {
                return;
            }
            ISheet sheet = string.IsNullOrEmpty(dt.TableName) ? workbook.CreateSheet("Sheet1") : workbook.CreateSheet(dt.TableName);
            //表头
            IRow row = sheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = row.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }
            //数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow row1 = sheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = row1.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }
            //转为字节数组
            using(var stream = new MemoryStream())
            {
                workbook.Write(stream);
                var buf = stream.ToArray();
                //保存为Excel文件
                using (FileStream fs = new FileStream(file, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.Write(buf, 0, buf.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>
        /// 获取单元格类型
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        private static object GetValueType(ICell cell)
        {
            if (cell == null)
                return null;
            switch (cell.CellType)
            {
                case CellType.Blank: //BLANK:  
                    return null;
                case CellType.Boolean: //BOOLEAN:  
                    return cell.BooleanCellValue;
                case CellType.Numeric: //NUMERIC:  
                    return cell.NumericCellValue;
                case CellType.String: //STRING:  
                    return cell.StringCellValue;
                case CellType.Error: //ERROR:  
                    return cell.ErrorCellValue;
                case CellType.Formula: //FORMULA:  
                default:
                    return "=" + cell.CellFormula;
            }
        }

        ///// <summary>
        ///// 从Excel获取数据，一次性返回DataTable
        ///// </summary>
        ///// <param name="fileName"></param>
        ///// <param name="startRow"></param>
        ///// <param name="msg"></param>
        ///// <returns></returns>
        //public System.Data.DataTable GetExcelData(string fileName, int startRow, out string msg)
        //{
        //    bool hasTitle = true;
        //    msg = "";
        //    Application app = new Application();
        //    Sheets sheets;
        //    object oMissiong = Missing.Value;
        //    Workbook workbook = null;
        //    System.Data.DataTable data = new System.Data.DataTable();
        //    try
        //    {
        //        if (app == null)
        //            return null;
        //        workbook = app.Workbooks.Open(fileName, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong,
        //                oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong);
        //        sheets = workbook.Worksheets;
        //        //读取第一张表 
        //        Worksheet worksheet = sheets.get_Item(1) as Worksheet;
        //        if (worksheet == null)
        //            return null;
        //        int iRowCount = worksheet.UsedRange.Rows.Count;
        //        int iColCount = worksheet.UsedRange.Columns.Count;
        //        //生成列头
        //        for (int i = 0; i < iColCount; i++)
        //        {
        //            var name = "column" + i;
        //            if(hasTitle)
        //            {
        //                var txt = (worksheet.Cells[startRow, i + 1] as Range).Text.ToString();
        //                if (!string.IsNullOrEmpty(txt))
        //                    name = txt;
        //            }
        //            while (data.Columns.Contains(name))
        //                //重复行名称会报错
        //                name = name + "_1";
        //            data.Columns.Add(new System.Data.DataColumn(name, typeof(string)));
        //        }
        //        //生成行数据
        //        Range range;
        //        int rowIdx = hasTitle ? 2 : 1;
        //        for (int iRow = rowIdx; iRow <= iRowCount; iRow++)
        //        {
        //            System.Data.DataRow dr = data.NewRow();
        //            for (int iCol = 1; iCol <= iColCount; iCol++)
        //            {
        //                range = worksheet.Cells[iRow, iCol] as Range;
        //                dr[iCol - 1] = (range.Value2 == null) ? "" : range.Text.ToString();
        //            }
        //            data.Rows.Add(dr);
        //        }
        //        msg = "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = "ERROR" + ex.InnerException != null ? ex.InnerException.ToString() : ex.Message;
        //        return null;
        //    }
        //    finally
        //    {
        //        workbook.Close(false, oMissiong, oMissiong);
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
        //        workbook = null;
        //        app.Workbooks.Close();
        //        app.Quit();
        //        System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
        //        app = null;
        //    }
        //    return data;
        //}

        ///// <summary>
        ///// 从Excel获取数据(NPOI)，同时返回当前Excel工作簿行数、列数，请求结果
        ///// </summary>
        ///// <param name="fileName"></param>
        ///// <param name="startRow">有列标题，都是从第一行取标题</param>
        ///// <param name="msg"></param>
        ///// <param name="iRowCount">实际活动行数</param>
        ///// <param name="iColCount">实际活动列数</param>
        ///// <param name="iiColCount">空列列数</param>
        ///// <returns></returns>
        //public System.Data.DataTable CreateDataTable(string fileName, int startRow, 
        //    out string msg, out int iRowCount, out int iColCount, out int iiColCount)
        //{
        //    //正确的应该返回前台提示：实际多少行、多少列，过滤掉多少空行、空列，最后处理多少行、多少列
        //    bool hasTitle = true;
        //    msg = "";
        //    iRowCount = 0;
        //    iColCount = 0;
        //    iiColCount = 0;
        //    app = new Application();
        //    Sheets sheets;
        //    oMissiong = Missing.Value;
        //    workbook = null;
        //    worksheet = null;
        //    System.Data.DataTable data = new System.Data.DataTable();
        //    try
        //    {
        //        if (app == null)
        //            return null;
        //        workbook = app.Workbooks.Open(fileName, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong,
        //                oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong, oMissiong);
        //        sheets = workbook.Worksheets;
        //        //告诉用户，我永远只取第一个sheet(Excel中，索引都是从1开始的) 
        //        worksheet = sheets.get_Item(1) as Worksheet;
        //        if (worksheet == null)
        //            return null;
        //        //这样直接计算出行数和列数，有可能大于用户实际认为的数值，因为其中有很多行和列为空，也被算上了
        //        iRowCount = worksheet.UsedRange.Rows.Count;
        //        iColCount = worksheet.UsedRange.Columns.Count;
        //        //生成列头
        //        for (int i = 0; i < iColCount; i++)
        //        {
        //            if (hasTitle)
        //            {
        //                var txt = (worksheet.Cells[startRow, i + 1] as Range).Text.ToString();
        //                //列名称不为空的，加进来
        //                if (!string.IsNullOrEmpty(txt))
        //                {
        //                    var name = txt;
        //                    if (data.Columns.Contains(name))
        //                    {
        //                        //重复行名称会报错，Excel中可以乱写，但我程序中不可以
        //                        msg = "WARNING" + "Excel中有重复列名称";
        //                        return data;
        //                    }
        //                    data.Columns.Add(new System.Data.DataColumn(name, typeof(string)));
        //                }
        //                else
        //                {
        //                    //列名称为空的，过滤掉，视为空列
        //                    iiColCount++;
        //                }
        //            }
        //        }
        //        msg = "OK";
        //        return data;
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = "ERROR" + ex.InnerException != null ? ex.InnerException.ToString() : ex.Message;
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// 为当前DataTable增加一行
        ///// </summary>
        ///// <param name="msg"></param>
        ///// <param name="iRow">行号，当前读取到哪一行</param>
        ///// <param name="iColCount">有效列数</param>
        ///// <param name="iiRowCount">空行行数</param>
        ///// <param name="iRowCount">有效行数</param>
        ///// <param name="data"></param>
        ///// <param name=""></param>
        //public void AddNewRow(ref string msg, int iRow, int iColCount, ref int iiRowCount,
        //    int iRowCount, System.Data.DataTable data)
        //{
        //    try
        //    {
        //        if(iRow==947)
        //        {

        //        }
        //        bool isNull = true;
        //        System.Data.DataRow dr = data.NewRow();
        //        for (int iCol = 1; iCol <= iColCount; iCol++)
        //        {
        //            Range range = worksheet.Cells[iRow, iCol] as Range;
        //            dr[iCol - 1] = (range.Value2 == null) ? "" : range.Text.ToString();
        //            if (!string.IsNullOrEmpty(dr[iCol - 1].ToString().Trim()))
        //            {
        //                isNull = false;
        //            }
        //        }
        //        if (isNull)
        //        {
        //            //整行都为空，视为空行
        //            iiRowCount++;
        //        }
        //        else
        //        {
        //            //不是空行的，才加到DataTable里面
        //            data.Rows.Add(dr);
        //        }
        //        //如果是最后一行，则调用ReleaseWorkbook方法
        //        if(iRow == iRowCount)
        //        {
        //            ReleaseWorkbook();
        //        }
        //        msg = "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = "ERROR" + ex.InnerException != null ? ex.InnerException.ToString() : ex.Message;
        //    }
        //}

        ///// <summary>
        ///// 释放Excel各项关键信息
        ///// </summary>
        //public void ReleaseWorkbook()
        //{
        //    workbook.Close(false, oMissiong, oMissiong);
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
        //    workbook = null;
        //    app.Workbooks.Close();
        //    app.Quit();
        //    System.Runtime.InteropServices.Marshal.ReleaseComObject(app);
        //    app = null;
        //}

        /// <summary>
        /// 对Excel存于DataTable的空行进行遍历处理
        /// </summary>
        /// <param name="dt"></param>
        public void RemoveEmptyRow(System.Data.DataTable dt)
        {
            List<System.Data.DataRow> removeList = new List<System.Data.DataRow>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bool isNull = true;
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i][j].ToString().Trim()))
                    {
                        isNull = false;
                    }
                }
                if (isNull)
                {
                    removeList.Add(dt.Rows[i]);
                }
            }
            for (int i = 0; i < removeList.Count; i++)
            {
                dt.Rows.Remove(removeList[i]);
            }
        }

        /// <summary>
        /// 对Excel存于DataTable的空列进行遍历处理
        /// </summary>
        /// <param name="dt"></param>
        public void RemoveEmptyColumn(System.Data.DataTable dt)
        {
            List<System.Data.DataColumn> removeList = new List<System.Data.DataColumn>();
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                bool isNull = true;
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[j][i].ToString().Trim()))
                    {
                        isNull = false;
                    }
                }
                if (isNull)
                {
                    removeList.Add(dt.Columns[i]);
                }
            }
            for (int i = 0; i < removeList.Count; i++)
            {
                dt.Columns.Remove(removeList[i]);
            }
        }
    }
}
