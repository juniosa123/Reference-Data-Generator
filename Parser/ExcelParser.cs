using System;
using System.IO;
using ExcelDataReader;
using System.Data;

namespace wedoIT.CATS.Tools.ReferenceDataGenerator.Parser
{
    public static class ExcelParser
    {
        public static DataSet ParseExcel(string Filepath)
        {
            var result = new DataSet();
            try
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (var stream = File.Open(Filepath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Choose one of either 1 or 2:

                        // 1. Use the reader methods
                        do
                        {
                            while (reader.Read())
                            {
                                // reader.GetDouble(0);
                            }
                        } while (reader.NextResult());

                        // 2. Use the AsDataSet extension method
                        result = reader.AsDataSet();


                        // The result of each spreadsheet is in result.Tables
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error while reading the excel file : " + ex.Message);
            }



            return result;
        }
    }
}
