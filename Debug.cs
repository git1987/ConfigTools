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
        public static void Log(string message)
        {
            Print(Type.Log, message);
        }
        public static void LogError(string message)
        {
            Print(Type.Error, message);
        }
        public static void LogWarning(string message)
        {
            Print(Type.Warning, message);
        }
        public static void Print(Type debugType, string message)
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
