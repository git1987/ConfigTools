using System.Collections;
using System.Text;
using LitJson;

namespace ConfigTools
{
    /// <summary>
    /// 程序配置、参数
    /// </summary>
    internal sealed class Config
    {
        static private string _appPath = string.Empty;
        static public string appPath
        {
            get
            {
                if (_appPath == string.Empty)
                {
                    _appPath = Tool.ResetPathSlash(System.AppDomain.CurrentDomain.BaseDirectory);
                    Console.WriteLine("工具路径：" + _appPath);
                }
                return _appPath;
            }
        }
        static public string configFile => $"{appPath}/config.json";
        /// <summary>
        /// 是否翻译
        /// </summary>
        static public bool isLanguage;
        static public void Init()
        {
            ///路径配置文件
            Console.WriteLine("配置文件：" + configFile);
            JsonData jd;
            if (File.Exists(configFile))
            {
                StreamReader sr = new StreamReader(configFile);
                StringBuilder sb = new StringBuilder(sr.ReadToEnd());
                sb.Replace("\\", "/");
                Console.WriteLine(sb.ToString());
                jd = JsonMapper.ToObject(sb.ToString());
                sr.Close();
            }
            else
            {
                jd = new JsonData();
                if (Directory.Exists($"{appPath}/excel"))
                    jd["excel"] = $"{appPath}/excel";
                else
                    jd["excel"] = "excel配置路径";
                jd["outputType"] = "0";
                FileStream fs = new FileStream(configFile, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(Tool.JsonFormat(jd.ToJson()));
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                //关闭文件
                fs.Close();
                Debug.Log("第一次创建config.json配置文件，修改对应路径");
            }
            readExcelPath = jd["excel"].ToString();
            if (((IDictionary)jd).Contains("outputType"))
                outputType = jd["outputType"].ToString().ToInt().ToString();
            else
                outputType = null;
        }
        /// <summary>
        /// excel路径
        /// </summary>
        internal static string readExcelPath { private set; get; }
        /// <summary>
        /// 导出的配置相关路径：unity工程目录
        /// </summary>
        internal static string outputPath
        {
            get
            {
                DirectoryInfo parentFolder = new DirectoryInfo(appPath).Parent;
                foreach (DirectoryInfo di in parentFolder.GetDirectories("*", SearchOption.AllDirectories))
                {
                    if (di.Name == ("Assets"))
                    {
                        return $"{di.FullName}/ConfigAssets";
                    }
                }
                Debug.LogError($"程序上层文件夹没有Unity工程：{parentFolder.FullName}");
                return string.Empty;
            }
        }
        /// <summary>
        /// 模板文件路径
        /// </summary>
        internal static string templatePath => $"{appPath}/template";
        /// <summary>
        /// 导出类型
        /// 0:全部导出
        /// 1:客户端
        /// 2:服务器
        /// </summary>
        internal static string outputType { set; get; }

        private static void Modify()
        {
            FileStream fs = new FileStream(configFile, FileMode.Create, FileAccess.Write);
            JsonData jd = new JsonData();
            jd["excel"] = readExcelPath;
            jd["outputType"] = outputType;
            StreamWriter writer = new StreamWriter(fs);
            writer.Write(Tool.JsonFormat(jd.ToJson()));
            //清空缓冲区
            writer.Flush();
            //关闭流
            writer.Close();
            //关闭文件
            fs.Close();
        }
    }
}
