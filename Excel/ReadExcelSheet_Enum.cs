using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
