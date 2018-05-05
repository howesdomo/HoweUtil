using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Util.Web
{
    public class IPAddressUtils
    {
        #region 获取IP&地址信息

        ///// <summary>
        ///// (弃用)获取IP地址
        ///// 只获取新浪接口返回信息，已改用 GetIPAddressAdv，从多个接口获取后比较获取最佳结果
        ///// </summary>
        ///// <returns></returns>
        //public static EnpotIPAddressModel GetIPAddress()
        //{
        //    string arg_Ip = IPAddressUtils.GetRequestIP();
        //    //arg_Ip = "205.251.196.138";

        //    if (!IPAddressUtils.IsIP(arg_Ip))
        //    {
        //        throw new Exception("错误：无效IP信息。");
        //    }

        //    string url = string.Format("http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=json&ip={0}", arg_Ip);
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());
        //    string tmpString = sr.ReadToEnd();

        //    SinaIPAddressModel tmpIpAddressModel = null;
        //    try
        //    {
        //        tmpIpAddressModel = JsonConvert.DeserializeObject<SinaIPAddressModel>(tmpString);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("错误：转换Json信息失败。", ex);
        //    }
        //    EnpotIPAddressModel result = new EnpotIPAddressModel(arg_Ip, tmpIpAddressModel);
        //    result.Device = IPAddressUtils.GetUserAgent();
        //    return result;
        //}

        /// <summary>
        /// 获取IP地址 多接口比较版
        /// </summary>
        /// <returns></returns>
        public static EnpotIPAddressModel GetIPAddressAdv()
        {
            string arg_Ip = IPAddressUtils.GetRequestIP();
            if (!IPAddressUtils.IsIP(arg_Ip))
            {
                throw new Exception("错误：无效IP信息。");
            }

            List<EnpotIPAddressModel> all = new List<EnpotIPAddressModel>();
            all.Add(IPAddressUtils.SinaGetIPAddress(arg_Ip));
            //all.Add(IPAddressUtils.BaiduGetIPAddress(arg_Ip));
            //all.Add(IPAddressUtils.TaobaoGetIPAddress(arg_Ip));

            EnpotIPAddressModel result = IPAddressUtils.SelectAddress(arg_Ip, all);
            result.Device = IPAddressUtils.GetUserAgent();
            return result;
        }

        /// <summary>
        /// 按 市=》省=》国 来选出较准确的地址信息
        /// </summary>
        /// <param name="arg_IP"></param>
        /// <param name="all"></param>
        /// <returns></returns>
        private static EnpotIPAddressModel SelectAddress(string arg_IP, List<EnpotIPAddressModel> all)
        {
            EnpotIPAddressModel result = null;
            var query_City = from i in all
                             where i.City != "未知"
                             group i by new { i.City } into ix
                             select new
                             {
                                 City = ix.Key.City,
                                 Count = ix.Count()
                             };

            var matchCity = query_City.FirstOrDefault(i => i.Count == query_City.Max(o => o.Count));
            if ((matchCity != null && query_City.Count() == 1)
                     || (matchCity != null && matchCity.Count > 1 && query_City.Count() > 1))
            {
                result = all.FirstOrDefault(i => i.City == matchCity.City);
            }
            else
            {
                var query_Province = from i in all
                                     where i.Province != "未知"
                                     group i by new { i.Province } into ix
                                     select new
                                     {
                                         Province = ix.Key.Province,
                                         Count = ix.Count()
                                     };

                var matchProvince = query_Province.FirstOrDefault(i => i.Count == query_Province.Max(o => o.Count));

                if (
                     ((matchProvince != null && query_Province.Count() == 1)
                     || (matchProvince != null && matchProvince.Count > 1 && query_Province.Count() > 1))

                )
                {
                    result = all.FirstOrDefault(i => i.Province == matchProvince.Province);
                    result.City = string.Empty;
                }
                else
                {
                    var query_Country = from i in all
                                        where i.Country != "未知"
                                        group i by new { i.Country } into ix
                                        select new
                                        {
                                            Country = ix.Key.Country,
                                            Count = ix.Count()
                                        };

                    var matchCountry = query_Country.FirstOrDefault(i => i.Count == query_Country.Max(o => o.Count));
                    if ((matchCountry != null && query_Country.Count() == 1)
                     || (matchCountry != null && matchCountry.Count > 1 && query_Country.Count() > 1))
                    {
                        result = all.FirstOrDefault(i => i.Country == matchCountry.Country);
                        result.Province = string.Empty;
                        result.City = string.Empty;
                    }
                }
            }

            if (result == null)
            {
                result = new EnpotIPAddressModel()
                {
                    IP = arg_IP,
                    Country = "未知",
                    Province = string.Empty,
                    City = string.Empty
                };
            }

            return result;
        }

        /// <summary>
        /// (测试用)获取IP地址多接口比较
        /// </summary>
        /// <returns></returns>
        public static List<EnpotIPAddressModel> TestGetIPAddressAdv(string ipAddress)
        {
            string arg_Ip = string.Empty;
            arg_Ip = ipAddress;
            if (!IPAddressUtils.IsIP(arg_Ip))
            {
                throw new Exception("错误：无效IP信息。");
            }

            List<EnpotIPAddressModel> all = new List<EnpotIPAddressModel>();
            all.Add(IPAddressUtils.SinaGetIPAddress(arg_Ip));
            //all.Add(IPAddressUtils.BaiduGetIPAddress(arg_Ip));
            //all.Add(IPAddressUtils.TaobaoGetIPAddress(arg_Ip));

            return all;
        }

        protected static EnpotIPAddressModel SinaGetIPAddress(string arg_Ip)
        {
            EnpotIPAddressModel result = null;

            string url = string.Format("http://int.dpool.sina.com.cn/iplookup/iplookup.php?format=json&ip={0}", arg_Ip);
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());
                string tmpString = sr.ReadToEnd();

                dynamic tmpIpAddressModel = JsonConvert.DeserializeObject<dynamic>(tmpString);


                result = new EnpotIPAddressModel()
                {
                    IP = arg_Ip,
                    Country = tmpIpAddressModel.country,
                    Province = tmpIpAddressModel.province,
                    City = tmpIpAddressModel.city,
                    District = tmpIpAddressModel.district,
                    Device = string.Empty
                };
            }
            catch (Exception)
            {
                //throw new Exception("错误：转换Json信息失败。", ex);
                result = new EnpotIPAddressModel();
                result.IP = arg_Ip;
                result.Country = "未知";
                result.Province = "未知";
                result.City = "未知";
                result.District = "未知";
            }

            return result;
        }


        private string ValidateUnknowData(string data)
        {
            string result = string.IsNullOrEmpty(data) ? "未知" : data;
            return result.Replace("省", "").Replace("市", "");
        }

        //protected static EnpotIPAddressModel BaiduGetIPAddress(string arg_Ip)
        //{
        //    BaiduIPAddressModel tmpIpAddressModel = null;
        //    EnpotIPAddressModel result = null;

        //    string url = string.Format("http://api.map.baidu.com/location/ip?ak=F454f8a5efe5e577997931cc01de3974&ip={0}", arg_Ip);
        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //        System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());
        //        string tmpString = sr.ReadToEnd();

        //        tmpIpAddressModel = JsonConvert.DeserializeObject<BaiduIPAddressModel>(tmpString);
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw new Exception("错误：转换Json信息失败。", ex);

        //        result = new EnpotIPAddressModel();
        //        result.IP = arg_Ip;
        //        result.Country = "未知";
        //        result.Province = "未知";
        //        result.City = "未知";
        //        result.District = "未知";
        //    }

        //    if (tmpIpAddressModel.content == null)
        //    {
        //        result = new EnpotIPAddressModel();
        //        result.IP = arg_Ip;
        //        result.Country = "未知";
        //        result.Province = "未知";
        //        result.City = "未知";
        //        result.District = "未知";
        //    }
        //    else
        //    {
        //        result = new EnpotIPAddressModel(arg_Ip, tmpIpAddressModel);
        //    }
        //    return result;
        //}


        //protected static EnpotIPAddressModel TaobaoGetIPAddress(string arg_Ip)
        //{
        //    TaobaoIPAddressModel tmpIpAddressModel = null;
        //    EnpotIPAddressModel result = null;

        //    string url = string.Format("http://ip.taobao.com/service/getIpInfo.php?ip={0}", arg_Ip);
        //    try
        //    {
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        //        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //        System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream());
        //        string tmpString = sr.ReadToEnd();

        //        tmpIpAddressModel = JsonConvert.DeserializeObject<TaobaoIPAddressModel>(tmpString);
        //    }
        //    catch (Exception ex)
        //    {
        //        //throw new Exception("错误：转换Json信息失败。", ex);
        //        result = new EnpotIPAddressModel();
        //        result.IP = arg_Ip;
        //        result.Country = "未知";
        //        result.Province = "未知";
        //        result.City = "未知";
        //        result.District = "未知";
        //    }


        //    if (tmpIpAddressModel == null || tmpIpAddressModel.data == null ||
        //        string.IsNullOrEmpty(tmpIpAddressModel.data.country) || tmpIpAddressModel.data.country.Contains("内网IP"))
        //    {
        //        result = new EnpotIPAddressModel();
        //        result.IP = arg_Ip;
        //        result.Country = "未知";
        //        result.Province = "未知";
        //        result.City = "未知";
        //        result.District = "未知";
        //    }
        //    else
        //    {
        //        result = new EnpotIPAddressModel(arg_Ip, tmpIpAddressModel);
        //    }
        //    return result;
        //}

        #endregion


        /// <summary>
        /// 客户端IP
        /// </summary>
        public static string ClientIP()
        {
            //ClinetIP=context.Request.ServerVariables["REMOTE_ADDR"].ToString();
            string result = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (null == result || result == String.Empty)
            {
                result = System.Web.HttpContext.Current.Request.UserHostAddress;
            }

            return result;
        }


        /// <summary> 
        /// 取得客户端真实IP。如果有代理则取第一个非内网地址 
        /// </summary> 
        public static string GetRequestIP()
        {

            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (result != null && result != String.Empty)
            {
                //可能有代理 
                if (result.IndexOf(".") == -1) //没有“.”肯定是非IPv4格式 
                    result = null;
                else
                {
                    if (result.IndexOf(",") != -1)
                    {
                        //有“,”，估计多个代理。取第一个不是内网的IP。 
                        result = result.Replace(" ", "").Replace("'", "");
                        string[] temparyip = result.Split(",;".ToCharArray());
                        for (int i = 0; i < temparyip.Length; i++)
                        {
                            if (IsIP(temparyip[i])
                            && temparyip[i].Substring(0, 3) != "10."
                            && temparyip[i].Substring(0, 7) != "192.168"
                            && temparyip[i].Substring(0, 7) != "172.16.")
                            {
                                return temparyip[i]; // 到不是内网的地址
                            }
                        }
                    }
                    else if (IsIP(result)) //代理即是IP格式 
                        return result;
                    else
                        result = null; //代理中的内容 非IP，取IP 
                }
            }
            if (null == result || result == String.Empty)
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (result == null || result == String.Empty)
                result = HttpContext.Current.Request.UserHostAddress;

            if (result == "127.0.0.1" || result == "::1")//获取本机内网IP
            {
                System.Net.IPAddress[] addressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;//获取本机内网IP
                string strgetIP = addressList.Length > 0 ? addressList[0].ToString() : String.Empty;
                if (IsIP(strgetIP)) //代理即是IP格式 
                    result = strgetIP;
            }
            return result;
        }

        #region 验证是否为IP

        /// <summary>
        /// 判断是否为IP
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return ip.IsMatch(@"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

        }

        #endregion

        public static string GetUserAgent()
        {
            return System.Web.HttpContext.Current.Request.UserAgent;
        }

    }
}
