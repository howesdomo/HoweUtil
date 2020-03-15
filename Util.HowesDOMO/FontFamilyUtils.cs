using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util
{
	/// <summary>
	/// V 1.0.0 - 2020-03-13 13:38:14
	/// 首次创建 方便获取指定字体
	/// </summary>
	public class FontFamilyUtils
	{
		public static string GetFontName(string fontFilePath)
		{
			var glyphTypeface = new System.Windows.Media.GlyphTypeface(new Uri(fontFilePath));
			return glyphTypeface.FamilyNames.Values.FirstOrDefault(); // 获取字体名
		}

		public static System.Windows.Media.FontFamily GetFontFamily_TypeOf_System_Windows_Media(string fontFilePath)
		{
			string fontName = GetFontName(fontFilePath);

			var directory = System.IO.Path.GetDirectoryName(fontFilePath);
			var fontUri = $"{new Uri(directory).AbsoluteUri}/#{fontName}";

			return new System.Windows.Media.FontFamily(fontUri);
		}

		public static System.Drawing.FontFamily GetFontFamily_TypeOf_System_Drawing(string fontFilePath)
		{
			var collection = new System.Drawing.Text.PrivateFontCollection();
			collection.AddFontFile(fontFilePath);
			string fontName = GetFontName(fontFilePath);

			return new System.Drawing.FontFamily(fontName, collection);
		}
	}
}
