using System;
using System.Collections.Generic;
using System.Text;

namespace CoreUtil
{
    public class EncodingUtils
    {
        public static string GetString(string gb2312info)
        {
            Encoding utf8 = Encoding.UTF8;
            Encoding gb2312 = Encoding.GetEncoding("gb2312");

            // Convert the string into a byte[].
            byte[] unicodeBytes = gb2312.GetBytes(gb2312info);
            // Perform the conversion from one encoding to the other.
            byte[] asciiBytes = Encoding.Convert(gb2312, utf8, unicodeBytes);
            // Convert the new byte[] into a char[] and then into a string.
            // This is a slightly different approach to converting to illustrate
            // the use of GetCharCount/GetChars.
            char[] asciiChars = new char[utf8.GetCharCount(asciiBytes, 0, asciiBytes.Length)];
            utf8.GetChars(asciiBytes, 0, asciiBytes.Length, asciiChars, 0);

            string utfinfo = string.Empty;
            utfinfo = new string(asciiChars);
            return utfinfo;
        }
    }
}
