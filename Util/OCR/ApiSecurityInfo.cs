using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Util.OCR
{
    [Serializable]
    public class ApiSecurityInfo
    {
        public string ApiID { get; set; }

        public string ApiKey { get; set; }

        public string SecretKey { get; set; }

        public override bool Equals(object obj)
        {
            bool r = false;

            if (obj is ApiSecurityInfo)
            {
                var toMatch = obj as ApiSecurityInfo;

                if (
                    toMatch.ApiID == this.ApiID &&
                    toMatch.ApiKey == this.ApiKey &&
                    toMatch.SecretKey == this.SecretKey
                    )
                {
                    r = true;
                }
                else
                {
                    r = false;
                }
            }
            else
            {
                r = base.Equals(obj);
            }

            return r;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
