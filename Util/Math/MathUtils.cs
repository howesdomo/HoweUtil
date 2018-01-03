using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
    public class MathUtils
    {
        /// <summary>
        /// 四舍五入
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public double ChineseRound(double value, int digits = 0)
        {
            return Math.Round(value, digits, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 四舍五入取整数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public double ChineseRound_ZhengShu(double value)
        {
            return ChineseRound(value, 0);
        }

        /// <summary>
        /// 四舍五入保留两位小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public double ChineseRound_2(double value)
        {
            return ChineseRound(value, 2);
        }
    }
}
