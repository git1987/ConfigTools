using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTools
{
    /// <summary>
    /// 读取sheet
    /// </summary>
    internal class ReadExcelSheet
    {
        //参数名
        public List<String> paramNameList;
        //参数类型
        String[] paramTypes;
        //参数注释
        String[] paramSummarys;
        //翻译标记：1=>翻译
        String[] languageSigns;
        //是否导出标记：0|""全部导出
        String[] buildSigns;
        //数据
        string[][] datas;
        ExcelWorksheet sheet;
        public ReadExcelSheet(ExcelWorksheet _sheet)
        {
            sheet = _sheet;
            //行
            int rowCount = sheet.Rows.Count();
            int dataCount = rowCount - 5;
            MainWindow.instance.AddLog($"行数量:{rowCount}，实际数据行为:{dataCount}");
            //列数
            int columnCount = sheet.Cells.Count() / rowCount;
            //name行
            paramNameList = new();
            for (int i = 1; i <= columnCount; i++)
            {
                string name = sheet.Cells[1, i].Text;
                if (name is { Length: > 0 })
                {
                    paramNameList.Add(name);
                }
                else
                {
                    Debug.Log(Debug.Type.Error, $"{sheet.Name}=>{paramNameList[paramNameList.Count - 1]}后，有空的变量名");
                    columnCount = paramNameList.Count;
                }
            }
            //参数类型
            SetValue(out paramTypes, 2, columnCount);
            //参数注释
            SetValue(out paramSummarys, 3, columnCount);
            //翻译标记：1=>翻译
            SetValue(out languageSigns, 4, columnCount);
            //是否导出标记：0|""全部导出
            SetValue(out buildSigns, 5, columnCount);

            //数据
            datas = new string[dataCount][];
            for (int i = 0; i < dataCount; i++)
            {
                datas[i] = new string[columnCount];
                for (int j = 0; j < columnCount; j++)
                {
                    datas[i][j] = sheet.Cells[i + 5 + 1, j + 1].Text;
                }
            }
            Debug.Log(Debug.Type.Log, "保存数据完毕");
        }
        void SetValue(out string[] strings, int rowIndex, int columnCount)
        {

            strings = new string[columnCount];
            for (int i = 1; i <= columnCount; i++)
            {
                string value = sheet.Cells[rowIndex, i].Text;
                strings[i - 1] = value;
            }
        }
    }
}
