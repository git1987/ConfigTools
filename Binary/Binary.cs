using ConfigTools.Excel;

namespace ConfigTools.Binary
{
    /// <summary>
    /// 二进制类型
    /// </summary>
    internal class Binary : ObjectType
    {
        public override void ReadExcels(string folder)
        {
            base.ReadExcels(folder);
        }
        public override void BuildCSharpFile(ReadExcelSheet sheet)
        {
            ScriptWrite scriptWrite = new ScriptWrite(sheet);
            scriptWrite.CreateScript_Binary(this, BuildScriptFilePath(sheet.sheetName));
            scriptWrite.Save();
            BuildDataFile(sheet);
        }
        //保存为bytes
        public override void BuildDataFile(ReadExcelSheet sheet)
        {
            //配置文件名称用首字母大写
            BinaryWrite write = new BinaryWrite(sheet.SheetName);
            write.SetData(sheet.datas,sheet.variableTypeList);
            write.Save();
        }
    }
}
