using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Drawing
{
    public static class ColorExtension
    {
        /// <summary>
        /// 获取16进制颜色
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string ToHexString(this System.Drawing.Color c)
        {
            return Util.Drawing.ColorUtils.GetColorHexString(c);
        }
    }
}
