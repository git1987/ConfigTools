using OfficeOpenXml;
using OfficeOpenXml.DataValidation;

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
        public SheetData(ExcelWorksheet sheet, string excelName)
        {
            this.sheet = sheet;
            this.excelName = excelName;
            SheetName = Tool.FirstUpper(sheetName);
        }
        /// <summary>
        /// 初始化数据
        /// </summary>
        protected void InitDatas()
        {
            //列数
            columnCount = sheet.Columns.Range.Columns;
            //行
            for (int h = rowStartIndex; ; h++)
            {
                bool hasData = false;
                for (int i = 0; i < columnCount; i++)
                {
                    string content = sheet.Cells[h + 1, i + 1].Text;
                    if (content is { Length: > 0 })
                    {
                        hasData = true;
                        break;
                    }
                }
                if (!hasData) break;
                else rowCount = h + 1;
            }
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
