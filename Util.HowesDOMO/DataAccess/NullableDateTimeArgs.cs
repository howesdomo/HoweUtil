using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.DataAccess
{
    public class NullableDateTimeArgs
    {
        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public NullableDateTimeArgs()
        {
            StartDateTime = Convert.ToDateTime(System.Data.SqlTypes.SqlDateTime.MinValue.ToString());
            EndDateTime = Convert.ToDateTime(System.Data.SqlTypes.SqlDateTime.MaxValue.ToString());
        }

        public NullableDateTimeArgs(DateTime? d)
        {
            StartDateTime = Convert.ToDateTime(System.Data.SqlTypes.SqlDateTime.MinValue.ToString());
            EndDateTime = Convert.ToDateTime(System.Data.SqlTypes.SqlDateTime.MaxValue.ToString());

            if (d.HasValue)
            {
                StartDateTime = d.Value.Date;
                EndDateTime = d.Value.Date.AddDays(1);
            }
        }


    }
}
