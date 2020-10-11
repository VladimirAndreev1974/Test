using AtsenergoGrabber.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Excel = Microsoft.Office.Interop.Excel;

namespace AtsenergoGrabber.Core
{
    public static class ExcelParser
    {
        public async static Task<List<LossModel>> Parse(string path)
        {

            Excel.Application xlApp = null;
            Excel.Workbook xlWorkbook = null;
            Excel._Worksheet xlWorksheet = null;
            Excel.Range xlRange = null;

            try
            {

                string separator = NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator;

                var models = new List<LossModel>();

                //Create COM Objects. Create a COM object for everything that is referenced
                xlApp = new Excel.Application();
                xlWorkbook = xlApp.Workbooks.Open(path);
                xlWorksheet = xlWorkbook.Sheets[1];
                xlRange = xlWorksheet.UsedRange;

                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                int rowStart = 6;

                //iterate over the rows and columns and print to the console as it appears in the file
                //excel is not zero based!!
                var res = await Task.Run(() =>
                {
                    for (int i = rowStart; i <= rowCount; i++)
                    {
                        var model = new LossModel();

                        for (int j = 1; j <= colCount; j++)
                        {
                            //either collect data cell by cell or DO you job like insert to DB 
                            if (xlRange.Cells[i, j] != null && xlRange.Cells[i, j].Value2 != null)
                            {
                                //data += xlRange.Cells[i, j].Value2.ToString();



                                if (j == 1)
                                {
                                    model.Region = xlRange.Cells[i, j].Value2.ToString();
                                }
                                else if (j == 2)
                                {
                                    string amount = xlRange.Cells[i, j].Value2.ToString().Replace(".", separator).Replace(",", separator);

                                    model.Amount = decimal.Parse(amount);
                                    //double _amount = double.Parse(amount);
                                    //model.Amount = (decimal)_amount;
                                    models.Add(model);
                                }

                            }
                        }
                    }
                    return models;
                });

                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {

                if (xlWorkbook != null) xlWorkbook.Close(false);

                if (xlApp != null) xlApp.Quit();


                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlRange);
                xlRange = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorksheet);
                xlWorksheet = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlWorkbook);
                xlWorkbook = null;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlApp);
                xlApp = null;

                GC.Collect();
                //GC.WaitForPendingFinalizers();
            }

        }
    }
    }