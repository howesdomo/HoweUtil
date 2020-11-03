using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Drawing.Printing
{
    public class ZPLPrinter : Printer
    {
        /// <summary>
        /// 浓度
        /// </summary>
        public int Darkness { get; set; }

        /// <summary>
        /// 打印速度
        /// </summary>
        public int Speed { get; set; }

        /// <summary>
        /// 向右偏移量
        /// </summary>
        public int Left { get; set; }

        /// <summary>
        /// 向下偏移量
        /// </summary>
        public int Top { get; set; }
    }
}
