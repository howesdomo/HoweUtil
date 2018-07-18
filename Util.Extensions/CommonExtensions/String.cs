using System;

// 拼音
using Microsoft.International.Converters.PinYinConverter;

// 简繁转换
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;


namespace System
{
    public static class StringExtension
    {

        #region 中文

        /// <summary>
        /// 获取拼音
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToPinYin(this string s)
        {
            string r = string.Empty;
            foreach (char obj in s)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString(); // 从拼音列表中获取第一个值 ( 例如 左 ==> ZUO3 )
                    r += t.Substring(0, t.Length - 1); // 去掉音调 ( 例如 ZUO3 ==> ZUO )
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }

        /// <summary>
        /// 获取拼音首字母
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToPinYinFirstLetter(this string s)
        {
            string r = string.Empty;
            foreach (char obj in s)
            {
                try
                {
                    ChineseChar chineseChar = new ChineseChar(obj);
                    string t = chineseChar.Pinyins[0].ToString();  // 从拼音列表中获取第一个值 ( 例如 左 ==> ZUO3 )
                    r += t.Substring(0, 1); // 获取拼音首字母 ( 例如 ZUO3 ==> Z )
                }
                catch
                {
                    r += obj.ToString();
                }
            }
            return r;
        }

        /// <summary>
        /// 转为繁体
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToTraditionalString(this string s)
        {
            string r = string.Empty;
            r = ChineseConverter.Convert(s, ChineseConversionDirection.SimplifiedToTraditional);
            return r;
        }

        /// <summary>
        /// 转为简体
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToSimplifiedString(this string s)
        {
            string r = string.Empty;
            r = ChineseConverter.Convert(s, ChineseConversionDirection.TraditionalToSimplified);
            return r;
        }

        #endregion
    }
}
