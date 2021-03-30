using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTest_Util_HowesDOMO
{
    /// <summary>
    /// Summary description for UriUtils_UnitTest
    /// </summary>
    [TestClass]
    public class UriUtils_UnitTest
    {
        public UriUtils_UnitTest()
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
        #endregion

        [TestMethod]
        [TestCategory("UriUtils.Combine")]
        public void Test_Combine_V1()
        {
            string expectResult = "http://192.168.1.215/User/Test";

            Uri a; // a 开头只有一层
            string b;
            Uri r;

            // 测试 1 a 结尾含有 /
            a = new Uri("http://192.168.1.215/");
            b = "User/Test";
            r = Util.UriUtils.Combine(a, b);

            Assert.AreEqual<string>(expectResult, r.ToString());

            // 测试 2 b 开头含有 /
            a = new Uri("http://192.168.1.215");
            b = "/User/Test";
            r = Util.UriUtils.Combine(a, b);

            Assert.AreEqual<string>(expectResult, r.ToString());

            // 测试 3 a 结尾含有 / 并且 b 开头含有 /
            a = new Uri("http://192.168.1.215/");
            b = "/User/Test";
            r = Util.UriUtils.Combine(a, b);

            Assert.AreEqual<string>(expectResult, r.ToString());

            // 测试 4 b的斜杠是反方向的
            a = new Uri("http://192.168.1.215/");
            b = @"\User\Test";
            r = Util.UriUtils.Combine(a, b);

            Assert.AreEqual<string>(expectResult, r.ToString());
        }

        [TestMethod]
        [TestCategory("UriUtils.Combine")]
        public void Test_Combine_V2()
        {
            string expectResult = "http://192.168.1.215/WebServer/User/Test";

            Uri a; // a 开头只有一层
            string b;
            Uri r;

            // 测试 1 a 结尾含有 /
            a = new Uri("http://192.168.1.215/WebServer/");
            b = "User/Test";
            r = Util.UriUtils.Combine(a, b);

            Assert.AreEqual<string>(expectResult, r.ToString());

            // 测试 2 b 开头含有 /
            a = new Uri("http://192.168.1.215/WebServer");
            b = "/User/Test";
            r = Util.UriUtils.Combine(a, b);

            Assert.AreEqual<string>(expectResult, r.ToString());

            // 测试 3 a 结尾含有 / 并且 b 开头含有 /
            a = new Uri("http://192.168.1.215/WebServer/");
            b = "/User/Test";
            r = Util.UriUtils.Combine(a, b);

            Assert.AreEqual<string>(expectResult, r.ToString());

            // 测试 4 b的斜杠是反方向的
            a = new Uri("http://192.168.1.215/WebServer");
            b = @"\User\Test";
            r = Util.UriUtils.Combine(a, b);

            Assert.AreEqual<string>(expectResult, r.ToString());
        }

        [TestMethod]
        [TestCategory("UriUtils.Combine ThrowEx")]
        public void Test_Combine_V3()
        {
            Uri a;
            string b;
            Uri r;

            Assert.ThrowsException<ArgumentNullException>
            (
                action: () =>
                {
                    a = null;
                    b = "User/Test";
                    r = Util.UriUtils.Combine(a, b);
                },
                message: "abc"
            );
        }

        [TestMethod]
        [TestCategory("UriUtils.Combine ThrowEx")]
        public void Test_Combine_V4()
        {
            Uri a;
            string b;
            Uri r;
            // 暂时无法拦截
            Assert.ThrowsException<ArgumentException>(() => 
            {
                // 测试 b 不一个正常的参数, 如何拦截一个异常的 b 参数
                a = new Uri("http://192.168.1.215/");
                b = "https://192.168.1.215/hellowordksl";
                r = Util.UriUtils.Combine(a, b);
            });
        }

        [TestMethod]
        [TestCategory("UriExtension_Combine")]
        public void Test_Combine_Extensions()
        {
            string expectResult = "http://192.168.1.215/WebServer/User/Test";

            Uri a; // a 开头只有一层
            string b;
            Uri r;

            // 测试 1 a 结尾含有 /
            a = new Uri("http://192.168.1.215/WebServer/");
            b = "User/Test";
            r = a.Combine(b);

            Assert.AreEqual<string>(expectResult, r.ToString());


            // 测试 2 b 开头含有 /
            a = new Uri("http://192.168.1.215/WebServer");
            b = "/User/Test";
            r = a.Combine(b);

            Assert.AreEqual<string>(expectResult, r.ToString());

            // 测试 3 a 结尾含有 / 并且 b 开头含有 /
            a = new Uri("http://192.168.1.215/WebServer/");
            b = "/User/Test";
            r = a.Combine(b);

            Assert.AreEqual<string>(expectResult, r.ToString());

            // 测试 4 b的斜杠是反方向的
            a = new Uri("http://192.168.1.215/WebServer");
            b = @"\User\Test";
            r = a.Combine(b);

            Assert.AreEqual<string>(expectResult, r.ToString());
        }
    }
}
