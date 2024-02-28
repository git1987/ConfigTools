using ConfigTools.Excel;

namespace ConfigTools.Binary
{
    /// <summary>
    /// 二进制类型
    /// </summary>
    internal class Binary : ObjectType
    {
        public override void ReadExcels(string folder, bool useLanguage)
        {
            base.ReadExcels(folder, useLanguage);
        }
        public override void BuildCSharpFile(ReadExcelSheet sheet, bool isLanguage)
        {
            ScriptWrite scriptWrite = new ScriptWrite(sheet.sheetName, isLanguage);

        }
        //保存为bytes
        public override void BuildDataFile(ReadExcelSheet sheet)
        {
            BinaryWrite write = new BinaryWrite(sheet.sheetName);
            write.SetData(sheet.datas);
        }
    }
}
