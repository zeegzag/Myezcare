using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SelectPdf;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System.IO;

namespace Zarephath.Core.Infrastructure.Utility
{
    class SelectHtmlToPdf
    {
        public async Task<byte[]> GeneratePDFAsync(string url, bool isLandscape = false, int webPageWidth = 1024, int webPageHeight = 0, int timeout = 30, PaperFormat pageSize = null, MediaType mediaType = MediaType.Screen)
        {
            byte[] pdf = Array.Empty<byte>();
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
                DefaultViewport = new ViewPortOptions ()
                {
                    Width = webPageWidth,
                    Height = webPageHeight,
                    IsLandscape = isLandscape,
                    DeviceScaleFactor = 1
                },
                Timeout = timeout * 1000
            });

            var optionsPDF = new PdfOptions
            {
                Width = $"{webPageWidth}px",
                Format = pageSize,
                Landscape = isLandscape,
                PrintBackground = true
            };

            if (pageSize == null) { pageSize = PaperFormat.A4; }

            using (var page = await browser.NewPageAsync())
            {
                await page.EmulateMediaTypeAsync(mediaType);
                await page.GoToAsync(url);

                var hwRatio = await page.EvaluateExpressionAsync<decimal>("document.body.offsetHeight / document.body.offsetWidth");
                optionsPDF.Scale = 1 / hwRatio;

                pdf = await page.PdfDataAsync(optionsPDF);
            }
            return pdf;
        }

        public byte[] GenerateHtmlUrlToPdf(string url, string pdfPageSize = "A4", string pdfPageOrientation = "Portrait", int webPageWidth = 1024, int webPageHeight = 0, int delay = 1, int timeout = 60, string cssMediaType = "Screen")
        {
            HtmlToPdf converter = new HtmlToPdf();

            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdfPageSize, true);
            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdfPageOrientation, true);
            HtmlToPdfCssMediaType mediaType = (HtmlToPdfCssMediaType)Enum.Parse(typeof(HtmlToPdfCssMediaType), cssMediaType, true);

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // specify the number of seconds the conversion is delayed
            converter.Options.MinPageLoadTime = delay;

            // set the page timeout
            converter.Options.MaxPageLoadTime = timeout;

            // set css media type
            converter.Options.CssMediaType = mediaType;

            converter.Options.JavaScriptEnabled = true;

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertUrl(url);
            
            // save pdf document
            byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();

            return pdf;
        }

        public byte[] GenerateHtmlUrlsToPdf(List<string> urls, string pdfPageSize = "A4", string pdfPageOrientation = "Portrait", int webPageWidth = 1024, int webPageHeight = 0)
        {
            HtmlToPdf converter = new HtmlToPdf();

            PdfPageSize pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), pdfPageSize, true);
            PdfPageOrientation pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), pdfPageOrientation, true);

            // set converter options
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.WebPageWidth = webPageWidth;
            converter.Options.WebPageHeight = webPageHeight;

            // create a new pdf document converting an url
            PdfDocument doc = new PdfDocument();
            if (urls != null)
            {
                foreach (var url in urls)
                {
                    doc.Append(converter.ConvertUrl(url));
                }
            }
            // save pdf document
            byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();

            return pdf;
        }

    }
}
