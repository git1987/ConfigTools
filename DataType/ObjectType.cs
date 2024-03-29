using ConfigTools.Excel;
using ConfigTools.UI;

namespace ConfigTools.DataType
{
    internal abstract class ObjectType
    {
        public static ConfigEnum configEnum;
        public virtual string Name => GetType().Name;
        public virtual void ReadExcels(string folder)
        {
            configEnum = new();
            ScriptWrite.Init();
            //删除导出的脚本文件夹
            if (Directory.Exists(ScriptWrite.outputScriptPath))
            {
                Directory.Delete(ScriptWrite.outputScriptPath, true);
            }
            Directory.CreateDirectory($"{ScriptWrite.outputScriptPath}/Editor");
            DirectoryInfo dir = new DirectoryInfo(Config.readExcelPath);
            Dictionary<string, string> languageDatas = null;
            if (Config.isLanguage)
                languageDatas = new Dictionary<string, string>();

            EventManager<int>.Broadcast(typeof(BuildProgress).Name, dir.GetFiles().Length + 1);
            int fileCount = 1;
            foreach (FileInfo file in dir.GetFiles("*.xlsx"))
            {
                //排除excel缓存文件
                if (file.Name.IndexOf("$") == -1)
                {
                    ReadExcel read = new ReadExcel(file.FullName);
                    EventManager<int>.Broadcast(BuildProgress.ProgressUpdate, fileCount++);
                    for (int i = 0; i < read.sheetDataList.Count; i++)
                    {
                        BuildCSharpFile(read.sheetDataList[i]);
                    }
                }
            }
            ScriptWrite.CopyCSharpFile();
            //导出每个表中的enum  sheet
            configEnum.Save($"{ScriptWrite.outputScriptPath}/Config_Enum.cs");
            configEnum = null;
        }
        public virtual string BuildScriptFilePath(string sheetName)
        {
            if (sheetName.ToLower().IndexOf("language") > -1)
            {
                return $"{Config.templatePath}/{GetType().Name}/LanguageConfigAsset.tem";
            }
            else
            {
                return $"{Config.templatePath}/{GetType().Name}/ConfigAsset.tem";
            }
        }
        public abstract void BuildCSharpFile(ReadExcelSheet sheet);
        public abstract void BuildDataFile(ReadExcelSheet sheet);
        /// <summary>
        /// 生成变量赋值代码块
        /// </summary>
        /// <param name="variableType">变量类型</param>
        /// <param name="variableType">变量名称</param>
        /// <returns></returns>
        public abstract string GetAssignmentValue(string variableType, string variableName);
    }
}