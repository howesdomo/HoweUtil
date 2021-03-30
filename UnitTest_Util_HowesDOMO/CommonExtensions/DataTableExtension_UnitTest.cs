using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace UnitTest_Util_HowesDOMO.CommonExtensions
{
    /// <summary>
    /// Summary description for DataTableExtension_UnitTest
    /// </summary>
    [TestClass]
    public class DataTableExtension_UnitTest
    {
        public DataTableExtension_UnitTest()
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
        public void Test_ToString2DArray()
        {
            string r0_Id = "9ae848d9-7580-44f2-a57f-944b38835432";
            string r0_OrderNo = "PO20210330000001";

            string r1_Id = "2cfc7a61-6e2f-489b-bf07-e8b986bfefe5";
            string r1_OrderNo = "PO20210331000002";

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(Guid));
            dt.Columns.Add("OrderNo", typeof(string));

            var row0 = dt.NewRow();
            dt.Rows.Add(row0);
            row0["Id"] = Guid.Parse(r0_Id);
            row0["OrderNo"] = r0_OrderNo;

            var row1 = dt.NewRow();
            dt.Rows.Add(row1);
            row1["Id"] = Guid.Parse(r1_Id);
            row1["OrderNo"] = r1_OrderNo;

            string[,] r = System.Data.DataTableExtension.ToString2DArray(dt, isContainColumnName: true);

            Assert.AreEqual<string>("Id", r[0, 0]);
            Assert.AreEqual<string>("OrderNo", r[0, 1]);

            Assert.AreEqual<string>(r0_Id, r[1, 0]);
            Assert.AreEqual<string>(r0_OrderNo, r[1, 1]);

            Assert.AreEqual<string>(r1_Id, r[2, 0]);
            Assert.AreEqual<string>(r1_OrderNo, r[2, 1]);
        }

        [TestMethod]
        public void Test_GetList()
        {
            string r0_Id = "9ae848d9-7580-44f2-a57f-944b38835432";
            string r0_OrderNo = "PO20210330000001";

            string r1_Id = "2cfc7a61-6e2f-489b-bf07-e8b986bfefe5";
            string r1_OrderNo = "PO20210331000002";

            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(Guid));
            dt.Columns.Add("OrderNo", typeof(string));

            var row0 = dt.NewRow();
            dt.Rows.Add(row0);
            row0["Id"] = Guid.Parse(r0_Id);
            row0["OrderNo"] = r0_OrderNo;

            var row1 = dt.NewRow();
            dt.Rows.Add(row1);
            row1["Id"] = Guid.Parse(r1_Id);
            row1["OrderNo"] = r1_OrderNo;

            List<OrderForDataTableExtension_UnitTest> l = System.Data.DataTableExtension.GetList<OrderForDataTableExtension_UnitTest>(dt);
            
            Assert.AreEqual<string>(r0_Id, l[0].Id.ToString());
            Assert.AreEqual<string>(r0_OrderNo, l[0].OrderNo);

            Assert.AreEqual<string>(r1_Id, l[1].Id.ToString());
            Assert.AreEqual<string>(r1_OrderNo, l[1].OrderNo);
        }
    }

    public class OrderForDataTableExtension_UnitTest
    {
        public Guid Id { get; set; }
        public string OrderNo { get; set; }
    }
}
