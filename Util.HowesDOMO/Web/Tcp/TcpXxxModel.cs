using System;

namespace Util.Web
{
    /// <summary>
    /// V 1.0.0
    /// 首次创建
    /// </summary>
    public class TcpXxxEventArgs : EventArgs
    {
        public TcpXxxEventArgs(string msg)
        {
            this.Msg = msg;
        }

        public string Msg { get; private set; }
    }
}
