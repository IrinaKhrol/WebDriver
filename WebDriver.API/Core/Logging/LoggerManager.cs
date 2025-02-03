using NLog;

namespace WebDriver.API.Core.Logging
{
    public static class LoggerManager
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        public static void LogInfo(string message)
        {
            Logger.Info(message);
        }

        public static void LogWarning(string message)
        {
            Logger.Warn(message);
        }

        public static void LogDebug(string message)
        {
            Logger.Debug(message);
        }

        public static void LogError(string message)
        {
            Logger.Error(message);
        }
    }
}
