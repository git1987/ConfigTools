using System.IO;
using System.IO.Pipes;

namespace ConfigTools
{
    internal class DataWrite
    {
        protected FileStream fileStream;
        protected void Init(string filePath, string fileName)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            fileStream = new FileStream(filePath + fileName, FileMode.OpenOrCreate, FileAccess.Write);
        }
        public virtual void Save()
        {
            fileStream.Flush();
            fileStream.Close();
            fileStream = null;
        }

    }
}
