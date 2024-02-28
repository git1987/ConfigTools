using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTools
{
    internal class Test
    {
        public void test(string excelPath)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage package = new(excelPath);
            ExcelWorksheets sheets = package.Workbook.Worksheets;
            foreach (ExcelWorksheet sheet in sheets)
            {

            }

        }
    }
}
