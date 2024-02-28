using ConfigTools.Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTools
{
    internal abstract class ObjectType
    {
        protected ConfigEnum configEnum = new();
        protected ScriptWrite scriptWrite;
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
                        BuildCSharpFile(read.sheetDataList[i]);
                    }
                }
            }
            //导出每个表中的enum  sheet
            configEnum.Save($"{Config.appPath}excel/Config_Enum.cs");
            ScriptWrite.CopyCSharpFile();
        }
        public abstract void BuildCSharpFile(ReadExcelSheet sheet);
        /// <summary>
        /// 复制单例类和Editor类
        /// </summary>
        public virtual void CopyCSharp()
        {

        }

    }
}