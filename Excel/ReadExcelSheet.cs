using ConfigTools.DataType;
using OfficeOpenXml;

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
                    Debug.LogError($"{sheet.Name}=>{variableNameList[variableNameList.Count - 1]}后，有空的变量名");
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

            if (sheetName == "language")
            {
                string languageEnumName = "Enum_LanguageType";
                //导出多语言枚举
                for (int i = 1; i < variableNameList.Count; i++)
                {
                    ObjectType.configEnum.AddEnum(languageEnumName,
                        variableNameList[i],
                        null,
                        summarylist[i]);
                }
                ObjectType.configEnum.AddEnumSummary(languageEnumName, "多语言枚举");
            }
            //数据
            SetDatas();
            Debug.Log("保存数据完毕");
        }
        public void SetExcelName(string excelName)
        {
            this.excelName = excelName;
        }

        void SetValue(out List<string> contentList, int rowIndex, int columnCount)
        {
            contentList = new(columnCount);
            for (int i = 1; i <= columnCount; i++)
            {
                string value = sheet.Cells[rowIndex, i].Text;
                if (rowIndex == 2)
                {
                    if (value.ToLower().IndexOf("list") > -1)
                    {
                        value = Tool.FirstUpper(value);
                    }
                }
                contentList.Add(value);
            }
        }
    }
}
