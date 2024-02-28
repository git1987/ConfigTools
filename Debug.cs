using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigTools
{
    internal class Debug
    {
        public enum Type
        {
            Log,
            Error,
            Warning
        }
        public static void Log(Type debugType, string message)
        {
            string content = debugType switch
            {
                Type.Error => "Error=====>",
                Type.Warning => "Warning--->",
                _ => string.Empty
            }; ;
            MainWindow.instance.AddLog(content + message);
        }
    }
}
