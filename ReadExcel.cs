using OfficeOpenXml;
using System;
using System.IO;

namespace ConfigTools
{
    internal class ReadExcel
    {
        public ReadExcel(string excelPath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage package = new(excelPath);
            ExcelWorksheets sheets = package.Workbook.Worksheets;
            foreach (ExcelWorksheet sheet in sheets)
            {
                MainWindow.instance.AddLog($"{package.File.Name}=>{sheet.Name}");
                string sheetName = sheet.Name.ToLower();
                //忽略的sheet
                if (sheetName.IndexOf("log") > -1 ||
                    sheetName.IndexOf("sheet") > -1 ||
                    Tool.IsChinese(sheetName)) continue;
                //枚举
                if (sheet.Name == "enum")
                {

                }
                else
                {
                    ReadExcelSheet excelSheet = new ReadExcelSheet(sheet);
                    break;
                }
            }
        }
        void EnumSheet(ExcelWorksheet enumSheet)
        {

        }
    }
}
