//using System.Globalization;
//using System.Runtime.InteropServices;
//using DocumentFormat.OpenXml.Spreadsheet;
//using Domain.Entities;
//using Microsoft.VisualBasic.FileIO;

//namespace Application.Business.Utilities
//{
//    public static class ExcelConnector
//    {
//        public static List<HistoricalData> GethHistoricalPricesFromDukasCopyFile(string excelPath)
//        {
//            var historicalPrices = new List<HistoricalData>();
//            using (TextFieldParser parser = new TextFieldParser(excelPath))
//            {
//                parser.TextFieldType = FieldType.Delimited;
//                parser.SetDelimiters(",");
//                var headersPassed = false;
//                while (!parser.EndOfData)
//                {
//                    //Process row
//                    string[] fields = parser.ReadFields();
//                    if (parser.LineNumber > 2)
//                    {
//                        var fieldCount = 0;
//                        var historicData = new HistoricalData();
//                        foreach (string field in fields)
//                        {
//                            try
//                            {
//                                switch (fieldCount)
//                                {
//                                    case 0:
//                                        historicData.Date = DateTime.ParseExact(field, "dd.MM.yyyy HH:mm:ss.fff 'GMT'K", CultureInfo.InvariantCulture);
//                                        break;
//                                    case 1:
//                                        historicData.OpenPrice = Convert.ToDecimal(field);
//                                        break;
//                                    case 2:
//                                        historicData.HighPrice = Convert.ToDecimal(field);
//                                        break;
//                                    case 3:
//                                        historicData.LowPrice = Convert.ToDecimal(field);
//                                        break;
//                                    case 4:
//                                        historicData.ClosePrice = Convert.ToDecimal(field);
//                                        break;
//                                    case 5:
//                                        historicData.Volume = Convert.ToDecimal(field);
//                                        break;
//                                }
//                            }
//                            catch (Exception ex)
//                            {
//                                Console.WriteLine(ex.Message);
//                            }
//                            historicalPrices.Add(historicData);
//                            fieldCount++;
//                        }
//                    }
//                    headersPassed = true;
//                }
//            }
//            //string str;
//            int rCnt;
//            int rw = 0;
//            int cl = 0;

//            var xlApp = new Application();
//            Workbook xlWorkBook = xlApp.Workbooks.Open(@excelPath, 0, true, 5, "", "", true, XlPlatform.xlWindows, "\t",
//                false, false, 0, true, 1, 0);
//            var xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

//            Range range = xlWorkSheet.UsedRange;
//            rw = range.Rows.Count;
//            cl = range.Columns.Count;


//            for (rCnt = 2; rCnt <= rw; rCnt++)
//            {

//                //historicalPrices.Add(new HistoricalPrice() { DateTime = date, Price = ((low + high) / 2).ToString() });
//                //historicalPrices.Add(new HistoricalPrice() { DateTime = date, Price = ((double)(range.Cells[rCnt, 5] as Excel.Range).Value2).ToString() });
//                //DateTime date = DateTime.FromOADate((double)(range.Cells[rCnt, 1] as Range).Value2);
//                //var low = (double) (range.Cells[rCnt, 3] as Range).Value2;
//                //var high = (double) (range.Cells[rCnt, 4] as Range).Value2;
//                //var close = (double) (range.Cells[rCnt, 5] as Range).Value2;
//                //historicalPrices.Add(new HistoricalData
//                //{
//                //    Date = date,
//                //    ClosePrice = Convert.ToDecimal(close.ToString(CultureInfo.InvariantCulture))
//                //});
//            }

//            xlWorkBook.Close(false, null, null);
//            xlApp.Quit();

//            Marshal.ReleaseComObject(xlWorkSheet);
//            Marshal.ReleaseComObject(xlWorkBook);
//            Marshal.ReleaseComObject(xlApp);
//            return historicalPrices;
//        }
//        public static List<HistoricalData> GethHistoricalPrices(string excelPath)
//        {
//            //string str;
//            int rCnt;
//            int rw = 0;
//            int cl = 0;

//            var xlApp = new Application();
//            Workbook xlWorkBook = xlApp.Workbooks.Open(@excelPath, 0, true, 5, "", "", true, XlPlatform.xlWindows, "\t",
//                false, false, 0, true, 1, 0);
//            var xlWorkSheet = (Worksheet)xlWorkBook.Worksheets.get_Item(1);

//            Range range = xlWorkSheet.UsedRange;
//            rw = range.Rows.Count;
//            cl = range.Columns.Count;
//            var historicalPrices = new List<HistoricalData>();

//            for (rCnt = 2; rCnt <= rw; rCnt++)
//            {
//                //var date = DateTime.FromOADate((double)(range.Cells[rCnt, 1] as Excel.Range).Value2);
//                //var low = (double)(range.Cells[rCnt, 3] as Excel.Range).Value2;
//                //var high = (double)(range.Cells[rCnt, 4] as Excel.Range).Value2;
//                //historicalPrices.Add(new HistoricalPrice() { DateTime = date, Price = ((low + high) / 2).ToString() });
//                //historicalPrices.Add(new HistoricalPrice() { DateTime = date, Price = ((double)(range.Cells[rCnt, 5] as Excel.Range).Value2).ToString() });
//                DateTime date = DateTime.FromOADate((double)(range.Cells[rCnt, 1] as Range).Value2);
//                var low = (double)(range.Cells[rCnt, 3] as Range).Value2;
//                var high = (double)(range.Cells[rCnt, 4] as Range).Value2;
//                var close = (double)(range.Cells[rCnt, 5] as Range).Value2;
//                historicalPrices.Add(new HistoricalData
//                {
//                    Date = date,
//                    ClosePrice = Convert.ToDecimal(close.ToString(CultureInfo.InvariantCulture))
//                });
//            }

//            xlWorkBook.Close(false, null, null);
//            xlApp.Quit();

//            Marshal.ReleaseComObject(xlWorkSheet);
//            Marshal.ReleaseComObject(xlWorkBook);
//            Marshal.ReleaseComObject(xlApp);
//            return historicalPrices;
//        }
//    }
//}