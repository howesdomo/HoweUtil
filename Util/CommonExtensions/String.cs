﻿using System;
using System.Text.RegularExpressions;

namespace System
{
    public static class StringExtension
    {
        #region 空值判断

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool IsNullOrWhiteSpace(this string s)
        {
            return string.IsNullOrWhiteSpace(s);
        }

        #endregion 空值判断

        #region Trim

        /// <summary>
        /// 去掉换行符号后 Trim
        /// </summary>
        /// <param name="content"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string TrimAdv(this string content, params object[] args)
        {
            if (content == null)
            {
                return null;
            }

            return content.Replace("\r", "").Replace("\n", "").Trim();
        }
        
        #endregion

        #region String.Format ( 格式化 )

        public static string FormatWith(this string format, params object[] args)
        {
            return string.Format(format, args);
        }

        public static string Format(IFormatProvider provider, string format, params object[] args)
        {
            return string.Format(provider, format, args);
        }

        // *** 尽管第1个的Format功能强大到可以取代中间的三个，但它的效率不高。
        // 以下3个重载是出于性能的考虑而增加的
        public static string Format(this string format, object arg0)
        {
            return string.Format(format, arg0);
        }

        public static string Format(this string format, object arg0, object arg1)
        {
            return string.Format(format, arg0, arg1);
        }

        public static string Format(this string format, object arg0, object arg1, object arg2)
        {
            return string.Format(format, arg0, arg1, arg2);
        }

        #endregion

        #region Regex 正则表达式

        /// <summary>
        /// 判断是否符合正则表达式
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsMatch(this string s, string pattern)
        {
            if (s == null) return false;
            else return Regex.IsMatch(s, pattern);
        }

        /// <summary>
        /// 获取符合正则表达式的值
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string Match(this string s, string pattern)
        {
            if (s == null) return "";
            return Regex.Match(s, pattern).Value;
        }

        /// <summary>
        /// 使用正则表达式进行替换字符串
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pattern"></param>
        /// <param name="replaceValue"></param>
        /// <returns></returns>
        public static string ReplaceWithRegexPattern(this string s, string pattern, string replaceValue)
        {
            if (s == null) return "";
            return Regex.Replace(s, pattern, replaceValue);
        }

        #endregion

        #region 常用类型转换

        public static bool IsBoolean(this string s)
        {
            bool i;
            return bool.TryParse(s, out i);
        }

        public static bool ToBoolean(this string s)
        {
            return bool.Parse(s);
        }

        public static bool IsInt(this string s)
        {
            int i;
            return int.TryParse(s, out i);
        }

        public static int ToInt(this string s)
        {
            return int.Parse(s);
        }

        public static bool IsLong(this string s)
        {
            long i;
            return long.TryParse(s, out i);
        }

        public static long ToLong(this string s)
        {
            return long.Parse(s);
        }

        public static bool IsDouble(this string s)
        {
            double i;
            return double.TryParse(s, out i);
        }

        public static double ToDouble(this string s)
        {
            return double.Parse(s);
        }

        public static bool IsDecimal(this string s)
        {
            decimal i;
            return decimal.TryParse(s, out i);
        }

        public static decimal ToDecimal(this string s)
        {
            return decimal.Parse(s);
        }

        public static bool IsDateTime(this string s)
        {
            DateTime i;
            return DateTime.TryParse(s, out i);
        }

        public static DateTime ToDateTime(this string s)
        {
            return DateTime.Parse(s);
        }

        #endregion

        #region 字符串转化 美化

        public static string ToCamel(this string s)
        {
            // 尚可进一步优化 根据英文单词 进行驼峰式命名
            if (s.IsNullOrEmpty()) return s;
            return s[0].ToString().ToLower() + s.Substring(1);
        }

        public static string ToPascal(this string s)
        {
            // 尚可进一步优化 根据英文单词 进行Pascal命名
            if (s.IsNullOrEmpty()) return s;
            return s[0].ToString().ToUpper() + s.Substring(1);
        }

        #endregion

        #region GZip

        public static string GZip_Compress2String(this string toZipString)
        {
            string r = Util.GZipUtils.Compress2String(toZipString);
            return r;
        }

        public static string GZip_Decompress2String(this string zippedString)
        {
            string r = Util.GZipUtils.Decompress2String(zippedString);
            return r;
        }

        #endregion

        #region DES 加密 & 解密

        /// <summary>
        /// DES 加密
        /// </summary>
        /// <param name="toEncryptString"></param>
        /// <returns></returns>
        public static string DES_Encrypt(this string toEncryptString)
        {
            return Util.Cryptography.DESUtils.Encrypt(toEncryptString);
        }

        /// <summary>
        /// DES 解密
        /// </summary>
        /// <param name="textToEncrypt"></param>
        /// <returns></returns>
        public static string DES_Decrypt(this string toDecryptString)
        {
            return Util.Cryptography.DESUtils.Decrypt(toDecryptString);
        }

        #endregion

        #region RSA 加密 & 解密

        /// <summary>
        /// RSA 加密
        /// </summary>
        /// <param name="toEncryptString"></param>
        /// <returns></returns>
        public static string RSA_Encrypt(this string toEncryptString, string publickey = "")
        {
            byte[] temp = Util.Cryptography.RSAUtils.Encrypt(toEncryptString, publickey);
            return temp.ToString();
        }

        /// <summary>
        /// RSA 解密
        /// </summary>
        /// <param name="textToEncrypt"></param>
        /// <returns></returns>
        public static string RSA_Decrypt(this string toDecryptString, string privateKey = "")
        {
            byte[] temp = Util.Cryptography.RSAUtils.Decrypt(toDecryptString, privateKey);
            return temp.ToString();
        }


        #endregion
    }
}
