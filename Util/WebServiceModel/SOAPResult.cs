﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.WebServiceModel
{
    public class SOAPResult
    {
        public SOAPResult()
        {

        }

        /// <summary>
        /// 方法运行成功
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// 运行报错信息
        /// </summary>
        public string ExceptionInfo { get; set; }

        #region 业务逻辑

        /// <summary>
        /// 业务逻辑运行成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 业务逻辑报错信息
        /// </summary>
        public string BusinessExceptionInfo { get; set; }

        /// <summary>
        /// 返回内容
        /// </summary>
        public string ReturnObjectJson { get; set; }

        #endregion
    }

    public static class SOAPResultExtension
    {

        public static void Success(this SOAPResult r, object data = null)
        {
            r.IsComplete = true;
            r.IsSuccess = true;
            if (data != null)
            {
                r.ReturnObjectJson = JsonConvert.SerializeObject(data);
            }
        }

        public static void SuccessNonJsonConvert(this SOAPResult r, string data)
        {
            r.IsComplete = true;
            r.IsSuccess = true;
            if (data != null)
            {
                r.ReturnObjectJson = data;
            }
        }

        public static void Error(this SOAPResult r, Exception ex)
        {
            r.IsComplete = false;
            r.ExceptionInfo = ex.GetFullInfo();
            r.IsSuccess = false;
        }

        public static void Fail(this SOAPResult r, BusinessException ex)
        {
            r.IsComplete = true;
            r.IsSuccess = false;
            r.BusinessExceptionInfo = ex.Message;
        }

    }
}
