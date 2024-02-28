﻿using System.IO;
using System.Text;

namespace ConfigTools
{
    internal class CsvWrite : DataWrite
    {
        StringBuilder config;
        protected StreamWriter writer;
        public CsvWrite(string configName)
        {
            string csvPath = Config.writeDataPath + "ConfigAssetCsv/";
            Init(csvPath, configName + "Config.csv");
            writer = new(fileStream, Encoding.UTF8);
            config = new StringBuilder();
        }
        public CsvWrite(string configName, string path)
        {
            string csvPath = path;
            Init(csvPath, configName + ".csv");
            config = new StringBuilder();
        }

        public void Append(string content)
        {
            config.Append(content);
        }
        public override void Save()
        {
            writer.Write(config.ToString());
            base.Save();
        }
    }
}
