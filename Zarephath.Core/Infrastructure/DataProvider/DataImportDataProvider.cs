using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Zarephath.Core.Models;

namespace Zarephath.Core.Infrastructure.DataProvider
{
    class DataImportDataProvider : BaseDataProvider
    {
        public ServiceResponse ReadAndValidateExcel(string path, string tableName,string columns,long userID, List<SearchValueData> customSPPara = null)
        {
            ServiceResponse response = new ServiceResponse();
            DataTable dataTable = new DataTable();
            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(path, false))
            {
                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();
                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();

                foreach (Cell cell in rows.ElementAt(0))
                {
                    dataTable.Columns.Add(GetCellValue(spreadSheetDocument, cell));
                }

                foreach (Row row in rows)
                {
                    DataRow dataRow = dataTable.NewRow();
                    var cellEnumerator = GetExcelCellEnumerator(row);
                    int i = 0;
                    while (cellEnumerator.MoveNext())
                    {
                        if (dataRow.ItemArray.Length > i)
                        {
                            dataRow[i] = GetCellValue(spreadSheetDocument, cellEnumerator.Current);
                            i++;
                        }
                    }
                    dataTable.Rows.Add(dataRow);
                }

            }
            dataTable.Rows.RemoveAt(0);
            if (dataTable.Rows.Count == 0)
            {
                response.IsSuccess = false;
                response.Message = "File does not contain any record(s).";
                return response;
            }
            dataTable = RemoveEmptyRows(dataTable);
            DeleteAllTempData(tableName, userID);
            string n = InsertData(tableName, dataTable, columns, userID);
            string status = ValidateAndSaveDate("ValidateAndInsert" + tableName, userID, customSPPara);
            if (status == "0")
            {
                response.IsSuccess = false;
                response.Message = "Problem in inserting data.";
            }
            else
            {
                response.IsSuccess = true;
                response.Message = n + " number of Records Inserted.";
            }
            return response;
        }
        
        private IEnumerator<Cell> GetExcelCellEnumerator(Row row)
        {
            int currentCount = 0;
            foreach (Cell cell in row.Descendants<Cell>())
            {
                string columnName = GetColumnName(cell.CellReference);

                int currentColumnIndex = ConvertColumnNameToNumber(columnName);

                for (; currentCount < currentColumnIndex; currentCount++)
                {
                    var emptycell = new Cell() { DataType = null, CellValue = new CellValue(string.Empty) };
                    yield return emptycell;
                }

                yield return cell;
                currentCount++;
            }
        }

        private string GetColumnName(string cellReference)
        {
            var regex = new Regex("[A-Za-z]+");
            var match = regex.Match(cellReference);

            return match.Value;
        }

        private int ConvertColumnNameToNumber(string columnName)
        {
            var alpha = new Regex("^[A-Z]+$");
            if (!alpha.IsMatch(columnName)) throw new ArgumentException();

            char[] colLetters = columnName.ToCharArray();
            Array.Reverse(colLetters);

            var convertedValue = 0;
            for (int i = 0; i < colLetters.Length; i++)
            {
                char letter = colLetters[i];
                int current = i == 0 ? letter - 65 : letter - 64; // ASCII 'A' = 65
                convertedValue += current * (int)Math.Pow(26, i);
            }

            return convertedValue;
        }

        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            string value = cell.CellValue == null ? "" : cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
            {

                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            }


            if (cell.StyleIndex != null)
            {
                CellFormat cellformat =
                    document.WorkbookPart.WorkbookStylesPart.Stylesheet.CellFormats.ChildElements[
                        int.Parse(cell.StyleIndex.InnerText)] as CellFormat;

                if (cellformat.NumberFormatId >= 14 && cellformat.NumberFormatId <= 22 && value != "")
                {
                    //try parse to check is it date else keep same text
                    try
                    {
                        value = String.Format("{0: yyyy-MM-dd}", DateTime.FromOADate(Double.Parse(value)));
                    }
                    catch (Exception e)
                    {
                    }
                }
            }



            return value;

        }

        public string InsertData(string tableName, DataTable table, string columns, long userID)
        {
            List<string> tColumns = columns.Split(',').ToList();
            string query = "Insert into " + tableName + " (CreatedBy," + columns + ") values ";
            DataColumnCollection dtColumns = table.Columns;
            long count = 0;
            long rows = 0;
            int jrow = 0;
            foreach (DataRow dr in table.Rows)
            {
                string newRowQuery = "(";
                bool isAllColumnNull = true;
                for (int i = 0; i < tColumns.Count; i++)
                {
                    string value = "";
                    if (i == 0)
                    {
                        newRowQuery += "'" + userID + "',";
                    }
                    if (dtColumns.Contains(tColumns[i]))
                    {
                        value = Convert.ToString(dr[tColumns[i]]).Contains("'")
                                    ? Convert.ToString(dr[tColumns[i]]).Replace("'", "''")
                                    : Convert.ToString(dr[tColumns[i]]);
                        value = Convert.ToString(value).Contains("@")
                                    ? Convert.ToString(value).Replace("@", "@@")
                                    : Convert.ToString(value);
                        value = value.Trim();
                        value = (value.ToLower() == "null") ? "" : value;

                    }
                    isAllColumnNull = string.IsNullOrEmpty(value) ? isAllColumnNull : false;
                    if (i < tColumns.Count - 1)
                        newRowQuery += string.IsNullOrEmpty(value) ? "null," : "'" + value + "',";
                    else
                        newRowQuery += string.IsNullOrEmpty(value) ? "null" : "'" + value + "'";
                }

                if (jrow < table.Rows.Count - 1)
                    newRowQuery += "),";
                else
                    newRowQuery += ")";

                if (!isAllColumnNull)
                {
                    query += newRowQuery;
                    count++;
                }

                if (count > 10 || jrow >= table.Rows.Count - 1)
                {
                    rows += Convert.ToInt64(ExecQuery(query.TrimEnd(',')));
                    query = "Insert into " + tableName + " (CreatedBy," + columns + ") values ";
                    count = 0;
                }
                jrow++;
            }
            return rows.ToString();
        }

        public string ValidateAndSaveDate(string spName, long userID, List<SearchValueData> customSPPara)
        {
            List<SearchValueData> searchPara = customSPPara ?? new List<SearchValueData>();
            
            searchPara.Add(
                     new SearchValueData
                     {
                         Name = "UserID",
                         Value = userID.ToString()
                     }
                 );
            return GetScalar(spName, searchPara).ToString();
        }

        public void DeleteAllTempData(string tableName, long userID)
        {
            ExecQuery("delete from " + tableName + " where CreatedBy=" + userID);
        }
        
        public DataTable RemoveEmptyRows(DataTable dataTable)
        {
            dataTable = dataTable.Rows.Cast<DataRow>()
                .Where(row => !row.ItemArray.All(field => field is DBNull || string.IsNullOrWhiteSpace(field as string)))
                .CopyToDataTable();
            return dataTable;
        }
    }
}
