using ConfigTools.DataType;
using ConfigTools.UI;
using OfficeOpenXml;

namespace ConfigTools.Excel
{
    internal class ReadExcel
    {
        public List<ReadExcelSheet> sheetDataList = new();
        public ReadExcel(string excelPath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage package = new(excelPath);
            ExcelWorksheets sheets = package.Workbook.Worksheets;
            foreach (ExcelWorksheet sheet in sheets)
            {
                Debug.Log($"{package.File.Name}=>{sheet.Name}");
                string sheetName = sheet.Name.ToLower();
                //忽略的sheet
                if (sheetName.IndexOf("log") > -1 ||
                    sheetName.IndexOf("sheet") > -1 ||
                    Tool.IsChinese(sheetName)) continue;
                //枚举
                if (sheet.Name == "enum")
                {
                    EnumSheet(sheet, package.File.Name);
                }
                else
                {
                    ReadExcelSheet excelSheet = new ReadExcelSheet(sheet, package.File.Name);
                    sheetDataList.Add(excelSheet);
                }
            }
        }
        void EnumSheet(ExcelWorksheet enumSheet, string excelName)
        {
            ReadExcelSheet_Enum readExcel = new ReadExcelSheet_Enum(enumSheet, excelName);
            string enumName = string.Empty;
            for (int i = 0; i < readExcel.datas.Length; i++)
            {
                string name = readExcel.datas[i][0];
                int? enumValue = readExcel.datas[i][4] == string.Empty ?
                    null : readExcel.datas[i][4].ToInt();
                if (name != string.Empty)
                {
                    enumName = name;
                    ObjectType.configEnum.AddEnumSummary(name, readExcel.datas[i][1]);
                }
                ObjectType.configEnum.AddEnum(enumName,
                    readExcel.datas[i][2],
                    enumValue,
                    readExcel.datas[i][3]);
            }
        }
    }
}
