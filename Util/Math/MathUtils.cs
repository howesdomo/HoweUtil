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
        public static decimal ChineseRound(decimal value, int digits = 0)
        {
            return Math.Round(value, digits, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 四舍五入取整数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static decimal ChineseRound_0(decimal value)
        {
            return ChineseRound(value, 0);
        }

        /// <summary>
        /// 四舍五入保留一位小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static decimal ChineseRound_1(decimal value)
        {
            return ChineseRound(value, 1);
        }

        /// <summary>
        /// 四舍五入保留两位小数
        /// </summary>
        /// <param name="value"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static decimal ChineseRound_2(decimal value)
        {
            return ChineseRound(value, 2);
        }
    }
}
