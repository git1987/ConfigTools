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
            string binaryPath = Config.writeDataPath + "ConfigAssetBinary/";
            Init(binaryPath, fileName + "Config.bytes");
        }
        public void SetData(string[][] datas)
        {
            BinaryWriter write = new BinaryWriter(fileStream);
            for (int i = 0; i < datas.Length; i++)
            {
                for (int j = 0; j < datas[i].Length; j++)
                {
                    write.Write(datas[i][j]);
                }
            }
        }
        public override void Save()
        {
            base.Save();
        }
    }
}
