using OfficeOpenXml;

namespace ConfigTools.Excel
{
    internal class ReadExcelSheet_Enum : SheetData
    {
        public ReadExcelSheet_Enum(ExcelWorksheet sheet,string excelName) : base(sheet, excelName)
        {
            rowStartIndex = 1;
            InitDatas();
            SetDatas();
        }
    }
}
