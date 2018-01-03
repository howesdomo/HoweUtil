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

// XSSFWorkbook 需要引用 // 需要引用 NPOI.OpenXml4Net

/// Version   : 5
/// Author    : Howe
/// Edit Date : 2017-3-30 15:30:16
/// 
/// 5 - 修复fillSheet方法  导出Excel时, 对于null值没有进行判断就执行ToString()方法出现报错
/// 4 - 修复读取列头信息   XSSFCell --> ICell
namespace Util.Excel
{
    public enum NPOICellType
    {
        Blank = 0, // 留空不读
        String = 1, // 读取数值
        DateTime = 2, // 读取日期
        Formula = 3, // 读取公式
    }

    /// <summary>
    /// 配置
    /// 
    /// </summary>
    public class SheetReadConfig
    {
        public int SheetNo { get; set; }

        public string SheetName { get; set; }

        /// <summary>
        /// 指定特殊的单元格读取规则
        /// </summary>
        public Dictionary<int, NPOICellType> CellReadRule { get; set; }
    }

    public class NPOIHelperReaderConfig
    {
        /// <summary>
        /// 填写上SheetNo 或 SheetName 的工作表才被读取, 其中若填入对应Column的读取规则, 按照规则进行读取
        /// </summary>
        public List<SheetReadConfig> Config { get; set; }
    }

    public class NPOIHelperWriterConfig
    {
        /// <summary>
        /// NPOI写入模式
        /// 0 全部重写,
        /// 1 追加
        /// </summary>
        public NPOIHelperWriteModel WriteMode { get; set; }

        /// <summary>
        /// 重命名SheetName
        /// </summary>
        public List<string> SheetNameList { get; set; }
    }

    public enum NPOIHelperWriteModel
    {
        CreateNew = 0,
        Append = 1
    }




    public class NPOIHelper
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
        public static DataSet Excel2DataSet(string filePath, NPOIHelperReaderConfig config = null)
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
                        matchReadConfig = config.Config.FirstOrDefault(j => j.SheetName == sheet.SheetName || j.SheetNo == sheetNo);
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
                                dt.Columns.Add(new DataColumn(obj.ToString()));
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
                                    NPOICellType t = NPOICellType.Blank;
                                    if (matchReadConfig.CellReadRule.TryGetValue(j, out t))
                                    {
                                        try
                                        {
                                            switch (t)
                                            {
                                                case NPOICellType.String:
                                                    {
                                                        dr[j] = sheet.GetRow(i).GetCell(j).StringCellValue;
                                                        if (rowHasValue != true && dr[j] != null && string.IsNullOrEmpty(dr[j].ToString()) == false)
                                                        {
                                                            rowHasValue = true;
                                                        }
                                                    }
                                                    break;
                                                case NPOICellType.Formula:
                                                    {
                                                        dr[j] = sheet.GetRow(i).GetCell(j).CellFormula;
                                                        if (rowHasValue != true && dr[j] != null && string.IsNullOrEmpty(dr[j].ToString()) == false)
                                                        {
                                                            rowHasValue = true;
                                                        }
                                                    }
                                                    break;
                                                case NPOICellType.DateTime:
                                                    {
                                                        dr[j] = sheet.GetRow(i).GetCell(j).DateCellValue.ToString("yyyy-MM-dd HH:mm:ss.fff");
                                                        if (rowHasValue != true && dr[j].Equals(DateTime.MinValue.ToString("yyyy-MM-dd HH:mm:ss.fff")) == false)
                                                        {
                                                            rowHasValue = true;
                                                        }
                                                    }
                                                    break;
                                                case NPOICellType.Blank:
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
        public static bool DataSet2Excel(DataSet ds, string filePath, NPOIHelperWriterConfig config = null)
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
                case CellType.Blank:
                    return null;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.Numeric:
                    return cell.NumericCellValue;
                case CellType.String:
                    return cell.StringCellValue;
                case CellType.Error:
                    return cell.ErrorCellValue;
                case CellType.Formula: // 客户Excel的公式读取值即可
                    return cell.NumericCellValue; // 读取值
                // return cell.CellFormula; // 读取公式
                default:
                    return "=" + cell.CellFormula;
            }
        }

        #endregion

    }

    /// <summary>
    /// ExportColumn
    /// 导出Excel时使用
    /// </summary>
    public class ExportColumn
    {
        public string Name { get; set; }

        public string Header { get; set; }
    }
}
