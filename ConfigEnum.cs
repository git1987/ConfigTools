﻿using System.Text;

namespace ConfigTools
{
    public class ConfigEnum
    {
        enum enumTest
        {
            test1,
            test2 = 2,
            test3
        }
        public class EnumValue
        {
            public string summary;
            public string enumValueName;
            public int? enumValue;
        }
        //所有的枚举词典
        /*
         * enums=>
         * key:枚举名称
         * value:枚举内数据
         * 
         * value=>
         * key2:枚举值名称
         * value:枚举信息（枚举值名称、枚举值注释、枚举值对应的int）
         */
        Dictionary<string, Dictionary<string, EnumValue>> enums;
        //枚举注释
        Dictionary<string, string> enumSummary;

        public ConfigEnum()
        {
            enums = new Dictionary<string, Dictionary<string, EnumValue>>();
            enumSummary = new Dictionary<string, string>();
        }
        public void AddEnum(string enumName, string enumValueName, int? enumValue, string summary)
        {
            if (enums.ContainsKey(enumName))
            {
                if (enums[enumName].ContainsKey(enumValueName))
                {
                    Console.WriteLine(enumName + "中已经存在" + enumValueName);
                }
                else
                {
                    enums[enumName].Add(enumValueName, new EnumValue
                    {
                        summary = summary,
                        enumValueName = enumValueName,
                        enumValue = enumValue
                    });
                }
            }
            else
            {
                EnumValue ev = new EnumValue()
                {
                    summary = summary,
                    enumValueName = enumValueName,
                    enumValue = enumValue
                };
                enums.Add(enumName, new Dictionary<string, EnumValue>() {
                    { enumValueName,ev}
                });
            }
        }
        public void AddEnumSummary(string enumName, string enumNameSummary)
        {
            if (!enumSummary.ContainsKey(enumName))
            {
                enumSummary.Add(enumName, enumNameSummary);
            }
        }
        public void Save(string filePath)
        {
            if (enums.Count > 0)
            {
                StringBuilder sb = new StringBuilder(/*配置中使用的枚举*/);
                foreach (string enumName in enums.Keys)
                {
                    sb.Append(EnumString(enumName, enums[enumName]));
                }
                FileStream fs = new FileStream(filePath, FileMode.Create);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(sb.ToString());
                Console.WriteLine(sb.ToString());
                sw.Close();
                fs.Close();
                Console.WriteLine("配置枚举保存完毕");
            }
            else
            {
                Console.WriteLine("没有要保存的枚举.cs文件");

            }
        }
        string EnumString(string enumName, Dictionary<string, EnumValue> enumValues)
        {
            StringBuilder sb;
            if (enumSummary.ContainsKey(enumName))
            {
                string str = string.Format("/// <summary>\n/// {0}\n/// </summary>\n", enumSummary[enumName]);
                sb = new StringBuilder(str);
            }
            else
            {
                sb = new StringBuilder();
            }
            sb.Append("public enum " + enumName + " {\n");
            if (enumName.IndexOf("LanguageType") > -1)
                sb.Append("\tBase = -1,\n");
            else
                sb.Append("\tNone = -1,\n");
            foreach (string key in enumValues.Keys)
            {
                EnumValue ev = enumValues[key];
                if (ev.summary != null && ev.summary != string.Empty)
                {
                    sb.Append(string.Format("\t/*{0}*/\n", ev.summary));
                }
                if (ev.enumValue != null)
                {
                    sb.Append(string.Format("\t{0} = {1},\n", key, enumValues[key].enumValue));
                }
                else
                {
                    sb.Append(string.Format("\t{0},\n", key));
                }
            }
            sb.Append("}\n");
            return sb.ToString();
        }
    }
}
