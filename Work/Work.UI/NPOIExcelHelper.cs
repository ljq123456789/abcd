using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Collections;
using System.Reflection;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HPSF;
using NPOI.HSSF.Util;
using NPOI.SS.Util;
using System.Data;
using System.Text;

namespace Work.UI
{
    public class NPOIExcelHelper<T>
    {
        public NPOIExcelHelper()
        {

        }
        /// <summary>  
        /// 导出Excel  
        /// </summary>  
        /// <param name="lists"></param>  
        /// <param name="head">中文列名对照</param>  
        /// <param name="fileName">文件名</param>  
        public static void ExportExcel(List<T> lists, Dictionary<string, string> head, string fileName)
        {          

            HSSFWorkbook workbook = new HSSFWorkbook();
            MemoryStream ms = new MemoryStream();
            HSSFSheet sheet = null;
            HSSFRow headerRow = null;

            //设置表头样式
            ICellStyle headStyle = workbook.CreateCellStyle();
            headStyle.VerticalAlignment = VerticalAlignment.CENTER;
            headStyle.Alignment = HorizontalAlignment.CENTER;
            IFont font = workbook.CreateFont();
            font.FontHeight = 14 * 14;
            font.Boldweight = 1000;
            headStyle.SetFont(font);

            //设置分组样式
            HSSFPalette palette = workbook.GetCustomPalette(); //调色板实例
            palette.SetColorAtIndex((byte)8, (byte)204, (byte)204, (byte)0);
            HSSFColor hssFColor = palette.FindColor((byte)204, (byte)204, (byte)0);

            ICellStyle GroupStyle = workbook.CreateCellStyle();
            GroupStyle.FillPattern = FillPatternType.SOLID_FOREGROUND;
            GroupStyle.FillForegroundColor =hssFColor.GetIndex();

            IFont Groupfont = workbook.CreateFont();        
            Groupfont.Boldweight = 1000;           
            GroupStyle.SetFont(Groupfont);

            bool h = false;

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();

            var mod = lists.Count() % 65535;
            var index = lists.Count() / 65535;
            if (mod > 0)
            {
                index = index + 1;
            }

            //没有数据时导出表头
            if (index == 0)
            {
                int i = 0;
                sheet = workbook.CreateSheet("Sheet0") as HSSFSheet;
                headerRow = sheet.CreateRow(0) as HSSFRow;
                foreach (var dic in head)//循环列表头集合作为列数
                {
                    string[] names = dic.Key.ToString().Split('@');
                    string colname = dic.Value.ToString();  

                    ICell cell = headerRow.CreateCell(i);
                    cell.CellStyle = headStyle;
                    cell.SetCellValue(colname);

                    i++;
                }
            }

            for (int idx = 1; idx <= index; idx++)
            {
                sheet = workbook.CreateSheet("Sheet" + idx) as HSSFSheet;
                headerRow = sheet.CreateRow(0) as HSSFRow;
                var count = 65535;
                if (idx == index)
                {
                    count = mod;
                }
                for (var j = 0; j < count; j++)//循环记录总数作为行数
                {
                    HSSFRow dataRow = sheet.CreateRow(j + 1) as HSSFRow;
                    int i = 0;
                    foreach (var dic in head)//循环列表头集合作为列数
                    {
                        string[] names = dic.Key.ToString().Split('@');
                        string colname = dic.Value.ToString();
                        string name = names[0];
                        bool isTrue = false;//是否基础数据
                        if (names.Length == 2)
                        {
                            isTrue = bool.Parse(names[1]);
                        }
                        var info = properties.Where(x => x.Name == name).FirstOrDefault();
                        object value = info == null ? null : info.GetValue(lists[65535 * (idx - 1) + j], null);
                        string colvalue = value == null ? "" : value.ToString();
                        //if (isTrue)//获取基础数据
                        //{
                        //    colvalue = HtmlExtensions.GetBasicObjectNameByValue(colvalue);
                        //}
                        if (!h)
                        {
                            if ((!name.Equals("IsGroup")))
                            {
                                ICell cell = headerRow.CreateCell(i);
                                cell.CellStyle = headStyle;
                                cell.SetCellValue(colname);
                                if (value != null)
                                {
                                    Type t = value.GetType();
                                    string strt = t.Name;
                                    if (t.Name == "Nullable`1")
                                    {
                                        strt = t.GetGenericArguments()[0].Name;
                                    }
                                    switch (strt)
                                    {
                                        case "Decimal":
                                        case "Double":
                                            dataRow.CreateCell(i).SetCellValue(Double.Parse(value.ToString()));
                                            break;
                                        case "Int":
                                            dataRow.CreateCell(i).SetCellValue(int.Parse(value.ToString()));
                                            break;
                                        case "Float":
                                            dataRow.CreateCell(i).SetCellValue(float.Parse(value.ToString()));
                                            break;
                                        default:
                                            dataRow.CreateCell(i).SetCellValue(colvalue);
                                            break;
                                    }
                                }
                                else
                                {
                                    dataRow.CreateCell(i).SetCellValue(colvalue);
                                }
                            }
                        }
                        else
                        {
                            if ((!name.Equals("IsGroup")))
                            {
                                if (value != null)
                                {
                                    Type t = value.GetType();
                                    string strt = t.Name;
                                    if (t.Name == "Nullable`1")
                                    {
                                        strt = t.GetGenericArguments()[0].Name;
                                    }
                                    switch (strt)
                                    {
                                        case "Decimal":
                                        case "Double":
                                            dataRow.CreateCell(i).SetCellValue(Double.Parse(value.ToString()));
                                            break;
                                        case "Int":
                                            dataRow.CreateCell(i).SetCellValue(int.Parse(value.ToString()));
                                            break;
                                        case "Float":
                                            dataRow.CreateCell(i).SetCellValue(float.Parse(value.ToString()));
                                            break;
                                        default:
                                            dataRow.CreateCell(i).SetCellValue(colvalue);
                                            break;
                                    }
                                }
                                else
                                {
                                    dataRow.CreateCell(i).SetCellValue(colvalue);
                                }
                            }
                        }
                        //========================对特定标志行设置颜色========================
                        if (name.Equals("IsGroup") && colvalue.Equals("Y"))
                        {
                            for (int m = 0; m < i; m++)
                            {
                                dataRow.GetCell(m).CellStyle = GroupStyle;
                            }
                        }
                        //================================end=================================
                        i++;
                    }
                    h = true;
                }
            }

            workbook.Write(ms);
            ms.Flush();
            ms.Position = 0;
            sheet = null;
            headerRow = null;
            workbook = null;

            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                byte[] data = ms.ToArray();
                fs.Write(data, 0, data.Length);
                fs.Flush();
            }

            ms.Close();
            ms.Dispose();
        }

        /// <summary>  
        /// 导入Excel  
        /// </summary>  
        /// <param name="lists"></param>  
        /// <param name="head">中文列名对照</param>  
        /// <param name="workbookFile">Excel所在路径</param>  
        /// <param name="sheetName">sheet名</param>
        /// <returns></returns>  
        public static List<T> ImportExcel(Dictionary<string, string> head, string workbookFile, string sheetName)
        {

            HSSFWorkbook hssfworkbook;
            List<T> lists = new List<T>();
            using (FileStream file = new FileStream(workbookFile, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            HSSFSheet sheet = (string.IsNullOrEmpty(sheetName) ?
                hssfworkbook.GetSheetAt(0) : hssfworkbook.GetSheet(sheetName)) as HSSFSheet;
            IEnumerator rows = sheet.GetRowEnumerator();
            HSSFRow headerRow = sheet.GetRow(0) as HSSFRow;
            int cellCount = headerRow.LastCellNum;
            PropertyInfo[] properties;
            T t = default(T);
            for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
            {
                HSSFRow row = sheet.GetRow(i) as HSSFRow;
                t = Activator.CreateInstance<T>();
                if (row != null)//判断是否空行
                {
                    properties = t.GetType().GetProperties();
                    foreach (var dic in head)//循环列表头集合作为列数
                    {
                        string colname = dic.Value.ToString();
                        string name = dic.Key.ToString();
                        var column = properties.Where(x => x.Name == name).FirstOrDefault();

                        int j = headerRow.Cells.FindIndex(delegate(ICell c)
                        {
                            return c.StringCellValue == colname;
                        });
                        if (j >= 0 && row.GetCell(j) != null)
                        {
                            if (row.GetCell(j).IsMergedCell)//判断单元格是否合并
                            {
                                bool isMerge = false;
                                for (int ii = 0; ii < sheet.NumMergedRegions; ii++)
                                {
                                    var cellrange = sheet.GetMergedRegion(ii);
                                    if (j >= cellrange.FirstColumn && j <= cellrange.LastColumn
                                      && i > cellrange.FirstRow && i <= cellrange.LastRow)//若合并则读取一个合并单元格数据
                                    {
                                        object value = valueType(column.PropertyType, sheet.GetRow(cellrange.FirstRow).Cells[cellrange.FirstColumn].ToString());
                                        column.SetValue(t, value, null);
                                        //是否有合并列
                                        var mergecolumn = properties.Where(x => x.Name == "IsMerge").FirstOrDefault();
                                        if (mergecolumn != null)
                                        {
                                            mergecolumn.SetValue(t, true, null);
                                        }
                                        isMerge = true;
                                        break;
                                    }
                                }
                                if (!isMerge)
                                {
                                    object value = valueType(column.PropertyType, row.GetCell(j).ToString());
                                    column.SetValue(t, value, null);
                                }
                            }
                            else
                            {
                                object value = valueType(column.PropertyType, row.GetCell(j).ToString());
                                column.SetValue(t, value, null);
                            }
                        }
                    }
                }
                lists.Add(t);
            }
            return lists;

        }

        /// <summary>  
        /// 导入Excel
        /// </summary>  
        /// <param name="lists"></param>  
        /// <param name="head">中文列名对照</param>  
        /// <param name="workbookFile">Excel所在路径</param>  
        /// <param name="sheetName">sheet名</param>
        /// <param name="StartRow">开始行</param>
        /// <returns></returns>  
        public static List<T> ImportExcel(Dictionary<string, string> head, string workbookFile, string sheetName, int StartRow)
        {
            bool judgeHead = true;
            HSSFWorkbook hssfworkbook;
            List<T> lists = new List<T>();
            using (FileStream file = new FileStream(workbookFile, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }

            HSSFSheet sheet = (string.IsNullOrEmpty(sheetName) ?
                hssfworkbook.GetSheetAt(0) : hssfworkbook.GetSheet(sheetName)) as HSSFSheet;
            IEnumerator rows = sheet.GetRowEnumerator();
            HSSFRow headerRow = sheet.GetRow(StartRow) as HSSFRow;   //增加了一行（用于显示需求单编号），头部行数为1
            int cellCount = headerRow.LastCellNum;
            PropertyInfo[] properties;
            T t = default(T);
            for (int i = sheet.FirstRowNum + 1 + StartRow; i <= sheet.LastRowNum; i++)
            {
                HSSFRow row = sheet.GetRow(i) as HSSFRow;
                t = Activator.CreateInstance<T>();
                if (row != null)//判断是否空行
                {
                    properties = t.GetType().GetProperties();
                    foreach (var dic in head)//循环列表头集合作为列数
                    {
                        string colname = dic.Value.ToString();
                        string name = dic.Key.ToString();
                        var column = properties.Where(x => x.Name == name).FirstOrDefault();

                        int j = headerRow.Cells.FindIndex(delegate(ICell c)
                        {
                            return c.StringCellValue == colname;
                        });
                        if (j >= 0 && row.GetCell(j) != null)
                        {
                            if (row.GetCell(j).IsMergedCell)//判断单元格是否合并
                            {
                                bool isMerge = false;
                                for (int ii = 0; ii < sheet.NumMergedRegions; ii++)
                                {
                                    var cellrange = sheet.GetMergedRegion(ii);
                                    if (j >= cellrange.FirstColumn && j <= cellrange.LastColumn
                                      && i > cellrange.FirstRow && i <= cellrange.LastRow)//若合并则读取一个合并单元格数据
                                    {
                                        object value = valueType(column.PropertyType, sheet.GetRow(cellrange.FirstRow).Cells[cellrange.FirstColumn].ToString());
                                        column.SetValue(t, value, null);
                                        //是否有合并列
                                        var mergecolumn = properties.Where(x => x.Name == "IsMerge").FirstOrDefault();
                                        if (mergecolumn != null)
                                        {
                                            mergecolumn.SetValue(t, true, null);
                                        }
                                        isMerge = true;
                                        break;
                                    }
                                }
                                if (!isMerge)
                                {
                                    object value = valueType(column.PropertyType, row.GetCell(j).ToString());
                                    column.SetValue(t, value, null);
                                }
                            }
                            else
                            {
                                object value = valueType(column.PropertyType, row.GetCell(j).ToString());
                                column.SetValue(t, value, null);
                            }
                        }
                    }
                    lists.Add(t);
                }

            }
            return lists;

        }

        /// <summary>
        /// 类型转换
        /// </summary>
        /// <param name="t"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        static object valueType(Type t, string value)
        {
            object o = null;
            string strt = "String";
            if (t.Name == "Nullable`1")
            {
                strt = t.GetGenericArguments()[0].Name;
            }
            else
            {
                strt = t.Name;
            }
            switch (strt)
            {
                case "Decimal":
                    o = decimal.Parse(value);
                    break;
                case "Int":              
                    o = int.Parse(value);
                    break;              
                case "Int32":
                    o = Int32.Parse(value);
                    break;
                case "Float":
                    o = float.Parse(value);
                    break;
                case "DateTime":
                    o = DateTime.Parse(value);
                    break;
                default:
                    o = value;
                    break;
            }
            return o;
        }
        /// <summary>
        /// DataTable导出到Excel文件
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">保存位置</param>
        public static void Export(DataTable dtSource, string strHeaderText, string strFileName)
        {
            using (MemoryStream ms = Export(dtSource, strHeaderText))
            {
                using (FileStream fs = new FileStream(strFileName, FileMode.Create, FileAccess.Write))
                {
                    byte[] data = ms.ToArray();
                    fs.Write(data, 0, data.Length);
                    fs.Flush();
                }
            }
        }

        /// <summary>
        /// DataTable导出到Excel的MemoryStream
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        public static MemoryStream Export(DataTable dtSource, string strHeaderText)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            #region 右击文件 属性信息
            {
                DocumentSummaryInformation dsi = PropertySetFactory.CreateDocumentSummaryInformation();
                dsi.Company = "NPOI";
                workbook.DocumentSummaryInformation = dsi;

                SummaryInformation si = PropertySetFactory.CreateSummaryInformation();
                si.Author = "文件作者信息"; //填加xls文件作者信息
                si.ApplicationName = "创建程序信息"; //填加xls文件创建程序信息
                si.LastAuthor = "最后保存者信息"; //填加xls文件最后保存者信息
                si.Comments = "作者信息"; //填加xls文件作者信息
                si.Title = "标题信息"; //填加xls文件标题信息
                si.Subject = "主题信息";//填加文件主题信息
                si.CreateDateTime = DateTime.Now;
                workbook.SummaryInformation = si;
            }
            #endregion


            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat format = workbook.CreateDataFormat();

            dateStyle.DataFormat = format.GetFormat("yyyy-mm-dd");

            //取得列宽
            int[] arrColWidth = new int[dtSource.Columns.Count];
            foreach (DataColumn item in dtSource.Columns)
            {
                arrColWidth[item.Ordinal] = Encoding.GetEncoding(936).GetBytes(item.ColumnName.ToString()).Length;
            }
            for (int i = 0; i < dtSource.Rows.Count; i++)
            {
                for (int j = 0; j < dtSource.Columns.Count; j++)
                {
                    int intTemp = Encoding.GetEncoding(936).GetBytes(dtSource.Rows[i][j].ToString()).Length;
                    if (intTemp > arrColWidth[j])
                    {
                        arrColWidth[j] = intTemp;
                    }
                }
            }
            int rowIndex = 0;
            foreach (DataRow row in dtSource.Rows)
            {
                #region 新建表，填充表头，填充列头，样式
                if (rowIndex == 65535 || rowIndex == 0)
                {
                    if (rowIndex != 0)
                    {
                        sheet = workbook.CreateSheet();
                    }
                    #region 表头及样式
                    {
                        IRow headerRow = sheet.CreateRow(0);
                        headerRow.HeightInPoints = 25;
                        headerRow.CreateCell(0).SetCellValue(strHeaderText);

                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.CENTER;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 20;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        headerRow.GetCell(0).CellStyle = headStyle;
                        sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, dtSource.Columns.Count - 1));
                        // headerRow.Dispose();
                    }
                    #endregion


                    #region 列头及样式
                    {
                        IRow headerRow = sheet.CreateRow(1);
                        ICellStyle headStyle = workbook.CreateCellStyle();
                        headStyle.Alignment = HorizontalAlignment.CENTER;
                        IFont font = workbook.CreateFont();
                        font.FontHeightInPoints = 10;
                        font.Boldweight = 700;
                        headStyle.SetFont(font);
                        foreach (DataColumn column in dtSource.Columns)
                        {
                            headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                            headerRow.GetCell(column.Ordinal).CellStyle = headStyle;

                            //设置列宽
                            // sheet.SetColumnWidth(column.Ordinal, (arrColWidth[column.Ordinal] + 1) * 256);
                        }
                        // headerRow.Dispose();
                    }
                    #endregion

                    rowIndex = 2;
                }
                #endregion


                #region 填充内容
                IRow dataRow = sheet.CreateRow(rowIndex);
                foreach (DataColumn column in dtSource.Columns)
                {
                    ICell newCell = dataRow.CreateCell(column.Ordinal);

                    string drValue = row[column].ToString();

                    switch (column.DataType.ToString())
                    {
                        case "System.String"://字符串类型
                            newCell.SetCellValue(drValue);
                            break;
                        case "System.DateTime"://日期类型
                            DateTime dateV;
                            DateTime.TryParse(drValue, out dateV);
                            newCell.SetCellValue(dateV);

                            newCell.CellStyle = dateStyle;//格式化显示
                            break;
                        case "System.Boolean"://布尔型
                            bool boolV = false;
                            bool.TryParse(drValue, out boolV);
                            newCell.SetCellValue(boolV);
                            break;
                        case "System.Int16"://整型
                        case "System.Int32":
                        case "System.Int64":
                        case "System.Byte":
                            int intV = 0;
                            int.TryParse(drValue, out intV);
                            newCell.SetCellValue(intV);
                            break;
                        case "System.Decimal"://浮点型
                        case "System.Double":
                            double doubV = 0;
                            double.TryParse(drValue, out doubV);
                            newCell.SetCellValue(doubV);
                            break;
                        case "System.DBNull"://空值处理
                            newCell.SetCellValue("");
                            break;
                        default:
                            newCell.SetCellValue("");
                            break;
                    }

                }
                #endregion

                rowIndex++;
            }
            using (MemoryStream ms = new MemoryStream())
            {
                workbook.Write(ms);
                ms.Flush();
                ms.Position = 0;

                // sheet.Dispose();

                //workbook.Dispose();//一般只用写这一个就OK了，他会遍历并释放所有资源，但当前版本有问题所以只释放sheet
                return ms;
            }
        }

        /*
        /// <summary>
        /// 用于Web导出
        /// </summary>
        /// <param name="dtSource">源DataTable</param>
        /// <param name="strHeaderText">表头文本</param>
        /// <param name="strFileName">文件名</param>
        public static void ExportByWeb(DataTable dtSource, string strHeaderText, string strFileName)
        {
            HttpContext curContext = HttpContext.Current;

            // 设置编码和附件格式
            curContext.Response.ContentType = "application/vnd.ms-excel";
            curContext.Response.ContentEncoding = Encoding.UTF8;
            curContext.Response.Charset = "";
            curContext.Response.AppendHeader("Content-Disposition",
                "attachment;filename=" + HttpUtility.UrlEncode(strFileName, Encoding.UTF8));

            curContext.Response.BinaryWrite(Export(dtSource, strHeaderText).GetBuffer());
            curContext.Response.End();
        }

         * */

        /// <summary>读取excel
        /// 默认第一行为表头
        /// </summary>
        /// <param name="strFileName">excel文档路径</param>
        /// <returns></returns>
        public static DataTable Import(string strFileName)
        {
            DataTable dt = new DataTable();

            HSSFWorkbook hssfworkbook;
            using (FileStream file = new FileStream(strFileName, FileMode.Open, FileAccess.Read))
            {
                hssfworkbook = new HSSFWorkbook(file);
            }


            ISheet sheet = hssfworkbook.GetSheetAt(0);
            System.Collections.IEnumerator rows = sheet.GetRowEnumerator();

            IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;

            for (int j = 0; j < cellCount; j++)
            {
                ICell cell = headerRow.GetCell(j);
                dt.Columns.Add(cell.ToString());
            }
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);
                DataRow dataRow = dt.NewRow();

                for (int j = row.FirstCellNum; j < cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                        dataRow[j] = row.GetCell(j).ToString();
                }

                dt.Rows.Add(dataRow);
            }
            return dt;
        }
    }

}