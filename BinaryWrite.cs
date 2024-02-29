using ConfigTools.Excel;
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
        public override void SetData(ReadExcelSheet sheet)
        {
            BinaryWriter write = new BinaryWriter(fileStream);
            write.Write(sheet.datas.Length);
            //遍历每行数据
            for (int i = 0; i < sheet.datas.Length; i++)
            {
                //遍历单个数据
                for (int j = 0; j < sheet.datas[i].Length; j++)
                {
                    Write(write, sheet.variableTypeList[j], sheet.datas[i][j]);
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
                    writer.Write(content == "1" || content.ToLower() == "true");
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
