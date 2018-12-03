using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// V 1.0.0.1 加上 GetInfo, 用于简化 UI 报错的代码
    /// </summary>
    public static class ExceptionExtension
    {
        /// <summary>
        /// 先判断是否为 BusinessException
        /// 
        /// 若是 返回 BusinessException 信息
        /// 否则 返回 GetFullInfo 信息
        /// 
        /// WPF 调用 WebService 时使用居多
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetInfo(this Exception e)
        {
            string msg = e.GetFullInfo();
            if (e.IsBusinessException() == true)
            {
                int cStartIndex = msg.IndexOf(BusinessException.cStart) + 1;
                int cEndIndex = msg.IndexOf(BusinessException.cEnd);
                return msg.Substring(cStartIndex, cEndIndex - cStartIndex);
            }
            else
            {
                return msg;
            }
        }

        public static string GetFullInfo(this Exception e)
        {
            StringBuilder sb = new StringBuilder();
            Util.ExceptionUtil.GetExceptionFullInfo(e, sb);
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
            Util.ExceptionUtil.GetExceptionFullInfoForInvoke(e, sb);
            return sb.ToString();
        }

        public static bool IsBusinessException(this Exception e)
        {
            string msg = e.GetFullInfo();
            
            int indexOf_cStart = msg.IndexOf(BusinessException.cStart);
            int indexOf_cEnd = msg.IndexOf(BusinessException.cEnd);

            bool r = indexOf_cStart >= 0 && indexOf_cEnd > indexOf_cStart;
            return r;
        }




    }
}
