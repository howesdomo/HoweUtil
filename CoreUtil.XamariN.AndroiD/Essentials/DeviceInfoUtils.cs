using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace CoreUtil.XamariN.Essentials
{

    public class DeviceInfoUtils
    {
        public static CoreUtil.XamariN.Essentials.DeviceInfo GetDeviceInfo()
        {
            return new CoreUtil.XamariN.Essentials.DeviceInfo()
            {
                Model = Xamarin.Essentials.DeviceInfo.Model,
                Manufacturer = Xamarin.Essentials.DeviceInfo.Manufacturer,
                DeviceName = Xamarin.Essentials.DeviceInfo.Name,

                VersionMajor = Xamarin.Essentials.DeviceInfo.Version.Major,
                VersionMinor = Xamarin.Essentials.DeviceInfo.Version.Minor,
                VersionBuild = Xamarin.Essentials.DeviceInfo.Version.Build,
                VersionRevision = Xamarin.Essentials.DeviceInfo.Version.Revision,
                VersionInfo = Xamarin.Essentials.DeviceInfo.VersionString,

                Platform = Xamarin.Essentials.DeviceInfo.Platform,
                Idiom = Xamarin.Essentials.DeviceInfo.Idiom,
                DeviceType = Xamarin.Essentials.DeviceInfo.DeviceType.ToString(),
            };
        }

        public static string GetDeviceInfoMessage()
        {
            //// Device Model (SMG-950U)
            //var device = Xamarin.Essentials.DeviceInfo.Model;

            //// Manufacturer (Samsung)
            //var manufacturer = Xamarin.Essentials.DeviceInfo.Manufacturer;

            //// Device Name (Motz's iPhone)
            //var deviceName = Xamarin.Essentials.DeviceInfo.Name;

            //// Operating System Version Number (7.0)
            //var version = Xamarin.Essentials.DeviceInfo.VersionString;

            //// Platform (Android)
            //var platform = Xamarin.Essentials.DeviceInfo.Platform;

            //// Idiom (Phone)
            //var idiom = Xamarin.Essentials.DeviceInfo.Idiom;

            //// Device Type (Physical)
            //var deviceType = Xamarin.Essentials.DeviceInfo.DeviceType.ToString();

            var m = GetDeviceInfo();
            return m.ToString();
        }

        public static string GetDeviceInfoJsonStr()
        {
            return CoreUtil.JsonUtils.SerializeObject(GetDeviceInfo());
        }
    }
}
