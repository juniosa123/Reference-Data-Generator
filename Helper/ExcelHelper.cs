using System;
using System.Collections.Generic;
using System.IO;
using GemBox.Spreadsheet;
using wedoIT.CATS.Tools.ReferenceDataGenerator.Model;

namespace wedoIT.CATS.Tools.ReferenceDataGenerator.Helper
{
    public class ExcelHelper
    {

        public void CreateExcelFile(string OutPutFileDirectory)
        {
            var data = CreateDummyData();

            var datetime = DateTime.Now.ToString().Replace("/", "_").Replace(":", "_");

            string fileFullname = Path.Combine(OutPutFileDirectory, "RefDataTemplate.xlsx");

            if (File.Exists(fileFullname))
            {
                fileFullname = Path.Combine(OutPutFileDirectory, "RefDataTemplate_" + datetime + ".xlsx");
            }

            SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY");
            var workbook = new ExcelFile();
            var ws = workbook.Worksheets.Add("Bulk Template");
            GenerateSheetdataForDetails(ws, data);
            workbook.Save(fileFullname);
        }
        private void GenerateSheetdataForDetails(ExcelWorksheet ws, List<ExcelTemplate> data)
        {
            CreateHeaderRowForExcel(ws, data);
            for (int i = 1; i <= data.Count; i++)
            {
                var detail = data[i - 1];
                ws.Cells[i, 0].Value = detail.ReferenceDataType;
                ws.Cells[i, 1].Value = detail.Code;
                ws.Cells[i, 2].Value = detail.CodeName;
                ws.Cells[i, 3].Value = detail.ParameterData;
                ws.Cells[i, 4].Value = detail.DescriptionEN;
                ws.Cells[i, 5].Value = detail.DescriptionID;
            }

        }

        private void CreateHeaderRowForExcel(ExcelWorksheet ws, List<ExcelTemplate> data)
        {
            ws.Cells[0, 0].Value = "Reference Data Type";
            ws.Cells[0, 1].Value = "Code";
            ws.Cells[0, 2].Value = "Code Name";
            ws.Cells[0, 3].Value = "Parameter Data (JSON)";
            ws.Cells[0, 4].Value = "Description (EN)";
            ws.Cells[0, 5].Value = "Description (ID)";
        }

        private List<ExcelTemplate> CreateDummyData()
        {
            var result = new List<ExcelTemplate>();

            var data = new ExcelTemplate();
            data.ReferenceDataType = "SECTOR";
            data.Code = "SECTOR_FORESTRY";
            data.CodeName = "FORESTRY";
            data.ParameterData = "{}";
            data.DescriptionEN = "This is forestry sector";
            data.DescriptionID = "Ini adalah sektor kehutanan";
            result.Add(data);

            var data2 = new ExcelTemplate();
            data2.ReferenceDataType = "SUBSECTOR";
            data2.Code = "SUBSECTOR_PLANTATION";
            data.CodeName = "PLANTATION";
            data2.ParameterData = "{\"SECTOR\":\"SECTOR_FORESTRY\"}";
            data2.DescriptionEN = "This is Plantation sector";
            data2.DescriptionID = "Ini adalah sektor perkebunan";
            result.Add(data2);

            return result;
        }
    }
}
