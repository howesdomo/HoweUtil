using Microsoft.Office.Interop.Excel;
using nsExcelApp = Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace Util.Excel
{
    [Obsolete]
    public class ExcelApp_V2 : IDisposable
    {
        private Application mExcelApp;
        private Workbooks mWorkbooks;
        private Workbook mWorkbook;
        private Worksheet mWorksheet;

        public ExcelApp_V2()
        {
            mExcelApp = new Application();

            mExcelApp.Visible = false;
            mExcelApp.DisplayAlerts = false;

            mWorkbooks = mExcelApp.Workbooks;
        }

        public int ColCount { get; set; }

        public int RowCount { get; set; }

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

        #region Get Excel Range

        public Range GetCell(int rowIndex, int cellIndex)
        {
            Range cells = null;
            Range range = null;

            try
            {
                cells = mExcelApp.Cells;
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
                cells = mExcelApp.Cells;
                rangeX = cells[1, 1 + cellIndex];
                rangeY = cells[RowCount, 1 + cellIndex];
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
                cells = mExcelApp.Cells;
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

        public void Save()
        {
            if (mWorkbook.Saved == false)
            {
                mWorkbook.Save();
            }
        }

        public void SaveCopyAs(string fullFilePath)
        {
            if (string.IsNullOrEmpty(fullFilePath))
            {
                throw new ArgumentNullException("fullFilePath");
            }

            string directory = Path.GetDirectoryName(fullFilePath);

            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentException("fullFilePath is not a valid file path.");
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            mWorkbook.SaveCopyAs(fullFilePath);
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

            mWorksheet = (Worksheet)mWorkbook.ActiveSheet;

            ColCount = 0;
            RowCount = 0;
        }

        public void AddWorkbook()
        {
            mWorkbook = mWorkbooks.Add(true);
            mWorksheet = (Worksheet)mWorkbook.ActiveSheet;

            ColCount = 0;
            RowCount = 0;
        }

        /// <summary>
        /// TODO 执行打印后无法执行关闭后台进程
        /// </summary>
        /// <param name="printerName"></param>
        /// <param name="isLandscape"></param>
        public void Print(string printerName = "", bool isLandscape = false)
        {
            try
            {
                for (int index = 0; index < mWorkbook.Sheets.Count; index++)
                {
                    nsExcelApp.Worksheet wookSheet = (nsExcelApp.Worksheet)mWorkbook.Worksheets[index + 1];

                    if (isLandscape == true)
                    {
                        wookSheet.PageSetup.Orientation = nsExcelApp.XlPageOrientation.xlLandscape; // 横向
                    }
                    else
                    {
                        wookSheet.PageSetup.Orientation = nsExcelApp.XlPageOrientation.xlPortrait; // 纵向
                    }
                }

                #region 选择打印机

                if (printerName.IsNullOrWhiteSpace() == false ||
                    mExcelApp.ActivePrinter.StartsWith(printerName) == false
                    )
                {
                    // TODO 如何获取这样特殊的打印机名称 Ne04 ==> net 04 号打印机

                    // 修改打印机测试成功
                    // Foxit Reader PDF Printer 在 Ne04:
                    // HP LaserJet Professional P1606dn 在 Ne03:
                    //string p = "Foxit Reader PDF Printer 在 Ne04:";
                    //excelApp.ActivePrinter = p;
                }

                #endregion

                mWorkbook.PrintOutEx();

                // *** 核心代码, 等待 x 秒, 让打印任务得以完成, 之后再进行Close的方法 ***
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
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
                if (mExcelApp != null)
                {
                    mExcelApp.Quit();
                    Marshal.ReleaseComObject(mExcelApp);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("dispose ExcelApp object failed", ex);
            }

            mWorkbooks = null;
            mExcelApp = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion
    }
}
