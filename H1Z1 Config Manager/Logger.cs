using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace H1Z1_Config_Manager
{
    public enum LogType : short
    {
        ERROR,
        WARNING,
        INFO,
        DEBUG
    }

    public class LogMessage
    {
        public LogType type;
        public string text;
        public LogMessage(LogType type, string text)
        {
            this.type = type;
            this.text = text;
        }
    }
    
    public class Logger
    {
        public delegate void LogFunctionDelegate(LogType type, string message);

        public static List<LogMessage> Messages = new List<LogMessage>();
        public static LogFunctionDelegate LogFunction = null;

        private static void AddMessage(LogType type, string message)
        {
#if !DEBUG
            if (type == LogType.DEBUG)
                return;
#endif

            LogMessage msg = new LogMessage(type, message);

            Messages.Add(msg);

            if (LogFunction != null)
                LogFunction(type, message);

#if DEBUG
            Console.Write(String.Format("[{0}] {1}", type, message));
#endif
        }

        public static void Write(LogType type, string message)
        {
            AddMessage(type, message);
        }

        public static void Write(LogType type, string message, params string[] args)
        {
            Write(type, string.Format(message, args));
        }

        public static void WriteLine(LogType type, string message)
        {
            Write(type, message + "\r\n");
        }

        public static void WriteLine(LogType type, string message, params string[] args)
        {
            WriteLine(type, string.Format(message, args));
        }
    }
}
