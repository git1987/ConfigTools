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
        protected string[][] datas;
        protected ExcelWorksheet sheet;
        public SheetData(ExcelWorksheet sheet)
        {
            this.sheet = sheet;
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        protected void SetDatas()
        {
            datas = new string[rowCount - rowStartIndex][];
            for (int i = rowStartIndex; i < rowCount; i++)
            {
                datas[i] = new string[columnCount];
                for (int j = 0; j < columnCount; j++)
                {
                    datas[i][j] = sheet.Cells[i + 1, j + 1].Text;
                }
            }
        }
    }
}
