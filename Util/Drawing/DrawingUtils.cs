using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Util.Drawing
{
    public class DrawingUtils
    {
        #region 旋转

        /// <summary>
        /// 对图像进行旋转(angle大于0 以逆时针方向旋转; angle小于0 以顺时针方向旋转)
        /// </summary>
        /// <param name="b">位图</param>
        /// <param name="angle">旋转角度[0,360]</param>
        /// <returns></returns>
        public static Bitmap Rotate(Bitmap b, int angle)
        {
            angle = angle % 360;

            //弧度转换
            double radian = angle * Math.PI / 180.0;
            double cos = Math.Cos(radian);
            double sin = Math.Sin(radian);

            //原图的宽和高
            int w = b.Width;
            int h = b.Height;
            int W = (int)(Math.Max(Math.Abs(w * cos - h * sin), Math.Abs(w * cos + h * sin)));
            int H = (int)(Math.Max(Math.Abs(w * sin - h * cos), Math.Abs(w * sin + h * cos)));

            //目标位图
            Bitmap dsImage = new Bitmap(W, H);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(dsImage))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Bilinear;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                //计算偏移量
                Point Offset = new Point((W - w) / 2, (H - h) / 2);

                //构造图像显示区域：让图像的中心与窗口的中心点一致
                Rectangle rect = new Rectangle(Offset.X, Offset.Y, w, h);
                Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
                g.TranslateTransform(center.X, center.Y);
                g.RotateTransform(360 - angle);

                //恢复图像在水平和垂直方向的平移
                g.TranslateTransform(-center.X, -center.Y);
                g.DrawImage(b, rect);

                //重至绘图的所有变换
                g.ResetTransform();
                g.Save();
            }

            return dsImage;
        }

        #endregion 旋转

        #region 缩放

        /// <summary>
        /// 缩放图像
        /// </summary>
        /// <param name="sourceImage">源图像</param>
        /// <param name="width">目标宽度</param>
        /// <param name="height">目标高度</param>
        /// <param name="mode">缩放模式</param>
        /// <returns></returns>
        public static Image Resize(Image sourceImage, int width, int height, string mode)
        {
            int towidth = width;
            int toheight = height;

            int x = 0;
            int y = 0;

            int ow = sourceImage.Width;
            int oh = sourceImage.Height;

            switch (mode)
            {
                case "HW":  // 指定高宽缩放（可能变形）                
                    break;
                case "W":   // 指定宽，高按比例                    
                    toheight = sourceImage.Height * width / sourceImage.Width;
                    break;
                case "H":   // 指定高，宽按比例
                    towidth = sourceImage.Width * height / sourceImage.Height;
                    break;
                case "Cut": // 指定高宽裁减（不变形）                
                    if ((double)sourceImage.Width / (double)sourceImage.Height > (double)towidth / (double)toheight)
                    {
                        oh = sourceImage.Height;
                        ow = sourceImage.Height * towidth / toheight;
                        y = 0;
                        x = (sourceImage.Width - ow) / 2;
                    }
                    else
                    {
                        ow = sourceImage.Width;
                        oh = sourceImage.Width * height / towidth;
                        x = 0;
                        y = (sourceImage.Height - oh) / 2;
                    }
                    break;
                default:
                    break;
            }

            System.Drawing.Image result = new System.Drawing.Bitmap(towidth, toheight);
            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(result)) // 新建一个画板
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High; // 设置高质量插值法
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality; // 设置高质量,低速度呈现平滑程度
                g.Clear(System.Drawing.Color.Transparent); // 清空画布并以透明背景色填充
                // 在指定位置并且按指定大小绘制原图片的指定部分
                g.DrawImage(sourceImage,
                    new System.Drawing.Rectangle(0, 0, towidth, toheight),
                    new System.Drawing.Rectangle(x, y, ow, oh),
                    System.Drawing.GraphicsUnit.Pixel);

                return result;
            }
        }

        #endregion

        #region 渲染

        /// <summary>
        /// PS 反相 / 底片效果
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Image Invert(Image img)
        {
            int Height = img.Height;
            int Width = img.Width;
            Bitmap result = new Bitmap(Width, Height);
            Bitmap bitmap = (Bitmap)img;
            Color pixel;
            for (int x = 1; x < Width; x++)
            {
                for (int y = 1; y < Height; y++)
                {
                    pixel = bitmap.GetPixel(x, y);

                    if (pixel.Name.Equals(Color.Transparent.Name) == false)
                    {
                        int r = 255 - pixel.R;
                        int g = 255 - pixel.G;
                        int b = 255 - pixel.B;

                        result.SetPixel(x, y, Color.FromArgb(r, g, b));
                    }
                    else
                    {
                        // TODO 原来透明的颜色,不能维持原本透明, 只能转为白色                        
                    }
                }
            }
            return result;
        }

        /// <summary>
        ///  PS 黑白 效率极低
        /// </summary>
        /// <param name="img"></param>
        /// <param name="iType">0 平均值法; 1 最大值法; 2 加权平均值法;</param>
        /// <returns></returns>
        [Obsolete]
        public static Image BlackWhite(Image img, int iType = 2)
        {
            // 测试所得 7M 的彩图需要 4000ms 左右时间才能处理完毕

            Bitmap result = new Bitmap(img.Width, img.Height);
            Bitmap oldBitmap = (Bitmap)img;
            Color pixel;
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    pixel = oldBitmap.GetPixel(x, y);
                    int r, g, b, Result = 0;
                    r = pixel.R;
                    g = pixel.G;
                    b = pixel.B;

                    switch (iType)
                    {
                        case 0://平均值法
                            Result = ((r + g + b) / 3);
                            break;
                        case 1://最大值法
                            Result = r > g ? r : g;
                            Result = Result > b ? Result : b;
                            break;
                        case 2://加权平均值法
                            Result = ((int)(0.7 * r) + (int)(0.2 * g) + (int)(0.1 * b));
                            break;
                    }
                    result.SetPixel(x, y, Color.FromArgb(Result, Result, Result));
                }
            }
            return result;

        }

        /// <summary>
        /// PS 黑白 极其高效
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="iType">0 平均值法; 1 最大值法; 2 加权平均值法;</param>
        /// <returns></returns>
        public static Bitmap BlackWhiteV2(Bitmap bitmap, int iType = 2)
        {
            // 测试所得 7M 的彩图只需要 500ms 内即可处理完毕

            int width = bitmap.Width;
            int height = bitmap.Height;

            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(rect,
                System.Drawing.Imaging.ImageLockMode.ReadWrite,
                System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            IntPtr ptr = bmpData.Scan0;
            int bytesCount = bmpData.Stride * bmpData.Height;
            byte[] arrDst = new byte[bytesCount];
            System.Runtime.InteropServices.Marshal.Copy(ptr, arrDst, 0, bytesCount);

            for (int i = 0; i < bytesCount; i += 3)
            {
                int colorTemp;
                byte r = arrDst[i + 2];
                byte g = arrDst[i + 1];
                byte b = arrDst[i];

                switch (iType)
                {
                    case 0://平均值法
                        colorTemp = (r + g + b) / 3;
                        break;
                    case 1://最大值法
                        colorTemp = r > g ? r : g;
                        colorTemp = colorTemp > b ? colorTemp : b;
                        break;
                    case 2://加权平均值法
                    default:
                        // arrDst[i + 2] * 0.299 + arrDst[i + 1] * 0.587 + arrDst[i] * 0.114
                        colorTemp = (int)(0.7 * r) + (int)(0.2 * g) + (int)(0.1 * b);
                        break;
                }
                arrDst[i] = arrDst[i + 1] = arrDst[i + 2] = (byte)colorTemp;
            }

            System.Runtime.InteropServices.Marshal.Copy(arrDst, 0, ptr, bytesCount);
            bitmap.UnlockBits(bmpData);

            return bitmap;
        }

        /// <summary>
        /// PS 柔化 / 高斯模糊 效率极低，请到DrawingUtil_GDI 调用gdiplus.dll来实现
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        [Obsolete]
        public static Image Gauss(Image img)
        {
            int Height = img.Height;
            int Width = img.Width;
            Bitmap result = new Bitmap(Width, Height);
            Bitmap MyBitmap = (Bitmap)img;
            Color pixel;

            // 高斯模板
            int[] Gauss = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };
            for (int x = 1; x < Width - 1; x++)
            {
                for (int y = 1; y < Height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    int Index = 0;
                    for (int col = -1; col <= 1; col++)
                    {
                        for (int row = -1; row <= 1; row++)
                        {
                            pixel = MyBitmap.GetPixel(x + row, y + col);
                            r += pixel.R * Gauss[Index];
                            g += pixel.G * Gauss[Index];
                            b += pixel.B * Gauss[Index];
                            Index++;
                        }
                    }
                    r /= 16;
                    g /= 16;
                    b /= 16;
                    //处理颜色值溢出
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    result.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));

                }
            }

            return result;
        }

        /// <summary>
        /// PS 柔化 / 高斯模糊 暂时没有效果
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        [Obsolete]
        public static Bitmap GaussV2(Bitmap img)
        {
            int width = img.Width;
            int height = img.Height;

            Rectangle rect = new Rectangle(0, 0, img.Width, img.Height);
            System.Drawing.Imaging.BitmapData bmpData = img.LockBits(rect,
                                                                    System.Drawing.Imaging.ImageLockMode.ReadWrite,
                                                                    System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            IntPtr ptr = bmpData.Scan0;
            int bytesCount = bmpData.Stride * bmpData.Height;
            byte[] arrDst = new byte[bytesCount];
            System.Runtime.InteropServices.Marshal.Copy(ptr, arrDst, 0, bytesCount);

            // 高斯模板
            int[] Gauss = { 1, 2, 1, 2, 4, 2, 1, 2, 1 };

            for (int i = 3; i < bytesCount - 3; i += 3)
            {
                int r = 0, g = 0, b = 0;
                int Index = 0;
                for (int col = -1; col <= 1; col++)
                {
                    for (int row = -1; row <= 1; row++)
                    {
                        r += arrDst[i + 2] * Gauss[Index];
                        g += arrDst[i + 1] * Gauss[Index];
                        b += arrDst[i] * Gauss[Index];
                        Index++;
                    }
                }
                r /= 16;
                g /= 16;
                b /= 16;
                //处理颜色值溢出
                r = r > 255 ? 255 : r;
                r = r < 0 ? 0 : r;
                g = g > 255 ? 255 : g;
                g = g < 0 ? 0 : g;
                b = b > 255 ? 255 : b;
                b = b < 0 ? 0 : b;

                arrDst[i + 2 - 3] = (byte)r;
                arrDst[i + 1 - 3] = (byte)g;
                arrDst[i - 3] = (byte)b;
            }
            System.Runtime.InteropServices.Marshal.Copy(arrDst, 0, ptr, bytesCount);
            img.UnlockBits(bmpData);

            return img;
        }

        /// <summary>
        /// PS 锐化 效率极低，请到DrawingUtil_GDI 调用gdiplus.dll来实现
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        [Obsolete]
        public static Image Laplacian(Image img)
        {
            //以锐化效果显示图像
            int Height = img.Height;
            int Width = img.Width;
            Bitmap result = new Bitmap(Width, Height);
            Bitmap oldBitmap = (Bitmap)img;
            Color pixel;

            // 拉普拉斯模板
            int[] Laplacian = { -1, -1, -1, -1, 9, -1, -1, -1, -1 };

            for (int x = 1; x < Width - 1; x++)
            {
                for (int y = 1; y < Height - 1; y++)
                {
                    int r = 0, g = 0, b = 0;
                    int Index = 0;
                    for (int col = -1; col <= 1; col++)
                    {
                        for (int row = -1; row <= 1; row++)
                        {
                            pixel = oldBitmap.GetPixel(x + row, y + col); r += pixel.R * Laplacian[Index];
                            g += pixel.G * Laplacian[Index];
                            b += pixel.B * Laplacian[Index];
                            Index++;
                        }
                    }
                    //处理颜色值溢出
                    r = r > 255 ? 255 : r;
                    r = r < 0 ? 0 : r;
                    g = g > 255 ? 255 : g;
                    g = g < 0 ? 0 : g;
                    b = b > 255 ? 255 : b;
                    b = b < 0 ? 0 : b;
                    result.SetPixel(x - 1, y - 1, Color.FromArgb(r, g, b));
                }
            }
            return result;
        }

        #endregion
    }
}
