using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Reflection;
using System.Collections.ObjectModel;

using NPOI.HSSF.UserModel; // 需要引用 NPOI.DLL
using NPOI.SS.UserModel; // 需要引用 NPOI.DLL
using NPOI.XSSF.UserModel; // 需要引用 NPOI.OOXML.DLL
using System.Threading.Tasks;

// XSSFWorkbook 需要引用 // 需要引用 NPOI.OpenXml4Net

/// Version   : 5
/// Author    : Howe
/// Edit Date : 2017-3-30 15:30:16
/// 
/// 5 - 修复fillSheet方法  导出Excel时, 对于null值没有进行判断就执行ToString()方法出现报错
/// 4 - 修复读取列头信息   XSSFCell --> ICell
namespace Util.Excel
{
    public class NPOIHelper : IExcelUtils
    {
        // IWorkbook 
        // HSSFWorkbook -- XLS
        // XSSFWorkbook -- XLSX

        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">文件名称</param>
        /// <returns>DataSet 结果集</returns>
        public static DataSet Excel2DataSet(string filePath, ExcelReaderConfig config = null)
        {
            DataSet result = new DataSet();

            IWorkbook iWorkbook = null;


            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (filePath.EndsWith("xlsx"))
                {
                    iWorkbook = new XSSFWorkbook(fs);
                }
                else if (filePath.EndsWith("xls"))
                {
                    iWorkbook = new NPOI.HSSF.UserModel.HSSFWorkbook(fs);
                }
                else
                {
                    throw new Exception("必须保存为Excel标准后缀（.xls, .xlsx）。");
                }

                int sheetCount = iWorkbook.NumberOfSheets; // 工作表总数
                for (int sheetNo = 0; sheetNo < sheetCount; sheetNo++)
                {
                    SheetReadConfig matchReadConfig = null;
                    ISheet sheet = iWorkbook.GetSheetAt(sheetNo);

                    if (config != null && config.Config != null)
                    {
                        matchReadConfig = config.Config.FirstOrDefault(j => j.SheetName == sheet.SheetName || (j.SheetIndex.HasValue == true && j.SheetIndex == sheetNo));
                        if (matchReadConfig == null)
                        {
                            continue;
                        }
                    }

                    DataTable dt = new DataTable();

                    #region 表头

                    IRow header = sheet.GetRow(sheet.FirstRowNum);
                    if (header == null) { continue; }

                    List<int> columns = new List<int>();
                    for (int i = 0; i < header.LastCellNum; i++)
                    {
                        if (header.GetCell(i) != null)
                        {
                            object obj = NPOIHelper.GetValueType(header.GetCell(i) as ICell);
                            if (obj == null || obj.ToString() == string.Empty)
                            {
                                dt.Columns.Add(new DataColumn("Columns" + i.ToString()));
                                //continue;
                            }
                            else
                            {
                                dt.Columns.Add(new DataColumn(obj.ToString()));
                            }
                            columns.Add(i);
                        }
                    }
                    #endregion

                    #region 数据

                    for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                    {
                        DataRow dr = dt.NewRow();
                        bool rowHasValue = false; // TODO : 遇到空行 计数问题未解决
                        // TODO : 计算行号
                        foreach (int j in columns)
                        {
                            if (sheet.GetRow(i) != null && sheet.GetRow(i).GetCell(j) != null)
                            {
                                if (matchReadConfig != null && matchReadConfig.CellReadRule != null && matchReadConfig.CellReadRule.ContainsKey(j))
                                {
                                    CellType t = CellType.Blank;
                                    if (matchReadConfig.CellReadRule.TryGetValue(j, out t))
                                    {
                                        try
                                        {
                                            switch (t)
                                            {
                                                case CellType.String:
                                                    {
                                                        dr[j] = sheet.GetRow(i).GetCell(j).StringCellValue;
                                                        if (rowHasValue != true && dr[j] != null && string.IsNullOrEmpty(dr[j].ToString()) == false)
                                                        {
                                                            rowHasValue = true;
                                                        }
                                                    }
                                                    break;
                                                case CellType.Formula:
                                                    {
                                                        dr[j] = sheet.GetRow(i).GetCell(j).CellFormula;
                                                        if (rowHasValue != true && dr[j] != null && string.IsNullOrEmpty(dr[j].ToString()) == false)
                                                        {
                                                            rowHasValue = true;
                                                        }
                                                    }
                                                    break;
                                                case CellType.DateTime:
                                                    {
                                                        dr[j] = sheet.GetRow(i).GetCell(j).DateCellValue.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                                        if (rowHasValue != true && dr[j].Equals(DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss.fff")) == false)
                                                        {
                                                            rowHasValue = true;
                                                        }
                                                    }
                                                    break;
                                                case CellType.Blank:
                                                    dr[j] = string.Empty;
                                                    break;
                                                default:
                                                    dr[j] = string.Empty;
                                                    break;
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            throw new Exception("NPOIHelperV3 - matchReadConfig.CellReadRule Swicth Throw Exception", ex);
                                        }
                                    }
                                    else
                                    {
                                        dr[j] = NPOIHelper.GetValueType(sheet.GetRow(i).GetCell(j) as ICell);
                                        if (rowHasValue != true && dr[j] != null && dr[j].ToString() != string.Empty)
                                        {
                                            rowHasValue = true;
                                        }
                                    }
                                }
                                else
                                {
                                    // TODO : 读取日期Cell问题仍需解决
                                    dr[j] = NPOIHelper.GetValueType(sheet.GetRow(i).GetCell(j) as ICell);
                                    if (rowHasValue != true && dr[j] != null && dr[j].ToString() != string.Empty)
                                    {
                                        rowHasValue = true;
                                    }
                                }
                            }
                        }

                        if (rowHasValue)
                        {
                            dt.Rows.Add(dr);
                        }
                    }
                    #endregion

                    result.Tables.Add(dt);
                }
            }
            return result;
        }

        /// <summary>
        /// 导出Excel
        /// 将 DataTable 内容导出到Excel文件中
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="dataSetName">默认为：DataSet1</param>
        /// <returns>导出结果</returns>
        public static bool DataTable2Excel(DataTable dt, string filePath, string sheetName = "Sheet1")
        {
            bool isSucceed = false;
            IWorkbook iWorkbook = null;
            if (filePath.EndsWith("xlsx"))
            {
                iWorkbook = new XSSFWorkbook();
            }
            else if (filePath.EndsWith("xls"))
            {
                iWorkbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            }
            else
            {
                throw new Exception("必须保存为Excel标准后缀（.xls, .xlsx）。");
            }

            ISheet workSheet = iWorkbook.CreateSheet(sheetName);

            //表头
            IRow headerRow = workSheet.CreateRow(0);
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                ICell cell = headerRow.CreateCell(i);
                cell.SetCellValue(dt.Columns[i].ColumnName);
            }

            //数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IRow dataRow = workSheet.CreateRow(i + 1);
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    ICell cell = dataRow.CreateCell(j);
                    cell.SetCellValue(dt.Rows[i][j].ToString());
                }
            }

            using (FileStream fOut = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite)) // 写入流
            {
                fOut.Flush();
                iWorkbook.Write(fOut);//写入文件
                isSucceed = true;
            }

            iWorkbook = null;
            return isSucceed;
        }

        /// <summary>
        /// 导出Excel
        /// 将 DataTable 内容导出到Excel文件中
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="filePath">文件路径</param>
        /// <param name="dataSetName">默认为：DataSet1</param>
        /// <returns>导出结果</returns>
        public static bool DataSet2Excel(DataSet ds, string filePath, ExcelWriterConfig config = null)
        {
            bool isSucceed = false;
            IWorkbook iWorkbook = null;
            if (filePath.EndsWith("xlsx"))
            {
                iWorkbook = new XSSFWorkbook();
            }
            else if (filePath.EndsWith("xls"))
            {
                iWorkbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
            }
            else
            {
                throw new Exception("必须保存为Excel标准后缀（.xls, .xlsx）。");
            }

            int sheetCount = 1;
            foreach (DataTable dt in ds.Tables)
            {
                string sheetName = string.Format("Sheet{0}", sheetCount.ToString());
                if (config != null && config.SheetNameList != null && config.SheetNameList.Count >= sheetCount)
                {
                    sheetName = config.SheetNameList[sheetCount - 1];
                }
                sheetCount = sheetCount + 1;

                ISheet workSheet = iWorkbook.CreateSheet(sheetName);
                //表头
                IRow headerRow = workSheet.CreateRow(0);
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    ICell cell = headerRow.CreateCell(i);
                    cell.SetCellValue(dt.Columns[i].ColumnName);
                }

                //数据
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    IRow dataRow = workSheet.CreateRow(i + 1);
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        ICell cell = dataRow.CreateCell(j);
                        cell.SetCellValue(dt.Rows[i][j].ToString());
                    }
                }
            }

            using (FileStream fOut = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite)) // 写入流
            {
                fOut.Flush();
                iWorkbook.Write(fOut);//写入文件
                isSucceed = true;
            }

            iWorkbook = null;
            return isSucceed;
        }

        // 由于 DataGridColumn 需要 WPF 的才能使用 暂时注释
        ///// <summary>
        ///// 导出Excel
        ///// 将表格控件的表头和内容导出到Excel文件
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="filePath">保存路径</param>
        ///// <param name="dgColumns">表格控件表头</param>
        ///// <param name="entitys">数据源</param>
        ///// <param name="isAddInExistWorksheets">追加内容到Excel文件中</param>
        ///// <returns>导出结果</returns>
        //public static bool ExportToExcel<T>(string filePath, ObservableCollection<DataGridColumn> dgColumns, List<T> entitys, bool isAddInExistWorksheets = false)
        //{
        //    List<ExportColumn> transDgColumns = new List<ExportColumn>();
        //    foreach (DataGridColumn column in dgColumns)
        //    {
        //        ExportColumn ecModel = new ExportColumn();

        //        System.Windows.Data.Binding columnPath = column.ClipboardContentBinding as System.Windows.Data.Binding;
        //        ecModel.Name = columnPath.Path.Path;
        //        ecModel.Header = column.Header.ToString();

        //        transDgColumns.Add(ecModel);
        //    }

        //    return NPOIHelper.ExportToExcelForAll<T>(filePath, transDgColumns, entitys, isAddInExistWorksheets);
        //}

        /// <summary>
        /// 导出Excel
        /// 将自定义的表头和内容导出到Excel文件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">保存路径</param>
        /// <param name="dgColumns">自定义表头集合</param>
        /// <param name="entitys">数据源</param>
        /// <param name="isAddInExistWorksheets">追加内容到Excel文件中</param>
        /// <returns>导出结果</returns>
        public static bool ExportToExcelForAll<T>(string filePath, IEnumerable<ExportColumn> dgColumns, List<T> entitys, bool isAddInExistWorksheets = false)
        {
            if (entitys == null) { throw new Exception("导出数据为 null。"); }

            bool isSucceed = false; // 导出结果

            IWorkbook iworkbook;
            if (isAddInExistWorksheets == false) // 覆盖已存在的Excel文件
            {
                if (filePath.EndsWith("xlsx", StringComparison.InvariantCultureIgnoreCase))
                {
                    iworkbook = new XSSFWorkbook();
                }
                else if (filePath.EndsWith("xls", StringComparison.InvariantCultureIgnoreCase))
                {
                    iworkbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
                }
                else
                {
                    throw new Exception("必须保存为Excel标准后缀（.xls, .xlsx）。");
                }

                ISheet sheet = iworkbook.CreateSheet();

                NPOIHelper.fillSheet<T>(sheet, dgColumns, entitys); // 填充工作表

                using (FileStream fOut = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite)) // 写入流
                {
                    fOut.Flush();
                    iworkbook.Write(fOut);//写入文件
                    isSucceed = true;
                }
            }
            else // 向已存在的Excel文件中追加内容
            {
                if (!File.Exists(filePath)) // 追加的文件不存在
                {
                    return NPOIHelper.ExportToExcelForAll<T>(filePath, dgColumns, entitys, false);
                }

                using (FileStream fRead = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) // 读取流
                {
                    if (filePath.EndsWith("xlsx", StringComparison.InvariantCultureIgnoreCase))
                    {
                        iworkbook = new XSSFWorkbook(fRead);
                    }
                    else if (filePath.EndsWith("xls", StringComparison.InvariantCultureIgnoreCase))
                    {
                        iworkbook = new NPOI.HSSF.UserModel.HSSFWorkbook(fRead);
                    }
                    else
                    {
                        throw new Exception("必须保存为Excel标准后缀（.xls, .xlsx）。");
                    }
                }
                ISheet sheet = iworkbook.GetSheetAt(0); // 默认追加到第一个工作表
                NPOIHelper.fillSheet<T>(sheet, dgColumns, entitys, true); // 填充工作表
                using (FileStream fOut = new FileStream(filePath, FileMode.Open, FileAccess.Write, FileShare.ReadWrite)) // 写入流
                {
                    fOut.Flush();
                    iworkbook.Write(fOut);//写入文件
                    isSucceed = true;
                }
            }
            iworkbook = null;
            return isSucceed;
        }

        // 通用方法
        /// <summary>
        /// 填充工作表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="worksheet">填充的工作表</param>
        /// <param name="dgColumns">Excel列头信息</param>
        /// <param name="entitys">Excel文档内容</param>
        /// <param name="isAppend">追加到现有文件</param>
        private static void fillSheet<T>(ISheet worksheet, IEnumerable<ExportColumn> dgColumns, List<T> entitys, bool isAppend = false)
        {
            Type entityType = entitys[0].GetType();
            List<PropertyInfo> entityProperties = new List<PropertyInfo>(entityType.GetProperties());

            if (isAppend == false)
            {
                // 列头 赋值
                IRow headerRow = worksheet.CreateRow(0);
                int headerColumnNo = 0;
                foreach (ExportColumn column in dgColumns)
                {
                    headerRow.CreateCell(headerColumnNo++).SetCellValue(column.Header);
                }
            }

            #region 校验列头信息

            if (isAppend && dgColumns != null && dgColumns.Count() > 0) // 追加到现有文件且含有列头信息
            {
                StringBuilder sb = new StringBuilder();
                List<ExportColumn> dgList = dgColumns.ToList();
                IRow oldHeaderRow = worksheet.GetRow(0);
                // for (int i = 0; i < oldHeaderRow.LastCellNum; i++)
                for (int i = 0; i < dgList.Count(); i++)
                {
                    ICell currentCell = oldHeaderRow.GetCell(i) as ICell;
                    string expectationColumnName = string.Empty;
                    string currentColumnName = string.Empty;
                    bool isColumnEquals = false;
                    try
                    {
                        expectationColumnName = string.Format("【{0}】", dgList[i].Header);
                        if (dgList[i].Header.Equals(currentCell.StringCellValue, StringComparison.InvariantCultureIgnoreCase) == false)
                        {
                            currentColumnName = string.Format("【{0}】", currentCell.StringCellValue);
                        }
                        else
                        {
                            isColumnEquals = true;
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        // 对于超出索引报错不进行提示
                        // ex1 => 索引超出范围。必须为非负值并小于集合大小。
                    }
                    catch (NullReferenceException)
                    {
                        currentColumnName = "空列";
                    }

                    if (isColumnEquals == false)
                    {
                        sb.Append(string.Format("第 {0} 列必须是{1}，实际是{2};\n", (i + 1).ToString(), expectationColumnName, currentColumnName));
                    }
                }

                if (!string.IsNullOrWhiteSpace(sb.ToString()))
                {
                    throw new Exception(string.Format("追加文件中列头不符合要求。\n{0}", sb.ToString()));
                }
            }

            #endregion

            #region 内容 赋值

            int rowIndex = worksheet.LastRowNum + 1;
            foreach (T entity in entitys)
            {
                IRow dataRow = worksheet.CreateRow(rowIndex);
                int columnIndex = 0;
                foreach (var column in dgColumns)
                {
                    PropertyInfo matchPropertyInfo = entityProperties.FirstOrDefault(p => p.Name == column.Name);
                    if (matchPropertyInfo != null)
                    {
                        object obj = matchPropertyInfo.GetValue(entity, null);
                        if (obj == null)
                        {
                            dataRow.CreateCell(columnIndex).SetCellValue(string.Empty);
                        }
                        else
                        {
                            dataRow.CreateCell(columnIndex).SetCellValue(obj.ToString());
                        }
                    }
                    columnIndex++;
                }
                rowIndex++;
            }

            #endregion
        }

        #region GetValueType

        /// <summary>
        /// 获取单元格类型
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static object GetValueType(ICell cell)
        {
            if (cell == null)
            {
                return null;
            }

            switch (cell.CellType)
            {
                case NPOI.SS.UserModel.CellType.Blank:
                    return null;
                case NPOI.SS.UserModel.CellType.Boolean:
                    return cell.BooleanCellValue;
                case NPOI.SS.UserModel.CellType.Numeric:
                    return cell.NumericCellValue;
                case NPOI.SS.UserModel.CellType.String:
                    return cell.StringCellValue;
                case NPOI.SS.UserModel.CellType.Error:
                    return cell.ErrorCellValue;
                case NPOI.SS.UserModel.CellType.Formula: // 客户Excel的公式读取值即可
                    // 读取值
                    {
                        string errorMsg = string.Empty;
                        object result = null;
                        try
                        {
                            result = cell.NumericCellValue;
                        }
                        catch (Exception ex)
                        {
                            errorMsg = "{0}，错误信息\r\n{1}\r\n".FormatWith("以NumericCellValue读取公式发生错误", ex.Message);
                        }

                        if (result == null)
                        {
                            try
                            {
                                result = cell.StringCellValue;
                            }
                            catch (Exception ex)
                            {
                                errorMsg = "{0}，错误信息\r\n{1}\r\n".FormatWith("以StringCellValue读取公式发生错误", ex.Message);
                            }
                        }

                        if (result == null)
                        {
                            throw new Exception(errorMsg);
                        }

                        return result;
                    }
                // 读取公式
                // return cell.CellFormula; 
                default:
                    return "=" + cell.CellFormula;
            }
        }

        #endregion

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
        /// <param name="ignoreRepeatColumnName">不检测重复列</param>
        /// <returns></returns>
        public List<T> WorkSheet2List<T>(
            string path,
            List<PropertyColumn> objectProps,
            int worksheetIndex = 0,
            bool isContainLieDingYi = true,
            int lieDingYi_RowIndex = 0, int lieDingYi_ColumnIndex = 0,
            bool ignoreRepeatColumnName = false
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

            // Workbook workbook = new Aspose.Cells.Workbook(copyToTempPath);
            IWorkbook workbook = null;
            using (FileStream fs = new FileStream(copyToTempPath, FileMode.Open, FileAccess.Read))
            {
                if (copyToTempPath.EndsWith("xlsx"))
                {
                    workbook = new XSSFWorkbook(fs);
                }
                else if (copyToTempPath.EndsWith("xls"))
                {
                    workbook = new NPOI.HSSF.UserModel.HSSFWorkbook(fs);
                }
                else
                {
                    throw new Exception("必须保存为Excel标准后缀（.xls, .xlsx）。");
                }
            }

            // Worksheet worksheet = workbook.Worksheets[worksheetIndex]; // 默认取第一张工作表
            ISheet worksheet = workbook.GetSheetAt(worksheetIndex);

            // var maxRowIndex = worksheet.Cells.MaxDataRow; // 读取工作表中最大的行指针 ( 由0开始 )
            var maxRowIndex = worksheet.LastRowNum - 1;
            // var maxColumnIndex = worksheet.Cells.MaxDataColumn; // 读取工作表中最大的列指针 ( 由0开始 )
            IRow headerRow = worksheet.GetRow(lieDingYi_ColumnIndex);
            var maxColumnIndex = headerRow.LastCellNum;


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
                    lieDingYi_ColumnIndex: lieDingYi_ColumnIndex,
                    ignoreRepeatColumnName: ignoreRepeatColumnName
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
                        objectProps[i].ColumnIndexName = Util.Excel.ExcelCommonMethod.ToExcelColumnName(objectProps[i].ColumnIndex);
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

            // 若为无单头, for ( lieDingYi_RowIndex + 1 == >  for ( lieDingYi_RowIndex + 0
            if (isContainLieDingYi == false)
            {
                lieDingYi_RowIndex = lieDingYi_RowIndex - 1;
            }

            // for (int rowIndex = lieDingYi_RowIndex + 1; rowIndex <= worksheet.Cells.MaxDataRow; rowIndex++)
            for (int rowIndex = lieDingYi_RowIndex + 1; rowIndex <= maxRowIndex; rowIndex++)
            {
                T item = new T();

                if (prop_ExcelRowNumber != null)
                {
                    prop_ExcelRowNumber.SetValue(item, rowIndex + 1, null); // RowNumber = 指针 + 1
                }

                errorInfoStringBuilder.Clear();

                foreach (PropertyColumn itemColumn in objectProps)
                {
                    // Cell cell = worksheet.Cells[rowIndex, itemColumn.ColumnIndex];
                    ICell cell = worksheet.GetRow(rowIndex).GetCell(itemColumn.ColumnIndex);

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
            bool isContainLieDingYi = true, int lieDingYi_RowIndex = 0, int lieDingYi_ColumnIndex = 0, bool ignoreRepeatColumnName = false
            ) where T : class, new()
        {
            Task<List<T>> mTask = new System.Threading.Tasks.Task<List<T>>(() => WorkSheet2List<T>
                    (
                        path: path,
                        objectProps: objectProps,
                        worksheetIndex: worksheetIndex,
                        isContainLieDingYi: isContainLieDingYi,
                        lieDingYi_RowIndex: lieDingYi_RowIndex,
                        lieDingYi_ColumnIndex: lieDingYi_ColumnIndex,
                        ignoreRepeatColumnName: ignoreRepeatColumnName
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
                    string v = Convert.ToString(value);
                    v = v.Trim();
                    propInfo.SetValue(item, v, index);
                }
            }
            // 若是 TimeSpan, 会输出 1899 12 31 的某个时间, 在业务逻辑中再进行处理D
            //else if (propFullName.StartsWith("System.TimeSpan") == true || (propFullName.StartsWith("System.Nullable") && propFullName.Contains("System.TimeSpan")))
            //{

            //}
            else
            {
                try
                {
                    propInfo.SetValue(item, value, index);
                }
                catch (System.ArgumentException argEx)
                {
                    // 需要慢慢完善
                    // 处理Excel文本格式无法 setValue的问题
                    // 报错信息 "类型“System.String”的对象无法转换为类型“System.Decimal”。"
                    int convertToXIndex = argEx.Message.LastIndexOf("System.");

                    string convertToX = argEx.Message.Substring(convertToXIndex);
                    convertToX = convertToX
                        .Replace(',', ' ')
                        .Replace('"', ' ')
                        .Replace('”', ' ')
                        .Replace('。', ' ')
                        ;

                    convertToX = convertToX.TrimAdv();
                    switch (convertToX.ToUpper())
                    {
                        case "SYSTEM.DECIMAL":
                            {
                                decimal valueAfterConvert = Convert.ToDecimal(value);
                                propInfo.SetValue(item, valueAfterConvert, index);
                            }
                            break;

                        default:
                            throw argEx;
                    }
                }
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
        public List<PropertyColumn> GetHeader(ISheet worksheet, int columnCount, int lieDingYi_RowIndex = 0, int lieDingYi_ColumnIndex = 0, bool ignoreRepeatColumnName = false)
        {
            string errorMsg = string.Empty;

            try
            {
                List<PropertyColumn> result = new List<PropertyColumn>();

                //object[,] object2DArray = worksheet.Cells.ExportArray
                //(
                //    firstRow: lieDingYi_RowIndex, // 默认是第一行 ( index = 0 )
                //    firstColumn: lieDingYi_ColumnIndex, // 默认是第一列 ( index = 0 )
                //    totalRows: 1, // 列定义通常是 1 行数据, 这里只取位于 firstRow, firstColumn 的一行
                //    totalColumns: columnCount - lieDingYi_ColumnIndex // 列个数
                //);



                // for (int i = 0; i < object2DArray.Length; i++)
                for (int i = 0; i < columnCount; i++)
                {

                    var cell = worksheet.GetRow(lieDingYi_RowIndex).GetCell(lieDingYi_ColumnIndex + i);

                    PropertyColumn toAdd = new PropertyColumn()
                    {
                        ColumnIndex = lieDingYi_ColumnIndex + i,
                        ExcelColumn = Util.CommonDal.ReadString(worksheet.GetRow(lieDingYi_RowIndex).GetCell(lieDingYi_ColumnIndex + i))
                    };

                    result.Add(toAdd);
                }

                var query_group_by = (from i in result
                                      group i by new { i.ExcelColumn } into ix
                                      select new
                                      {
                                          ExcelColumn = ix.Key.ExcelColumn,
                                          OccurTimes = ix.Count()
                                      }).ToList();


                foreach (var item in query_group_by.Where(i => i.OccurTimes > 1))
                {
                    if (errorMsg.IsNullOrEmpty() == false)
                    {
                        errorMsg += "\r\n";
                    }

                    errorMsg += item.ExcelColumn + ";";
                }

                if (errorMsg.IsNullOrEmpty() == false)
                {
                    if (ignoreRepeatColumnName == false)
                    {
                        throw new Exception("列定义出现重复。\r\n" + errorMsg);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                if (errorMsg.IsNullOrEmpty() == false)
                {
                    throw new Exception("列定义出现重复。\r\n" + errorMsg);
                }
                else
                {
                    throw new Exception("获取工作表列头信息发生了错误。（ExcelUtils_Aspose GetHeader）\r\n", ex);
                }
            }
        }

        #endregion

        public DataSet Excel2DataSetStepByStep(string path, ExcelReaderConfig config = null)
        {
            throw new NotImplementedException();
        }

        public void DataSet2ExcelStepByStep(string path, DataSet dataSet, bool[] showColumnNameArray = null, int[,] positionArray = null)
        {
            throw new NotImplementedException();
        }

    }

}
