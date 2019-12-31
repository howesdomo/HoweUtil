using System;

namespace Util.Web
{
    /// <summary>
    /// V 1.0.0 - 2019-11-11 17:41:04
    /// 首次创建 用于 MyTcpServer 与 MyTcpClient 的状态提示
    /// </summary>
    public class TcpXxxStatusChangeEventArgs : EventArgs
    {
        public TcpXxxStatusChangeEventArgs(bool isConnect, int linkedClientCount, Util.UIModel.ConsoleMsgType consoleMsgType, string consoleMsg)
        {
            this.IsConnect = isConnect;
            this.LinkedClientCount = linkedClientCount;
            this.ConsoleMsgType = consoleMsgType;
            this.ConsoleMsg = consoleMsg;
        }

        public bool IsConnect { get; private set; }

        public int LinkedClientCount { get; private set; }

        public Util.UIModel.ConsoleMsgType ConsoleMsgType { get; private set; }

        public string ConsoleMsg { get; private set; }

    }
}
