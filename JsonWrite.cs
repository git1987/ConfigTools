﻿using System.Collections;
using LitJson;

namespace ConfigTools
{
    internal class JsonWrite : DataWrite
    {
        public JsonData jsonData;
        protected StreamWriter writer;
        public JsonWrite(string fileName)
        {
            string jsonPath = Config.writeDataPath + "ConfigAssetJson/";
            Init(jsonPath, fileName + "Config.json");
            writer = new(fileStream, System.Text.Encoding.UTF8);
            jsonData = new JsonData();
            jsonData["name"] = fileName + "Config";
        }
        public void SetValue(int index, string key, string value, string type)
        {
            if (value == null || value == "")
            {
                if (type.ToLower().IndexOf("string") > -1)
                    value = "";
                else
                    value = "0";
            }
            if (((IDictionary)jsonData).Contains("config"))
            {
                while (jsonData["config"].Count <= index)
                {
                    jsonData["config"].Add(new JsonData());
                }
                jsonData["config"][index][key] = value;
            }
            else
            {
                jsonData["config"] = new JsonData();
                JsonData item = new JsonData();
                item[key] = value;
                jsonData["config"].Add(item);
            }
        }

        public override void Save()
        {
            writer.Write(Tool.JsonFormat(jsonData));
            base.Save();
        }

    }
}
