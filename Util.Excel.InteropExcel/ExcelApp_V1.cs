using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Excel
{

    //using System;
    //using System.Collections.Generic;
    //using System.Linq;
    //using System.Text;

    //using System.Data;
    //using System.Data.OleDb;
    //using System.Reflection;
    //using System.Runtime.InteropServices;
    //using System.Collections.ObjectModel;
    //using System.Collections;
    //using System.IO;
    //using System.Threading.Tasks;

    //using nsExcelApp = Microsoft.Office.Interop.Excel;

    //namespace Util.Excel
    //{
    //    public class ExcelUtil_InteropExcel : Util.Excel.IExcelUtils
    //    {
    //        /// <summary>
    //        /// 读取Excel文件
    //        /// </summary>
    //        public System.Data.DataTable LoadExcel(string pPath)
    //        {
    //            string connString = string.Empty;
    //            if (pPath.EndsWith(".xlsx"))
    //            {
    //                connString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"" + pPath + "\";Extended Properties=Excel 12.0 Xml;";
    //            }
    //            else if (pPath.EndsWith(".xls"))
    //            {
    //                connString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" + pPath + "\";Extended Properties=Excel 8.0;";
    //            }

    //            List<string> sheetNames = this.ExcelSheetName(connString);

    //            DataTable dt = new DataTable();
    //            using (OleDbConnection dbConn = new System.Data.OleDb.OleDbConnection(connString))
    //            {
    //                dbConn.Open();
    //                string sheetName = sheetNames.FirstOrDefault(p => p.EndsWith("$"));
    //                string strSql = "select * from [" + sheetName.Replace('.', '#') + "]";
    //                // string strSql = "select * from [$Sheet1]";
    //                OleDbDataAdapter dapter = new OleDbDataAdapter(strSql, dbConn);
    //                DataSet ds = new DataSet();
    //                dapter.Fill(ds);
    //                dt = ds.Tables[0];
    //                if (dbConn.State != ConnectionState.Closed)
    //                {
    //                    dbConn.Close();
    //                }
    //            }

    //            return dt;
    //        }

    //        /// <summary>
    //        /// 获得Excel表中的表名
    //        /// 值是类似这样的：Sheet1$表1$表2$表3$ 
    //        /// </summary>
    //        /// <returns>Excel文件所有工作表名(工作簿)</returns>
    //        private List<string> ExcelSheetName(string conString)
    //        {
    //            List<string> sheetNames = new List<string>();
    //            using (OleDbConnection con = new OleDbConnection(conString))
    //            {
    //                con.Open();
    //                DataTable sheetName = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "table" });
    //                con.Close();
    //                foreach (DataRow var in sheetName.Rows)
    //                {
    //                    sheetNames.Add(var[2].ToString());
    //                }
    //            }
    //            return sheetNames;
    //        }

    //        public static bool ExportToExcel(string filepath, DataTable dt)
    //        {
    //            if (dt == null || dt.Rows.Count == 0) return false;
    //            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
    //            if (xlApp == null)
    //            {
    //                return false;
    //            }
    //            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
    //            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
    //            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
    //            object missing = System.Reflection.Missing.Value;
    //            for (int r = 0; r < dt.Rows.Count; r++)
    //            {
    //                for (int i = 0; i < dt.Columns.Count; i++)
    //                {
    //                    worksheet.Cells[1, i + 1] = dt.Columns[i].ColumnName;
    //                    worksheet.Cells[r + 2, i + 1] = dt.Rows[r][i].ToString();
    //                }
    //            }

    //            workbook.Saved = true;
    //            workbook.SaveCopyAs(filepath);
    //            workbook.Close(true, Type.Missing, Type.Missing);
    //            workbook = null;
    //            xlApp.Quit();
    //            return true;
    //        }

    //        ///// <summary>
    //        ///// 导出Excel
    //        ///// </summary>
    //        ///// <param name="filepath">保存路径</param>
    //        ///// <param name="dgColumns">表格控件表头</param>
    //        ///// <param name="entitys">数据源</param>
    //        //public static bool ExportToExcel<T>(string filepath, ObservableCollection<DataGridColumn> dgColumns, List<T> entitys)
    //        //{
    //        //    if (entitys == null) { return false; }

    //        //    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
    //        //    if (xlApp == null) { return false; }
    //        //    //System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture; 
    //        //    //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); 
    //        //    Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
    //        //    Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
    //        //    Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
    //        //    object missing = System.Reflection.Missing.Value;

    //        //    ExcelUtil_InteropExcel.GetTable<T>(worksheet, dgColumns, entitys);

    //        //    workbook.Saved = true;
    //        //    workbook.SaveCopyAs(filepath);
    //        //    // worksheet.SaveAs(filepath, missing, missing, missing, missing, missing, missing, missing, missing, missing);
    //        //    workbook.Close(true, Type.Missing, Type.Missing);
    //        //    workbook = null;
    //        //    xlApp.Quit();
    //        //    return true;
    //        //}

    //        ///// <summary>
    //        ///// 导出Excel
    //        ///// </summary>
    //        ///// <param name="filepath">保存路径</param>
    //        ///// <param name="dgColumns">表格控件表头</param>
    //        ///// <param name="entitys">数据源</param>
    //        ///// <param name="isAddworksheet">工作表是否添加到当前工作薄（否则完全新建）</param>
    //        //public static bool ExportToExcel<T>(string filepath, ObservableCollection<DataGridColumn> dgColumns, List<T> entitys, bool isAddInExistWorksheets = false)
    //        //{
    //        //    if (entitys == null) { return false; }

    //        //    Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
    //        //    if (xlApp == null) { return false; }
    //        //    //System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture; 
    //        //    //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); 
    //        //    Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
    //        //    if (isAddInExistWorksheets == true)
    //        //    {
    //        //        Microsoft.Office.Interop.Excel.Workbook workbook = xlApp.Workbooks.Open(filepath);
    //        //        Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add();
    //        //        object missing = System.Reflection.Missing.Value;

    //        //        ExcelUtil_InteropExcel.GetTable<T>(worksheet, dgColumns, entitys);

    //        //        workbook.Saved = true;
    //        //        workbook.Save();
    //        //        // worksheet.SaveAs(filepath, missing, missing, missing, missing, missing, missing, missing, missing, missing);
    //        //        workbook.Close(true, Type.Missing, Type.Missing);
    //        //        workbook = null;
    //        //        xlApp.Quit();
    //        //        return true;
    //        //    }
    //        //    else
    //        //    {
    //        //        Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
    //        //        Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
    //        //        object missing = System.Reflection.Missing.Value;

    //        //        ExcelUtil_InteropExcel.GetTable<T>(worksheet, dgColumns, entitys);

    //        //        workbook.Saved = true;
    //        //        workbook.SaveCopyAs(filepath);
    //        //        // worksheet.SaveAs(filepath, missing, missing, missing, missing, missing, missing, missing, missing, missing);
    //        //        workbook.Close(true, Type.Missing, Type.Missing);
    //        //        workbook = null;
    //        //        xlApp.Quit();
    //        //        return true;
    //        //    }
    //        //}

    //        //private static void GetTable<T>(Microsoft.Office.Interop.Excel.Worksheet worksheet, ObservableCollection<DataGridColumn> dgColumns, List<T> entitys)
    //        //{
    //        //    //全部数据
    //        //    string[,] excelData = new string[entitys.Count + 1, dgColumns.Count];
    //        //    if (entitys.Count == 0)//没有数据，就只好写标题
    //        //    {
    //        //        int i = 0;
    //        //        foreach (DataGridColumn column in dgColumns)
    //        //        {
    //        //            excelData[1 - 1, i + 1 - 1] = column.Header.ToString();
    //        //            i++;
    //        //        }
    //        //    }
    //        //    else
    //        //    {

    //        //        Type entityType = entitys[0].GetType();
    //        //        List<PropertyInfo> entityProperties = new List<PropertyInfo>(entityType.GetProperties());

    //        //        int r = 0;


    //        //        foreach (T entity in entitys)
    //        //        {
    //        //            if (entity.GetType() == entityType)
    //        //            {
    //        //                int i = 0;
    //        //                foreach (DataGridColumn column in dgColumns)
    //        //                {
    //        //                    excelData[1 - 1, i + 1 - 1] = column.Header.ToString();
    //        //                    System.Windows.Data.Binding columnPath = column.ClipboardContentBinding as System.Windows.Data.Binding;
    //        //                    if (columnPath != null)
    //        //                    {
    //        //                        PropertyInfo newPropertyInfo = entityProperties.FirstOrDefault(p => p.Name == columnPath.Path.Path);
    //        //                        if (newPropertyInfo != null)
    //        //                        {
    //        //                            var obj = newPropertyInfo.GetValue(entity, null);
    //        //                            if (obj != null)
    //        //                            {
    //        //                                excelData[r + 2 - 1, i + 1 - 1] = obj.ToString();
    //        //                            }
    //        //                        }
    //        //                    }
    //        //                    i++;
    //        //                }
    //        //                r++;
    //        //            }
    //        //        }
    //        //    }

    //        //    Microsoft.Office.Interop.Excel.Range ran = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[entitys.Count + 1, dgColumns.Count]];

    //        //    ran.Value2 = excelData;

    //        //    //foreach (T entity in entitys)
    //        //    //{
    //        //    //    if (entity.GetType() == entityType)
    //        //    //    {
    //        //    //        int i = 0;
    //        //    //        foreach (DataGridColumn column in dgColumns)
    //        //    //        {
    //        //    //            worksheet.Cells[1, i + 1] = column.Header.ToString();
    //        //    //            System.Windows.Data.Binding columnPath = column.ClipboardContentBinding as System.Windows.Data.Binding;
    //        //    //            if (columnPath != null)
    //        //    //            {
    //        //    //                PropertyInfo newPropertyInfo = entityProperties.FirstOrDefault(p => p.Name == columnPath.Path.Path);
    //        //    //                if (newPropertyInfo != null)
    //        //    //                {
    //        //    //                    worksheet.Cells[r + 2, i + 1] = newPropertyInfo.GetValue(entity, null);
    //        //    //                }
    //        //    //            }
    //        //    //            i++;
    //        //    //        }
    //        //    //        r++;
    //        //    //    }
    //        //    //}
    //        //}

    //        public static bool ExportToExcelForAll<T>(string filepath, IEnumerable<ExportColumn> dgColumns, List<T> entitys, bool isAddInExistWorksheets = false)
    //        {
    //            if (entitys == null) { return false; }

    //            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
    //            if (xlApp == null) { return false; }
    //            object missing = System.Reflection.Missing.Value;
    //            if (isAddInExistWorksheets == true)//不是覆盖，而是添加
    //            {

    //                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
    //                Microsoft.Office.Interop.Excel.Workbook workbook = xlApp.Workbooks.Open(filepath);//打开一个现有的工作簿(即文件)
    //                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets.Add();//添加一个工作表
    //                                                                                                                                         //= (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[2];
    //                                                                                                                                         //object missing = System.Reflection.Missing.Value;

    //                ExcelUtil_InteropExcel.GetTable<T>(worksheet, dgColumns, entitys);

    //                workbook.Saved = true;
    //                workbook.Save();

    //                // worksheet.SaveAs(filepath, missing, missing, missing, missing, missing, missing, missing, missing, missing);
    //                workbook.Close(true, Type.Missing, Type.Missing);
    //                workbook = null;
    //                xlApp.Quit();
    //                return true;
    //            }
    //            else
    //            {
    //                //System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture; 
    //                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); 
    //                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
    //                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
    //                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];
    //                //object missing = System.Reflection.Missing.Value;

    //                ExcelUtil_InteropExcel.GetTable<T>(worksheet, dgColumns, entitys);

    //                workbook.Saved = true;
    //                workbook.SaveCopyAs(filepath);

    //                // worksheet.SaveAs(filepath, missing, missing, missing, missing, missing, missing, missing, missing, missing);
    //                workbook.Close(true, Type.Missing, Type.Missing);
    //                workbook = null;
    //                xlApp.Quit();
    //                return true;
    //            }
    //        }

    //        private static void GetTable<T>(Microsoft.Office.Interop.Excel.Worksheet worksheet, IEnumerable<ExportColumn> dgColumns, List<T> entitys)
    //        {
    //            Type entityType = entitys[0].GetType();
    //            List<PropertyInfo> entityProperties = new List<PropertyInfo>(entityType.GetProperties());

    //            int r = 0;
    //            string[,] excelData = new string[entitys.Count + 1, dgColumns.Count()];
    //            foreach (T entity in entitys)
    //            {
    //                if (entity.GetType() == entityType)
    //                {
    //                    int i = 0;
    //                    foreach (ExportColumn column in dgColumns)
    //                    {
    //                        excelData[1 - 1, i + 1 - 1] = column.Header.ToString();

    //                        PropertyInfo newPropertyInfo = entityProperties.FirstOrDefault(p => p.Name == column.Name);
    //                        if (newPropertyInfo != null)
    //                        {
    //                            var obj = newPropertyInfo.GetValue(entity, null);
    //                            if (obj != null)
    //                            {
    //                                excelData[r + 2 - 1, i + 1 - 1] = obj.ToString();
    //                            }
    //                        }
    //                        i++;
    //                    }
    //                    r++;
    //                }
    //            }

    //            Microsoft.Office.Interop.Excel.Range ran = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[entitys.Count + 1, dgColumns.Count()]];
    //            ran.Value2 = excelData;
    //        }

    //        public static void OpenExcel(string filepath)
    //        {
    //            if (System.IO.File.Exists(filepath))
    //            {
    //                using (System.Diagnostics.Process browser = new System.Diagnostics.Process())
    //                {
    //                    browser.StartInfo = new System.Diagnostics.ProcessStartInfo
    //                    {
    //                        FileName = filepath,
    //                        UseShellExecute = true,
    //                        ErrorDialog = true
    //                    };
    //                    browser.Start();
    //                }
    //            }
    //        }

    //        public static bool CopyExcel(string resourcePath, string copypath)
    //        {
    //            if (System.IO.File.Exists(resourcePath))
    //            {

    //                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
    //                if (xlApp == null) { return false; }
    //                //System.Globalization.CultureInfo CurrentCI = System.Threading.Thread.CurrentThread.CurrentCulture; 
    //                //System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); 
    //                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
    //                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Open(resourcePath);



    //                workbook.Saved = true;
    //                workbook.SaveCopyAs(copypath);
    //                // worksheet.SaveAs(filepath, missing, missing, missing, missing, missing, missing, missing, missing, missing);
    //                workbook.Close(true, Type.Missing, Type.Missing);
    //                workbook = null;
    //                xlApp.Quit();

    //                return true;
    //            }

    //            return false;
    //        }

    //        //遍历工作表得到导入Excel的dataset
    //        public DataSet GetDataFromExcel(string strPath)
    //        {
    //            string strCon = "";
    //            string strSql = "";
    //            OleDbConnection dbConn = null;
    //            DataSet ds = null;
    //            Microsoft.Office.Interop.Excel.Application xlApp = null;
    //            try
    //            {
    //                if (strPath.EndsWith(".xlsx"))
    //                {
    //                    strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Persist Security Info=False;Data Source=\"" + strPath + "\";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'";
    //                }
    //                else if (strPath.EndsWith(".xls"))
    //                {
    //                    strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" + strPath + "\";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
    //                }

    //                //遍历工作表
    //                xlApp = new Microsoft.Office.Interop.Excel.Application();

    //                Microsoft.Office.Interop.Excel.Workbook workbook = xlApp.Workbooks.Open(strPath, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, 1, 0);
    //                int n = workbook.Worksheets.Count;
    //                string[] sheetSet = new string[n];
    //                ArrayList al = new ArrayList();
    //                for (int i = 0; i < n; i++)
    //                {
    //                    sheetSet[i] = ((Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[i + 1]).Name;
    //                }
    //                xlApp.Workbooks.Close();
    //                ds = new DataSet();
    //                for (int i = 0; i < n; i++)
    //                {
    //                    ds.Tables.Add();
    //                    strSql = "select * from [" + sheetSet[i] + "$]";
    //                    string strConn = strCon;
    //                    OleDbConnection myConnection = new OleDbConnection(strConn);
    //                    myConnection.Open();

    //                    OleDbDataAdapter da = new OleDbDataAdapter(strSql, myConnection);
    //                    try
    //                    {
    //                        da.Fill(ds.Tables[i]);
    //                    }
    //                    catch (Exception ex)
    //                    {
    //                        throw ex;
    //                    }
    //                    finally
    //                    {
    //                    }
    //                    if (i == n - 1)
    //                    {
    //                        da.Dispose();
    //                        da = null;
    //                        myConnection.Close();
    //                        myConnection = null;
    //                    }
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                throw ex;
    //            }
    //            finally
    //            {
    //                if (dbConn != null)
    //                {
    //                    dbConn.Close();
    //                }
    //                if (xlApp != null)
    //                {
    //                    xlApp.Quit();
    //                    KillExcel(xlApp);
    //                    GC.WaitForPendingFinalizers();
    //                    GC.Collect();
    //                }
    //            }
    //            return ds;
    //        }

    //        public DataSet GetExcelToDataSetByCondition(string strPath, string connCondition)
    //        {
    //            OleDbConnection myConnection = null;
    //            string strCon = "";
    //            string strSql = "";
    //            DataSet ds = null;
    //            try
    //            {
    //                if (strPath.EndsWith(".xlsx"))
    //                {
    //                    strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Persist Security Info=False;Data Source=\"" + strPath + "\";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'";
    //                }
    //                else if (strPath.EndsWith(".xls"))
    //                {
    //                    strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" + strPath + "\";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
    //                }

    //                //遍历工作表
    //                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
    //                Microsoft.Office.Interop.Excel.Workbook workbook = xlApp.Workbooks.Open(strPath, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, 1, 0);
    //                int n = workbook.Worksheets.Count;
    //                string[] sheetSet = new string[n];
    //                ArrayList al = new ArrayList();
    //                for (int i = 0; i < n; i++)
    //                {
    //                    sheetSet[i] = ((Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[i + 1]).Name;
    //                }
    //                xlApp.Workbooks.Close();
    //                ds = new DataSet();

    //                ds.Tables.Add();
    //                //strSql = "select * from [" + sheetSet[i] + "$]";
    //                strSql = "select * from [" + sheetSet[0] + "$] " + connCondition;

    //                string strConn = strCon;
    //                myConnection = new OleDbConnection(strConn);
    //                myConnection.Open();

    //                OleDbDataAdapter da = new OleDbDataAdapter(strSql, myConnection);
    //                da.Fill(ds.Tables[0]);
    //            }
    //            catch (Exception ex)
    //            {
    //                throw ex;
    //            }
    //            finally
    //            {
    //                if (myConnection.State != ConnectionState.Closed)
    //                {
    //                    myConnection.Close();
    //                }
    //            }
    //            return ds;
    //        }

    //        public void GetExcelToDataTable(string strPath, DataTable dt)
    //        {
    //            OleDbConnection myConnection = null;
    //            string strCon = "";
    //            string strSql = "";
    //            try
    //            {
    //                if (strPath.EndsWith(".xlsx"))
    //                {
    //                    strCon = "Provider=Microsoft.ACE.OLEDB.12.0;Persist Security Info=False;Data Source=\"" + strPath + "\";Extended Properties='Excel 12.0;HDR=Yes;IMEX=1'";
    //                }
    //                else if (strPath.EndsWith(".xls"))
    //                {
    //                    strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=\"" + strPath + "\";Extended Properties='Excel 8.0;HDR=Yes;IMEX=1'";
    //                }

    //                //遍历工作表
    //                Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();
    //                Microsoft.Office.Interop.Excel.Workbook workbook = xlApp.Workbooks.Open(strPath, 0, false, 5, "", "", false, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "", true, false, 0, true, 1, 0);
    //                int n = workbook.Worksheets.Count;
    //                string[] sheetSet = new string[n];
    //                ArrayList al = new ArrayList();
    //                for (int i = 0; i < n; i++)
    //                {
    //                    sheetSet[i] = ((Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[i + 1]).Name;
    //                }
    //                xlApp.Workbooks.Close();

    //                strSql = "select * from [" + sheetSet[0] + "$] ";

    //                string strConn = strCon;
    //                myConnection = new OleDbConnection(strConn);
    //                myConnection.Open();

    //                OleDbDataAdapter da = new OleDbDataAdapter(strSql, myConnection);
    //                da.Fill(dt);
    //            }
    //            catch (Exception ex)
    //            {
    //                throw ex;
    //            }
    //            finally
    //            {
    //                if (myConnection.State != ConnectionState.Closed)
    //                {
    //                    myConnection.Close();
    //                }
    //            }
    //        }

    //        /// <summary>
    //        /// 打开并保存, 返回保存的文件路径
    //        /// </summary>
    //        public static string OpenAndSave(string filepath)
    //        {
    //            // 测试用于打开含有公式的Excel文件, 测试其打开后保存能否读取到公式的值 -- 测试成功

    //            string r = string.Empty;

    //            Microsoft.Office.Interop.Excel.Application xlApp = null;
    //            Microsoft.Office.Interop.Excel.Workbooks workbooks = null;
    //            Microsoft.Office.Interop.Excel.Workbook workbook = null;
    //            try
    //            {
    //                xlApp = new Microsoft.Office.Interop.Excel.Application();
    //                workbooks = xlApp.Workbooks;
    //                workbook = xlApp.Workbooks.Open(filepath); // 打开一个现有的工作簿(即文件)
    //                bool isSaved = workbook.Saved;
    //                if (isSaved == false)
    //                {
    //                    FileInfo fi = new FileInfo(filepath);
    //                    string saveFilePath = Path.Combine(fi.Directory.FullName, "{0}{1}".FormatWith(DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"), fi.Extension));
    //                    workbook.SaveCopyAs(saveFilePath);

    //                    r = saveFilePath;
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                throw ex;
    //            }
    //            finally
    //            {
    //                if (workbook != null)
    //                {
    //                    workbook.Close(true, Type.Missing, Type.Missing);
    //                }
    //                workbook = null;

    //                if (workbooks != null)
    //                {
    //                    workbooks.Close();
    //                }
    //                workbooks = null;

    //                xlApp.Quit();
    //            }

    //            return r;
    //        }

    //        /// <summary>
    //        /// 打开Excel文件, 遍历所有Sheet, 设置打印方向, 最后打印
    //        /// </summary>
    //        /// <param name="filePath">文件路径</param>
    //        /// <param name="printerName">选择打印机</param>
    //        /// <param name="isLandscape">设置为横向输出, 默认纵向输出</param>
    //        public static void OpenAndPrint(string filePath, string printerName = "", bool isLandscape = false)
    //        {
    //            nsExcelApp.Application excelApp = null;
    //            nsExcelApp.Workbook workbook = null;

    //            try
    //            {
    //                excelApp = new nsExcelApp.Application();
    //                workbook = excelApp.Workbooks.Open(filePath); // 打开一个现有的工作簿(即文件)

    //                for (int index = 1; index <= workbook.Worksheets.Count; index++)
    //                {
    //                    nsExcelApp.Worksheet wookSheet = (nsExcelApp.Worksheet)workbook.Worksheets[index];

    //                    if (isLandscape == true)
    //                    {
    //                        wookSheet.PageSetup.Orientation = nsExcelApp.XlPageOrientation.xlLandscape; // 横向
    //                    }
    //                    else
    //                    {
    //                        wookSheet.PageSetup.Orientation = nsExcelApp.XlPageOrientation.xlPortrait; // 纵向
    //                    }
    //                }

    //                workbook.Save();

    //                #region 选择打印机

    //                if (printerName.IsNullOrWhiteSpace() == false ||
    //                    excelApp.ActivePrinter.StartsWith(printerName) == false
    //                    )
    //                {
    //                    // 测试成功, TODO 如何获取这样特殊的打印机名称 Ne04 ==> net 04 号打印机
    //                    // Foxit Reader PDF Printer 在 Ne04:
    //                    // HP LaserJet Professional P1606dn 在 Ne03:
    //                    //string p = "Foxit Reader PDF Printer 在 Ne04:";
    //                    //excelApp.ActivePrinter = p;
    //                }
    //                #endregion

    //                workbook.PrintOutEx();

    //            }
    //            catch (Exception ex)
    //            {
    //                string msg = "{0}".FormatWith(ex.GetFullInfo());
    //                System.Diagnostics.Debug.WriteLine(msg);
    //                throw new Exception(msg, ex);
    //            }
    //            finally
    //            {
    //                if (workbook != null)
    //                {
    //                    workbook.Close(false, null, null);
    //                    Marshal.ReleaseComObject(workbook);
    //                }

    //                if (excelApp != null)
    //                {
    //                    excelApp.Quit();
    //                    // KillExcel(excelApp);
    //                    // Marshal.ReleaseComObject(workbooks);
    //                    Marshal.ReleaseComObject(excelApp);

    //                    excelApp = null;
    //                }
    //            }
    //        }

    //        List<T> IExcelUtils.WorkSheet2List<T>(string path, List<PropertyColumn> objectProps, int worksheetIndex, bool isContainLieDingYi, int lieDingYi_RowIndex, int lieDingYi_ColumnIndex, bool ignoreRepeatColumnName)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        void IExcelUtils.WorkSheet2ListAsync<T>(Action<Task<List<T>>> actionHandler, string path, List<PropertyColumn> objectProps, int worksheetIndex, bool isContainLieDingYi, int lieDingYi_RowIndex, int lieDingYi_ColumnIndex, bool ignoreRepeatColumnName)
    //        {
    //            throw new NotImplementedException();
    //        }

    //        #region 关闭 Excel Process

    //        [DllImport("User32.dll", CharSet = CharSet.Auto)]
    //        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

    //        public static void KillExcel(Microsoft.Office.Interop.Excel.Application excelApplication)
    //        {
    //            IntPtr t = new IntPtr(excelApplication.Hwnd); //得到这个句柄，具体作用是得到这块内存入口 
    //            int processId = -9876;
    //            GetWindowThreadProcessId(t, out processId); //得到本进程唯一标志k
    //                                                        //if (processId != -9876)
    //                                                        //{
    //            System.Diagnostics.Process excelProcess = System.Diagnostics.Process.GetProcessById(processId: processId); //得到对进程k的引用
    //            excelProcess.Kill();
    //            //}
    //        }

    //        #endregion 关闭 Excel Process
    //    }
    //}


}
