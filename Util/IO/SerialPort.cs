using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.IO
{
    public static class SerialPortUtil
    {
        public static System.IO.Ports.Parity GetParity(this string args)
        {
            System.IO.Ports.Parity r = System.IO.Ports.Parity.None;

            switch (args.ToUpper())
            {
                case "NONE": r = System.IO.Ports.Parity.None; break;
                case "ODD": r = System.IO.Ports.Parity.Odd; break;
                case "EVEN": r = System.IO.Ports.Parity.Even; break;
                case "MARK": r = System.IO.Ports.Parity.Mark; break;
                case "SPACE": r = System.IO.Ports.Parity.Space; break;
                default:
                    throw new Exception("值不在 Parity 枚举内。（args = {0}）".FormatWith(args));
            }

            return r;
        }

        public static System.IO.Ports.StopBits GetStopBits(this string args)
        {
            System.IO.Ports.StopBits r = System.IO.Ports.StopBits.None;

            switch (args.ToUpper())
            {
                case "NONE": r = System.IO.Ports.StopBits.None; break;
                case "ONE": r = System.IO.Ports.StopBits.One; break;
                case "ONEPOINTFIVE": r = System.IO.Ports.StopBits.OnePointFive; break;
                case "TWO": r = System.IO.Ports.StopBits.Two; break;
                default:
                    throw new Exception("值不在 StopBits 枚举内。（args = {0}）".FormatWith(args));
            }

            return r;
        }
    }
}
