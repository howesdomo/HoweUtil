using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.IO
{
    public static class SerialPortUtil
    {
        #region BaudRate - 波特率

        public static List<BaudRate> GetBaudRateList()
        {
            List<BaudRate> r = new List<BaudRate>();

            r.Add(new BaudRate(110));
            r.Add(new BaudRate(300));
            r.Add(new BaudRate(1200));
            r.Add(new BaudRate(2400));
            r.Add(new BaudRate(4800));
            r.Add(new BaudRate(9600));
            r.Add(new BaudRate(19200));
            r.Add(new BaudRate(38400));
            r.Add(new BaudRate(57600));
            r.Add(new BaudRate(115200));
            r.Add(new BaudRate(230400));
            r.Add(new BaudRate(460800));
            r.Add(new BaudRate(921600));

            return r;
        }

        #endregion

        #region DataBits - 数据位

        public static List<DataBits> GetDataBitsList()
        {
            List<DataBits> r = new List<DataBits>();

            r.Add(new DataBits(5));
            r.Add(new DataBits(6));
            r.Add(new DataBits(7));
            r.Add(new DataBits(8));

            return r;
        }

        #endregion

        #region Parity - 奇偶校验

        public static List<Parity> GetParityList()
        {
            List<Parity> r = new List<Parity>();

            r.Add(new Parity() { Value = System.IO.Ports.Parity.None, XMLValue = "NONE", DisplayName = "None(无)" });
            r.Add(new Parity() { Value = System.IO.Ports.Parity.Odd, XMLValue = "ODD", DisplayName = "Odd(奇校验)" });
            r.Add(new Parity() { Value = System.IO.Ports.Parity.Even, XMLValue = "EVEN", DisplayName = "Even(偶校验)" });
            r.Add(new Parity() { Value = System.IO.Ports.Parity.Mark, XMLValue = "MARK", DisplayName = "Mark(标志)" });
            r.Add(new Parity() { Value = System.IO.Ports.Parity.Space, XMLValue = "SPACE", DisplayName = "Space(空格)" });

            return r;
        }



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

        #endregion

        #region StopBits - 停止位

        public static List<StopBits> GetStopBitsList()
        {
            List<StopBits> r = new List<StopBits>();

            r.Add(new StopBits() { Value = System.IO.Ports.StopBits.None, XMLValue = "NONE", DisplayName = "None(无)" });
            r.Add(new StopBits() { Value = System.IO.Ports.StopBits.One, XMLValue = "ONE", DisplayName = "1" });
            r.Add(new StopBits() { Value = System.IO.Ports.StopBits.OnePointFive, XMLValue = "ONEPOINTFIVE", DisplayName = "1.5" });
            r.Add(new StopBits() { Value = System.IO.Ports.StopBits.Two, XMLValue = "TWO", DisplayName = "2" });

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

        #endregion
    }

    /// <summary>
    /// BaudRate - 波特率
    /// </summary>
    public class BaudRate
    {
        public int Value { get; set; }

        public string XMLValue { get; set; }

        public string DisplayName { get; set; }

        public BaudRate()
        {

        }

        public BaudRate(int v)
        {
            this.Value = v;
            this.XMLValue = v.ToString();
            this.DisplayName = v.ToString();
        }
    }

    /// <summary>
    /// DataBits - 数据位
    /// </summary>
    public class DataBits
    {
        public int Value { get; set; }

        public string XMLValue { get; set; }

        public string DisplayName { get; set; }

        public DataBits()
        {

        }

        public DataBits(int v)
        {
            this.Value = v;
            this.XMLValue = v.ToString();
            this.DisplayName = v.ToString();
        }
    }

    /// <summary>
    /// Parity - 奇偶校验
    /// </summary>
    public class Parity
    {
        public System.IO.Ports.Parity Value { get; set; }

        public string XMLValue { get; set; }

        public string DisplayName { get; set; }

        public Parity()
        {

        }
    }

    /// <summary>
    /// StopBits - 停止位
    /// </summary>
    public class StopBits
    {
        public System.IO.Ports.StopBits Value { get; set; }

        public string XMLValue { get; set; }

        public string DisplayName { get; set; }

        public StopBits()
        {

        }
    }
}
