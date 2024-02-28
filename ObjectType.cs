using ConfigTools.Excel;

namespace ConfigTools
{
    internal abstract class ObjectType
    {
        protected ConfigEnum configEnum = new();
        public virtual void ReadExcels(string folder, bool useLanguage)
        {
            DirectoryInfo dir = new DirectoryInfo(Config.readExcelPath);
            Dictionary<string, string> languageDatas = null;
            if (useLanguage)
                languageDatas = new Dictionary<string, string>();
            foreach (FileInfo file in dir.GetFiles("*.xlsx"))
            {
                //排除excel缓存文件
                if (file.Name.IndexOf("$") == -1)
                {
                    ReadExcel read = new ReadExcel(file.FullName, configEnum);
                    for (int i = 0; i < read.sheetDataList.Count; i++)
                    {
                        BuildCSharpFile(read.sheetDataList[i],useLanguage);
                    }
                }
            }
            //导出每个表中的enum  sheet
            configEnum.Save($"{Config.appPath}excel/Config_Enum.cs");
            ScriptWrite.CopyCSharpFile(GetType().Name);
        }
        public abstract void BuildCSharpFile(ReadExcelSheet sheet, bool isLanguage);
        public abstract void BuildDataFile(ReadExcelSheet sheet);
    }
}