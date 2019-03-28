using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using Microsoft.Office.Interop.Excel;
using nsExcelApp = Microsoft.Office.Interop.Excel;

namespace Util.Excel
{
    /// <summary>
    /// 一般情况下 $不$使用 此Class 进行Excel文件信息的读取和写入,
    /// 此Class 多数情况下用于
    /// 1 公式的计算(打开Excel文件时, 公式值为Null的单元格会进行计算, 计算完毕后 workbook.Saved 的属性会改为 false, 此时进行一次保存。保存后的文件就能读取Excel公式的值 )
    /// 2 Excel文件打印 (用第三方软件进行打印, 字体会出现变形 \ 图片有可能丢失)
    /// 
    /// Application sExcelApp是静态且唯一的
    /// 注意点 : 整个程序退出时, 请执行 QuitExcelApplication 清除Excel 后台进程
    /// </summary>

    // ****** 更新日志 ****** 
    // V4
    // 由于 V3 版本 在WebService中并行测试中效果不理想 ( 多于3个客户端连续访问时会报错, 并且我对 并行WebService方法中转为排队仍未深入掌握 )
    // 最终改回 V2 的设计型式, 并加以优化
    // 
    // V3
    // 理顺调用概念，采用 Appliaction 单例 ( 整个程序只会有一个引用 )， 首次打开时定义，整个程序退出时请执行 QuitExcelApplication 清除Excel 后台进程
    // 
    // V2
    // Marshal.ReleaseComObject(mWorkbooks); 清除 mXXXX (工作簿s、工作簿、工作表）
    // 
    // V1
    // 旧项目迁移过来, 里面的很多对 Application 和 Workbooks 处理方法都存在问题, 但读取数据的方式仍有些参考价值(读取数据有Bug的问题仍不能解决, 故仍使用
    // 第三方控件进行数据的读取和写入
    public class ExcelUtil_InteropExcel : IDisposable
    // : Util.Excel.IExcelUtils
    {
        private Application mExcelApp;

        private Workbooks mWorkbooks;

        /// <summary>
        /// 当前工作簿
        /// </summary>
        private Workbook mWorkbook;

        /// <summary>
        /// 当前工作簿的WorkSheet Array
        /// </summary>
        private Worksheet[] mWorksheetArray;

        /// <summary>
        /// 工作簿内的 WorkSheet 总数
        /// </summary>
        public int WorkSheetsCount
        {
            get
            {
                int r = 0;
                if (mWorksheetArray != null)
                {
                    r = mWorksheetArray.Length;
                }
                return r;
            }
        }

        /// <summary>
        /// 当前工作簿中激活的WorkSheet
        /// </summary>
        private Worksheet mWorksheet;

        /// <summary>
        /// 调用自服务器?  true = 服务器; false = 客户端
        /// 根据不同选项, 错误日志的路径有所区别
        /// </summary>
        private bool mIsWebApp;

        #region 构造函数

        public ExcelUtil_InteropExcel(bool isWebApp = false)
        {
            mExcelApp = new Application();
            mExcelApp.Visible = false;
            mExcelApp.DisplayAlerts = false;


            mWorkbooks = mExcelApp.Workbooks;

            mIsWebApp = false;
        }

        public ExcelUtil_InteropExcel(string filePath)
        {
            mExcelApp = new Application();
            mExcelApp.Visible = false;
            mExcelApp.DisplayAlerts = false;

            mWorkbooks = mExcelApp.Workbooks;

            mIsWebApp = false;

            this.Open(filePath);
        }

        public ExcelUtil_InteropExcel(string filePath, bool isWebApp)
        {
            mExcelApp = new Application();
            mExcelApp.Visible = false;
            mExcelApp.DisplayAlerts = false;

            mWorkbooks = mExcelApp.Workbooks;

            mIsWebApp = isWebApp;

            this.Open(filePath);
        }

        #endregion 构造函数

        /// <summary>
        /// 打开工作簿
        /// </summary>
        /// <param name="fullFilePath"></param>
        public void Open(string fullFilePath)
        {
            mWorkbook = mWorkbooks._Open(fullFilePath,
                                            Missing.Value, Missing.Value,
                                            Missing.Value, Missing.Value,
                                            Missing.Value, Missing.Value,
                                            Missing.Value, Missing.Value,
                                            Missing.Value, Missing.Value,
                                            Missing.Value, Missing.Value);

            mWorksheetArray = new Worksheet[mWorkbook.Sheets.Count];

            for (int i = 0; i < mWorksheetArray.Length; i++)
            {
                mWorksheetArray[i] = (Worksheet)mWorkbook.Sheets[i + 1]; // Excel 的 Sheet 索引由 1 开始算, 故 i + 1
            }

            mWorksheet = (Worksheet)mWorkbook.ActiveSheet;
        }

        private void Close()
        {
            if (mWorksheet != null)
            {
                Marshal.ReleaseComObject(mWorksheet);
            }

            if (mWorkbook != null)
            {
                mWorkbook.Close(false, null, null);
                Marshal.ReleaseComObject(mWorkbook);
            }

            mWorksheet = null;
            mWorkbook = null;
        }

        public void Dispose()
        {
            try
            {
                Close();

                if (mWorkbooks != null)
                {
                    Marshal.ReleaseComObject(mWorkbooks);
                }

                if (mExcelApp != null)
                {
                    mExcelApp.Quit();
                    Marshal.ReleaseComObject(mExcelApp);
                }
            }
            catch (Exception ex)
            {
                writeExceptionLog(ex);
            }

            mWorkbooks = null;
            mExcelApp = null;

            // 关闭Excel进程 -- 核心代码 
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }



        public string ActiveWorksheetName
        {
            get
            {
                return mWorksheet != null ? mWorksheet.Name : null;
            }
            set
            {
                if (mWorksheet != null)
                {
                    mWorksheet.Name = value;
                }
            }
        }

        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            mWorkbook.Save();
        }

        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="saveFilePath"></param>
        public string SaveCopyAs(string saveFilePath = "")
        {
            if (saveFilePath.IsNullOrWhiteSpace())
            {
                if (mIsWebApp == true)
                {
                    saveFilePath = this.GetSaveFilePath_WebService();
                }
                else
                {
                    saveFilePath = this.GetSaveFilePath();
                }
            }

            string directory = Path.GetDirectoryName(saveFilePath);

            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentException("saveFilePath is not a valid file path.saveFilePath:\r\n{0}".FormatWith(saveFilePath));
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            mWorkbook.SaveCopyAs(saveFilePath);

            return saveFilePath;
        }

        public string GetSaveFilePath(bool isXLS = false)
        {
            string r = System.IO.Path.Combine
            (
                Environment.CurrentDirectory,
                "Temp",
                "ExcelFiles",
                "{0}.{1}".FormatWith(Guid.NewGuid().ToString(), isXLS ? "xls" : "xlsx")
            );

            return r;
        }

        public string GetSaveFilePath_WebService(bool isXLS = false)
        {
            string r = System.IO.Path.Combine
            (
                System.Web.Hosting.HostingEnvironment.MapPath("~"),
                "Export",
                "ExcelFiles",
                "{0}.{1}".FormatWith(Guid.NewGuid().ToString(), isXLS ? "xls" : "xlsx")
            );

            return r;
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="printerName">选择打印机</param>
        /// <param name="paperSize">选择纸张</param>
        /// <param name="isLandscape">打印方向, 默认纵向打印(false), 横向打印请填写(true)</param>
        public void Print
            (
                string printerName = "",
                int? paperSize = null,
                bool isLandscape = false
            )
        {
            try
            {
                // !!!! 重点, 先要设置打印机, 然后再设置纸张 !!!!
                
                #region 设置打印机

                if (printerName.IsNullOrWhiteSpace() == false ||
                    mExcelApp.ActivePrinter.StartsWith(printerName) == false
                    )
                {
                    mExcelApp.ActivePrinter = printerName;

                    // 需要获取这样特殊的打印机名称 Ne04 ==> net 04 号打印机
                    // 修改打印机测试成功
                    // Foxit Reader PDF Printer 在 Ne04:
                    // HP LaserJet Professional P1606dn 在 Ne03:
                    //string p = "Foxit Reader PDF Printer 在 Ne04:";
                    //excelApp.ActivePrinter = p;
                }

                #endregion

                #region 设置纸张大小 & 旋转

                for (int index = 0; index < mWorksheetArray.Length; index++)
                {
                    nsExcelApp.Worksheet wookSheet = mWorksheetArray[index];

                    if (paperSize.HasValue)
                    {
                        wookSheet.PageSetup.PaperSize = (XlPaperSize)paperSize.Value;
                    }

                    if (isLandscape == true)
                    {
                        wookSheet.PageSetup.Orientation = nsExcelApp.XlPageOrientation.xlLandscape; // 横向
                    }
                    else
                    {
                        wookSheet.PageSetup.Orientation = nsExcelApp.XlPageOrientation.xlPortrait; // 纵向
                    }
                }

                #endregion

                mWorkbook.PrintOutEx();
            }
            catch (Exception ex)
            {
                string msg = "{0}".FormatWith(ex.GetFullInfo());
                System.Diagnostics.Debug.WriteLine(msg);
                throw new Exception(msg, ex);
            }
        }

        /// <summary>
        /// 获取默认打印机名称
        /// 
        /// 由于修改打印机名称需要打印机名称 + 参数, 例子如下
        /// 修改打印机测试成功
        /// string p = "Foxit Reader PDF Printer 在 Ne04:";
        /// excelApp.ActivePrinter = p;
        /// 
        /// 故提供本方法查询具体参数
        /// </summary>
        /// <returns></returns>
        public string GetActivePrinterName()
        {
            return mExcelApp.ActivePrinter;
        }

        /// <summary>
        /// 记录错误日志
        /// </summary>
        /// <param name="ex"></param>
        private void writeExceptionLog(Exception ex)
        {
            if (mIsWebApp)
            {
                Util.LogUtils.WebServiceLogAsync(ex.GetFullInfo());
            }
            else
            {
                Util.LogUtils.LogAsync(ex.GetFullInfo());
            }
        }




        public void DataTableToExcel(System.Data.DataTable dt, string file_name, string sheet_name)
        {
            Microsoft.Office.Interop.Excel.Application Myxls = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook Mywkb = Myxls.Workbooks.Add();
            Microsoft.Office.Interop.Excel.Worksheet MySht = Mywkb.ActiveSheet;
            MySht.Name = sheet_name;
            Myxls.Visible = false;
            Myxls.DisplayAlerts = false;

            //写入表头
            object[] arrHeader = new object[dt.Columns.Count];
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                arrHeader[i] = dt.Columns[i].ColumnName;
            }
            MySht.Range[MySht.Cells[1, 1], MySht.Cells[1, dt.Columns.Count]].Value2 = arrHeader;
            //写入表体数据
            object[,] arrBody = new object[dt.Rows.Count, dt.Columns.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    arrBody[i, j] = dt.Rows[i][j].ToString();
                }
            }
            MySht.Range[MySht.Cells[2, 1], MySht.Cells[dt.Rows.Count + 1, dt.Columns.Count]].Value2 = arrBody;
            if (Mywkb != null)
            {
                Mywkb.SaveAs(file_name);
                Mywkb.Close(Type.Missing, Type.Missing, Type.Missing);
                Mywkb = null;
            }
        }
		
		
		
		public static void PrintSpecial(string printerName,
                string filePath,
                int? paperSize = null,
                bool isLandscape = false
            )
        {
            Microsoft.Office.Interop.Excel.Application mExcelApp = null;
            Microsoft.Office.Interop.Excel.Workbooks mWorkbooks = null;
            Microsoft.Office.Interop.Excel.Workbook mWorkbook = null;
            Microsoft.Office.Interop.Excel.Worksheet[] mWorksheetArray = null;

            try
            {
                #region 打开Excel文档 ( 静默模式 )

                mExcelApp = new Microsoft.Office.Interop.Excel.Application();
                mExcelApp.Visible = false;
                mExcelApp.DisplayAlerts = false;

                mWorkbooks = mExcelApp.Workbooks;
                mWorkbook = mWorkbooks._Open
                (
                    Filename: filePath,
                    UpdateLinks: Missing.Value,
                    ReadOnly: Missing.Value,
                    Format: Missing.Value,
                    Password: Missing.Value,
                    WriteResPassword: Missing.Value,
                    IgnoreReadOnlyRecommended: Missing.Value,
                    Origin: Missing.Value,
                    Delimiter: Missing.Value,
                    Editable: Missing.Value,
                    Notify: Missing.Value,
                    Converter: Missing.Value,
                    AddToMru: Missing.Value
                );

                mWorksheetArray = new Microsoft.Office.Interop.Excel.Worksheet[mWorkbook.Sheets.Count];

                for (int i = 0; i < mWorksheetArray.Length; i++)
                {
                    mWorksheetArray[i] = (Microsoft.Office.Interop.Excel.Worksheet)mWorkbook.Sheets[i + 1]; // Excel 的 Sheet 索引由 1 开始算, 故 i + 1
                }

                if (mWorkbook.Saved == false) // 打开Excel文件后, 公式会自动运算, 运算结果需要保存
                {
                    mWorkbook.Save();
                }

                #endregion

                #region 设置打印机

                if (printerName.IsNullOrWhiteSpace() == false ||
                    mExcelApp.ActivePrinter.StartsWith(printerName) == false
                    )
                {
                    mExcelApp.ActivePrinter = printerName;

                    // 需要获取这样特殊的打印机名称 Ne04 ==> net 04 号打印机
                    // 修改打印机测试成功
                    // Foxit Reader PDF Printer 在 Ne04:
                    // HP LaserJet Professional P1606dn 在 Ne03:
                    //string p = "Foxit Reader PDF Printer 在 Ne04:";
                    //excelApp.ActivePrinter = p;
                }

                #endregion

                #region 设置纸张大小 & 旋转

                for (int index = 0; index < mWorksheetArray.Length; index++)
                {
                    Microsoft.Office.Interop.Excel.Worksheet wookSheet = mWorksheetArray[index];

                    if (paperSize.HasValue)
                    {
                        wookSheet.PageSetup.PaperSize = (Microsoft.Office.Interop.Excel.XlPaperSize)paperSize.Value;
                    }

                    if (isLandscape == true)
                    {
                        wookSheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlLandscape; // 横向
                    }
                    else
                    {
                        wookSheet.PageSetup.Orientation = Microsoft.Office.Interop.Excel.XlPageOrientation.xlPortrait; // 纵向
                    }
                }

                #endregion

                #region 个性化代码


                #endregion 个性化代码
            }
            catch (Exception ex)
            {
                #region 关闭 Excel 进程 & 释放资源

                try
                {
                    if (mWorksheetArray != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(mWorksheetArray);
                    }
                    mWorksheetArray = null;


                    if (mWorkbook != null)
                    {
                        mWorkbook.Close
                        (
                            SaveChanges: false,
                            Filename: null,
                            RouteWorkbook: null
                        );

                        System.Runtime.InteropServices.Marshal.ReleaseComObject(mWorkbook);
                    }
                    mWorkbook = null;

                    if (mWorkbooks != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(mWorkbooks);
                    }
                    mWorkbooks = null;

                    if (mExcelApp != null)
                    {
                        mExcelApp.Quit();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(mExcelApp);
                    }
                    mExcelApp = null;
                }
                catch (Exception ex2)
                {
                    Util.LogUtils.LogAsync(ex2.GetFullInfo());
                }

                // 关闭Excel进程 -- 核心代码 
                GC.Collect();
                GC.WaitForPendingFinalizers();

                #endregion

                throw ex;
            }
        }

    }
}
