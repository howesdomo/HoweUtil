using Aspose.Pdf;
using Aspose.Pdf.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Util.PDF
{
    public class PDFUtils_Aspose
    {
        #region **** Hot Patch 破解方式 **** 

        /// <summary>
        /// 最新版本的破解方法
        /// </summary>
        public static void InitializeAsposePDF()
        {
            InitializeAsposePDF_v18_12_0();
        }

        /// <summary>
        /// Aspose 18.12.0 Hot Patch 破解方式
        /// 
        /// Winform, WPF 请在 OnStartup 中调用此方法
        /// Web程序, 请在全局应用类(Global.asax)中 Application_Start 方法中调用此方法
        /// </summary>
        public static bool InitializeAsposePDF_v18_12_0()
        {
            var name = Assembly.CreateQualifiedName(typeof(Aspose.Pdf.License).Assembly.FullName,
               "\u000F\u200A\u200B\u2008");
            var licType = Type.GetType(name, false, false);
            var helperName = Assembly.CreateQualifiedName(typeof(Aspose.Pdf.License).Assembly.FullName,
                "\u000F\u2001\u2000\u2009");
            var helperType = Type.GetType(helperName, false, false);

            if (licType == null || helperType == null)
                return false;
            try
            {


                var helperInstance = helperType.GetField("\u0002", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                          .GetValue(null);

                helperInstance.GetType().GetField("\u0008", BindingFlags.NonPublic | BindingFlags.Instance).SetValue(helperInstance, 256);

                object lic = Activator.CreateInstance(licType);
                licType.GetField("\u0008\u2000", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                    .SetValue(lic, (int)1);
                licType.GetField("\u0002\u2000", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                    .SetValue(lic, DateTime.Today.AddDays(1000));
                licType.GetField("\u000e\u2000", BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)
                    .SetValue(null, lic);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion

        /// <summary>
        /// 检查是否授权成功
        /// </summary>
        /// <returns></returns>
        public static bool IsLicensed()
        {
            Aspose.Pdf.Document doc = new Aspose.Pdf.Document();
            // return doc.Is;
            return false;
        }

        public static void TestCreate()
        {
            Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document();

            DocumentInfo docInfo = pdfDocument.Info;
            docInfo.Author = "HOWE";
            docInfo.CreationDate = DateTime.Now;
            docInfo.Keywords = "Aspose.Pdf, DOM, API";
            docInfo.ModDate = DateTime.Now;
            docInfo.Subject = "PDF Information";
            docInfo.Title = "Setting PDF Document Information";

            pdfDocument.Pages.Add();

            // Get particular page
            Page pdfPage = (Page)pdfDocument.Pages[1];

            // Create text fragment
            TextFragment textFragment = new TextFragment("main text");
            textFragment.Position = new Position(100, 600);

            // Set text properties
            textFragment.TextState.FontSize = 12;
            textFragment.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textFragment.TextState.BackgroundColor = Aspose.Pdf.Color.FromRgb(System.Drawing.Color.LightGray);
            textFragment.TextState.ForegroundColor = Aspose.Pdf.Color.FromRgb(System.Drawing.Color.Red);

            // Create TextBuilder object
            TextBuilder textBuilder = new TextBuilder(pdfPage);

            // Append the text fragment to the PDF page
            textBuilder.AppendText(textFragment);

            var outPutPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "testAspose.pdf");
            pdfDocument.Save(outPutPath);

        }

        public static string TestReadDocInfo()
        {
            var path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "testAspose.pdf");
            Document pdfDocument = new Aspose.Pdf.Document(path);
            var r1 = Util.JsonUtils.SerializeObject(pdfDocument.Info);
            return r1;
        }

        public static string TestRead()
        {
            var path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "TestReadGAC.pdf");
            Document pdfDocument = new Aspose.Pdf.Document(path);

            // instantiate TextFragment Absorber object
            Aspose.Pdf.Text.TextFragmentAbsorber TextFragmentAbsorberAddress = new Aspose.Pdf.Text.TextFragmentAbsorber();
            // search text within page bound
            TextFragmentAbsorberAddress.TextSearchOptions.LimitToPageBounds = true;
            // specify the page region for TextSearch Options
            TextFragmentAbsorberAddress.TextSearchOptions.Rectangle = new Aspose.Pdf.Rectangle(0, 0, 600, 800);
            // search text from first page of PDF file
            pdfDocument.Pages[1].Accept(TextFragmentAbsorberAddress);

            List<string> l = new List<string>();

            foreach (Aspose.Pdf.Text.TextFragment tf in TextFragmentAbsorberAddress.TextFragments)
            {
                l.Add(tf.Text);
            }

            if (l.Count > 0)
            {
                return l[0];
            }
            else
            {
                return string.Empty;
            }
        }

        public static string TestReadGAC(string path)
        {
            List<string> l = new List<string>();
            Document pdfDocument = new Aspose.Pdf.Document(path);

            // Get particular page
            Page pdfPage = (Page)pdfDocument.Pages[1];

            // Create text fragment
            TextFragment textFragment = new TextFragment("I------------------I");
            textFragment.Position = new Position(20, 585);

            // Set text properties
            textFragment.TextState.FontSize = 12;
            textFragment.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textFragment.TextState.BackgroundColor = Aspose.Pdf.Color.FromRgb(System.Drawing.Color.LightGray);
            textFragment.TextState.ForegroundColor = Aspose.Pdf.Color.FromRgb(System.Drawing.Color.Red);

            // Create TextBuilder object
            TextBuilder textBuilder = new TextBuilder(pdfPage);

            // Append the text fragment to the PDF page
            textBuilder.AppendText(textFragment);



            #region 

            // Create text fragment
            textFragment = new TextFragment("O------------------O");
            // textFragment.Position = new Position(20, 554);
            textFragment.Position = new Position(20, 556);

            // Set text properties
            textFragment.TextState.FontSize = 12;
            textFragment.TextState.Font = FontRepository.FindFont("TimesNewRoman");
            textFragment.TextState.BackgroundColor = Aspose.Pdf.Color.FromRgb(System.Drawing.Color.LightGray);
            textFragment.TextState.ForegroundColor = Aspose.Pdf.Color.FromRgb(System.Drawing.Color.Red);

            // Create TextBuilder object
            textBuilder = new TextBuilder(pdfPage);

            // Append the text fragment to the PDF page
            textBuilder.AppendText(textFragment);

            #endregion

            var outPutPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "testAspose{0}.pdf".FormatWith(DateTime.Now.ToString("HHmmss")));
            pdfDocument.Save(outPutPath);

            return string.Empty;
        }

        public static string TestReadGAC_BAK(string path)
        {
            List<string> l = new List<string>();


            Document pdfDocument = new Aspose.Pdf.Document(path);

            Aspose.Pdf.Text.TextFragmentAbsorber textFragmentAbsorberAddress = new Aspose.Pdf.Text.TextFragmentAbsorber();

            double width = 50d;
            double height = 30d;

            double x = 20d;
            double y = 585d;

            for (int i = 0; i < 3; i++)
            {
                // textFragmentAbsorberAddress.TextSearchOptions.LimitToPageBounds = true;
                textFragmentAbsorberAddress.TextSearchOptions.LimitToPageBounds = false;

                var rectangle = new Aspose.Pdf.Rectangle
                (
                    //llx: x,
                    //lly: y - i * height,
                    //urx: width,
                    //ury: height

                    llx: width,
                    lly: height,
                    urx: x,
                    ury: y - i * height
                );

                textFragmentAbsorberAddress.TextSearchOptions.Rectangle = rectangle;
                pdfDocument.Pages[1].Accept(textFragmentAbsorberAddress);
                foreach (Aspose.Pdf.Text.TextFragment tf in textFragmentAbsorberAddress.TextFragments)
                {
                    l.Add(tf.Text);
                }
            }

            l = l.Distinct().ToList();

            if (l.Count > 0)
            {
                return l[0];
            }
            else
            {
                return string.Empty;
            }
        }

    }
}
