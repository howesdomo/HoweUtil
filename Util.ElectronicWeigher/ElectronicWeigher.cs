using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;

namespace Util.ElectronicWeigher
{
    public class ElectronicWeigher
    {
        /// <summary>
        /// 电子秤型号
        /// </summary>
        public string ElectronicWeigherModel { get; set; }

        SerialPort mSerialPort { get; set; }

        public ElectronicWeigher(ElectronicWeigherInitData args)
        {
            setInitData(args);
        }

        private void setInitData(ElectronicWeigherInitData args)
        {
            this.ElectronicWeigherModel = args.ElectronicWeigherModel;

            #region 串口

            mSerialPort = new SerialPort(args.PortName);
            if (args.BaudRate.HasValue == true)
            {
                mSerialPort.BaudRate = args.BaudRate.Value;
            }

            // TODO Special Setting
            // mSerialPort.Parity = Parity.None; 

            mSerialPort.DataReceived += SerialPort_DataReceived;
            mSerialPort.ErrorReceived += SerialPort_ErrorReceived;

            #endregion

            #region 称重

            mMinWeight = args.MinWeight;
            mMaxWeight = args.MaxWeight;
            mCalcCount = args.CalcCount;
            mFloatValue = args.FloatValue;
            mStableCount = args.StableCount;

            mUnitOfWeight = args.UnitOfWeight;

            #endregion
        }

        public void Open()
        {
            mSerialPort.Open();
        }

        public void Close()
        {
            if (mSerialPort != null && mSerialPort.IsOpen == true)
            {
                mSerialPort.DataReceived -= SerialPort_DataReceived;
                mSerialPort.ErrorReceived -= SerialPort_ErrorReceived;

                mSerialPort.Close();
            }
        }

        public void Reset(ElectronicWeigherInitData args)
        {
            Close();
            setInitData(args);
        }

        #region 串口数据接收

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            dataReceived();
        }

        /// <summary>
        /// 测试错误信息显示
        /// </summary>
        bool mIsTestShowError = false;


        Random rand = new Random();

        private void dataReceived()
        {
            string content = readSerialPortData();
            // System.Diagnostics.Debug.WriteLine("dataReceived : {0}".FormatWith(content));

            string valueStr = this.fixData(content); // 根据电子秤型号处理数据
            // System.Diagnostics.Debug.WriteLine("valueStr : {0}".FormatWith(valueStr));

            #region 测试错误信息显示

            //mIsTestShowError = true;
            //if (mIsTestShowError == true)
            //{
            //    int temp = rand.Next(100);
            //    if (temp % 10 == 0)
            //    {
            //        valueStr = valueStr + "mIsTestShowError ** mIsTestShowError";
            //    }
            //}

            #endregion

            decimal inputValue = 0M;
            if (decimal.TryParse(valueStr, out inputValue) == true)
            {
                CalcStableResult r = calcStable(inputValue); // 计算净重, 并判断是否稳定
                // System.Diagnostics.Debug.WriteLine(r.GetConsoleInfo());
                onNetWeightReceive(r);
            }
            else
            {
                CalcStableResult r = new CalcStableResult()
                {
                    IsComplete = false,
                    ExceptionInfo = "valueStr的值无法转换为 decimal。\r\ncontent ：{0}\r\nvalueStr ：{1}\r\n".FormatWith(content, valueStr),
                    EntryTime = DateTime.Now
                };
                onNetWeightReceive(r);
            }
        }

        private string readSerialPortData()
        {
            int tryTimes = 0;
            while (mSerialPort != null && mSerialPort.BytesToRead <= 0)
            {
                System.Threading.Thread.Sleep(100);
                tryTimes = tryTimes + 1;

                if (tryTimes >= 10)
                {
                    return string.Empty;
                }
            }

            string r = string.Empty;
            if (mSerialPort != null)
            {
                r = mSerialPort.ReadExisting();

                string msg = "readSerialPortData:{0}".FormatWith(r);
                System.Diagnostics.Debug.WriteLine(msg);
            }
            return r;
        }

        public const char char_Equal = '=';

        public const char char_Dot = '.';

        public const char char_Space = ' ';

        public const char char_minus = '-';

        private string fixData(string arg)
        {
            string r = arg;
            switch (this.ElectronicWeigherModel)
            {
                case "DHIII-30电子计重秤":
                    r = fixData_DHIII_30(arg);
                    break;
                case "T2000":
                    r = fixData_T2000(arg);
                    break;
                default:
                    break;
            }

            return r;
        }

        private string fixData_DHIII_30(string arg)
        {
            string temp = new string(arg.ToCharArray().Reverse().ToArray());
            return temp.Replace(char_Equal, char_Space).TrimAdv();
        }

        private string fixData_T2000(string arg)
        {
            string r = string.Empty;
            foreach (char c in arg)
            {
                if (char.IsDigit(c))
                {
                    r = r + c.ToString();
                }
                else if (c.Equals(char_Dot))
                {
                    r = r + c.ToString();
                }
                else if (c.Equals(char_minus))
                {
                    r = r + c.ToString();
                }
            }
            return r;
        }

        private void SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {

        }

        public void ErrorReceived()
        {

        }

        #endregion

        #region 称重计算

        List<decimal> mBuffer = new List<decimal>();

        decimal mMinWeight;

        decimal mMaxWeight;

        /// <summary>
        /// buffer大小
        /// </summary>
        int mCalcCount;

        /// <summary>
        /// 稳定最小个数
        /// </summary>
        int mStableCount;

        /// <summary>
        /// 允许浮动差异
        /// </summary>
        decimal mFloatValue;

        /// <summary>
        /// Round处理净重保留的位数
        /// </summary>
        int mRoundLenght
        {
            get { return 2; }
        }

        /// <summary>
        /// 计算稳定处理 mBuffer 保留的位数
        /// </summary>
        int mBufferRoundLenght
        {
            get { return 1; }
        }

        /// <summary>
        /// 毛重
        /// </summary>
        decimal mGrossWeight;

        ///// <summary>
        ///// 净重
        ///// </summary>
        //decimal NetWeight;

        /// <summary>
        /// 重量单位
        /// </summary>
        string mUnitOfWeight;

        private CalcStableResult calcStable(decimal inputValue)
        {
            decimal finalValue = inputValue - mGrossWeight;

            CalcStableResult r = new CalcStableResult();
            r.IsComplete = true;
            r.EntryTime = DateTime.Now;
            r.IsStable = false;

            r.OriginWeight = inputValue;
            r.GrossWeight = mGrossWeight;
            r.NetWeight = finalValue;

            r.UnitOfWeight = mUnitOfWeight;

            if (finalValue <= 0)
            {
                System.Diagnostics.Debug.WriteLine("由于净重值[{0}]小于0, 清空Buffer。".FormatWith(finalValue, mMinWeight));
                mBuffer.Clear();
                return r;
            }

            if (finalValue > 0 && finalValue < this.mMinWeight)
            {
                System.Diagnostics.Debug.WriteLine("由于净重值[{0}]小于最小值[{1}], 清空Buffer。".FormatWith(finalValue, mMinWeight));
                mBuffer.Clear();
                return r;
            }

            // 使用缓存设定的小数位值处理当前重量
            mBuffer.Add(decimal.Round(finalValue, mRoundLenght));
            if (mBuffer.Count > this.mCalcCount)
            {
                mBuffer.RemoveAt(0);
            }

            // 使用用于比较的小数位数处理所有缓存数据，排序并取出现次数最多的值
            var query = mBuffer.Select(i => Math.Round(i, mBufferRoundLenght));

            ValueOccur matchMaxOccur = (from p in query
                                        group p by p into g
                                        select new ValueOccur
                                        {
                                            Value = g.Key,
                                            Count = g.Count()
                                        })
                       .OrderByDescending(i => i.Count)
                       .FirstOrDefault()
                       ;


            if (matchMaxOccur != null &&
                Math.Abs(finalValue - matchMaxOccur.Value) <= this.mFloatValue &&
                matchMaxOccur.Count >= this.mStableCount &&
                finalValue >= mMinWeight && finalValue <= mMaxWeight
                )
            {
                r.IsStable = true;
            }

            return r;
        }

        #endregion

        #region 重设毛重

        /// <summary>
        /// 重设毛重
        /// </summary>
        public void ResetGrossWeight(decimal value)
        {
            mGrossWeight = value;
            mBuffer.Clear();
        }

        #endregion

        public EventHandler<Util.ElectronicWeigher.CalcStableResult> NetWeightReceiveEventHandler;

        private void onNetWeightReceive(CalcStableResult args)
        {
            if (this.NetWeightReceiveEventHandler != null)
            {
                NetWeightReceiveEventHandler.Invoke(sender: null, e: args);
            }
        }
    }

    internal class ValueOccur
    {
        public decimal Value { get; set; }
        public int Count { get; set; }
    }
}
