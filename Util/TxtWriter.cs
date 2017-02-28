using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Util
{
    public class TxtWriter
    {
        public static void TxtWriter(string path, string content, FileMode fileModel, Encoding encoding)
        {
            using (FileStream fs = new FileStream(path, fileModel))
            {
                using (StreamWriter sw = new StreamWriter(fs, encoding))
                {
                    sw.Write(content);
                    sw.Flush();
                    sw.Close();
                }
                fs.Close();
            }
        }
    }
}
