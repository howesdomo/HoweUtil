using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Web
{
    public class EnpotIPAddressModel
    {
        public EnpotIPAddressModel()
        {

        }

        private string ValidateUnknowData(string data)
        {
            string result = string.IsNullOrEmpty(data) ? "未知" : data;
            return result.Replace("省", "").Replace("市", "");
        }

        ///// <summary>
        ///// 新浪
        ///// </summary>
        ///// <param name="ip"></param>
        ///// <param name="tmpModel"></param>
        //public EnpotIPAddressModel(string ip, SinaIPAddressModel tmpModel)
        //{
        //    this.IP = ip;
        //    this.Country = this.ValidateUnknowData(tmpModel.country);
        //    this.Province = this.ValidateUnknowData(tmpModel.province);
        //    this.City = this.ValidateUnknowData(tmpModel.city);
        //    this.District = this.ValidateUnknowData(tmpModel.district);
        //}

        ///// <summary>
        ///// 百度
        ///// </summary>
        ///// <param name="ip"></param>
        ///// <param name="tmpModel"></param>
        //public EnpotIPAddressModel(string ip, BaiduIPAddressModel tmpModel)
        //{
        //    this.IP = ip;
        //    this.Country = "中国"; // 由于百度只能定位中国，并且返回内容中没有国家信息，暂写死 中国。
        //    this.Province = this.ValidateUnknowData(tmpModel.content.address_detail.province);
        //    this.City = this.ValidateUnknowData(tmpModel.content.address_detail.city);
        //    this.District = this.ValidateUnknowData(tmpModel.content.address_detail.district);
        //}

        ///// <summary>
        ///// 淘宝
        ///// </summary>
        ///// <param name="ip"></param>
        ///// <param name="tmpModel"></param>
        //public EnpotIPAddressModel(string ip, TaobaoIPAddressModel tmpModel)
        //{
        //    this.IP = ip;
        //    this.Country = this.ValidateUnknowData(tmpModel.data.country);
        //    this.Province = this.ValidateUnknowData(tmpModel.data.region);
        //    this.City = this.ValidateUnknowData(tmpModel.data.city);
        //    this.District = "未知"; // 淘宝没有街区信息
        //}


        public string IP { get; set; }

        /// <summary>
        /// 国
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// 省
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 区
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 客户端浏览器的原始用户代理信息
        /// </summary>
        public string Device { get; set; }

        public override string ToString()
        {
            
            return JsonConvert.SerializeObject(this);
        }

        /// <summary>
        /// 获取地址完整信息
        /// </summary>
        /// <returns></returns>
        public string GetAddressAll()
        {
            return this.Country + this.Province + this.City;
        }
    }
}
