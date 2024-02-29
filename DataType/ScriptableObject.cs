using ConfigTools.Excel;
using Excel;
using System.Text;

namespace ConfigTools.DataType
{
    /// <summary>
    /// unity ScriptableObject
    /// </summary>
    internal class ScriptableObject : ObjectType
    {
        public override void BuildCSharpFile(ReadExcelSheet sheet)
        {
            ScriptWrite scriptWrite = new ScriptWrite(sheet);
            scriptWrite.CreateScript(this, BuildScriptFilePath(sheet.sheetName));
            scriptWrite.Save();
            BuildDataFile(sheet);
        }
        //保存为json
        public override void BuildDataFile(ReadExcelSheet sheet)
        {
            //配置文件名称用首字母大写
            JsonWrite write = new JsonWrite(sheet.SheetName);
            write.SetData(sheet);
            write.Save();
        }

        public override string GetAssignmentValue(string variableType, string variableName)
        {
            string assignmentValue;
            if (variableType.ToLower() == "string")
            {
                assignmentValue = $"jd[\"{variableName}\"].ToString()";
            }
            else if (variableType.ToLower() == "bool")
            {
                assignmentValue = $"jd[\"{variableName}\"].ToString() == \"1\" || jd[\"{variableName}\"].ToString().ToLower() == \"true\"";
            }
            else if (variableType.ToLower().IndexOf("list") >= 0)
            {
                assignmentValue = $"jd[\"{variableName}\"].ToList<{variableType.Substring(4)}()";
            }
            else if (variableType.ToLower() == "int" ||
                variableType.ToLower() == "float" ||
                variableType.ToLower() == "long")
            {
                assignmentValue = $"jd[\"{variableName}\"].ToString() == string.Empty ? 0 :{variableType}.Parse(jd[\"{variableName}\"].ToString())";
            }
            else
            {
                //自定义枚举
                assignmentValue = (string.Format("({0})System.Enum.Parse(typeof({0}), jd[\"{1}\"].ToString() == \"\" ? \"None\" : jd[\"{1}\"].ToString())", variableType, variableName));
            }
            return assignmentValue;
        }
    }
}
