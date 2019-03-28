using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.Drawing.Printing
{
    public class Printer
    {
        public string DisplayName { get; set; }

        public string OriginValue { get; set; }

        public string ExcelValue { get; set; }

        public List<PaperSize> PaperSizeList { get; set; }
    }
}
