using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;

namespace UnitTest_Util_HowesDOMO.Web
{
    /// <summary>
    /// Summary description for IPAddressUtils_UnitTest
    /// </summary>
    [TestClass]
    public class IPAddressUtils_UnitTest
    {
        public IPAddressUtils_UnitTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            HttpContext.Current = new HttpContext
            (
                request: new HttpRequest("", "http://192.168.1.2", ""),
                response: new HttpResponse(new StringWriter(new StringBuilder()))
            );
        }

        #endregion

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestMethod1()
        {
            var r = Util.Web.IPAddressUtils.GetIPAddressAdv();
            Assert.AreEqual(r.Country, "未知");
        }
    }
}
