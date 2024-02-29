using ConfigTools.DataType;
using ConfigTools.Excel;

namespace ConfigTools.DataType
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
            scriptWrite.CreateScript(this, BuildScriptFilePath(sheet.sheetName));
            scriptWrite.Save();
            BuildDataFile(sheet);
        }

        public override string GetAssignmentValue(string variableType, string variableName)
        {
            string assignmentValue;
            if (variableType.ToLower() == "int")
            {
                assignmentValue = $"reader.ReadInt32()";
            }
            else if (variableType.ToLower() == "long")
            {
                assignmentValue = $"reader.ReadInt64()";
            }
            else if (variableType.ToLower() == "float")
            {
                assignmentValue = $"reader.ReadSingle()";
            }
            else if (variableType.ToLower() == "string")
            {
                assignmentValue = $"reader.ReadString()";
            }
            else if (variableType.ToLower() == "bool")
            {
                assignmentValue = $"reader.ReadBoolean()";
            }
            else if (variableType.ToLower().IndexOf("list") >= 0)
            {
                assignmentValue = $"reader.ReadString().ToList<{variableType.Substring(4)}";
            }
            else if (variableType.ToLower().IndexOf("enum_") >= 0)
            {
                //自定义枚举
                assignmentValue = $"reader.ReadString().ToEnum<{variableType}>()";
            }
            else
            {
                Debug.LogError($"==>存在类型错误");
                throw new Exception("类型错误");
            }
            return assignmentValue;
        }
        //保存为bytes
        public override void BuildDataFile(ReadExcelSheet sheet)
        {
            //配置文件名称用首字母大写
            BinaryWrite write = new BinaryWrite(sheet.SheetName);
            write.SetData(sheet);
            write.Save();
        }
    }
}
