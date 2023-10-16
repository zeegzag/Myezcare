using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace HomeCareApi.Helpers
{
    public class PdfHelper
    {
        public string GereratePdf(List<string> pdfbodyArr, byte[] signature)
        {
            string fileName = "Mobile Agreemenet " + DateTime.Now.ToString("yyyyMMddhmmss") + ".pdf";
            string fullPath = System.Web.HttpContext.Current.Server.MapPath("~/AgreementPDF/") + fileName;
            string imgBase64 = Convert.ToBase64String(signature);

            // create directory if does not exists already 
            if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath("~/AgreementPDF/"))) Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath("~/AgreementPDF/"));

            // create signature img tab,using signature base64
            string finalImageHtml = string.Format("<div><br><br><img src=\"data:image/png;base64, {0}\" /></div>", imgBase64);

            //pdfbodyArr.Add(finalImageHtml);
            pdfbodyArr[pdfbodyArr.Count - 1] = pdfbodyArr[pdfbodyArr.Count - 1] + finalImageHtml;

            //if previous file is there delete it
            System.IO.File.Delete(fullPath);


            PdfDocument doc = new PdfDocument();
            if (pdfbodyArr != null && pdfbodyArr.Count > 0)
            {
                foreach (var pdf in pdfbodyArr)
                {
                    PdfDocument tempDoc = getPdfDocFrom(pdf.ToString());
                    addPagesToPdf(ref doc, tempDoc);
                }
            }

            //save pdf document
            doc.Save(fullPath);

            // return path
            return fullPath;
        }

        private PdfDocument getPdfDocFrom(string htmlString)
        {
            PdfGenerateConfig config = new PdfGenerateConfig();
            config.PageOrientation = PageOrientation.Portrait;
            config.SetMargins(0);
            config.ManualPageSize = new PdfSharp.Drawing.XSize(3500, 2400);

            PdfDocument doc = PdfGenerator.GeneratePdf(htmlString, config);

            return doc;
        }

        private void addPagesToPdf(ref PdfDocument mainDoc, PdfDocument sourceDoc)
        {
            MemoryStream tempMemoryStream = new MemoryStream();

            sourceDoc.Save(tempMemoryStream, false);

            PdfDocument openedDoc = PdfReader.Open(tempMemoryStream, PdfDocumentOpenMode.Import);
            foreach (PdfPage page in openedDoc.Pages)
            {
                mainDoc.AddPage(page);
            }
        }
    }
}