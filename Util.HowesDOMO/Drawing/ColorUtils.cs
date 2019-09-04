using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Drawing
{
    public class ColorUtils
    {
        public static string GetColorHexString(System.Drawing.Color c)
        {
            return ColorUtils.GetColorHexString(c.R, c.G, c.B);
        }

        public static string GetColorHexString(int r, int g, int b)
        {
            return string.Format("#{0}{1}{2}",
                    r.ToString("X").PadLeft(2, '0'),
                    g.ToString("X").PadLeft(2, '0'),
                    b.ToString("X").PadLeft(2, '0'));
        }


        /// <summary>
        /// 根据字符串("#??????") 获取颜色
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static System.Drawing.Color getColor(string args)
        {
            string r = args.Substring(1, 2);
            string g = args.Substring(3, 2);
            string b = args.Substring(5, 2);

            int intR = Convert.ToInt32(r, 16);
            int intG = Convert.ToInt32(g, 16);
            int intB = Convert.ToInt32(b, 16);

            return System.Drawing.Color.FromArgb(intR, intG, intB);
        }
    }
}
