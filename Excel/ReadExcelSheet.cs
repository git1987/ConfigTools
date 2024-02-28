using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTools.Excel
{
    /// <summary>
    /// 读取sheet
    /// </summary>
    internal class ReadExcelSheet : SheetData
    {
        //变量名
        public List<string> variableNameList;
        //变量类型
        public List<string> variableTypeList;
        //注释
        public List<string> summarylist;
        //翻译标记：1=>翻译
        public List<string> languageList;
        //是否导出标记：0|""全部导出
        public List<string> buildSignList;
        public ReadExcelSheet(ExcelWorksheet _sheet) : base(_sheet)
        {
            rowStartIndex = 5;
            //name行
            variableNameList = new();
            for (int i = 1; i <= columnCount; i++)
            {
                string name = sheet.Cells[1, i].Text;
                if (name is { Length: > 0 })
                {
                    variableNameList.Add(name);
                }
                else
                {
                    Debug.Log(Debug.Type.Error, $"{sheet.Name}=>{variableNameList[variableNameList.Count - 1]}后，有空的变量名");
                    columnCount = variableNameList.Count;
                }
            }
            //参数类型
            SetValue(out variableTypeList, 2, columnCount);
            //参数注释
            SetValue(out summarylist, 3, columnCount);
            //翻译标记：1=>翻译
            SetValue(out languageList, 4, columnCount);
            //是否导出标记：0|""全部导出
            SetValue(out buildSignList, 5, columnCount);

            //数据
            SetDatas();
            Debug.Log(Debug.Type.Log, "保存数据完毕");
        }
        void SetValue(out List<string> contentList, int rowIndex, int columnCount)
        {
            contentList = new(columnCount);
            for (int i = 1; i <= columnCount; i++)
            {
                string value = sheet.Cells[rowIndex, i].Text;
                contentList.Add(value);
            }
        }
    }
}
