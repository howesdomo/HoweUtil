using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Util.Excel
{
    public class ExcelUtils_Aspose
    {
        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">文件名称</param>
        /// <returns>DataSet 结果集</returns>
        public static DataSet Excel2DataSet(string filePath, NPOIHelperReaderConfig config = null)
        {
            DataSet result = new DataSet();

            Workbook iWorkbook = null; // TODO 修改名称


            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (filePath.EndsWith("xlsx"))
                {
                    iWorkbook = new Workbook();
                    iWorkbook.Open(fs);
                }
                else if (filePath.EndsWith("xls"))
                {
                    iWorkbook = new Workbook();
                    iWorkbook.Open(fs);
                }
                else
                {
                    throw new Exception("必须保存为Excel标准后缀（.xls, .xlsx）。");
                }

                int sheetCount = iWorkbook.Worksheets.Count; // 工作表总数
                for (int sheetNo = 0; sheetNo < sheetCount; sheetNo++)
                {
                    SheetReadConfig matchReadConfig = null;
                    Worksheet sheet = iWorkbook.Worksheets[sheetNo];

                    var TestSheeName = sheet.Name;
                    if (config != null && config.Config != null) // 跳过不读取的Sheet (名称 或 顺序)
                    {
                        // matchReadConfig = config.Config.FirstOrDefault(j => j.SheetName == sheet.SheetName || j.SheetNo == sheetNo);
                        matchReadConfig = config.Config.FirstOrDefault(j => j.SheetName == sheet.Name || j.SheetNo == sheetNo);
                        if (matchReadConfig == null)
                        {
                            continue;
                        }
                    }

                    DataTable dt = new DataTable();
                    if (sheet.Cells.Rows.Count <= 0)
                    {
                        result.Tables.Add(dt);
                        continue;
                    }

                    bool isFirstRowDefineColumn = true; // TODO 加一个配置 定义第一行是否列定义行

                    #region 表头

                    if (isFirstRowDefineColumn == true)
                    {
                        int cellRow = 0;
                        int cellColumnCount = sheet.Cells.Columns.Count;
                        Row header = sheet.Cells.Rows[0];

                        if (header == null) { continue; }

                        List<int> columns = new List<int>();
                        for (int cellColumn = 0; cellColumn < cellColumnCount; cellColumn++)
                        {
                            Cell cell = sheet.Cells[cellRow, cellColumn];
                            object obj = ExcelUtils_Aspose.GetValueType(cell);
                            if (obj == null || obj.ToString() == string.Empty)
                            {
                                dt.Columns.Add(new DataColumn("Columns" + cellColumn.ToString()));
                            }
                            else
                            {
                                dt.Columns.Add(new DataColumn(obj.ToString()));
                            }
                            columns.Add(cellColumn);

                        }
                    }

                    #endregion

                    #region 数据

                    //for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                    //{
                    //    DataRow dr = dt.NewRow();
                    //    bool rowHasValue = false; // TODO : 遇到空行 计数问题未解决
                    //    // TODO : 计算行号
                    //    foreach (int j in columns)
                    //    {
                    //        if (sheet.GetRow(i) != null && sheet.GetRow(i).GetCell(j) != null)
                    //        {
                    //            if (matchReadConfig != null && matchReadConfig.CellReadRule != null && matchReadConfig.CellReadRule.ContainsKey(j))
                    //            {
                    //                NPOICellType t = NPOICellType.Blank;
                    //                if (matchReadConfig.CellReadRule.TryGetValue(j, out t))
                    //                {
                    //                    try
                    //                    {
                    //                        switch (t)
                    //                        {
                    //                            case NPOICellType.String:
                    //                                {
                    //                                    dr[j] = sheet.GetRow(i).GetCell(j).StringCellValue;
                    //                                    if (rowHasValue != true && dr[j] != null && string.IsNullOrEmpty(dr[j].ToString()) == false)
                    //                                    {
                    //                                        rowHasValue = true;
                    //                                    }
                    //                                }
                    //                                break;
                    //                            case NPOICellType.Formula:
                    //                                {
                    //                                    dr[j] = sheet.GetRow(i).GetCell(j).CellFormula;
                    //                                    if (rowHasValue != true && dr[j] != null && string.IsNullOrEmpty(dr[j].ToString()) == false)
                    //                                    {
                    //                                        rowHasValue = true;
                    //                                    }
                    //                                }
                    //                                break;
                    //                            case NPOICellType.DateTime:
                    //                                {
                    //                                    dr[j] = sheet.GetRow(i).GetCell(j).DateCellValue.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    //                                    if (rowHasValue != true && dr[j].Equals(DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss.fff")) == false)
                    //                                    {
                    //                                        rowHasValue = true;
                    //                                    }
                    //                                }
                    //                                break;
                    //                            case NPOICellType.Blank:
                    //                                dr[j] = string.Empty;
                    //                                break;
                    //                            default:
                    //                                dr[j] = string.Empty;
                    //                                break;
                    //                        }
                    //                    }
                    //                    catch (Exception ex)
                    //                    {
                    //                        throw new Exception("NPOIHelperV3 - matchReadConfig.CellReadRule Swicth Throw Exception", ex);
                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    dr[j] = NPOIHelper.GetValueType(sheet.GetRow(i).GetCell(j) as ICell);
                    //                    if (rowHasValue != true && dr[j] != null && dr[j].ToString() != string.Empty)
                    //                    {
                    //                        rowHasValue = true;
                    //                    }
                    //                }
                    //            }
                    //            else
                    //            {
                    //                // TODO : 读取日期Cell问题仍需解决
                    //                dr[j] = NPOIHelper.GetValueType(sheet.GetRow(i).GetCell(j) as ICell);
                    //                if (rowHasValue != true && dr[j] != null && dr[j].ToString() != string.Empty)
                    //                {
                    //                    rowHasValue = true;
                    //                }
                    //            }
                    //        }
                    //    }

                    //    if (rowHasValue)
                    //    {
                    //        dt.Rows.Add(dr);
                    //    }
                    //}
                    #endregion

                    result.Tables.Add(dt);
                }
            }
            return result;
        }


        /// <summary>
        /// 获取单元格类型
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static object GetValueType(Cell cell)
        {
            if (cell == null)
            {
                return null;
            }

            switch (cell.Type)
            {
                case CellValueType.IsBool:
                    return cell.BoolValue;
                case CellValueType.IsDateTime:
                    return cell.DateTimeValue;
                case CellValueType.IsError:
                    return cell.IsErrorValue;
                case CellValueType.IsNull:
                    return null;
                case CellValueType.IsNumeric:
                    return Util.CommonDal.ReadDecimal(cell.Value);
                case CellValueType.IsString:
                    return cell.StringValue;
                case CellValueType.IsUnknown:
                default:
                    {
                        if (cell.IsFormula == true)
                        {
                            // return cell.Formula; // TODO 应该用配置
                            return cell.StringValue;
                        }
                        else
                        {
                            return null;
                        }
                    }
            }
        }

        public static DataTable Excel2DataTable(string path, int sheetNo = 0)
        {
            DataTable dt = null;
            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
            try
            {
                workbook.Open(path);
                Aspose.Cells.Worksheet worksheet = workbook.Worksheets[sheetNo];

                var maxRowIndex = worksheet.Cells.MaxDataRow; // 读取工作表中最大的行指针 ( 由0开始 )
                var maxColumnIndex = worksheet.Cells.MaxDataColumn; // 读取工作表中最大的列指针 ( 由0开始 )

                var rowsCount = maxRowIndex + 1; // 总行数
                var columnsCount = maxColumnIndex + 1; // 总列数

                Cells cells = worksheet.Cells;
                dt = cells.ExportDataTable
                (

                    firstRow: 0,
                    firstColumn: 0,
                    rowNumber: rowsCount,
                    columnNumber: columnsCount,
                    exportColumnName: true
                );
            }
            catch (Exception ex)
            {
                throw new Exception("读取Excel失败", ex);
            }

            return dt;
        }

        public static DataSet Excel2DataSet(string path)
        {
            DataSet ds = new DataSet();

            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
            try
            {
                workbook.Open(path);
                Aspose.Cells.Worksheet worksheet = workbook.Worksheets[0];
                for (int sheetCount = 0; sheetCount < workbook.Worksheets.Count; sheetCount++)
                {
                    var maxRowIndex = worksheet.Cells.MaxDataRow; // 读取工作表中最大的行指针 ( 由0开始 )
                    var maxColumnIndex = worksheet.Cells.MaxDataColumn; // 读取工作表中最大的列指针 ( 由0开始 )

                    var rowsCount = maxRowIndex + 1; // 总行数
                    var columnsCount = maxColumnIndex + 1; // 总列数

                    Cells cells = workbook.Worksheets[0].Cells;
                    DataTable dt = cells.ExportDataTable
                    (

                        firstRow: 0,
                        firstColumn: 0,
                        rowNumber: rowsCount,
                        columnNumber: columnsCount,
                        exportColumnName: true
                    );

                    ds.Tables.Add(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("读取Excel失败", ex);
            }

            return ds;
        }

        #region WorkSheet2List

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">Excel文件路径</param>
        /// <param name="objectProps">转换规则</param>
        /// <param name="worksheetIndex">获取工作表Index 默认获取第一张工作表内容( Index = 0 )</param>
        /// <param name="isContainLieDingYi">是否含有列定义行， 默认true</param>
        /// <param name="lieDingYi_RowIndex">首位列定义 RowIndex</param>
        /// <param name="lieDingYi_ColumnIndex">首位列定义 ColumnIndex</param>
        /// <returns></returns>
        public List<T> WorkSheet2List<T>(string path, List<PropertyColumn> objectProps, int worksheetIndex = 0,
            bool isContainLieDingYi = true, int lieDingYi_RowIndex = 0, int lieDingYi_ColumnIndex = 0
            ) where T : class, new()
        {
            #region 复制需要导入的文件到 \exe目录\Temp\ExcelFiles， 用于解决文件占用问题

            string copyToTempDirectory = System.IO.Path.Combine(Environment.CurrentDirectory, "Temp", "ExcelFiles");
            if (System.IO.Directory.Exists(copyToTempDirectory) == false)
            {
                System.IO.Directory.CreateDirectory(copyToTempDirectory);
            }

            string copyToTempPath = System.IO.Path.Combine(copyToTempDirectory, "{0}.{1}".FormatWith(Guid.NewGuid().ToString(), path.Substring(path.LastIndexOf(".") + 1)));
            System.IO.File.Copy(path, copyToTempPath, true);

            #endregion 复制需要导入的文件到 \exe目录\Temp\ExcelFiles， 用于解决文件占用问题

            Workbook workbook = new Aspose.Cells.Workbook();
            workbook.Open(copyToTempPath);
            Worksheet worksheet = workbook.Worksheets[worksheetIndex]; // 默认取第一张工作表
            var maxRowIndex = worksheet.Cells.MaxDataRow; // 读取工作表中最大的行指针 ( 由0开始 )
            var maxColumnIndex = worksheet.Cells.MaxDataColumn; // 读取工作表中最大的列指针 ( 由0开始 )

            var rowsCount = maxRowIndex + 1; // 总行数
            var columnsCount = maxColumnIndex + 1; // // 总列数

            StringBuilder columnNotExist = new StringBuilder();

            #region Columns

            List<PropertyColumn> header = null;

            // 若已定义 Header Index, 则跳过下面方法
            if (objectProps.Exists(i => i.ColumnIndex < 0) == true) // 获取工作表列头信息
            {
                header = this.GetHeader
                (
                    worksheet: worksheet,
                    columnCount: columnsCount,
                    lieDingYi_RowIndex: lieDingYi_RowIndex,
                    lieDingYi_ColumnIndex: lieDingYi_ColumnIndex
                );

                for (int i = 0; i < objectProps.Count; i++)
                {
                    var matchObjectProps = header.FirstOrDefault(h => h.ExcelColumn == objectProps[i].ExcelColumn);

                    if (matchObjectProps == null)
                    {
                        columnNotExist.AppendLine("找不到列【{0}】;".FormatWith(objectProps[i].ExcelColumn));
                    }
                    else
                    {
                        objectProps[i].ColumnIndex = matchObjectProps.ColumnIndex;
                        objectProps[i].ColumnIndexName = toExcelColumnName(objectProps[i].ColumnIndex);
                    }
                }
            }
            else if (isContainLieDingYi == false && objectProps.Exists(i => i.ColumnIndex < 0) == true)
            {
                columnNotExist.Append("无列定义行，且程序员未指定固定的列位置与对象的属性关联。");
            }

            #endregion

            string errorMsg = columnNotExist.ToString();
            if (errorMsg.IsNullOrEmpty() == false)
            {
                workbook = null;
                GC.Collect();
                throw new Exception(errorMsg);
            }

            List<T> result = new List<T>();

            #region Rows

            Type type = typeof(T);

            // 若 T 包含 ExcelRowNumber属性 (int)，将 Excel行号赋值给 ExcelRowNumber属性
            System.Reflection.PropertyInfo prop_ExcelRowNumber = type.GetProperty("ExcelRowNumber"); // Excel 行号

            // 若 T 包含 ExcelRowErrorInfo (string)，将 Excel 每行转换时遇到的问题赋值到本属性
            System.Reflection.PropertyInfo prop_ExcelRowErrorInfo = type.GetProperty("ExcelRowErrorInfo"); // 记录读取 Excel 的异常信息
            StringBuilder errorInfoStringBuilder = new StringBuilder(); // 每行转换时遇到的问题记录

            // TODO 若为无单头 lieDingYi_RowIndex + 1 == > lieDingYi_RowIndex + 0
            for (int rowIndex = lieDingYi_RowIndex + 1; rowIndex <= worksheet.Cells.MaxDataRow; rowIndex++)
            {
                T item = new T();

                if (prop_ExcelRowNumber != null)
                {
                    prop_ExcelRowNumber.SetValue(item, rowIndex + 1, null); // RowNumber = 指针 + 1
                }

                errorInfoStringBuilder.Clear();

                foreach (PropertyColumn itemColumn in objectProps)
                {
                    Cell cell = worksheet.Cells[rowIndex, itemColumn.ColumnIndex];

                    System.Reflection.PropertyInfo propInfo = type.GetProperty(itemColumn.PropertyName);
                    if (propInfo == null) { throw new Exception("返回结果List<{0}>，{0} 不包含有属性【{1}】。".FormatWith(type.Name, itemColumn.PropertyName)); }

                    object value = GetValueType(cell);
                    try
                    {
                        PropertyInfoSetValue(propInfo, item, value);
                    }
                    catch (Exception ex)
                    {
                        if (propInfo.PropertyType.FullName.ToString().IndexOf("System.Nullable") == 0) // 可空属性
                        {
                            propInfo.SetValue(item, null, null);
                        }
                        else
                        {
                            errorInfoStringBuilder.AppendLine("读取【{0}】发生错误（{1}）；".FormatWith(itemColumn.ExcelColumn, ex.Message));
                        }
                    }
                }

                string errorInfo = errorInfoStringBuilder.ToString();

                if (prop_ExcelRowErrorInfo != null && errorInfo.IsNullOrEmpty() == false)
                {
                    prop_ExcelRowErrorInfo.SetValue(item, errorInfo, null);
                }

                result.Add(item);
            }

            #endregion

            workbook = null;
            GC.Collect();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">Excel文件路径</param>
        /// <param name="objectProps">转换规则</param>
        /// <param name="worksheetIndex">获取工作表Index 默认获取第一张工作表内容( Index = 0 )</param>
        /// <param name="isContainLieDingYi">是否含有列定义行， 默认true</param>
        /// <param name="lieDingYi_RowIndex">首位列定义 RowIndex</param>
        /// <param name="lieDingYi_ColumnIndex">首位列定义 ColumnIndex</param>
        /// <returns></returns>
        public void WorkSheet2ListAsync<T>(Action<Task<List<T>>> actionHandler, string path, List<PropertyColumn> objectProps, int worksheetIndex = 0,
            bool isContainLieDingYi = true, int lieDingYi_RowIndex = 0, int lieDingYi_ColumnIndex = 0
            ) where T : class, new()
        {
            Task<List<T>> mTask = new System.Threading.Tasks.Task<List<T>>(() => WorkSheet2List<T>
                    (
                        path: path,
                        objectProps: objectProps,
                        worksheetIndex: worksheetIndex,
                        isContainLieDingYi: isContainLieDingYi,
                        lieDingYi_RowIndex: lieDingYi_RowIndex,
                        lieDingYi_ColumnIndex: lieDingYi_ColumnIndex
                    )
                );

            mTask.ContinueWith((task) => actionHandler(task));
            mTask.Start();

            #region (UI代码参考)下载完毕 Handler 可以参考以下代码

            //public void DownloadFileByHttpRequestAynsc_Handler(System.Threading.Tasks.Task task)
            //{
            //    string msg = "IsCanceled={0}\nIsCompleted={1}\nIsFaulted={2};"
            //    .FormatWith
            //    (
            //        task.IsCanceled,  // 因被取消而完成
            //        task.IsCompleted, // 成功完成
            //        task.IsFaulted    // 因发生异常而完成
            //    );

            //    Console.WriteLine(msg);

            //    if (task.IsFaulted == true)
            //    {
            //        if (task.Exception != null)
            //        {
            //            Console.WriteLine(task.IsFaulted);
            //            MessageBox.Show(task.Exception.GetFullInfo());
            //        }
            //        else
            //        {
            //            MessageBox.Show("下载失败。");
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("下载完成。");
            //    }
            //}

            #endregion
        }

        private void PropertyInfoSetValue<T>(PropertyInfo propInfo, T item, object value, object[] index = null) where T : class, new()
        {
            // propInfo 如果 是某些特殊类型 要进行什么处理?
            string propFullName = propInfo.PropertyType.FullName.ToString();
            if (propFullName.StartsWith("System.String") == true)
            {
                if (value == null)
                {
                    propInfo.SetValue(item, null, index);
                }
                else
                {
                    propInfo.SetValue(item, Convert.ToString(value), index);
                }
            }
            // 若是 TimeSpan, 会输出 1899 12 31 的某个时间, 在业务逻辑中再进行处理D
            //else if (propFullName.StartsWith("System.TimeSpan") == true || (propFullName.StartsWith("System.Nullable") && propFullName.Contains("System.TimeSpan")))
            //{

            //}
            else
            {
                propInfo.SetValue(item, value, index);
            }
        }

        /// <summary>
        /// 获取工作表列头信息
        /// </summary>
        /// <param name="worksheet">工作表</param>
        /// <param name="columnCount">列个数</param>
        /// <param name="lieDingYi_RowIndex">起始读取位置RowIndex，默认 0 （第一行）</param>
        /// <param name="lieDingYi_ColumnIndex">起始读取位置ColumnIndex，默认 0 （第一列）</param>
        /// <returns></returns>
        public List<PropertyColumn> GetHeader(Worksheet worksheet, int columnCount, int lieDingYi_RowIndex = 0, int lieDingYi_ColumnIndex = 0)
        {
            try
            {
                List<PropertyColumn> result = new List<PropertyColumn>();

                object[,] object2DArray = worksheet.Cells.ExportArray
                (
                    firstRow: lieDingYi_RowIndex, // 默认是第一行 ( index = 0 )
                    firstColumn: lieDingYi_ColumnIndex, // 默认是第一列 ( index = 0 )
                    rowNumber: 1, // 列定义通常是 1 行数据, 这里只取位于 firstRow, firstColumn 的一行
                    columnNumber: columnCount - lieDingYi_ColumnIndex // 列个数
                );

                for (int i = 0; i < object2DArray.Length; i++)
                {
                    PropertyColumn toAdd = new PropertyColumn()
                    {
                        ColumnIndex = lieDingYi_ColumnIndex + i,
                        ExcelColumn = Util.CommonDal.ReadString(object2DArray[0, i])
                    };

                    result.Add(toAdd);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("获取工作表列头信息发生了错误。（ExcelUtils_Aspose GetHeader）\r\n", ex);
            }

        }

        #endregion

        #region ExcelColumnName, Index 互转

        /// <summary>
        /// Excel 列名转 Index
        /// A -> 0
        /// AA -> 26
        /// </summary>
        /// <param name="excelColumnName">Excel 列名</param>
        /// <returns>Index</returns>
        private int toIndex(string excelColumnName)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(excelColumnName.ToUpper(), @"[A-Z]+"))
            {
                throw new Exception("invalid parameter");
            }
            int index = 0;
            char[] chars = excelColumnName.ToUpper().ToCharArray();
            for (int i = 0; i < chars.Length; i++)
            {
                index += ((int)chars[i] - (int)'A' + 1) * (int)Math.Pow(26, chars.Length - i - 1);
            }
            return index - 1;
        }

        /// <summary>
        /// Index 转 Excel 列名
        /// 0 -> A
        /// 26 -> AA
        /// </summary>
        /// <param name="index">Index</param>
        /// <returns>Excel 列名</returns>
        private string toExcelColumnName(int index)
        {
            if (index < 0)
            {
                throw new Exception("invalid parameter");
            }
            List<string> chars = new List<string>();
            do
            {
                if (chars.Count > 0)
                {
                    index--;
                }
                chars.Insert(0, ((char)(index % 26 + (int)'A')).ToString());
                index = (int)((index - index % 26) / 26);
            } while (index > 0);

            return String.Join(string.Empty, chars.ToArray());
        }


        #endregion














        #region 思宇 Apose 源码参考

        //public static bool ExportExcelWithAspose(DataTable dt, string path)
        //{
        //    bool succeed = false;
        //    if (dt != null)
        //    {
        //        try
        //        {
        //            Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
        //            Aspose.Cells.Worksheet cellSheet = workbook.Worksheets[0];
        //            //为单元格添加样式      
        //            Aspose.Cells.Style style = workbook.Styles[workbook.Styles.Add()];
        //            //设置居中  
        //            //style.HorizontalAlignment = Aspose.Cells.TextAlignmentType.Center;  
        //            //设置背景颜色  
        //            //style.ForegroundColor = System.Drawing.Color.FromArgb(153, 204, 0);  
        //            //style.Pattern = BackgroundType.Solid;  
        //            //style.Font.IsBold = true;  
        //            int rowIndex = 0;
        //            int colIndex = 0;
        //            int colCount = dt.Columns.Count;
        //            int rowCount = dt.Rows.Count;
        //            //列名的处理  
        //            for (int i = 0; i < colCount; i++)
        //            {
        //                cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Columns[i].ColumnName);
        //                //cellSheet.Cells[rowIndex, colIndex].Style.Font.IsBold = true;  
        //                //cellSheet.Cells[rowIndex, colIndex].Style.Font.Name = "宋体";  
        //                cellSheet.Cells[rowIndex, colIndex].Style = style;
        //                colIndex++;
        //            }
        //            rowIndex++;
        //            for (int i = 0; i < rowCount; i++)
        //            {
        //                colIndex = 0;
        //                for (int j = 0; j < colCount; j++)
        //                {
        //                    cellSheet.Cells[rowIndex, colIndex].PutValue(dt.Rows[i][j].ToString());
        //                    colIndex++;
        //                }
        //                rowIndex++;
        //            }
        //            cellSheet.AutoFitColumns();
        //            path = Path.GetFullPath(path);
        //            workbook.Save(path);
        //            succeed = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            succeed = false;
        //        }
        //    }
        //    return succeed;
        //}

        //public static DataTable ToDataTable<T>(IList<T> list, List<ExportColumn> columnList)
        //{
        //    List<string> property = new List<string>();

        //    columnList.ForEach(p =>
        //    {
        //        property.Add(p.Name);
        //    });
        //    List<string> propertyNameList = new List<string>();
        //    //property= columnList.ConvertAll(i => i.Name).ToList();
        //    if (columnList != null)
        //        propertyNameList.AddRange(property);

        //    DataTable result = new DataTable();
        //    if (list.Count > 0)
        //    {
        //        PropertyInfo[] propertys = list[0].GetType().GetProperties();
        //        foreach (PropertyInfo pi in propertys)
        //        {
        //            if (propertyNameList.Count == 0)
        //            {
        //                result.Columns.Add(pi.Name, pi.PropertyType);
        //            }
        //            else
        //            {
        //                if (propertyNameList.Contains(pi.Name))
        //                    result.Columns.Add(pi.Name, pi.PropertyType);
        //            }
        //        }

        //        for (int i = 0; i < list.Count; i++)
        //        {
        //            ArrayList tempList = new ArrayList();
        //            foreach (PropertyInfo pi in propertys)
        //            {
        //                if (propertyNameList.Count == 0)
        //                {
        //                    object obj = pi.GetValue(list[i], null);
        //                    tempList.Add(obj);
        //                }
        //                else
        //                {
        //                    if (propertyNameList.Contains(pi.Name))
        //                    {
        //                        object obj = pi.GetValue(list[i], null);
        //                        tempList.Add(obj);
        //                    }
        //                }
        //            }
        //            object[] array = tempList.ToArray();
        //            result.LoadDataRow(array, true);
        //        }
        //    }

        //    foreach (ExportColumn excol in columnList)
        //    {
        //        foreach (DataColumn dc in result.Columns)
        //        {
        //            if (excol.Name == dc.ToString())
        //            {
        //                result.Columns[dc.ToString()].ColumnName = excol.Header;

        //            }
        //        }
        //    }



        //    return result;
        //}

        #endregion

        #region Cyber Apose 源码参考

        //public class AsposeHelper_Cyber
        //{
        //    /// <summary>
        //    /// 
        //    /// </summary>
        //    /// <typeparam name="T"></typeparam>
        //    /// <param name="path"></param>
        //    /// <param name="objectProps"></param>
        //    /// <returns></returns>
        //    public List<T> Import<T>(string path, List<PropertyColumn> objectProps) where T : class, new()
        //    {
        //        Workbook workbook = new Aspose.Cells.Workbook();
        //        workbook.Open(path);
        //        Worksheet worksheet = workbook.Worksheets[0];
        //        var maxRowIndex = worksheet.Cells.MaxDataRow; // 读取工作表中最大的行指针 ( 由0开始 )
        //        var maxColumnIndex = worksheet.Cells.MaxDataColumn; // 读取工作表中最大的列指针 ( 由0开始 )

        //        var rowsCount = maxRowIndex + 1; // 总行数
        //        var columnsCount = maxColumnIndex + 1; // // 总列数

        //        StringBuilder columnNotExist = new StringBuilder();

        //        #region Columns

        //        List<PropertyColumn> header = GetHeader(worksheet, columnsCount);

        //        for (int i = 0; i < objectProps.Count; i++)
        //        {
        //            var matchObjectProps = header.FirstOrDefault(h => h.ExcelColumn == objectProps[i].ExcelColumn);

        //            if (matchObjectProps == null)
        //            {
        //                columnNotExist.AppendLine("找不到列【{0}】;".FormatWith(objectProps[i].ExcelColumn));
        //            }
        //            else
        //            {
        //                objectProps[i].ColumnIndex = matchObjectProps.ColumnIndex;
        //            }
        //        }

        //        #endregion

        //        string errorMsg = columnNotExist.ToString();
        //        if (errorMsg.IsNullOrEmpty() == false)
        //        {
        //            workbook = null;
        //            GC.Collect();
        //            throw new Exception(errorMsg);
        //        }

        //        List<T> result = new List<T>();

        //        #region Rows

        //        Type type = typeof(T);

        //        // 若 T 包含 ExcelRowNumber属性 (int)，将 Excel行号赋值给 ExcelRowNumber属性
        //        System.Reflection.PropertyInfo prop_ExcelRowNumber = type.GetProperty("ExcelRowNumber"); // Excel 行号
        //        for (int rowIndex = 1; rowIndex <= worksheet.Cells.MaxDataRow; rowIndex++) // TODO 若为无单头 rowIndex 的值要改变
        //        {
        //            T item = new T();
        //            foreach (PropertyColumn itemColumn in objectProps)
        //            {
        //                Cell cell = worksheet.Cells[rowIndex, itemColumn.ColumnIndex];
        //                System.Reflection.PropertyInfo propInfo = type.GetProperty(itemColumn.PropertyName);
        //                object value = GetValueType(cell);
        //                propInfo.SetValue(item, value, null);
        //            }

        //            if (prop_ExcelRowNumber != null)
        //            {
        //                prop_ExcelRowNumber.SetValue(item, rowIndex, null);
        //            }

        //            result.Add(item);
        //        }

        //        #endregion

        //        workbook = null;
        //        GC.Collect();
        //        return result;
        //    }

        //    /// <summary>
        //    /// 获取工作表列头信息
        //    /// </summary>
        //    /// <param name="worksheet"></param>
        //    /// <param name="maxColumnIndex"></param>
        //    /// <returns></returns>
        //    public List<PropertyColumn> GetHeader(Worksheet worksheet, int maxColumnIndex)
        //    {
        //        List<PropertyColumn> result = new List<PropertyColumn>();

        //        object[,] object2DArray = worksheet.Cells.ExportArray
        //        (
        //            firstRow: 0,
        //            firstColumn: 0,
        //            rowNumber: 1,
        //            columnNumber: maxColumnIndex
        //        );

        //        for (int i = 0; i < object2DArray.Length; i++)
        //        {
        //            PropertyColumn toAdd = new PropertyColumn()
        //            {
        //                ColumnIndex = i,
        //                ExcelColumn = Util.CommonDal.ReadString(object2DArray[0, i])
        //            };

        //            result.Add(toAdd);
        //        }

        //        return result;
        //    }

        //    /// <summary>
        //    /// 获取单元格类型
        //    /// </summary>
        //    /// <param name="cell"></param>
        //    /// <returns></returns>
        //    public static object GetValueType(Cell cell)
        //    {
        //        if (cell == null)
        //        {
        //            return null;
        //        }

        //        switch (cell.Type)
        //        {
        //            case CellValueType.IsBool:
        //                return cell.BoolValue;
        //            case CellValueType.IsDateTime:
        //                return cell.DateTimeValue;
        //            case CellValueType.IsError:
        //                return cell.IsErrorValue;
        //            case CellValueType.IsNull:
        //                return null;
        //            case CellValueType.IsNumeric:
        //                return Util.CommonDal.ReadDecimal(cell.Value);
        //            case CellValueType.IsString:
        //                return cell.StringValue;
        //            case CellValueType.IsUnknown:
        //            default:
        //                {
        //                    if (cell.IsFormula == true)
        //                    {
        //                        // return cell.Formula; // TODO 应该用配置
        //                        return cell.StringValue;
        //                    }
        //                    else
        //                    {
        //                        return null;
        //                    }
        //                }
        //        }
        //    }

        //}

        ///// <summary>
        ///// 转换器Model
        ///// Excel Column 转换 实体类
        ///// </summary>
        //public class PropertyColumn
        //{
        //    public string PropertyName
        //    {
        //        get;
        //        set;
        //    }

        //    public string ExcelColumn
        //    {
        //        get;
        //        set;
        //    }

        //    public int ColumnIndex
        //    {
        //        get;
        //        set;
        //    }
        //}

        ///// <summary>
        ///// 扩展方法
        ///// </summary>
        //public static class PropertyColumnExtension
        //{
        //    public static List<PropertyColumn> Add(this List<PropertyColumn> list, string propertyName, string excelColumn)
        //    {
        //        list.Add(new PropertyColumn()
        //        {
        //            PropertyName = propertyName,
        //            ExcelColumn = excelColumn
        //        });
        //        return list;
        //    }
        //}

        #endregion
    }
}
