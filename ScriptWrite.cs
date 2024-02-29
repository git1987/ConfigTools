using System.Text;
using ConfigTools.DataType;
using ConfigTools.Excel;

namespace ConfigTools
{
    /// <summary>
    /// 写入脚本类型的文件
    /// </summary>
    internal class ScriptWrite : DataWrite
    {
        /// <summary>
        /// 保存类名
        /// </summary>
        static private List<string> classNameList;
        static public void Init() { classNameList = new(); }
        StringBuilder scriptContent;
        StreamWriter writer;

        string className;
        ReadExcelSheet sheet;

        static public string outputScriptPath
        {
            get { return Config.outputPath + "/ConfigAssetScript/"; }
        }
        static ObjectType objectType;
        public ScriptWrite(ReadExcelSheet sheet)
        {
            this.sheet = sheet;
            Init(outputScriptPath, $"{sheet.SheetName}ConfigAsset.cs");
            writer = new(fileStream, Encoding.UTF8);
            classNameList.Add(sheet.SheetName);
            className = sheet.SheetName + "ConfigAsset";
        }

        public override void SetData(ReadExcelSheet sheet)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据ObjectType类型创建对应的配置类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="templateFilePath"></param>
        /// <exception cref="Exception"></exception>
        public void CreateScript(ObjectType type, string templateFilePath)
        {
            objectType = type;
            StreamReader sr = new StreamReader(templateFilePath);
            string template = sr.ReadToEnd();
            scriptContent = new StringBuilder(template);
            sr.Close();
            //类注释
            scriptContent.Replace("#{ClassSummary}", $"excel:{sheet.excelName}===>{sheet.sheetName}");
            //类名修改
            scriptContent.Replace("#{ClassName}", sheet.SheetName);
            //#if 和 #endif标记的index集合：成对出现
            List<int> signList = new();
            int temp = 0;
            foreach (string str in template.Split('\n'))
            {
                int indexIf = str.IndexOf("#if");
                int indexEndif = str.IndexOf("#endif");
                if (indexIf >= 0)
                    signList.Add(indexIf + temp);
                if (indexEndif >= 0)
                    signList.Add(indexEndif + temp);
                temp += str.Length + 1;
            }
            if (signList.Count % 2 != 0) throw new Exception("#if和#endif不相等");
            //倒序修改：先修改后面的，防止影响标记的index
            for (int i = signList.Count - 1; i > 0; i -= 2)
            {
                string oldContent = template.Substring(signList[i - 1], signList[i] - signList[i - 1] + 6);
                //所有内容整合之后的内容
                StringBuilder newContents = new StringBuilder();
                ///List
                if (oldContent.IndexOf("List") > -1)
                {
                    //List中每个单独的内容
                    StringBuilder newContent = new StringBuilder();
                    string[] oldContents = oldContent.Split('\n');
                    for (int j = 1; j < oldContents.Length - 1; j++)
                    {
                        newContent.Append(oldContents[j]);
                    }
                    if (newContent.ToString() == string.Empty)
                    {
                        throw new Exception("#if中没有内容");
                    }
                    int languageIndex;
                    //第1列固定使用ID变量名，使用ID变量名
                    for (int k = 0; k < sheet.variableNameList.Count; k++)
                    {
                        if (sheet.buildSignList[k] != "0" ||
                            sheet.buildSignList[k] != Config.outputType) continue;
                        StringBuilder changeContent = new StringBuilder(newContent.ToString());
                        string variableName;
                        if (k == 0 && sheet.sheetName.ToLower().IndexOf("language") == -1)
                        {
                            variableName = "ID";
                        }
                        else variableName = sheet.variableNameList[k];
                        //变量名
                        if (changeContent.ToString().IndexOf("#{variableName}") > -1)
                        {
                            if (changeContent.ToString().IndexOf("Enum_LanguageType") > -1)
                            {
                                continue;
                            }
                            if (Config.isLanguage && sheet.languageList[k] == "1")
                                changeContent.Replace("#{variableName}", "_" + variableName);
                            else
                            {
                                changeContent.Replace("#{variableName}", variableName);
                            }
                        }
                        //赋值
                        if (changeContent.ToString().IndexOf("#{assignment}") >= 0)
                        {
                            newContents.AppendLine("\t\t\t/*" + sheet.summarylist[k] + "*/");
                            string assignmentValue =
                                objectType.GetAssignmentValue(sheet.variableTypeList[k], sheet.variableNameList[k]);
                            changeContent.Replace("#{assignment}", assignmentValue);
                        }
                        //变量类型
                        if (changeContent.ToString().IndexOf("#{typeName}") >= 0)
                        {
                            newContents.AppendLine("\t\t/*" + sheet.summarylist[k] + "*/");
                            //类
                            if (className.IndexOf("ConfigAsset") > -1)
                            {
                                if (Config.isLanguage && sheet.languageList[k] == "1")
                                {
                                    changeContent.Replace("#{typeName}", "[SerializeField]\n        private " + sheet.variableTypeList[k]);
                                    changeContent.AppendLine("\t\tpublic " + sheet.variableTypeList[k] + " " + variableName +
                                        "\n\t\t{" +
                                        "\n\t\t\tget" +
                                        "\n\t\t\t{" +
                                        "\n\t\t\t\tif (ConfigAssetsData.instance == null) return _" + variableName + ";" +
                                        "\n\t\t\t\treturn ConfigAssetsData.instance.GetLanguageText(_" + variableName + ");" +
                                        "\n\t\t\t}\n\t\t\tset { _" + Tool.LowerToUpper(variableName) + " = value; }" +
                                        "\n\t\t}");
                                }
                                else
                                    changeContent.Replace("#{typeName}", "public " + sheet.variableTypeList[k]);
                            }
                            else
                                changeContent.Replace("#{typeName}", sheet.variableTypeList[k]);
                        }
                        newContents.Append(changeContent);
                    }
                }
                scriptContent.Replace(oldContent, newContents.ToString());
            }
            scriptContent.Replace("#{ClassName}", sheet.SheetName);
            writer.Write(scriptContent.ToString());
        }

        /// <summary>
        /// 根据要导出的配置类修改并导出配置中心类脚本
        /// </summary>
        static public void BuildConfigAssetsData()
        {
            WriteCSFile("ConfigAssetsData.tem", $"{outputScriptPath}/ConfigAssetsData.cs");
        }
        /// <summary>
        /// 复制不需要修改的cs脚本
        /// </summary>
        /// <param name="folder">子文件夹名称</param>
        public static void CopyCSharpFile()
        {
            BuildConfigAssetsData();
            DirectoryInfo folder = new DirectoryInfo(Config.templatePath);
            foreach (FileInfo file in folder.GetFiles("*.cs"))
            {
                file.CopyTo($"{outputScriptPath}/{file.Name}", true);
            }
            folder = new DirectoryInfo($"{Config.templatePath}/{objectType.Name}");
            foreach (FileInfo file in folder.GetFiles("*.cs", SearchOption.AllDirectories))
            {
                file.CopyTo($"{outputScriptPath}/{file.Name}", true);
            }
            folder = new DirectoryInfo($"{Config.templatePath}/{objectType.Name}");
            foreach (FileInfo file in folder.GetFiles("*.cseditor"))
            {
                string fileName = file.Name;
                fileName = fileName.Substring(0, fileName.Length - "editor".Length);
                file.CopyTo($"{outputScriptPath}/Editor/{fileName}", true);
            }
        }
        //生成完整的.cs脚本文件
        static private void WriteCSFile(string temFileName, string csFilePath)
        {
            string tempFilePath = $"{Config.templatePath}/{objectType.Name}/{temFileName}";
            if (!File.Exists(tempFilePath))
            {
                Debug.LogError($"{temFileName}文件不存在====>");
                return;
            }
            StreamReader sr = new StreamReader(tempFilePath);
            StringBuilder template = new StringBuilder(sr.ReadToEnd());
            List<int> list = new List<int>();
            int temp = 0;
            foreach (string str in template.ToString().Split('\n'))
            {
                int indexIf = str.IndexOf("#if");
                int indexEndif = str.IndexOf("#endif");
                if (indexIf >= 0)
                    list.Add(indexIf + temp);
                if (indexEndif >= 0)
                    list.Add(indexEndif + temp);
                temp += str.Length + 1;
            }
            if (list.Count % 2 != 0) throw new Exception("#if和#endif不相等");
            for (int i = list.Count - 1; i > 0; i -= 2)
            {
                string oldContent = template.ToString().Substring(list[i - 1], list[i] - list[i - 1] + 6);
                StringBuilder newContent = new StringBuilder();
                StringBuilder newContents = new StringBuilder();
                if (oldContent.IndexOf("List") >= 0)
                {
                    string[] oldContents = oldContent.Split('\n');
                    for (int j = 1; j < oldContents.Length - 1; j++)
                    {
                        newContent.AppendLine(oldContents[j]);
                    }
                    if (newContent.ToString() == string.Empty)
                    {
                        throw new Exception("#if中没有内容");
                    }
                    for (int j = 0; j < classNameList.Count; j++)
                    {
                        StringBuilder changeContent = new StringBuilder(newContent.ToString());
                        changeContent.Replace("#{ClassName}", Tool.FirstUpper(classNameList[j]));
                        changeContent.Replace("#{className}", Tool.FirstLower(classNameList[j]));
                        newContents.Append(changeContent.ToString());
                    }
                    template.Replace(oldContent, newContents.ToString());
                }
            }
            //保存
            FileStream fs = new FileStream(csFilePath.IndexOf(".cs") >= 0 ? csFilePath : csFilePath + ".cs", FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(template.ToString());
            sw.Flush();
            sw.Close();
            fs.Close();
            Console.WriteLine("CS脚本文件" + csFilePath + "创建完成");
        }
        public override void Save()
        {
            writer.Flush();
            writer.Close();
            base.Save();
        }
    }
}
