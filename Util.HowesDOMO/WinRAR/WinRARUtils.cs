using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;


namespace Util
{
    public class WinRARUtils
    {
        /// <summary>
        /// 自动从注册表获取WinRAR程序路径
        /// </summary>
        public WinRARUtils()
        {
            this.ApplicationPath_WinRAR = this.GetApplicationPath();
        }

        /// <summary>
        /// 手动设置WinRAR程序路径
        /// </summary>
        /// <param name="_ApplicationPath_WinRAR"></param>
        public WinRARUtils(string _ApplicationPath_WinRAR)
        {
            if (File.Exists(_ApplicationPath_WinRAR))
            {
                this.ApplicationPath_WinRAR = _ApplicationPath_WinRAR;
            }
            else
            {
                throw new FileNotFoundException("文件不存在。文件路径：{0}".FormatWith(_ApplicationPath_WinRAR));
            }
        }

        /// <summary>
        /// WinRAR安装路径
        /// </summary>
        private string applicationPath_WinRAR;

        /// <summary>
        /// WinRAR安装路径，可以自己设置，默认读取系统注册表
        /// </summary>
        public string ApplicationPath_WinRAR
        {
            get
            {
                if (applicationPath_WinRAR.IsNullOrEmpty())
                {
                    applicationPath_WinRAR = this.GetApplicationPath();
                }

                return applicationPath_WinRAR;
            }
            set
            {
                applicationPath_WinRAR = value;
            }
        }

        /// <summary>
        /// 是否安装了WinRAR程序
        /// </summary>
        /// <returns></returns>
        public bool IsSetupWinRAR()
        {
            if (!string.IsNullOrEmpty(this.ApplicationPath_WinRAR))
            {
                return File.Exists(this.ApplicationPath_WinRAR);
            }
            string inistallPath = this.GetApplicationPath();
            if (inistallPath.IsNullOrEmpty())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 从注册表中获取WinRAR的安装路径
        /// </summary>
        /// <returns></returns>
        public string GetApplicationPath()
        {
            string r = string.Empty;

            using (RegistryKey reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\WinRAR.exe\"))
            {
                if (reg == null)
                {
                    throw new Exception("错误：当前操作系统没有安装WinRAR。");
                }
                Object regValue = reg.GetValue("");
                r = regValue.ToString(); // 从注册表信息中获取本机WinRAR程序安装路径
                if (r.IsNullOrWhiteSpace() == true)
                {
                    throw new Exception("错误：当前操作系统没有安装WinRAR。");
                }
            }

            return r;

            #region 网络搜索的 未验证

            //var openKey = @"SOFTWARE\Wow6432Node\WinRAR";//64位注册表
            //if (IntPtr.Size == 4)
            //{
            //    openKey = @"SOFTWARE\WinRAR";//32位注册表路径
            //}
            //RegistryKey appPath = Registry.LocalMachine.OpenSubKey(openKey);
            //if (appPath != null)
            //{
            //    // WinRAR安装具体路径
            //    string path = appPath.GetValue("exe32").ToString();
            //    if (File.Exists(path))
            //    {
            //        return path;
            //    }
            //}
            //return null; 

            #endregion
        }


        #region (弃用) 压缩文件夹

        /// <summary>
        /// 每x秒检测一次打包是否完成
        /// </summary>
        //public int WaitForExitMilliseconds = TimeSpan.FromSeconds(5).Milliseconds;

        ///// <summary>
        ///// 打包文件夹所有内容为RAR文件
        ///// </summary>
        ///// <param name="dealWithDirectory">待压缩文件夹路径</param>
        ///// <param name="rarDirectory">RAR存放目录</param>
        ///// <param name="rarPath">RAR文件完整路径</param>
        ///// <param name="level">设置压缩级别, 默认值 5 (最高)</param>
        //public void SaveDirectoryAsRAR(string dealWithDirectory, string rarDirectory, string rarPath, int level = 5)
        //{
        //    if (this.IsSetupWinRAR() == false)
        //    {
        //        throw new Exception("WinRAR未安装。");
        //    }

        //    string arguments = string.Empty;
        //    System.Diagnostics.ProcessStartInfo processStartInfo;
        //    Process winRARProcess = null;

        //    try
        //    {
        //        // -a : 添加文件到压缩文件
        //        // -ep1 : 表示不把路径%USERPROFILE%保存在压缩文件中 -- 不保存绝对路径
        //        // -m<0..5> : 设置压缩级别(0-存储...3-默认...5-最大)
        //        // -r : 递归子目录

        //        //arguments = " a " + rarName + " " + patch + " -r"; // 压缩时候采用  绝对路径  保存RAR文件
        //        arguments = " a -ep1 -m" + level + " " + rarPath + " " + dealWithDirectory + " -r"; // 压缩时候采用  相对路径  保存RAR文件

        //        processStartInfo = new ProcessStartInfo();
        //        processStartInfo.FileName = this.ApplicationPath_WinRAR;
        //        processStartInfo.Arguments = arguments;
        //        //the_StartInfo.WindowStyle = ProcessWindowStyle.Normal;
        //        processStartInfo.WindowStyle = ProcessWindowStyle.Hidden;

        //        // 打包文件存放路径
        //        if (!System.IO.Directory.Exists(rarDirectory))
        //        {
        //            System.IO.Directory.CreateDirectory(rarDirectory);
        //        }

        //        winRARProcess = new Process();
        //        winRARProcess.StartInfo = processStartInfo;
        //        winRARProcess.Start();
        //        bool isExit = false;
        //        while (isExit == false)
        //        {
        //            System.Threading.Thread.Sleep(this.WaitForExitMilliseconds);
        //            if (winRARProcess.HasExited || this.IsRARFileFinish(rarPath))
        //            {
        //                isExit = true;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("执行SaveDirectoryAsRAR时发生错误。", ex);
        //    }
        //    finally
        //    {
        //        if (winRARProcess != null)
        //        {
        //            winRARProcess.Close();
        //        }
        //    }
        //}

        #endregion

        #region 压缩

        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="filePath">需要压缩的文件/文件夹全路径</param>
        /// <param name="rarPath">压缩文件保存全路径</param>
        /// <param name="level">设置压缩级别, 默认值 5 (最高)</param>
        /// <param name="password">设置压缩包密码，默认不设置密码</param>
        /// <param name="containCurrentDirectory">若filePath是文件夹，压缩包是否包含该文件夹名字，默认值 True</param>
        public void RAR
            (
                string filePath,
                string rarPath,
                int level = 5,
                string password = "",
                bool containCurrentDirectory = true
            )
        {
            if (this.IsSetupWinRAR() == false)
            {
                throw new Exception("WinRAR未安装。");
            }

            try
            {
                #region 目录不存在创建目录 && RAR文件存在删除文件

                FileInfo fi = new FileInfo(rarPath);
                if (fi.Directory.Exists == false)
                {
                    Directory.CreateDirectory(fi.Directory.FullName);
                }
                else
                {
                    if (fi.Exists == true)
                    {
                        File.Delete(fi.FullName);
                    }
                }

                #endregion

                // 若filePath是文件夹，压缩包是否包含该文件夹名字
                FileInfo filePath_fi = new FileInfo(filePath);
                if (filePath_fi.Attributes == FileAttributes.Directory)
                {
                    int tmp = filePath.LastIndexOf('\\');
                    if (containCurrentDirectory == true && // 包含当前目录名称，最后不能为 \ 
                        tmp == filePath.Length  // '\' 在最后的位置
                        )
                    {
                        filePath = filePath.Substring(0, filePath.Length - 1);
                    }
                    else if(containCurrentDirectory == false && // 不包含当前目录名称，最后一定要带 \ 
                        tmp < filePath.Length // '\' 不在最后的位置
                        )
                    {
                        filePath = filePath + "\\";
                    }
                }

                // a : 添加文件到压缩文件
                // -ep : -ep[无, 0 ~ 3] 详细可以参考WinRAR_参数ep图例.docx
                // -o : o+ (覆盖已存在文件); o- (不覆盖已存在文件)
                // -m<0..5> : 设置压缩级别(0-存储...3-默认...5-最大)
                // -r : 递归子目录，若指定的是文件，压缩所有子目录中同名的文件
                string arguments = "a -ep1 -r -m{0}{1} \"{2}\" \"{3}\"".FormatWith
                (
                    level,
                    password.IsNullOrEmpty() == true ? "" : " -p" + password,
                    rarPath, // 压缩文件保存全路径
                    filePath // 需要压缩的文件/文件夹全路径
                );

                ProgressUtils.ExcuteProgressWithArguments(this.ApplicationPath_WinRAR, arguments);
            }
            catch (Exception ex)
            {
                throw new Exception("执行RAR时发生错误。", ex);
            }
        }

        #endregion

        #region 解压

        /// <summary>
        /// 解压到某个文件夹中
        /// </summary>
        /// <param name="rarFilePath">rar文件全路径</param>
        /// <param name="unRarPath">解压到哪个文件夹</param>
        /// <param name="password">解压密码</param>
        /// <param name="isOverride">是否覆盖</param>
        public void UnRAR(string rarFilePath, string unRarPath, string password = null)
        {
            if (this.IsSetupWinRAR() == false)
            {
                throw new Exception("WinRAR未安装。");
            }

            try
            {
                string arguments = "x -o{0}{1} \"{2}\" \"{3}\"".FormatWith
                (
                    password == null ? "" : " -p" + password,
                    rarFilePath,
                    unRarPath
                );

                ProgressUtils.ExcuteProgressWithArguments(this.ApplicationPath_WinRAR, arguments);
            }
            catch (Exception ex)
            {
                throw new Exception("执行UnRAR时发生错误。", ex);
            }
        }

        #endregion

        /// <summary>
        /// 判断压缩文件是否压缩完毕
        /// 判断依据:压缩文件不被占用
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool IsRARFileFinish(string path)
        {
            bool fileRARFinsh = false;
            if (File.Exists(path))
            {
                FileStream fs = null;
                try
                {
                    fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                    fileRARFinsh = true;
                }
                catch (Exception)
                {
                    fileRARFinsh = false;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                    }
                }
            }
            return fileRARFinsh;
        }
    }
}


