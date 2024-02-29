using System.Collections.Generic;
using System.Text;
using ConfigTools.Excel;
using LitJson;

namespace ConfigTools
{
    public enum InitDataType
    {
        Json,
        Csv
    }
    /// <summary>
    /// 
    /// </summary>
    internal class ScriptWrite : DataWrite
    {
        /// <summary>
        /// 保存类名
        /// </summary>
        static private List<string> classNameList;
        static public void Init() { classNameList = new(); }
        static string _languageScriptTemplate = string.Empty;
        static string languageScriptTemplate
        {
            get
            {
                if (_languageScriptTemplate == string.Empty)
                {
                    StreamReader sr = new StreamReader(Config.templatePath + "/LanguageConfigAsset.tem");
                    _languageScriptTemplate = sr.ReadToEnd();
                }
                return _languageScriptTemplate;
            }
        }
        static string _jsonScriptTemplate = string.Empty;
        static string jsonScriptTemplate
        {
            get
            {
                if (_jsonScriptTemplate == string.Empty)
                {
                    StreamReader sr = new StreamReader(Config.templatePath + "/ConfigAssetJson.tem");
                    _jsonScriptTemplate = sr.ReadToEnd();
                }
                return _jsonScriptTemplate;
            }
        }
        static string _csvScriptTemplate = string.Empty;
        static string csvScriptTemplate
        {
            get
            {
                if (_csvScriptTemplate == string.Empty)
                {
                    StreamReader sr = new StreamReader(Config.templatePath + "/ConfigAssetCsv.tem");
                    _csvScriptTemplate = sr.ReadToEnd();
                }
                return _csvScriptTemplate;
            }
        }
        StringBuilder scriptContent;
        StreamWriter writer;

        string className;
        ReadExcelSheet sheet;
        ////变量名称
        //public List<string> variableNameList = new List<string>();
        ////是否导出数据
        //public List<bool> buildSignList = new List<bool>();
        ////备注
        //public List<string> summarylist = new List<string>();
        ////变量类型
        //public List<string> variableTypeList = new List<string>();
        ////是否是多语言
        //public List<bool> languageList = new List<bool>();
        /// <summary>
        /// 创建脚本的路径
        /// </summary>
        //static public string scriptPath
        //{
        //    get { return Config.writeScriptPath + "ConfigAssetScript/"; }
        //}

        static public string outputScriptPath
        {
            get { return Config.outputPath + "/ConfigAssetScript/"; }
        }
        static string objectType;
        public ScriptWrite(ReadExcelSheet sheet)
        {
            this.sheet = sheet;
            Init(outputScriptPath, $"{sheet.SheetName}ConfigAsset.cs");
            writer = new(fileStream, Encoding.UTF8);
            classNameList.Add(sheet.SheetName);
            className = sheet.SheetName + "ConfigAsset";
        }
        public void CreateScript_Binary(ObjectType type, string templateFilePath)
        {
            objectType = type.GetType().Name;
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
                            string assignmentValue;
                            if (sheet.variableTypeList[k].ToLower() == "int")
                            {
                                assignmentValue = $"reader.ReadInt32()";
                            }
                            else if (sheet.variableTypeList[k].ToLower() == "long")
                            {
                                assignmentValue = $"reader.ReadInt64()";
                            }
                            else if (sheet.variableTypeList[k].ToLower() == "float")
                            {
                                assignmentValue = $"reader.ReadSingle()";
                            }
                            else if (sheet.variableTypeList[k].ToLower() == "string")
                            {
                                assignmentValue = $"reader.ReadString()";
                            }
                            else if (sheet.variableTypeList[k].ToLower() == "bool")
                            {
                                assignmentValue = $"reader.ReadBoolean()";
                            }
                            else if (sheet.variableTypeList[k].ToLower().IndexOf("list") >= 0)
                            {
                                assignmentValue = $"reader.ReadString().ToList<{sheet.variableTypeList[k].Substring(4)}";
                            }
                            else if (sheet.variableTypeList[k].ToLower().IndexOf("enum_") >= 0)
                            {
                                //自定义枚举
                                assignmentValue = $"reader.ReadString().ToEnum<{sheet.variableTypeList[k]}>()";
                            }
                            else
                            {
                                Debug.LogError($"{sheet.excelName}=>{sheet.sheetName}==>{variableName}类型错误");
                                throw new Exception("类型错误");
                            }
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
        /// 根据模版内容创建脚本文件
        /// </summary>
        /// <param name="template"></param>
        public void CreateScript_ScriptableObject(string templateFilePath)
        {
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
                StringBuilder newContent = new StringBuilder();
                StringBuilder newContents = new StringBuilder();
                ///List
                if (oldContent.IndexOf("List") > -1)
                {
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
                    for (int k = 1; k < sheet.variableNameList.Count; k++)
                    {
                        if (sheet.buildSignList[k] != "0" ||
                            sheet.buildSignList[k] != Config.outputType) continue;
                        StringBuilder changeContent = new StringBuilder(newContent.ToString());
                        //变量名
                        if (changeContent.ToString().IndexOf("#{variableName}") >= 0)
                        {
                            if (Config.isLanguage && sheet.languageList[k] == "1")
                                changeContent.Replace("#{variableName}", "_" + Tool.LowerToUpper(sheet.variableNameList[k]));
                            else
                                changeContent.Replace("#{variableName}", Tool.LowerToUpper(sheet.variableNameList[k]));
                        }
                        //赋值
                        if (changeContent.ToString().IndexOf("#{assignment}") >= 0)
                        {
                            newContents.AppendLine("\t\t\t/*" + sheet.summarylist[k] + "*/");
                            StringBuilder str = new StringBuilder(string.Format("jd[\"{0}\"]", sheet.variableNameList[k]));
                            if (sheet.variableTypeList[k].ToLower() == "string")
                            {
                                str = new StringBuilder(string.Format("{0}.ToString()", str));
                            }
                            else if (sheet.variableTypeList[k].ToLower() == "bool")
                            {
                                str = new StringBuilder(string.Format("{0}.ToString() == \"1\" ||{0}.ToString().ToLower() == \"true\"", str));
                            }
                            else if (sheet.variableTypeList[k].ToLower().IndexOf("list") >= 0)
                            {
                                str.Append(string.Format(".ToList{0}()", sheet.variableTypeList[k].Substring(4)));
                            }
                            else if (sheet.variableTypeList[k].ToLower().IndexOf("enum_") >= 0)
                            {
                                //自定义枚举
                                str = new StringBuilder(string.Format("({0})System.Enum.Parse(typeof({0}), jd[\"{1}\"].ToString() == \"\" ? \"None\" : jd[\"{1}\"].ToString())", sheet.variableTypeList[k], sheet.variableNameList[k]));
                            }
                            else
                            {
                                str = new StringBuilder(string.Format("{0}.Parse({1}.ToString())", sheet.variableTypeList[k], str));
                            }
                            changeContent.Replace("#{assignment}", str.ToString());
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
                                    changeContent.AppendLine("\t\tpublic " + sheet.variableTypeList[k] + " " + Tool.LowerToUpper(sheet.variableNameList[k]) +
                                        "\n\t\t{" +
                                        "\n\t\t\tget" +
                                        "\n\t\t\t{" +
                                        "\n\t\t\t\tif (ConfigAssetsData.instance == null) return _" + Tool.LowerToUpper(sheet.variableNameList[k]) + ";" +
                                        "\n\t\t\t\treturn ConfigAssetsData.instance.GetLanguageText(_" + Tool.LowerToUpper(sheet.variableNameList[k]) + ");" +
                                        "\n\t\t\t}\n\t\t\tset { _" + Tool.LowerToUpper(sheet.variableNameList[k]) + " = value; }" +
                                        "\n\t\t}");
                                    for (languageIndex = k; languageIndex < sheet.variableNameList.Count; languageIndex++)
                                    {
                                        if (sheet.variableNameList[languageIndex].IndexOf(sheet.variableNameList[k]) < 0)
                                        {
                                            //k = languageIndex;
                                            break;
                                        }
                                    }
                                }
                                else
                                    changeContent.Replace("#{typeName}", "public " + sheet.variableTypeList[k]);
                            }
                            else
                                changeContent.Replace("#{typeName}", sheet.variableTypeList[k]);
                        }
                        //翻译配置
                        //if (changeContent.ToString().IndexOf("#{languageType,}") >= 0)
                        //{
                        //changeContent.Replace("#{languageType,}", string.Format("{0} = {1},", variableNameList[k], (k - 1)));
                        //Program.configEnum.AddEnum("Eunm_LanguageType", variableNameList[k], (k - 1), variableNameList[k]);

                        //}
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
            folder = new DirectoryInfo($"{Config.templatePath}/{objectType}");
            foreach (FileInfo file in folder.GetFiles("*.cs", SearchOption.AllDirectories))
            {
                file.CopyTo($"{outputScriptPath}/{file.Name}", true);
            }
            folder = new DirectoryInfo($"{Config.templatePath}/{objectType}");
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
            string tempFilePath = $"{Config.templatePath}/{objectType}/{temFileName}";
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
