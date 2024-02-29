using LitJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTools
{
    internal class BinaryWrite : DataWrite
    {
        public BinaryWrite(string fileName)
        {
            string binaryPath = $"{Config.outputPath}/ConfigAssetBinary";
            Init(binaryPath, fileName + "Config.bytes");
        }
        public void SetData(string[][] datas,List<string> typeList)
        {
            BinaryWriter write = new BinaryWriter(fileStream);
            write.Write(datas.Length);
            //遍历每行数据
            for (int i = 0; i < datas.Length; i++)
            {
                //遍历单个数据
                for (int j = 0; j < datas[i].Length; j++)
                {
                    Write(write, typeList[j], datas[i][j]);
                    //write.Write(datas[i][j]);
                }
            }
        }
        void Write(BinaryWriter writer, string type, string content)
        {
            switch (type)
            {
                case "int":
                    writer.Write(content.ToInt());
                    break;
                case "long":
                    writer.Write(content.ToLong());
                    break;
                case "float":
                    writer.Write(content.ToFloat());
                    break;
                case "bool":
                    writer.Write(content == "1");
                    break;
                default:
                    writer.Write(content);
                    break;
            }
        }
        public override void Save()
        {
            base.Save();
        }
    }
}
