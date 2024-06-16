

using iText.IO.Source;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Kernel.Pdf.Xobject;
using NPOI.Util;

namespace QA.Common.Utilities
{
    interface IPDFUtilities
    {
        static abstract bool IsStringInPDF(Stream pdfStream, string stringToFind);

        static abstract void ExtractImage();
    }

    public class PDFUtilities : IPDFUtilities
    {

        public static bool IsStringInPDF(Stream pdfStream, string stringToFind)
        {
            PdfReader pdfReader = new PdfReader(pdfStream);
            PdfDocument pdfDocument = new PdfDocument(pdfReader);

            int pageFrom = 1;
            int pageTo = pdfDocument.GetNumberOfPages();
            bool isStringFound = false;

            for(int i =pageFrom; i <= pageTo; i++)
            {
                PdfPage currentPage = pdfDocument.GetPage(i);
                string pageText = PdfTextExtractor.GetTextFromPage(currentPage);
                if (pageText.Contains(stringToFind))
                {
                    isStringFound = true;
                    break;
                }
            }
            pdfReader.Close();
            return isStringFound;
        }

        public static void ExtractImage()

        {
         

        }
    }
}
