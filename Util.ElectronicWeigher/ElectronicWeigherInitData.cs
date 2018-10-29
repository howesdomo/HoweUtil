using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.ElectronicWeigher
{
    public class ElectronicWeigherInitData
    {
        /// <summary>
        /// 电子秤型号
        /// </summary>
        public string ElectronicWeigherModel { get; set; }

        #region 串口

        public string PortName { get; set; }


        public int? BaudRate { get; set; }

        public int? ThreadSleep_BeforeReadExisting { get; set; }

        #endregion

        #region 称重

        public decimal MinWeight { get; set; }

        public decimal MaxWeight { get; set; }

        /// <summary>
        /// buffer大小
        /// </summary>
        public int CalcCount { get; set; }

        /// <summary>
        /// 稳定个数
        /// </summary>
        public int StableCount { get; set; }

        /// <summary>
        /// 允许浮动差异
        /// </summary>
        public decimal FloatValue { get; set; }

        /// <summary>
        /// 重量单位
        /// </summary>
        public string UnitOfWeight { get; set; }

        #endregion
    }
}
