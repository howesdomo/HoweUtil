using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace CoreUtil.XamariN.AndroiD
{

    public class DeviceInfomation // : CoreUtil.Xamarin.IDeviceInformation
    {
        public static string GetDeviceInfo()
        {
            // Device Model (SMG-950U)
            var device = Xamarin.Essentials.DeviceInfo.Model;

            // Manufacturer (Samsung)
            var manufacturer = DeviceInfo.Manufacturer;

            // Device Name (Motz's iPhone)
            var deviceName = DeviceInfo.Name;

            // Operating System Version Number (7.0)
            var version = DeviceInfo.VersionString;

            // Platform (Android)
            var platform = DeviceInfo.Platform;

            // Idiom (Phone)
            var idiom = DeviceInfo.Idiom;

            // Device Type (Physical)
            var deviceType = DeviceInfo.DeviceType;

            return string.Format("device:{0}\r\nmanufacturer:{1}\r\ndeviceName:{2}\r\nversion:{3}\r\nplatform:{4}\r\nidiom:{5}\r\ndeviceType:{6}",
                device, manufacturer, deviceName, version, platform, idiom, deviceType);
        }
    }
}
