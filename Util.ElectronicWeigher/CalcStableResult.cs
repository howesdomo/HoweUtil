using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.ElectronicWeigher
{
    public class CalcStableResult : EventArgs
    {
        /// <summary>
        /// 方法运行成功
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// 运行报错信息
        /// </summary>
        public string ExceptionInfo { get; set; }

        /// <summary>
        /// 生成时间
        /// </summary>
        public DateTime EntryTime { get; set; }

        /// <summary>
        /// 是否稳定
        /// </summary>
        public bool IsStable { get; set; }

        /// <summary>
        /// 源重 - 串口接收的值
        /// </summary>
        public decimal OriginWeight { get; set; }

        /// <summary>
        /// 净重 = 源重 - 毛重
        /// </summary>
        public decimal NetWeight { get; set; }

        /// <summary>
        /// 毛重
        /// </summary>
        public decimal GrossWeight { get; set; }

        /// <summary>
        /// 重量单位
        /// </summary>
        public string UnitOfWeight { get; set; }

        public string GetConsoleInfo()
        {
            return "*****\r\n稳定 : {0}\r\nOriginWeight ：{1}\r\nNetWeight : {2}\r\n".FormatWith(this.IsStable, OriginWeight, NetWeight);
        }
    }
}
