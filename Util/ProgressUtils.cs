using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Util
{
    public class ProgressUtils
    {
        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="fileName">运行的程序</param>
        /// <param name="arguments">参数</param>
        /// <param name="windowStyle">程序运行窗口模式(默认静默模式)</param>
        public static void ExcuteProgressWithArguments(string fileName, string arguments, ProcessWindowStyle windowStyle = ProcessWindowStyle.Hidden)
        {
            using (var p = new Process())
            {
                p.StartInfo.FileName = fileName;
                p.StartInfo.Arguments = arguments;
                p.StartInfo.WindowStyle = windowStyle;

                p.Start();
                p.WaitForExit();
            }
        }
    }
}
