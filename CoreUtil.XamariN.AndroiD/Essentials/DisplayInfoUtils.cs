using System;
using System.Collections.Generic;
using System.Text;

namespace CoreUtil.XamariN.Essentials
{
    public class DisplayInfoUtils
    {
        public static DisplayInfo GetDisplayInfo()
        {
            // Get Metrics
            var metrics = Xamarin.Essentials.DeviceDisplay.ScreenMetrics;

            //// Orientation (Landscape, Portrait, Square, Unknown)
            //var orientation = metrics.Orientation;

            //// Rotation (0, 90, 180, 270)
            //var rotation = metrics.Rotation;

            //// Width (in pixels)
            //var width = metrics.Width;

            //// Height (in pixels)
            //var height = metrics.Height;

            //// Screen density
            //var density = metrics.Density;

            return new DisplayInfo()
            {
                Orientation = metrics.Orientation.ToString(),
                RotationInfo = metrics.Rotation.ToString(),
                Rotation = int.Parse(metrics.Rotation.ToString().ToUpper().Replace("ROTATION", "")),
                Width = metrics.Width,
                Height = metrics.Height,
                Density = metrics.Density
            };
        }
    }
}
