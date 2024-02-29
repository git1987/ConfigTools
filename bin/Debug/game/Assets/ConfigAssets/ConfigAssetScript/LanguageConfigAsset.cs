﻿using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageConfigAsset : ConfigAssetBase
{
    [System.Serializable]
    public class LanguageConfig : ConfigAssetBase.ConfigAsset
    {
		/*翻译key*/
        public string key;
        public string zh;
        public string en;
        public string jp;
        public string de;
        public string GetLanguageText(Enum_LanguageType type)
        {
            switch (type)
            {
                case Enum_LanguageType.Base:

                default:
                    UnityEngine.Debug.LogError(type.ToString() + " is null===>" + key);
                    return key;
            }
        }
    }
    public List<LanguageConfig> configList;
    public Dictionary<string, LanguageConfig> configs;
    
    public override string GetConfigName()
    {
        return "LanguageConfig";
    }
    public string GetLanguageText(string key)
    {
        if (configs.TryGetValue(key, out LanguageConfig config))
            return config.GetLanguageText(ConfigAssetsData.languageType);
        else
        {
            return key;
        }
    }
    protected override void ReadList()
    {
        configs = new Dictionary<string, LanguageConfig>();
        for (int i = 0; i < configList.Count; i++)
        {
            if (!configs.ContainsKey(configList[i].key))
                configs.Add(configList[i].key, configList[i]);
        }
    }
    public override void ReadFromBytes(byte[] bytes)
    {
        MemoryStream stream = new(bytes);
        BinaryReader reader = new BinaryReader(stream);
        int dataCount = reader.ReadInt32();
        configList = new List<LanguageConfig>(dataCount);
	for(int i = 0;i < dataCount; i++)
        {
            LanguageConfig config = new();
			/*翻译key*/
            config.key = reader.ReadString();
            config.zh = reader.ReadString();
            config.en = reader.ReadString();
            config.jp = reader.ReadString();
            config.de = reader.ReadString();
            configList.Add(config);
        }
        ReadList();
    }
}