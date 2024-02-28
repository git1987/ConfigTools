using ConfigTools.Excel;
using Excel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTools.ScriptableObject
{
    /// <summary>
    /// unity ScriptableObject
    /// </summary>
    internal class ScriptableObjectType : ObjectType
    {
        string templateFile = "template/ScriptableObject";
        public override void BuildCSharpFile(ReadExcelSheet sheet)
        {

        }
        List<string> ReadExcel(string excelPath, Dictionary<string, string> languageDatas)
        {
            Test test = new();
            test.test(excelPath);
            List<string> nameList = new List<string>();
            using (FileStream fileStream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                do
                {
                    if (excelReader.Name.ToLower() == "log"
                        || excelReader.Name.ToLower().IndexOf("sheet") > -1
                        || Tool.IsChinese(excelReader.Name))
                        continue;
                    if (excelReader.Name.ToLower() == "enum")
                    {
                        ReadEnumExcel(excelReader);
                        continue;
                    }
                    //excelReader.Name：sheet名称
                    string sheetName = Tool.LowerToUpper(excelReader.Name, true);
                    int index = 0;
                    //CsvWrite csv = new CsvWrite(fileName);
                    JsonWrite jsonConfig = new JsonWrite(sheetName);
                    ScriptWrite script = new ScriptWrite(sheetName, languageDatas != null);
                    nameList.Add(sheetName);
                    while (excelReader.Read())
                    {
                        //数据起点
                        int dataIndex = (Config.outputType == null ? 5 : 6) - 1;
                        //读取每行的内容
                        if (index < dataIndex)
                        {
                            bool isLanguageConfig = sheetName.ToLower().IndexOf("language") > -1;
                            //第一行至第三行生成配置类 第四行是多语言 第五行是导出类型（客户端、服务器）
                            for (int i = 0; i < excelReader.FieldCount; i++)
                            {
                                if (index == 0)
                                {
                                    if (excelReader.GetString(i) == null || excelReader.GetString(i) == string.Empty)
                                    {
                                        if (i == 0)
                                            Console.WriteLine(sheetName + "====>第一个变量名称为空！！");
                                        else
                                            Console.WriteLine(sheetName + "====>有空的变量名称！！前一个变量名成为：" + excelReader.GetString(i - 1));
                                        break;
                                    }
                                    if (isLanguageConfig && i > 0)
                                    {
                                        //列创建枚举
                                        configEnum.AddEnumSummary("Enum_LanguageType", "翻译类型");
                                        configEnum.AddEnum("Enum_LanguageType",
                                            excelReader.GetString(i), i - 1, excelReader.GetString(i));
                                    }
                                }
                                else if (index == 1)
                                {
                                    if (excelReader.GetString(i) == null || excelReader.GetString(i) == string.Empty)
                                    {
                                        Console.WriteLine(sheetName + "====>有空的变量类型！！！！！！");
                                        break;
                                    }
                                }
                                //数据列数超过名称、类型后的数据忽略
                                else if (i >= script.variableNameList.Count || i >= script.variableTypeList.Count)
                                {
                                    break;
                                }
                                script.Append(index, excelReader.GetString(i));
                            }
                        }
                        else
                        {
                            //从第四行开始储存配置：根据变量名长度
                            for (int i = 0; i < script.variableNameList.Count && i < script.variableTypeList.Count; i++)
                            {
                                //从第一列开始遍历内容
                                if (excelReader.GetString(i) == null || excelReader.GetString(i) == string.Empty)
                                {
                                    //ID值为空
                                    if (i == 0) break;
                                }
                                //内容
                                string content = excelReader.GetString(i);
                                if (content == null) content = String.Empty;
                                if (languageDatas != null && script.languageList[i])
                                {
                                    //判断是否在翻译配置
                                    //language_sheetName_变量名_id
                                    string key = string.Format("language_{0}_{1}_{2}", sheetName, script.variableNameList[i], excelReader.GetString(0));
                                    if (languageDatas.ContainsKey(key))
                                        Console.WriteLine("====>翻译有相同的Key：" + key);
                                    else
                                        languageDatas.Add(key, content);
                                    content = key;
                                }
                                else
                                {
                                    //csv.Append("\"" + content + "\"");
                                    //if (i < script.variableNameList.Count - 1) csv.Append(",");
                                    //else csv.Append("\n");
                                }
                                ///Json数据
                                if (i == 0)
                                {
                                    if (sheetName.ToLower().IndexOf("language") > -1)
                                    {
                                        //翻译配置
                                        jsonConfig.SetValue(index - dataIndex, "key", content, script.variableTypeList[i]);
                                    }
                                    else
                                        jsonConfig.SetValue(index - dataIndex, "ID", content, script.variableTypeList[i]);
                                }
                                jsonConfig.SetValue(index - dataIndex, script.variableNameList[i], content, script.variableTypeList[i]);
                                ///Csv数据
                            }
                        }
                        index++;
                    }
                    script.SaveScript(InitDataType.Json, sheetName);
                    jsonConfig.Save();
                    //csv.Save();
                    Console.WriteLine("导出：" + excelReader.Name + "完成");
                } while (excelReader.NextResult());
            }
            return nameList;
        }
        void ReadEnumExcel(IExcelDataReader excelReader)
        {
            //using (FileStream fileStream = File.Open(excelPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                //excelReader = ExcelReaderFactory.CreateOpenXmlReader(fileStream);
                string currentName = string.Empty;
                do
                {
                    if (excelReader.Name != "enum")
                        continue;
                    //excelReader.Name：sheet名称
                    int index = 0;
                    while (excelReader.Read())
                    {
                        //数据起点
                        int dataIndex = 2 - 1;
                        //读取每行的内容
                        if (index < dataIndex)
                        {
                        }
                        else
                        {
                            //枚举名称
                            string enumName = excelReader.GetString(0);
                            //枚举注释
                            string enumNameSummary;
                            if (enumName != null && enumName != string.Empty && currentName != enumName)
                            {
                                currentName = excelReader.GetString(0);
                                enumNameSummary = excelReader.GetString(1);
                                configEnum.AddEnumSummary(currentName, enumNameSummary);
                            }
                            //枚举值名称
                            string enumValueName = excelReader.GetString(2);
                            //枚举值注释
                            string enumValueNameSummary = excelReader.GetString(3);
                            int? enumValue = excelReader.GetString(4).ToIntOrNull();
                            if (enumValueName == null)
                            {
                                continue;
                            }
                            configEnum.AddEnum(currentName, enumValueName, enumValue, enumValueNameSummary);
                        }
                        index++;
                    }
                } while (false);/*主代码里有NextResoult*/
                //} while (excelReader.NextResult());
                //configEnum.Save(Config.currentPath + "excel/Config_Enum.cs");
            }
        }
    }
}
