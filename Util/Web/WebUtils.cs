using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Util.Web
{
    public class WebUtils
    {
        public static string WebRequest(string url, string param = "")
        {
            Uri uri = new Uri(url);
            WebRequest request = System.Net.WebRequest.Create(uri);
            request.Timeout = 120 * 1000;
            request.ContentType = "application/x-www-form-urlencoded;";

            if (param.IsNullOrWhiteSpace() == true)
            {
                request.Method = "GET";
                request.ContentLength = 0;
            }
            else
            {
                request.Method = "POST";
                byte[] bts = System.Text.Encoding.UTF8.GetBytes(param);
                request.ContentLength = bts.Length;

                using (Stream reqStream = request.GetRequestStream())
                {
                    reqStream.Write(bts, 0, bts.Length);
                    reqStream.Flush();
                }
            }


            string responseString = string.Empty;
            using (WebResponse response = request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8))
                    {
                        responseString = reader.ReadToEnd();
                    }
                }
            }

            return responseString;
        }

        public static T WebRequest<T>(string url, string param = "")
        {
            string jsonStr = WebRequest(url, param);
            return Util.JsonUtils.DeserializeObject<T>(jsonStr);
        }

        /// <summary>
        /// <para>调用 HttpWebRequest 方法前调用一次本方法能修复报错</para>
        /// <para>执行本方法原因:在 Windows 2003 服务器中运行 HttpWebRequest 会发生报错。报错信息为【基础连接已经关闭: 未能为 SSL/TLS 安全通道建立信任关系。】</para>
        /// </summary>
        public static void FixWebRequest()
        {
            // Trust all certificates
            System.Net.ServicePointManager.ServerCertificateValidationCallback += ((sender, certificate, chain, sslPolicyErrors) => true);

            // Trust sender
            System.Net.ServicePointManager.ServerCertificateValidationCallback += ((sender, cert, chain, errors) => true);
        }
    }
}
