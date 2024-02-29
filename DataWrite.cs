using ConfigTools.Excel;
using System.IO;
using System.IO.Pipes;

namespace ConfigTools
{
    internal abstract class DataWrite
    {
        protected FileStream fileStream;
        protected void Init(string filePath, string fileName)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            fileStream = new FileStream($"{filePath}/{fileName}", FileMode.OpenOrCreate, FileAccess.Write);
        }
        public virtual void Save()
        {
            if (fileStream != null)
            {
                //fileStream.Flush();
                fileStream.Close();
                fileStream = null;
            }
            else {
                Debug.LogError("已经被释放掉了");
            }
        }
        public abstract void SetData(ReadExcelSheet sheet);
    }
}
