/*
 * HtmlToPdfBuilder.cs
 * ---------------------------------
 * Hugo Bonacci (webdev_hb@yahoo.com)
 * www.hugoware.net
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections;
using iTextSharp.text.html;
using System.Text.RegularExpressions;

namespace PDFBuilder
{

    #region HtmlToPdfBuilder Class

    /// <summary>
    /// Simplifies generating HTML into a PDF file
    /// </summary>
    public class HtmlToPdfBuilder
    {

        #region Constants

        private const string StyleDefaultType = "style";
        private const string DocumentHTMLStart = "<html><head></head><body>";
        private const string DocumentHTMLEnd = "</body></html>";
        private const string RegexGroupSelector = "selector";
        private const string RegexGroupStyle = "style";

        //amazing regular expression magic
        private const string RegexGetStyles = @"(?<selector>[^\{\s]+\w+(\s\[^\{\s]+)?)\s?\{(?<style>[^\}]*)\}";

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new PDF document template. Use PageSizes.{DocumentSize}
        /// </summary>
        public HtmlToPdfBuilder(Rectangle size)
        {
            PageSize = size;
            _pages = new List<HtmlPdfPage>();
            _styles = new StyleSheet();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Method to override to have additional control over the document
        /// </summary>
        public event RenderEvent BeforeRender = (writer, document) => { };

        /// <summary>
        /// Method to override to have additional control over the document
        /// </summary>
        public event RenderEvent AfterRender = (writer, document) => { };

        #endregion

        #region Properties

        /// <summary>
        /// The page size to make this document
        /// </summary>
        public Rectangle PageSize { get; set; }

        /// <summary>
        /// Returns the page at the specified index
        /// </summary>
        public HtmlPdfPage this[int index]
        {
            get
            {
                return _pages[index];
            }
        }

        /// <summary>
        /// Returns a list of the pages available
        /// </summary>
        public IEnumerable<HtmlPdfPage> Pages
        {
            get
            {
                return _pages.AsEnumerable();
            }
        }

        #endregion

        #region Members

        private readonly List<HtmlPdfPage> _pages;
        private readonly StyleSheet _styles;

        #endregion

        #region Working With The Document

        /// <summary>
        /// Appends and returns a new page for this document
        /// </summary>
        public HtmlPdfPage AddPage()
        {
            HtmlPdfPage page = new HtmlPdfPage();
            _pages.Add(page);
            return page;
        }

        /// <summary>
        /// Removes the page from the document
        /// </summary>
        public void RemovePage(HtmlPdfPage page)
        {
            _pages.Remove(page);
        }

        /// <summary>
        /// Appends a style for this sheet
        /// </summary>
        public void AddStyle(string selector, string styles)
        {
            _styles.LoadTagStyle(selector, StyleDefaultType, styles);
        }

        /// <summary>
        /// Imports a stylesheet into the document
        /// </summary>
        public void ImportStylesheet(string path)
        {

            //load the file
            string content = File.ReadAllText(path);

            //use a little regular expression magic
            foreach (Match match in Regex.Matches(content, RegexGetStyles))
            {
                string selector = match.Groups[RegexGroupSelector].Value;
                string style = match.Groups[RegexGroupStyle].Value;
                AddStyle(selector, style);
            }

        }


        #endregion

        #region Document Navigation

        /// <summary>
        /// Moves a page before another
        /// </summary>
        public void InsertBefore(HtmlPdfPage page, HtmlPdfPage before)
        {
            _pages.Remove(page);
            _pages.Insert(
                Math.Max(_pages.IndexOf(before), 0),
                page);
        }

        /// <summary>
        /// Moves a page after another
        /// </summary>
        public void InsertAfter(HtmlPdfPage page, HtmlPdfPage after)
        {
            _pages.Remove(page);
            _pages.Insert(
                Math.Min(_pages.IndexOf(after) + 1, _pages.Count),
                page);
        }


        #endregion

        #region Rendering The Document

        /// <summary>
        /// Renders the PDF to an array of bytes
        /// </summary>
        public byte[] RenderPdf()
        {

            //Document is inbuilt class, available in iTextSharp
            MemoryStream file = new MemoryStream();
            Document document = new Document(PageSize);
            PdfWriter writer = PdfWriter.GetInstance(document, file);

            //allow modifications of the document
            BeforeRender(writer, document);

            //header
            //document.Add(new Header(Markup.HTML_ATTR_STYLESHEET, string.Empty));
            document.Open();

            //render each page that has been added
            foreach (HtmlPdfPage page in _pages)
            {
                document.NewPage();

                //generate this page of text
                MemoryStream output = new MemoryStream();
                StreamWriter html = new StreamWriter(output, Encoding.UTF8);

                //get the page output
                html.Write(string.Concat(DocumentHTMLStart, page.Html.ToString(), DocumentHTMLEnd));
                html.Close();
                html.Dispose();

                //read the created stream
                MemoryStream generate = new MemoryStream(output.ToArray());
                StreamReader reader = new StreamReader(generate);
                foreach (var item in (IEnumerable)HTMLWorker.ParseToList(reader, _styles))
                {
                    document.Add((IElement)item);
                }

                //cleanup these streams
                html.Dispose();
                reader.Dispose();
                output.Dispose();
                generate.Dispose();

            }

            //after rendering
            AfterRender(writer, document);

            //return the rendered PDF
            document.Close();
            return file.ToArray();

        }

        #endregion

    }

    #endregion


    #region HtmlPdfPage Class

    /// <summary>
    /// A page to insert into a HtmlToPdfBuilder Class
    /// </summary>
    public class HtmlPdfPage
    {

        #region Constructors

        /// <summary>
        /// The default information for this page
        /// </summary>
        public HtmlPdfPage()
        {
            Html = new StringBuilder();
        }

        #endregion

        #region Fields

        //parts for generating the page
        internal StringBuilder Html;

        #endregion

        #region Working With The Html

        /// <summary>
        /// Appends the formatted HTML onto a page
        /// </summary>
        public virtual void AppendHtml(string content, params object[] values)
        {
            Html.AppendFormat(content, values);
        }

        #endregion

    }

    #endregion


    #region Rendering Delegate

    /// <summary>
    /// Delegate for rendering events
    /// </summary>
    public delegate void RenderEvent(PdfWriter writer, Document document);

    #endregion

}
