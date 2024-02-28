using ConfigTools.Template;
using System;
using System.Collections.Generic;
using System.Diagnostics;

///导出的类模板
namespace ConfigTools.Template
{
    public class ScriptableObject { }
    public abstract class ConfigBase_ScriptableObject : ScriptableObject
    {
        public abstract string ConfigName { get; }
        public class ConfigAsset
        {
            public int ID;
        }
        public abstract void ReadFromData(object obj);
        public abstract void ReadList();
    }
    public abstract class ConfigBase_Binary
    {
        public abstract string ConfigName { get; }
        public class ConfigAsset
        {
            public int ID;
        }
        /*
         * MemoryStream ms = new MemoryStream(bytes);
         * BinaryReader reader = new BinaryReader(ms);
         */
        public abstract void ReadList();
        public abstract void Read(byte[] bytes);
    }

    public class TestConfigAsset : ConfigBase_Binary
    {
        public class TestConfig : ConfigBase_Binary.ConfigAsset
        {
            public int id;
            public string name;
            public int type;
        }
        public List<TestConfig> testList;
        public Dictionary<int, TestConfig> configs;
        public override string ConfigName => GetType().Name;
        public override void ReadList()
        {
            configs = new();
            for (int i = 0; i < testList.Count; i++)
            {
                configs.Add(testList[i].ID, testList[i]);
            }
        }
        public override void Read(byte[] bytes)
        {
            testList = new();
            MemoryStream ms = new(bytes);
            BinaryReader reader = new(ms);
            int column = reader.ReadInt32();
            int objectCount = (bytes.Length - 1) / column;
            if (objectCount * column != bytes.Length)
            {
                //Debug.LogError($"object:{objList.Count}\ncolumn:{column}");
                return;
            }
            for (int i = 0; i < objectCount; i++)
            {
                TestConfig test = new();
                test.ID = reader.ReadInt32();
                test.id = reader.ReadInt32();
                test.name = reader.ReadString();
                test.type = reader.ReadInt32();
            }
            ReadList();
        }
    }
}
