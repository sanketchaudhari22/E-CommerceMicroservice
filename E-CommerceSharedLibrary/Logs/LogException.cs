using Serilog;

namespace E_CommerceSharedLibrary.Logs
{

    // Logging class for handling exceptions and logging them to various outputs
    public static class LogException
    {
        public static void logException(Exception ex)
        {
            LogToFile(ex.Message);
            LogToConsole(ex.Message);
            LogToDebuger(ex.Message);

        }

        public static void LogToFile(string message) => Log.Information(message);
        public static void LogToConsole(string message) => Log.Warning(message);
        public static void LogToDebuger(string message) => Log.Debug(message);

    }
           
}
