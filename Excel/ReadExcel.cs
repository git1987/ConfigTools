using OfficeOpenXml;

namespace ConfigTools.Excel
{
    internal class ReadExcel
    {
        public List<ReadExcelSheet> sheetDataList = new();
        public ReadExcel(string excelPath, ConfigEnum configEnum)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage package = new(excelPath);
            ExcelWorksheets sheets = package.Workbook.Worksheets;
            foreach (ExcelWorksheet sheet in sheets)
            {
                Debug.Log(Debug.Type.Log, $"{package.File.Name}=>{sheet.Name}");
                string sheetName = sheet.Name.ToLower();
                //忽略的sheet
                if (sheetName.IndexOf("log") > -1 ||
                    sheetName.IndexOf("sheet") > -1 ||
                    Tool.IsChinese(sheetName)) continue;
                //枚举
                if (sheet.Name == "enum")
                {
                    EnumSheet(configEnum, sheet);
                }
                else
                {
                    ReadExcelSheet excelSheet = new ReadExcelSheet(sheet);
                    sheetDataList.Add(excelSheet);
                }
            }
        }
        void EnumSheet(ConfigEnum configEnum, ExcelWorksheet enumSheet)
        {
            ReadExcelSheet_Enum readExcel = new ReadExcelSheet_Enum(enumSheet);
        }
    }
}
