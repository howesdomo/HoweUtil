using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.E;

namespace CoreUtil.Xamarin
{
    public interface DeviceInformation
    {
        /// <summary>
        /// 获取机器设备
        /// </summary>
        /// <returns></returns>
        string GetDeviceInfo();
    }
}
