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
    public class ExcelUtils_Aspose : IExcelUtils
    {
        #region **** Hot Patch 破解方式 **** 

        /// <summary>
        /// Aspose 8.6.3 Hot Patch 破解方式
        /// 
        /// Winform, WPF 请在 OnStartup 中调用此方法
        /// Web程序, 请在全局应用类(Global.asax)中 Application_Start 方法中调用此方法
        /// </summary>
        public static void InitializeAsposeCells()
        {
            const BindingFlags BINDING_FLAGS_ALL = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            const string CLASS_LICENSER = "\u0092\u0092\u0008.\u001C";
            const string CLASS_LICENSERHELPER = "\u0011\u0001\u0006.\u001A";
            const string ENUM_ISTRIAL = "\u0092\u0092\u0008.\u001B";

            const string FIELD_LICENSER_CREATED_LICENSE = "\u0001";     // static
            const string FIELD_LICENSER_EXPIRY_DATE = "\u0002";         // instance
            const string FIELD_LICENSER_ISTRIAL = "\u0001";             // instance

            const string FIELD_LICENSERHELPER_INT128 = "\u0001";        // static
            const string FIELD_LICENSERHELPER_BOOLFALSE = "\u0001";     // static

            const int CONST_LICENSER_ISTRIAL = 1;
            const int CONST_LICENSERHELPER_INT128 = 128;
            const bool CONST_LICENSERHELPER_BOOLFALSE = false;

            //- Field setter for convinient
            Action<FieldInfo, Type, string, object, object> setValue =
                delegate (FieldInfo field, Type chkType, string chkName, object obj, object value)
                {
                    if ((field.FieldType == chkType) && (field.Name == chkName))
                    {
                        field.SetValue(obj, value);
                    }
                };


            //- Get types
            Assembly assembly = Assembly.GetAssembly(typeof(Aspose.Cells.License));
            Type typeLic = null, typeIsTrial = null, typeHelper = null;
            foreach (Type type in assembly.GetTypes())
            {
                if ((typeLic == null) && (type.FullName == CLASS_LICENSER))
                {
                    typeLic = type;
                }
                else if ((typeIsTrial == null) && (type.FullName == ENUM_ISTRIAL))
                {
                    typeIsTrial = type;
                }
                else if ((typeHelper == null) && (type.FullName == CLASS_LICENSERHELPER))
                {
                    typeHelper = type;
                }
            }
            if (typeLic == null || typeIsTrial == null || typeHelper == null)
            {
                throw new Exception();
            }

            //- In class_Licenser
            object license = Activator.CreateInstance(typeLic);
            foreach (FieldInfo field in typeLic.GetFields(BINDING_FLAGS_ALL))
            {
                setValue(field, typeLic, FIELD_LICENSER_CREATED_LICENSE, null, license);
                setValue(field, typeof(DateTime), FIELD_LICENSER_EXPIRY_DATE, license, DateTime.MaxValue);
                setValue(field, typeIsTrial, FIELD_LICENSER_ISTRIAL, license, CONST_LICENSER_ISTRIAL);
            }

            //- In class_LicenserHelper
            foreach (FieldInfo field in typeHelper.GetFields(BINDING_FLAGS_ALL))
            {
                setValue(field, typeof(int), FIELD_LICENSERHELPER_INT128, null, CONST_LICENSERHELPER_INT128);
                setValue(field, typeof(bool), FIELD_LICENSERHELPER_BOOLFALSE, null, CONST_LICENSERHELPER_BOOLFALSE);
            }
        }

        /// <summary>
        /// 测试Hot Patch破解方式是否成功
        /// 检验 X 次, 查看是否含有未注册的Worksheet出现
        /// </summary>
        /// <returns></returns>
        public static string TestAsposeCellsHotPatch(string pathTemplate = null, int totalTestCount = 30)
        {
            if (pathTemplate.IsNullOrEmpty())
            {
                pathTemplate = @"C:\Test\TestAspose{0}.xlsx";
            }
            string path = pathTemplate.FormatWith(0);
            string exportPath = string.Empty;

            List<string> headList = new List<string>();
            headList.Add("HLD");
            headList.Add("HSN");
            headList.Add("HSW");

            Random random = new Random();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < totalTestCount; i++)
            {
                if (i % 100 == 0)
                {
                    GC.Collect();
                }

                Aspose.Cells.Workbook wb = null;

                if (System.IO.File.Exists(path) == false)
                {
                    wb = new Aspose.Cells.Workbook();
                }
                else
                {
                    wb = new Aspose.Cells.Workbook(path);
                }

                if (wb != null)
                {
                    bool isLicensed = wb.IsLicensed;
                    if (isLicensed == false)
                    {
                        sb.AppendLine("Run Time {0} : isLicensed = false;".FormatWith(i));
                    }

                    if (wb.Worksheets.Count > 1)
                    {
                        sb.AppendLine("Run Time {0} : Worksheets Count = {1};".FormatWith(i, wb.Worksheets.Count));
                    }

                    var ws = wb.Worksheets[0];
                    var cell0 = ws.Cells[i, 0];
                    cell0.Value = i;

                    var cell1 = ws.Cells[i, 1];
                    cell1.Value = "{0}ABC".FormatWith(i);

                    var cell2 = ws.Cells[i, 2];

                    int randomIndex = random.Next(3);
                    cell2.Value = "{0}{1}".FormatWith(headList[randomIndex], random.Next(1000000).ToString().PadLeft(9, '0'));

                    var cell3 = ws.Cells[i, 3];
                    string formula = "=CHOOSE(MATCH(MID(C" + (i + 1).ToString() + ",1,3),{\"HLD\",\"HSN\",\"HSW\"}),\"123HLD\",\"123HSN\",\"123HSW\")";
                    cell3.SetFormula(formula, headList[randomIndex]);

                    var cell4 = ws.Cells[i, 4];
                    cell4.SetFormula(formula, headList[randomIndex]);

                    exportPath = pathTemplate.FormatWith(i + 1);
                    wb.Save(exportPath);
                    wb.Dispose();

                    using (Aspose.Cells.Workbook checkFormula = new Aspose.Cells.Workbook(exportPath))
                    {
                        var checkCell3 = checkFormula.Worksheets[0].Cells[i, 3];
                        var checkCell4 = checkFormula.Worksheets[0].Cells[i, 4];

                        if (checkCell4.Value == null)
                        {
                            sb.AppendLine("Run Time {0} : cell4 value is null, Expect [{1}]".FormatWith(i, headList[randomIndex]));
                        }
                        else if (checkCell4.Value.ToString().EndsWith(headList[randomIndex]) == false)
                        {
                            sb.AppendLine("Run Time {0} : cell4 value [{1}], Expect [{2}]".FormatWith(i, checkCell4.StringValue, headList[randomIndex]));
                        }

                        checkFormula.Dispose();
                    }

                    path = exportPath;
                }
            }

            string errorMsg = sb.ToString();
            return errorMsg;
        }

        #endregion 

        /// <summary>
        /// 读取Excel文件
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="fileName">文件名称</param>
        /// <returns>DataSet 结果集</returns>
        public static DataSet Excel2DataSet(string filePath, ExcelReaderConfig config = null)
        {
            // TODO 完善此方法读取数据部分

            DataSet result = new DataSet();

            Workbook iWorkbook = null;


            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                if (filePath.EndsWith("xlsx"))
                {
                    iWorkbook = new Workbook(fs);
                }
                else if (filePath.EndsWith("xls"))
                {
                    iWorkbook = new Workbook(fs);
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
        /// 读取Excel文件到DataTable
        /// </summary>
        /// <param name="path">Excel文件路径</param>
        /// <param name="sheetIndex">默认读取第 1 个工作表</param>
        /// <param name="exportColumnName">将首行的值设置为DataColumn, 默认设置</param>
        /// <returns></returns>
        public static DataTable Excel2DataTable(string path, int sheetIndex = 0, bool exportColumnName = true)
        {
            DataTable dt = null;
            Aspose.Cells.Workbook workbook = null;
            try
            {
                workbook = new Aspose.Cells.Workbook(path);
                Aspose.Cells.Worksheet worksheet = workbook.Worksheets[sheetIndex];

                var maxRowIndex = worksheet.Cells.MaxDataRow; // 读取工作表中最大的行指针 ( 由0开始 )
                var maxColumnIndex = worksheet.Cells.MaxDataColumn; // 读取工作表中最大的列指针 ( 由0开始 )

                var rowsCount = maxRowIndex + 1; // 总行数
                var columnsCount = maxColumnIndex + 1; // 总列数

                Cells cells = worksheet.Cells;
                dt = cells.ExportDataTable
                    (
                        firstRow: 0,
                        firstColumn: 0,
                        totalRows: rowsCount,
                        totalColumns: columnsCount,
                        exportColumnName: exportColumnName
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

            Aspose.Cells.Workbook workbook = null;
            try
            {
                workbook = new Workbook(path);
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
                        totalRows: rowsCount,
                        totalColumns: columnsCount,
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

        #region GetValueType

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
                            // return cell.Formula; // TODO 读取复杂的公式时出现问题
                            return cell.StringValue;
                        }
                        else
                        {
                            return null;
                        }
                    }
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

            Workbook workbook = new Aspose.Cells.Workbook(copyToTempPath);
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
                        case "SYSTEM.INT32":
                            {
                                int valueAfterConvert = Convert.ToInt32(value);
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
        public List<PropertyColumn> GetHeader(Worksheet worksheet, int columnCount, int lieDingYi_RowIndex = 0, int lieDingYi_ColumnIndex = 0, bool ignoreRepeatColumnName = false)
        {
            string errorMsg = string.Empty;

            try
            {
                List<PropertyColumn> result = new List<PropertyColumn>();

                object[,] object2DArray = worksheet.Cells.ExportArray
                (
                    firstRow: lieDingYi_RowIndex, // 默认是第一行 ( index = 0 )
                    firstColumn: lieDingYi_ColumnIndex, // 默认是第一列 ( index = 0 )
                    totalRows: 1, // 列定义通常是 1 行数据, 这里只取位于 firstRow, firstColumn 的一行
                    totalColumns: columnCount - lieDingYi_ColumnIndex // 列个数
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

        /// <summary>
        /// 打印DEMO
        /// Aspose.Cell 原理输出为Image, 打印Image
        /// </summary>
        public static void PrintDemo(string filePath)
        {
            Workbook workbook = new Workbook(filePath);

            //Get the worksheet to be printed
            Worksheet worksheet = workbook.Worksheets[0];

            PageSetup pageSetup = worksheet.PageSetup;
            pageSetup.Orientation = PageOrientationType.Landscape;
            //pageSetup.LeftMargin = 0;
            //pageSetup.RightMargin = 0.1;
            //pageSetup.BottomMargin = 0.3;
            //pageSetup.PrintArea = "A2:J29"; // 打印区域
            //Apply different Image / Print options.

            Aspose.Cells.Rendering.ImageOrPrintOptions options = new Aspose.Cells.Rendering.ImageOrPrintOptions();

            //Set the Printing page property
            //options.PrintingPage = PrintingPageType.IgnoreStyle;
            //Render the worksheet

            Aspose.Cells.Rendering.SheetRender sr = new Aspose.Cells.Rendering.SheetRender(worksheet, options);

            System.Drawing.Image map = sr.ToImage(0); // 核心 -- 将 SheetRendar转为 Image 进行打印
                                                      // map.Save(@"D:\HoweDesktop\A{0}".FormatWith(DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")));

            // send to printer 核心打印 Image
            System.Drawing.Printing.PrinterSettings printSettings = new System.Drawing.Printing.PrinterSettings();
            string strPrinterName = printSettings.PrinterName;
            sr.ToPrinter(strPrinterName);
        }

        public static void DataSet2Excel(string filePath, DataSet ds, bool[] showColumnNameArray = null, int[,] positionArray = null)
        {
            FileFormatType fileFormatType = FileFormatType.Xlsx;
            if (filePath.EndsWith("xlsx", StringComparison.InvariantCultureIgnoreCase))
            {
                fileFormatType = FileFormatType.Xlsx;
            }
            else if (filePath.EndsWith("xls", StringComparison.InvariantCultureIgnoreCase))
            {
                fileFormatType = FileFormatType.Xlsx;
            }

            Workbook workbook = new Workbook(fileFormatType: fileFormatType);
            for (int index = 0; index < ds.Tables.Count; index++)
            {
                Worksheet workSheet = workbook.Worksheets[index];

                DataTable dt = ds.Tables[index];
                if (dt.TableName.IsNullOrEmpty() == true)
                {
                    workSheet.Name = "Sheet{0}".FormatWith(index + 1);
                }
                else
                {
                    workSheet.Name = dt.TableName;
                }

                bool isFieldNameShown = true;
                if (showColumnNameArray != null)
                {
                    isFieldNameShown = showColumnNameArray[index];
                }

                int tmpFirstRow = 0;
                int tmpFirstColumn = 0;
                if (positionArray != null)
                {
                    tmpFirstRow = positionArray[index, 0];
                    tmpFirstColumn = positionArray[index, 1];
                }

                workSheet.Cells.ImportDataTable
                (
                    dataTable: dt,
                    isFieldNameShown: isFieldNameShown,
                    firstRow: tmpFirstRow,
                    firstColumn: tmpFirstColumn
                );
            }

            workbook.Save(filePath);
        }

    }
}
