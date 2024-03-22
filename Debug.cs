
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
        public static void Log(object message)
        {
            Print(Type.Log, message);
        }
        public static void LogError(object message)
        {
            Print(Type.Error, message);
        }
        public static void LogWarning(object message)
        {
            Print(Type.Warning, message);
        }
        public static void Print(Type debugType, object message)
        {
            string content = debugType switch
            {
                Type.Error => "Error=====>",
                Type.Warning => "Warning--->",
                _ => string.Empty
            }; ;
            MainWindow.instance.AddLog(content + message.ToString());
        }
    }
}
