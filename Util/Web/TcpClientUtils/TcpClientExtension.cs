using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Net.Sockets
{
    public static class TcpClientExtension
    {
        /// <summary>
        /// 接收
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <param name="encoding">默认UTF8</param>
        /// <returns></returns>
        public static string Receive(this System.Net.Sockets.TcpClient tcpClient, System.Text.Encoding encoding = null)
        {
            return Util.Web.TcpClientUtils.Receive(tcpClient, encoding);
        }

        /// <summary>
        /// 发送 ( 转换为 TcpClientModel )
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <param name="toSend">发送内容</param>
        /// <param name="encoding">默认UTF-8</param>
        public static void Send(this System.Net.Sockets.TcpClient tcpClient, string toSend, System.Text.Encoding encoding = null)
        {
            Util.Web.TcpClientUtils.Send(tcpClient, toSend, encoding);
        }

        /// <summary>
        /// 发送 ( 简单直接发送, 不转换为 TcpClientModel )
        /// </summary>
        /// <param name="tcpClient"></param>
        /// <param name="toSend">发送内容</param>
        /// <param name="encoding">默认UTF-8</param>
        public static void SimpleSend(this System.Net.Sockets.TcpClient tcpClient, string toSend, System.Text.Encoding encoding = null)
        {
            Util.Web.TcpClientUtils.SimpleSend(tcpClient, toSend, encoding);
        }
    }
}
