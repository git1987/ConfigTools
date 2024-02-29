using OfficeOpenXml;

namespace ConfigTools.Excel
{
    internal class ReadExcelSheet_Enum : SheetData
    {
        public ReadExcelSheet_Enum(ExcelWorksheet sheet) : base(sheet)
        {
            rowStartIndex = 1;
            SetDatas();
        }
    }
}
