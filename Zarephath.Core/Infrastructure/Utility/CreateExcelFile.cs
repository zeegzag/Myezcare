#define INCLUDE_WEB_FUNCTIONS

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Office2010.Drawing.Charts;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using OpenXml.Excel.Data;
using PetaPoco;
using Zarephath.Core.Helpers;
using Zarephath.Core.Infrastructure.Attributes;
using Zarephath.Core.Models;
using BottomBorder = DocumentFormat.OpenXml.Spreadsheet.BottomBorder;
using Fill = DocumentFormat.OpenXml.Spreadsheet.Fill;
using Fonts = DocumentFormat.OpenXml.Spreadsheet.Fonts;
using ForegroundColor = DocumentFormat.OpenXml.Spreadsheet.ForegroundColor;
using LeftBorder = DocumentFormat.OpenXml.Spreadsheet.LeftBorder;
using PatternFill = DocumentFormat.OpenXml.Spreadsheet.PatternFill;
using RightBorder = DocumentFormat.OpenXml.Spreadsheet.RightBorder;
using TopBorder = DocumentFormat.OpenXml.Spreadsheet.TopBorder;

namespace ExportToExcel
{

    #region Note
    //November 2013
    //http://www.mikesknowledgebase.com

    //Note: if you plan to use this in an ASP.Net application, remember to add a reference to "System.Web", and to uncomment
    //the "INCLUDE_WEB_FUNCTIONS" definition at the top of this file.

    //Release history
    // - Nov 2013: 
    //      Changed "CreateExcelDocument(DataTable dt, string xlsxFilePath)" to remove the DataTable from the DataSet after creating the Excel file.
    //      You can now create an Excel file via a Stream (making it more ASP.Net friendly)
    // - Jan 2013: Fix: Couldn't open .xlsx files using OLEDB  (was missing "WorkbookStylesPart" part)
    // - Nov 2012: 
    //      List<>s with Nullable columns weren't be handled properly.
    //      If a value in a numeric column doesn't have any data, don't write anything to the Excel file (previously, it'd write a '0')
    // - Jul 2012: Fix: Some worksheets weren't exporting their numeric data properly, causing "Excel found unreadable content in '___.xslx'" errors.
    // - Mar 2012: Fixed issue, where Microsoft.ACE.OLEDB.12.0 wasn't able to connect to the Excel files created using this class.
    #endregion

    public class CreateExcelFile
    {
        public static bool CreateExcelDocument<T>(List<T> list, string xlsxFilePath, List<List<T>> additionalList = null)
        {
            int i = 1;
            DataSet ds = new DataSet();
            ds.Tables.Add(ListToDataTable(list, i));
            if (additionalList != null)
            {
                foreach (var item in additionalList)
                {
                    i += 1;
                    ds.Tables.Add(ListToDataTable(item, i));
                }
            }
            return CreateExcelDocument(ds, xlsxFilePath);
        }

        public static bool CreateExcelDocumentOfTwoDifferentType<T, K>(List<List<T>> list, string xlsxFilePath, List<List<K>> additionalList = null)
        {
            int i = 0;
            DataSet ds = new DataSet();
            if (list != null)
            {
                foreach (var item in list)
                {
                    i += 1;
                    ds.Tables.Add(ListToDataTable(item, i));
                }
            }

            if (additionalList != null)
            {
                foreach (var item in additionalList)
                {
                    i += 1;
                    ds.Tables.Add(ListToDataTable(item, i));
                }
            }
            return CreateExcelDocument(ds, xlsxFilePath);
        }


        private static Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            Fonts fonts = new Fonts(
                new Font( // Index 0 - default
                    new FontSize() { Val = 10 }
                ),
                new Font( // Index 1 - header
                    new FontSize() { Val = 10 },
                    new Bold(),
                    new Color() { Rgb = "FFFFFF" }

                ));

            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // Index 0 - default
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }), // Index 1 - default
                    new Fill(new PatternFill(new ForegroundColor { Rgb = new HexBinaryValue() { Value = "66666666" } }) { PatternType = PatternValues.Solid }) // Index 2 - header
                );

            Borders borders = new Borders(
                    new Border(), // index 0 default
                    new Border( // index 1 black border
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // default
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }, // body
                    new CellFormat { FontId = 1, FillId = 2, BorderId = 1, ApplyFill = true }, // header
                    new CellFormat { NumberFormatId = 166, ApplyNumberFormat = true },
                    new CellFormat { NumberFormatId = 2, ApplyNumberFormat = true },
                //new NumberingFormat { NumberFormatId = (UInt32Value)164U, FormatCode = StringValue.FromString("m/d/yyyy") },
                    new CellFormat { NumberFormatId = 14, ApplyNumberFormat = true }

                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);










            return styleSheet;
        }

        #region HELPER_FUNCTIONS

        //This function is adapated from: http://www.codeguru.com/forum/showthread.php?t=450171
        //My thanks to Carl Quirion, for making it "nullable-friendly".

        [AttributeUsage(AttributeTargets.All)]
        public class ReportIgnoreAttribute : System.Attribute
        {
            public ReportIgnoreAttribute()
            { }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
        public class ExcelHeadAttribute : Attribute
        {
            public ExcelHeadAttribute(string headText, Type resourceType = null)
            {
                if (resourceType != null)
                {
                    Head = ResourceHelper.GetResourceLookup(resourceType, headText);
                }
                else
                {
                    Head = headText;
                }
            }
            public string Head { get; set; }
        }

        public static DataTable ListToDataTable<T>(List<T> list, int i = 1)
        {
            DataTable dt = new DataTable();

            var tableNameAttribute = typeof(T).GetCustomAttribute(typeof(TableNameAttribute), true) as TableNameAttribute;
            dt.TableName = tableNameAttribute != null ? tableNameAttribute.Value : "Sheet";
            dt.TableName = dt.TableName + "_" + i;


            foreach (PropertyInfo info in typeof(T).GetProperties())
            {
                object[] attributes = info.GetCustomAttributes(false);
                bool toSkip = false;
                string headText = info.Name;
                foreach (object attr in attributes)
                {
                    ExcelHeadAttribute customHead = attr as ExcelHeadAttribute;
                    if (customHead != null)
                    {
                        headText = customHead.Head;
                    }
                    ReportIgnoreAttribute ignore = attr as ReportIgnoreAttribute;
                    if (ignore != null)
                    {
                        toSkip = true;
                        break;
                    }
                }
                if (toSkip)
                    continue;

                dt.Columns.Add(new DataColumn(headText, GetNullableType(info.PropertyType)));

            }
            foreach (T t in list)
            {
                DataRow row = dt.NewRow();
                foreach (PropertyInfo info in typeof(T).GetProperties())
                {
                    object[] attributes = info.GetCustomAttributes(false);
                    bool toSkip = false;
                    string headText = info.Name;
                    foreach (object attr in attributes)
                    {
                        ExcelHeadAttribute customHead = attr as ExcelHeadAttribute;
                        if (customHead != null)
                        {
                            headText = customHead.Head;
                        }

                        ReportIgnoreAttribute ignore = attr as ReportIgnoreAttribute;
                        if (ignore != null)
                        {
                            toSkip = true;
                            break;
                        }
                    }
                    if (toSkip)
                        continue;

                    if (!IsNullableType(info.PropertyType))
                        row[headText] = info.GetValue(t, null);
                    else
                        row[headText] = (info.GetValue(t, null) ?? DBNull.Value);
                }
                dt.Rows.Add(row);
            }
            return dt;
        }

        private static Type GetNullableType(Type t)
        {
            Type returnType = t;
            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                returnType = Nullable.GetUnderlyingType(t);
            }
            return returnType;
        }

        private static bool IsNullableType(Type type)
        {
            return (type == typeof(string) ||
                    type.IsArray ||
                    (type.IsGenericType &&
                     type.GetGenericTypeDefinition().Equals(typeof(Nullable<>))));
        }

        public static bool CreateExcelDocument(DataTable dt, string xlsxFilePath)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            bool result = CreateExcelDocument(ds, xlsxFilePath);
            ds.Tables.Remove(dt);
            return result;
        }

        #endregion

        #region #IF
#if INCLUDE_WEB_FUNCTIONS
        /// <summary>
        /// Create an Excel file, and write it out to a MemoryStream (rather than directly to a file)
        /// </summary>
        /// <param name="dt">DataTable containing the data to be written to the Excel.</param>
        /// <param name="filename">The filename (without a path) to call the new Excel file.</param>
        /// <param name="Response">HttpResponse of the current page.</param>
        /// <returns>True if it was created succesfully, otherwise false.</returns>

        public static bool CreateExcelDocument(DataTable dt, string filename, System.Web.HttpResponse Response)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(dt);
                CreateExcelDocumentAsStream(ds, filename, Response);
                ds.Tables.Remove(dt);
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed, exception thrown: " + ex.Message);
                return false;
            }
        }

        public static bool CreateExcelDocument<T>(List<T> list, string filename, System.Web.HttpResponse Response, bool skipHeaders = false)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(ListToDataTable(list));
                Debug.WriteLine("List to Data Table conversion complete");
                CreateExcelDocumentAsStream(ds, filename, Response, skipHeaders);
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed, exception thrown: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Create an Excel file, and write it out to a MemoryStream (rather than directly to a file)
        /// </summary>
        /// <param name="ds">DataSet containing the data to be written to the Excel.</param>
        /// <param name="filename">The filename (without a path) to call the new Excel file.</param>
        /// <param name="Response">HttpResponse of the current page.</param>
        /// <returns>Either a MemoryStream, or NULL if something goes wrong.</returns>

        public static bool CreateExcelDocumentAsStream(DataSet ds, string filename, System.Web.HttpResponse Response, bool skipHeaders = false)
        {
            try
            {
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true))
                {
                    WriteExcelFile(ds, document, skipHeaders);
                }
                stream.Flush();
                stream.Position = 0;

                Response.ClearContent();
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";

                //  NOTE: If you get an "HttpCacheability does not exist" error on the following line, make sure you have
                //  manually added System.Web to this project's References.

                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.AddHeader("content-disposition", "attachment; filename=" + filename);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                byte[] data1 = new byte[stream.Length];
                stream.Read(data1, 0, data1.Length);

                Debug.WriteLine("Stream Data Read");

                stream.Close();
                Response.BinaryWrite(data1);

                Debug.WriteLine("Stream Data Written");
                Response.Flush();
                Response.End();

                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed, exception thrown: " + ex.Message);
                return false;
            }
        }

#endif      //  End of "INCLUDE_WEB_FUNCTIONS" section
        #endregion

        /// <summary>
        /// Create an Excel file, and write it to a file.
        /// </summary>
        /// <param name="ds">DataSet containing the data to be written to the Excel.</param>
        /// <param name="excelFilename">Name of file to be written.</param>
        /// <returns>True if successful, false if something went wrong.</returns>

        public static bool CreateExcelDocument(DataSet ds, string excelFilename, bool skipHeaders = false)
        {
            try
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(excelFilename, SpreadsheetDocumentType.Workbook))
                {
                    WriteExcelFile(ds, document, skipHeaders);
                }
                Trace.WriteLine("Successfully created: " + excelFilename);
                return true;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Failed, exception thrown: " + ex.Message);
                return false;
            }
        }

        private static void WriteExcelFile(DataSet ds, SpreadsheetDocument spreadsheet, bool skipHeaders = false)
        {
            //  Create the Excel file contents.  This function is used when creating an Excel file either writing 
            //  to a file, or writing to a MemoryStream.
            spreadsheet.AddWorkbookPart();
            spreadsheet.WorkbookPart.Workbook = new DocumentFormat.OpenXml.Spreadsheet.Workbook();

            //  My thanks to James Miera for the following line of code (which prevents crashes in Excel 2010)
            spreadsheet.WorkbookPart.Workbook.Append(new BookViews(new WorkbookView()));

            //  If we don't add a "WorkbookStylesPart", OLEDB will refuse to connect to this .xlsx file !
            WorkbookStylesPart workbookStylesPart = spreadsheet.WorkbookPart.AddNewPart<WorkbookStylesPart>("rIdStyles");

            Stylesheet stylesheet = new Stylesheet();
            workbookStylesPart.Stylesheet = GenerateStylesheet();
            workbookStylesPart.Stylesheet.Save();

            //  Loop through each of the DataTables in our DataSet, and create a new Excel Worksheet for each.
            uint worksheetNumber = 1;
            foreach (DataTable dt in ds.Tables)
            {
                //  For each worksheet you want to create
                string workSheetID = "rId" + worksheetNumber.ToString();
                string worksheetName = dt.TableName;

                WorksheetPart newWorksheetPart = spreadsheet.WorkbookPart.AddNewPart<WorksheetPart>();
                newWorksheetPart.Worksheet = new Worksheet();

                // create sheet data
                newWorksheetPart.Worksheet.AppendChild(new SheetData());

                // save worksheet
                WriteDataTableToExcelWorksheet(dt, newWorksheetPart, skipHeaders);
                newWorksheetPart.Worksheet.Save();


                // create the worksheet to workbook relation
                if (worksheetNumber == 1)
                    spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());


                if (string.IsNullOrEmpty(dt.TableName) || dt.TableName == "Sheet")
                    dt.TableName = "Sheet_" + worksheetNumber;
                spreadsheet.WorkbookPart.Workbook.GetFirstChild<Sheets>().AppendChild(new Sheet()
                {
                    Id = spreadsheet.WorkbookPart.GetIdOfPart(newWorksheetPart),
                    SheetId = (uint)worksheetNumber,
                    Name = dt.TableName
                });

                worksheetNumber++;
            }

            spreadsheet.WorkbookPart.Workbook.Save();
        }

        private static void WriteDataTableToExcelWorksheet(DataTable dt, WorksheetPart worksheetPart, bool skipHeaders = false)
        {
            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();

            string cellValue = "";

            //  Create a Header Row in our Excel file, containing one header for each Column of data in our DataTable.
            //
            //  We'll also create an array, showing which type each column of data is (Text or Numeric), so when we come to write the actual
            //  cells of data, we'll know if to write Text values or Numeric cell values.
            int numberOfColumns = dt.Columns.Count;
            bool[] IsNumericColumn = new bool[numberOfColumns];
            bool[] IsDecimalColumn = new bool[numberOfColumns];
            bool[] IsDateColumn = new bool[numberOfColumns];

            string[] excelColumnNames = new string[numberOfColumns];
            for (int n = 0; n < numberOfColumns; n++)
                excelColumnNames[n] = GetExcelColumnName(n);

            //
            //  Create the Header row in our Excel Worksheet
            //
            uint rowIndex = 1;
            if (!skipHeaders)
            {
                var headerRow = new Row { RowIndex = rowIndex }; // add a row at the top of spreadsheet
                sheetData.Append(headerRow);

                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    DataColumn col = dt.Columns[colInx];
                    AppendHeaderTextCell(excelColumnNames[colInx] + "1", col.ColumnName, headerRow);
                    IsNumericColumn[colInx] = ((col.DataType.FullName == "System.Int") ||
                                                (col.DataType.FullName == "System.Int32") || (col.DataType.FullName == "System.Int64"));

                    IsDecimalColumn[colInx] = (col.DataType.FullName == "System.Decimal");
                    IsDateColumn[colInx] = (col.DataType.FullName == "System.DateTime");
                }
            }

            //
            //  Now, step through each row of data in our DataTable...
            //
            double cellNumericValue = 0;
            foreach (DataRow dr in dt.Rows)
            {
                // ...create a new row, and append a set of this row's data to it.
                ++rowIndex;
                var newExcelRow = new Row { RowIndex = rowIndex };  // add a row at the top of spreadsheet
                sheetData.Append(newExcelRow);

                for (int colInx = 0; colInx < numberOfColumns; colInx++)
                {
                    cellValue = dr.ItemArray[colInx].ToString();

                    // Create cell with data
                    if (IsNumericColumn[colInx])
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        //  If this numeric value is NULL, then don't write anything to the Excel file.
                        cellNumericValue = 0;
                        if (double.TryParse(cellValue, out cellNumericValue))
                        {
                            cellValue = cellNumericValue.ToString();
                            AppendNumericCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                        }
                    }
                    else if (IsDecimalColumn[colInx])
                    {
                        //  For numeric cells, make sure our input data IS a number, then write it out to the Excel file.
                        //  If this numeric value is NULL, then don't write anything to the Excel file.
                        cellNumericValue = 0;
                        if (double.TryParse(cellValue, out cellNumericValue))
                        {
                            cellValue = cellNumericValue.ToString();
                            AppendDecimalCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                        }
                    }
                    else if (IsDateColumn[colInx])
                    {
                        string val = "";
                        if (!string.IsNullOrEmpty(cellValue))
                        {
                            var dateTime = cellValue;
                            DateTime date = Convert.ToDateTime(dateTime);
                            val = date.ToString("MM/dd/yyyy");
                        }

                        AppendDateCell(excelColumnNames[colInx] + rowIndex.ToString(), val, newExcelRow);
                    }
                    else
                    {
                        //  For text cells, just write the input data straight out to the Excel file.
                        AppendTextCell(excelColumnNames[colInx] + rowIndex.ToString(), cellValue, newExcelRow);
                    }
                }
            }
        }

        private static void AppendHeaderTextCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell()
            {
                CellReference = cellReference,
                DataType = CellValues.String,
                StyleIndex = 2,
            };

            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        private static void AppendTextCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell()
                {
                    CellReference = cellReference,
                    DataType = CellValues.String,
                };

            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        //private Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        //{
        //    return new Cell()
        //    {
        //        CellValue = new CellValue(value),
        //        DataType = new EnumValue<CellValues>(dataType),
        //        StyleIndex = 1
        //    };
        //}

        private static void AppendNumericCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell()
                {
                    CellReference = cellReference,
                    StyleIndex = 0
                };
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }

        private static void AppendDecimalCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell()
            {
                CellReference = cellReference,
                StyleIndex = 4
            };
            CellValue cellValue = new CellValue();
            cellValue.Text = cellStringValue;
            cell.Append(cellValue);
            excelRow.Append(cell);
        }


        private static void AppendDateCell(string cellReference, string cellStringValue, Row excelRow)
        {
            //  Add a new Excel Cell to our Row 
            Cell cell = new Cell()
            {
                CellReference = cellReference,
                //DataType = CellValues.Date,
                StyleIndex = 5
            };

            DateTime dateTime = DateTime.Parse(cellStringValue);
            double oaValue = dateTime.ToOADate();
            cell.CellValue = oaValue <= 0 ? new CellValue("") : new CellValue(oaValue.ToString(CultureInfo.InvariantCulture));


            excelRow.Append(cell);


        }


        private static string GetExcelColumnName(int columnIndex)
        {
            //  Convert a zero-based column index into an Excel column reference  (A, B, C.. Y, Y, AA, AB, AC... AY, AZ, B1, B2..)
            //
            //  eg  GetExcelColumnName(0) should return "A"
            //      GetExcelColumnName(1) should return "B"
            //      GetExcelColumnName(25) should return "Z"
            //      GetExcelColumnName(26) should return "AA"
            //      GetExcelColumnName(27) should return "AB"
            //      ..etc..
            //
            if (columnIndex < 26)
                return ((char)('A' + columnIndex)).ToString();

            char firstChar = (char)('A' + (columnIndex / 26) - 1);
            char secondChar = (char)('A' + (columnIndex % 26));

            return string.Format("{0}{1}", firstChar, secondChar);
        }



        public static string GetMemberName<TValue>(Expression<Func<TValue>> memberAccess)
        {
            return ((MemberExpression)memberAccess.Body).Member.Name;
        }

        public static ServiceResponse CreateCsvFromList<T>(List<T> listItem, string fileName,bool showHeader=true, bool quoteString = false)
        {
            ServiceResponse response = new ServiceResponse();

            StringBuilder master = new StringBuilder();

            if (listItem.Count > 0)
            {
                #region Header FOR CSV FILE
                if (showHeader)
                {
                    T fitem = listItem.First();
                    foreach (var prop in fitem.GetType().GetProperties())
                    {
                        TitleAttribute titleAttribute = Attribute.GetCustomAttributes(prop).OfType<TitleAttribute>().FirstOrDefault();
                        string header = string.Empty;
                        if (titleAttribute != null)
                            header = titleAttribute.Title;
                        if (string.IsNullOrEmpty(header))
                            header = Convert.ToString(prop.Name);
                        if (quoteString)
                        {
                            master.AppendFormat("\"{0}\",", header.Replace("\"", "\"\""));
                        }
                        else
                        {
                            master.AppendFormat("{0},", header);
                        }
                    }
                    master.Append(Environment.NewLine);
                }
                #endregion

                #region Row FOR CSV FILE

                foreach (var item in listItem)
                {
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        string value = Convert.ToString(prop.GetValue(item, null));
                        if (quoteString && prop.PropertyType == typeof(string))
                        {
                            master.AppendFormat("\"{0}\",", value.Replace("\"", "\"\""));
                        }
                        else
                        {
                            master.AppendFormat("{0},", value);
                        }
                    }

                    master.Append(Environment.NewLine);
                }

                #endregion
            }

            string csvOut = master.ToString();

            using (Stream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(outputStream))
                {
                    writer.Write(csvOut);
                    writer.Close();
                }
                outputStream.Close();
            }

            response.IsSuccess = true;
            return response;

        }



        public static ServiceResponse CreateTxtFromList<T>(List<T> listItem, string fileName,string seprator="," ,bool showHeader = false)
        {
            ServiceResponse response = new ServiceResponse();

            StringBuilder master = new StringBuilder();

            if (listItem.Count > 0)
            {
                #region Header FOR CSV FILE

                if (showHeader)
                {
                    T fitem = listItem.First();
                    foreach (var prop in fitem.GetType().GetProperties())
                    {
                        TitleAttribute titleAttribute =
                            Attribute.GetCustomAttributes(prop).OfType<TitleAttribute>().FirstOrDefault();
                        string header = string.Empty;
                        if (titleAttribute != null)
                            header = titleAttribute.Title;
                        if (string.IsNullOrEmpty(header))
                            header = Convert.ToString(prop.Name);
                        master.AppendFormat("{0}{1}", header, seprator);
                    }

                    char[] charsToTrim = { '\t' };
                    master=new StringBuilder(master.ToString().Trim(charsToTrim));
                    master.Append(Environment.NewLine);
                }

                #endregion

                #region Row FOR TXT FILE

                foreach (var item in listItem)
                {
                    foreach (var prop in item.GetType().GetProperties())
                    {
                        string value = Convert.ToString(prop.GetValue(item, null));
                        master.AppendFormat("{0}{1}", value, seprator);
                    }
                    char[] charsToTrim = { '\t' };
                    master = new StringBuilder(master.ToString().Trim(charsToTrim));
                    master.Append(Environment.NewLine);
                }

                #endregion
            }
            master = new StringBuilder(master.ToString().Trim());
            string csvOut = master.ToString();

            using (Stream outputStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(outputStream))
                {
                    writer.Write(csvOut);
                    writer.Close();
                }
                outputStream.Close();
            }

            response.IsSuccess = true;
            return response;

        }

    }









}

