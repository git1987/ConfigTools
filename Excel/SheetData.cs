using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTools.Excel
{
    internal class SheetData
    {
        //有效数据列的个数
        protected int columnCount;
        //有效数据行的个数
        protected int rowCount;
        //数据行起点
        protected int rowStartIndex;
        //数据集合
        public string[][] datas { private set; get; }
        protected ExcelWorksheet sheet;
        public string sheetName => sheet.Name;
        public string excelName { protected set; get; }
        /// <summary>
        /// 首字母大写sheet名称
        /// </summary>
        public string SheetName { private set; get; }
        public SheetData(ExcelWorksheet sheet)
        {
            this.sheet = sheet;
            SheetName = Tool.FirstUpper(sheetName);
            //行
            rowCount = sheet.Rows.Count();
            //列数
            columnCount = sheet.Cells.Count() / rowCount;
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        protected void SetDatas()
        {
            datas = new string[rowCount - rowStartIndex][];
            for (int i = rowStartIndex; i < rowCount; i++)
            {
                datas[i - rowStartIndex] = new string[columnCount];
                for (int j = 0; j < columnCount; j++)
                {
                    datas[i - rowStartIndex][j] = sheet.Cells[i + 1, j + 1].Text;
                }
            }
        }
    }
}
