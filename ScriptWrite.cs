﻿using System.Text;
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
        //static string scriptTemplate;
        /// <summary>
        /// 保存类名
        /// </summary>
        static public List<string> classNameList;
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
        public enum StringType
        {
            //变量名
            variableName = 0,
            //变量类型
            variableType = 1,
            //注释
            summary,
            //翻译
            language = 3,
            //导出类型
            output
        }
        StringBuilder scriptContent;
        StreamWriter writer;

        string className;
        string fileName;
        //变量名称
        public List<string> variableNameList = new List<string>();
        //是否导出数据
        public List<bool> buildSignList = new List<bool>();
        //备注
        public List<string> summarylist = new List<string>();
        //变量类型
        public List<string> variableTypeList = new List<string>();
        //是否是多语言
        public List<bool> languageList = new List<bool>();
        //是否翻译配置
        bool isLanguage;
        /// <summary>
        /// 创建脚本的路径
        /// </summary>
        static public string scriptPath
        {
            get { return Config.writeScriptPath + "ConfigAssetScript/"; }
        }

        static public string outputScriptPath
        {
            get { return Config.outputDataPath + "ConfigAssetScript/"; }
        }
        public ScriptWrite(string _fileName, bool _isLanguage)
        {
            Init(scriptPath, _fileName + "ConfigAsset.cs");
            writer = new(fileStream, Encoding.UTF8);
            this.fileName = _fileName;
            classNameList.Add(fileName);
            className = _fileName + "ConfigAsset";
            this.isLanguage = _isLanguage;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="nameList"></param>
        /// <param name="typeList"></param>
        /// <param name="summaryList"></param>
        /// <param name="languageList"></param>
        /// <param name="buildSignList"></param>
        /// <param name=""></param>
        public void InitClassInfo(List<string> nameList,
            List<string> typeList,
            List<string> summaryList,
            List<string> languageList,
            List<string> buildSignList)
        {
            variableNameList = nameList;
            variableTypeList = typeList;
            this.summarylist = summaryList;
            this.languageList = new(languageList.Count);
            for (int i = 0; i < languageList.Count; i++)
            {
                this.languageList.Add(languageList[i] == "1");
            }
            this.buildSignList = new(buildSignList.Count);
            for (int i = 0; i < buildSignList.Count; i++)
            {
                this.buildSignList.Add(
                    buildSignList[i] == string.Empty ||
                    buildSignList[i] == "0" ||
                    buildSignList[i] == Config.outputType
                    );
            }
        }
        public void Append(int type, string content)
        {
            if (content == null)
                content = "";
            switch ((StringType)type)
            {
                case StringType.variableName:
                    variableNameList.Add(content);
                    break;
                case StringType.summary:
                    summarylist.Add(content);
                    break;
                case StringType.variableType:
                    variableTypeList.Add(content);
                    break;
                case StringType.language:
                    //输入1才有翻译
                    if (isLanguage)
                        languageList.Add(content == "1");
                    break;
                case StringType.output:
                    buildSignList.Add(content == string.Empty || content == "0" || content == Config.outputType);
                    break;
            }
        }
        void SetVariableByJson()
        {
            if (variableNameList.Count != variableTypeList.Count)
            {
                //throw new Exception("变量名、变量类型、备注数量不相等：" + className);
                Console.WriteLine("变量名、变量类型、备注数量不相等：" + className);
            }
            //变量个数
            int forCount = variableNameList.Count;
            List<int> list = new List<int>();
            int temp = 0;
            string scriptTemplate;
            if (fileName.ToLower().IndexOf("language") > -1)
            {
                scriptTemplate = languageScriptTemplate;
            }
            else
            {
                scriptTemplate = jsonScriptTemplate;
            }
            foreach (string str in scriptTemplate.Split('\n'))
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
                string oldContent = scriptTemplate.Substring(list[i - 1], list[i] - list[i - 1] + 6);
                StringBuilder newContent = new StringBuilder();
                StringBuilder newContents = new StringBuilder();
                if (oldContent.IndexOf("List") >= 0)
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
                    for (int k = 1; k < forCount; k++)
                    {
                        if (buildSignList.Count > 0 && !buildSignList[k]) continue;
                        StringBuilder changeContent = new StringBuilder(newContent.ToString());
                        //变量名
                        if (changeContent.ToString().IndexOf("#{variableName}") >= 0)
                        {
                            if (isLanguage && languageList[k])
                                changeContent.Replace("#{variableName}", "_" + Tool.LowerToUpper(variableNameList[k]));
                            else
                                changeContent.Replace("#{variableName}", Tool.LowerToUpper(variableNameList[k]));
                        }
                        //赋值
                        if (changeContent.ToString().IndexOf("#{assignment}") >= 0)
                        {
                            newContents.AppendLine("\t\t\t/*" + summarylist[k] + "*/");
                            StringBuilder str = new StringBuilder(string.Format("jd[\"{0}\"]", variableNameList[k]));
                            if (variableTypeList[k].ToLower() == "string")
                            {
                                str = new StringBuilder(string.Format("{0}.ToString()", str));
                            }
                            else if (variableTypeList[k].ToLower() == "bool")
                            {
                                str = new StringBuilder(string.Format("{0}.ToString() == \"1\" ||{0}.ToString().ToLower() == \"true\"", str));
                            }
                            else if (variableTypeList[k].ToLower().IndexOf("list") >= 0)
                            {
                                str.Append(string.Format(".ToList{0}()", variableTypeList[k].Substring(4)));
                            }
                            else if (variableTypeList[k].ToLower().IndexOf("enum_") >= 0)
                            {
                                //自定义枚举
                                str = new StringBuilder(string.Format("({0})System.Enum.Parse(typeof({0}), jd[\"{1}\"].ToString() == \"\" ? \"None\" : jd[\"{1}\"].ToString())", variableTypeList[k], variableNameList[k]));
                            }
                            else
                            {
                                str = new StringBuilder(string.Format("{0}.Parse({1}.ToString())", variableTypeList[k], str));
                            }
                            changeContent.Replace("#{assignment}", str.ToString());
                        }
                        //变量类型
                        if (changeContent.ToString().IndexOf("#{typeName}") >= 0)
                        {
                            newContents.AppendLine("\t\t/*" + summarylist[k] + "*/");
                            //类
                            if (className.IndexOf("ConfigAsset") > -1)
                            {
                                if (isLanguage && languageList[k])
                                {
                                    changeContent.Replace("#{typeName}", "[SerializeField]\n        private " + variableTypeList[k]);
                                    changeContent.AppendLine("\t\tpublic " + variableTypeList[k] + " " + Tool.LowerToUpper(variableNameList[k]) +
                                        "\n\t\t{" +
                                        "\n\t\t\tget" +
                                        "\n\t\t\t{" +
                                        "\n\t\t\t\tif (ConfigAssetsData.instance == null) return _" + Tool.LowerToUpper(variableNameList[k]) + ";" +
                                        "\n\t\t\t\treturn ConfigAssetsData.instance.GetLanguageText(_" + Tool.LowerToUpper(variableNameList[k]) + ");" +
                                        "\n\t\t\t}\n\t\t\tset { _" + Tool.LowerToUpper(variableNameList[k]) + " = value; }" +
                                        "\n\t\t}");
                                    for (languageIndex = k; languageIndex < forCount; languageIndex++)
                                    {
                                        if (variableNameList[languageIndex].IndexOf(variableNameList[k]) < 0)
                                        {
                                            //k = languageIndex;
                                            break;
                                        }
                                    }
                                }
                                else
                                    changeContent.Replace("#{typeName}", "public " + variableTypeList[k]);
                            }
                            else
                                changeContent.Replace("#{typeName}", variableTypeList[k]);
                        }
                        //翻译配置
                        if (changeContent.ToString().IndexOf("#{languageType,}") >= 0)
                        {
                            //changeContent.Replace("#{languageType,}", string.Format("{0} = {1},", variableNameList[k], (k - 1)));
                            //Program.configEnum.AddEnum("Eunm_LanguageType", variableNameList[k], (k - 1), variableNameList[k]);

                        }
                        newContents.Append(changeContent);
                    }
                }
                scriptContent.Replace(oldContent, newContents.ToString());
            }
        }
        void SetVariableByCsv()
        {
            if (variableNameList.Count != summarylist.Count ||
                variableNameList.Count != variableTypeList.Count)
            {
                throw new Exception("变量名、变量类型、备注数量不相等：" + className);
            }
            //变量个数
            int forCount = variableNameList.Count;
            List<int> list = new List<int>();
            int temp = 0;
            foreach (string str in csvScriptTemplate.Split('\n'))
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
                string oldContent = csvScriptTemplate.Substring(list[i - 1], list[i] - list[i - 1] + 6);
                StringBuilder newContent = new StringBuilder();
                StringBuilder newContents = new StringBuilder();
                if (oldContent.IndexOf("List") >= 0)
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
                    for (int k = 1; k < forCount; k++)
                    {
                        StringBuilder changeContent = new StringBuilder(newContent.ToString());
                        //变量名称
                        if (changeContent.ToString().IndexOf("#{variableName}") >= 0)
                        {
                            if (isLanguage && languageList[k])
                                changeContent.Replace("#{variableName}", "_" + Tool.LowerToUpper(variableNameList[k]));
                            else
                                changeContent.Replace("#{variableName}", Tool.LowerToUpper(variableNameList[k]));
                        }
                        //赋值
                        if (changeContent.ToString().IndexOf("#{assignment}") >= 0)
                        {
                            newContents.AppendLine("\t\t\t/*" + summarylist[k] + "*/");
                            StringBuilder str = new StringBuilder(string.Format("list[{0}]", k));
                            if (variableTypeList[k].ToLower() == "string")
                            {
                                str = new StringBuilder(string.Format("{0} as {1}", str, variableTypeList[k]));
                            }
                            else if (variableTypeList[k].ToLower().IndexOf("list") >= 0)
                            {
                                str.Append(string.Format(".To{0}()", variableTypeList[k]));
                            }
                            else
                            {
                                str = new StringBuilder(string.Format("{0}.Parse({1} as string)", variableTypeList[k], str));
                            }
                            changeContent.Replace("#{assignment}", str.ToString());
                        }
                        //变量类型
                        if (changeContent.ToString().IndexOf("#{typeName}") >= 0)
                        {
                            newContents.AppendLine("\t\t/*" + summarylist[k] + "*/");
                            //类
                            if (className.IndexOf("ConfigAsset") > -1)
                            {
                                if (isLanguage && languageList[k])
                                {
                                    changeContent.Replace("#{typeName}", "[SerializeField]\n        private " + variableTypeList[k]);
                                    changeContent.AppendLine("\t\tpublic " + variableTypeList[k] + " " + Tool.LowerToUpper(variableNameList[k]) +
                                        "\n\t\t{" +
                                        "\n\t\t\tget" +
                                        "\n\t\t\t{" +
                                        "\n\t\t\t\tif (ConfigAssetsData.instance == null) return _" + Tool.LowerToUpper(variableNameList[k]) + ";" +
                                        "\n\t\t\t\treturn ConfigAssetsData.instance.GetLanguageText(_" + Tool.LowerToUpper(variableNameList[k]) + ");" +
                                        "\n\t\t\t}\n\t\t\tset { _" + Tool.LowerToUpper(variableNameList[k]) + " = value; }" +
                                        "\n\t\t}");
                                    for (languageIndex = k; languageIndex < forCount; languageIndex++)
                                    {
                                        if (variableNameList[languageIndex].IndexOf(variableNameList[k]) < 0)
                                        {
                                            //k = languageIndex;
                                            break;
                                        }
                                    }
                                }
                                else
                                    changeContent.Replace("#{typeName}", "public " + variableTypeList[k]);
                            }
                            else
                                changeContent.Replace("#{typeName}", variableTypeList[k]);
                        }
                        newContents.Append(changeContent);
                    }
                }
                scriptContent.Replace(oldContent, newContents.ToString());
            }
        }

        public void SaveScript(InitDataType type, string language = "")
        {
            switch (type)
            {
                case InitDataType.Json:
                    if (language.ToLower().IndexOf("language") > -1)
                        scriptContent = new StringBuilder(languageScriptTemplate);
                    else
                        scriptContent = new StringBuilder(jsonScriptTemplate);
                    SetVariableByJson();
                    break;
                case InitDataType.Csv:
                    if (language.ToLower().IndexOf("language") > -1)
                        scriptContent = new StringBuilder(languageScriptTemplate);
                    else
                        scriptContent = new StringBuilder(csvScriptTemplate);
                    SetVariableByCsv();
                    break;
            }
            scriptContent.Replace("#{ClassName}", fileName);
            writer.Write(scriptContent.ToString());
            writer.Flush();
            writer.Close();
            fileStream.Close();
            JsonData jd = new JsonData();
        }
        static public void BuildConfigAssetsData()
        {
            WriteCSFile("ConfigAssetsData.tem", Config.writeScriptPath + "ConfigAssetsData.cs");
        }
        static public void BuildConfigAssetBase()
        {
            WriteCSFile("ConfigAssetBase.tem", Config.writeScriptPath + "ConfigAssetBase.cs");
        }
        /// <summary>
        /// 复制不需要修改的cs脚本
        /// </summary>
        /// <param name="folder">子文件夹名称</param>
        public static void CopyCSharpFile(string sonFolder)
        {
            if (!Directory.Exists($"{scriptPath}/Editor"))
                Directory.CreateDirectory($"{scriptPath}/Editor");
            DirectoryInfo folder = new DirectoryInfo(Config.templatePath);
            foreach (FileInfo file in folder.GetFiles("*.cs"))
            {
                file.CopyTo(scriptPath + file.Name, true);
            }
            folder = new DirectoryInfo($"{Config.templatePath}{sonFolder}");
            foreach (FileInfo file in folder.GetFiles("*.cseditor"))
            {
                string fileName = file.Name;
                fileName = fileName.Substring(0, fileName.Length - "editor".Length);
                file.CopyTo($"{scriptPath}/Editor/{fileName}", true);
            }
        }
        //生成完整的.cs脚本文件
        static private void WriteCSFile(string temFileName, string csFilePath)
        {
            if (!File.Exists(Config.templatePath + temFileName))
            {
                Console.WriteLine(".cstem文件不存在====>" + temFileName);
            }
            StreamReader sr = new StreamReader(Config.templatePath + temFileName);
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
    }
}
