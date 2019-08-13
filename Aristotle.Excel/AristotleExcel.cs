using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Aristotle.Excel
{
    public class AristotleExcel
    {
        private string _workbookFile { get; set; }
        private ExcelPackage XlPackage { get; set; }
        public Dictionary<int, ExcelRange> ColumnFormat { get; set; }

        public AristotleExcel(string workbookFile)
        {
            _workbookFile = workbookFile;
            XlPackage = new ExcelPackage(new FileInfo(_workbookFile));
        }


        #region File Creation
        public void AddWorksheet(IEnumerable dataList, string tabName, IEnumerable<AristotleExcelStyle> columnStyleList)
        {
            var ws = XlPackage.Workbook.Worksheets.Add(tabName);

            var row = 2;
            var col = 1;
            var ctr = 0;
            var headers = new List<string>();
            Color shadedBackground = ColorTranslator.FromHtml("#cbdaf2");
            Color nonShadedBackground = ColorTranslator.FromHtml("#ffffff");

            try
            {
                FormatColumns(ws, columnStyleList);

                PropertyInfo[] properties;
                foreach (var item in dataList)
                {
                    properties = item.GetType().GetProperties();
                    ctr++;
                    col = 1;
                    foreach (PropertyInfo property in properties)
                    {
                        if (row.Equals(2))
                            headers.Add(property.Name);

                        if (tabName.Contains("Summary"))
                        {
                            ws.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[row, col].Style.Fill.BackgroundColor.SetColor(row % 2 == 0 ? shadedBackground : nonShadedBackground);
                        }

                        if(col == 2)
                            if ( (string) property.GetValue(item, null).ToString() == "z--Totals")
                            {
                                property.SetValue(item, "Totals");
                                ws.Cells[row, 1].Style.Font.Bold = true;
                                for (int i = 1; i < properties.Length; i++)
                                    ws.Cells[row, i].Style.Font.Bold = true;
                            }

                        ws.Cells[row, col++].Value = property.GetValue(item, null);
                    }

                    if ((string) ws.Cells[row, 2].Value.ToString() == "Totals")
                        row++;
                    row++;
                }

                col = 1;
                row = 1;
                foreach (string header in headers)
                    ws.Cells[row, col++].Value = header;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        private void FormatColumns(ExcelWorksheet ws, IEnumerable<AristotleExcelStyle> columnStyleList)
        {
            foreach (AristotleExcelStyle item in columnStyleList)
            {
                ws.Column(item.ColumnNumber).Style.Numberformat.Format = item.Format;
                ws.Column(item.ColumnNumber).Width = item.ColumnWidth;
                ws.Column(item.ColumnNumber).Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                ws.Column(item.ColumnNumber).Style.Font.Color.SetColor(item.FontColor);
            }
        }

        public void SaveWorkbook()
        {
            XlPackage.Save();
        }
        #endregion
    }
}