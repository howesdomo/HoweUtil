using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using Microsoft.Office.Interop.Excel;
using nsExcelApp = Microsoft.Office.Interop.Excel;


namespace Util.Excel
{
    /// <summary>
    /// V3 -- 2018-6-19 15:23:48
    /// 理顺调用概念，采用 Appliaction 单例 ( 整个程序只会有一个引用 )， 首次打开时定义，整个程序退出时请执行 QuitExcelApplication 清除Excel 后台进程
    /// 
    /// V2
    /// Marshal.ReleaseComObject(mWorkbooks); 清除 mXXXX (工作簿s、工作簿、工作表）
    /// 
    /// Application sExcelApp是单例
    /// 注意点 : 整个程序退出时, 请执行 QuitExcelApplication 清除Excel 后台进程
    /// </summary>
    [Obsolete]
    public class ExcelApp_V3 : IDisposable
    {
        // 一般情况下不使用 InteropExcel 进行Excel文件信息的读取和写入,
        // 多数情况下使用它来进行
        // 1 公式的计算(打开Excel文件时, 公式值为Null的单元格会进行计算, 计算完毕后 workbook.Saved 的属性会改为 false, 此时进行一次保存。保存后的文件就能读取Excel公式的值 )
        // 2 Excel文件打印 (用第三方软件进行打印, 字体会出现变形 \ 图片有可能丢失)

        private static Application sExcelApp;

        private Workbooks mWorkbooks;

        /// <summary>
        /// 当前工作簿
        /// </summary>
        private Workbook mWorkbook;

        private Worksheet[] mWorksheetArray;

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
        /// 当前工作簿内 的 工作表
        /// </summary>
        private Worksheet mWorksheet;

        public ExcelApp_V3(string filePath)
        {
            if (sExcelApp == null)
            {
                sExcelApp = new Application();

                sExcelApp.Visible = false;
                sExcelApp.DisplayAlerts = false;
            }

            mWorkbooks = sExcelApp.Workbooks;
            this.Open(filePath);
        }

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
                mWorksheetArray[i] =
                    (Worksheet)mWorkbook.Sheets[i + 1]; // Excel 的 Sheet 索引由 1 开始算, 故 i + 1
            }

            mWorksheet = (Worksheet)mWorkbook.ActiveSheet;

            mColCount = 0;
            mRowCount = 0;
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

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                Close();

                if (mWorkbooks != null)
                {
                    Marshal.ReleaseComObject(mWorkbooks);
                }

                //if (sExcelApp != null)
                //{
                //    sExcelApp.Quit();
                //    Marshal.ReleaseComObject(sExcelApp);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine("dispose ExcelApp object failed", ex);
            }

            mWorkbooks = null;
            //sExcelApp = null;
        }

        #endregion

        public static void QuitExcelApplication()
        {
            try
            {
                if (sExcelApp != null)
                {
                    sExcelApp.Quit();
                    Marshal.ReleaseComObject(sExcelApp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("dispose ExcelApp object failed", ex);
            }

            sExcelApp = null;
        }







        public int mColCount { get; set; }

        public int mRowCount { get; set; }

        public string WorksheetName
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

        // TODO Range ==> object[]
        #region Get Excel Range

        public Range GetCell(int rowIndex, int cellIndex)
        {
            Range cells = null;
            Range range = null;

            try
            {
                cells = sExcelApp.Cells;
                range = (Range)cells[1 + rowIndex, 1 + cellIndex];
            }
            finally
            {
                Marshal.ReleaseComObject(cells);
            }

            return range;
        }

        public Range GetColumn(int cellIndex)
        {
            Range range = null;
            Range cells = null;
            object rangeX = null;
            object rangeY = null;

            try
            {
                cells = sExcelApp.Cells;
                rangeX = cells[1, 1 + cellIndex];
                rangeY = cells[mRowCount, 1 + cellIndex];
                range = mWorksheet.get_Range(rangeX, rangeY);
            }
            finally
            {
                Marshal.ReleaseComObject(rangeX);
                Marshal.ReleaseComObject(rangeY);
                Marshal.ReleaseComObject(cells);
            }

            return range;
        }

        public Range GetRange(int xRowIndex, int xCellIndex, int yRowIndex, int yCellIndex)
        {
            Range range = null;
            Range cells = null;
            object rangeX = null;
            object rangeY = null;

            try
            {
                cells = sExcelApp.Cells;
                rangeX = cells[1 + xRowIndex, 1 + xCellIndex];
                rangeY = cells[yRowIndex + 1, yCellIndex + 1];
                range = mWorksheet.get_Range(rangeX, rangeY);
            }
            finally
            {
                Marshal.ReleaseComObject(rangeX);
                Marshal.ReleaseComObject(rangeY);
                Marshal.ReleaseComObject(cells);
            }

            return range;
        }

        #endregion











        public void AddWorkbook()
        {
            mWorkbook = mWorkbooks.Add(true);
            mWorksheet = (Worksheet)mWorkbook.ActiveSheet;

            mColCount = 0;
            mRowCount = 0;
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
        public void Save(string saveFilePath)
        {
            if (saveFilePath.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException("saveFilePath");
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
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="printerName"></param>
        /// <param name="isLandscape"></param>
        public void Print(string printerName = "", bool isLandscape = false)
        {
            try
            {
                for (int index = 0; index < mWorksheetArray.Length; index++)
                {
                    nsExcelApp.Worksheet wookSheet = mWorksheetArray[index];

                    if (isLandscape == true)
                    {
                        wookSheet.PageSetup.Orientation = nsExcelApp.XlPageOrientation.xlLandscape; // 横向
                    }
                    else
                    {
                        wookSheet.PageSetup.Orientation = nsExcelApp.XlPageOrientation.xlPortrait; // 纵向
                    }
                }

                mWorkbook.Save();

                // TODO 如何获取这样特殊的打印机名称 Ne04 ==> net 04 号打印机
                #region 选择打印机

                if (printerName.IsNullOrWhiteSpace() == false ||
                    sExcelApp.ActivePrinter.StartsWith(printerName) == false
                    )
                {
                    // 修改打印机测试成功
                    // Foxit Reader PDF Printer 在 Ne04:
                    // HP LaserJet Professional P1606dn 在 Ne03:
                    //string p = "Foxit Reader PDF Printer 在 Ne04:";
                    //excelApp.ActivePrinter = p;
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

        public void Reset()
        {
            Close();
            AddWorkbook();
        }


    }

}
