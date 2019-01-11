using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Excel
{
    public enum CellType
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
        public int? SheetIndex { get; set; }

        public string SheetName { get; set; }

        /// <summary>
        /// 指定特殊的单元格读取规则
        /// </summary>
        public Dictionary<int, CellType> CellReadRule { get; set; }

    }

    public class ExcelReaderConfig
    {
        /// <summary>
        /// 填写上SheetNo 或 SheetName 的工作表才被读取, 其中若填入对应Column的读取规则, 按照规则进行读取
        /// </summary>
        public List<SheetReadConfig> Config { get; set; }
    }


    
}
