using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class ExceptionExtension
    {
        public static string GetFullInfo(this Exception e)
        {
            StringBuilder sb = new StringBuilder();
            CoreUtil.ExceptionUtil.GetExceptionFullInfo(e, sb);
            return sb.ToString();
        }

        /// <summary>
        /// 此方法用于反射调用方法时显示错误
        /// 跳过异常信息 "调用的目标发生了异常。"
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetFullInfoForInvoke(this Exception e)
        {
            StringBuilder sb = new StringBuilder();
            CoreUtil.ExceptionUtil.GetExceptionFullInfoForInvoke(e, sb);
            return sb.ToString();
        }
    }
}
