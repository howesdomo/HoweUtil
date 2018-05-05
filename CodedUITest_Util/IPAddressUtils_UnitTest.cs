using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web;
using System.IO;
using System.Text;

namespace CodedUITest_Util
{
    [TestClass]
    public class IPAddressUtils_UnitTest
    {

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            HttpContext.Current = new HttpContext
            (
                request: new HttpRequest("", "http://192.168.1.2", ""),
                response: new HttpResponse(new StringWriter(new StringBuilder()))
            );
        }


        [TestMethod]
        public void TestMethod1()
        {
            var r = Util.Web.IPAddressUtils.GetIPAddressAdv();
            Assert.AreEqual(r.Country, "未知");
        }
    }
}
